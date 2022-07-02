using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using MediatR;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.Events;
using Otp.Core.Utils;

namespace Otp.Application.App.EventHandlers;

public class CallbackTriggeredEventHandler : INotificationHandler<CallbackTriggeredEvent>
{
	private const string SignatureHeaderKey = "OTP_SIGNATURE";
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly IApplicationDbContext _dbContext;

	public CallbackTriggeredEventHandler(IHttpClientFactory httpClientFactory, IApplicationDbContext dbContext)
	{
		_httpClientFactory = httpClientFactory;
		_dbContext = dbContext;
	}

	public async Task Handle(CallbackTriggeredEvent notification, CancellationToken cancellationToken)
	{
		var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		var data = JsonSerializer.Serialize(new CallbackEventResponse
		{
			RequestId = notification.CallbackEvent.RequestId,
			Contact = notification.CallbackEvent.Contact,
			Channel = notification.CallbackEvent.Channel,
			Type = notification.CallbackEvent.Type
		});

		var rawSignature = $"{currentTime}.{data}";

		var signature = new SignatureResponse(currentTime, CryptoUtil.Hash256(rawSignature, notification.EndpointSecret));

		var httpMessage = new HttpRequestMessage(HttpMethod.Post, notification.CallbackUrl)
		{
			Headers =
			{
				{ SignatureHeaderKey, JsonSerializer.Serialize(signature) },
			},
			Content = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json),
		};
		var httpClient = _httpClientFactory.CreateClient();
		HttpResponseMessage responseMessage = new HttpResponseMessage();
		try
		{
			responseMessage = await httpClient.SendAsync(httpMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			responseMessage.StatusCode = ex.StatusCode ?? HttpStatusCode.ServiceUnavailable;
			responseMessage.ReasonPhrase = ex.Message;
		}
		finally
		{
			notification.CallbackEvent.SetResponse((int) responseMessage.StatusCode, responseMessage.ReasonPhrase);
			await _dbContext.CallbackEvents.AddAsync(notification.CallbackEvent, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	public record CallbackEventResponse
	{
		public Guid RequestId { get; init; }
		public Channel Channel { get; init; }
		public string Contact { get; init; } = default!;
		public CallbackEventType Type { get; init; }
	};
	
	public record SignatureResponse(long Timestamp, string Value);
}