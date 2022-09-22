using System.Text.Json;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Serilog;

namespace Otp.Infrastructure.Services.ChannelProviders.EmailProviders;

public class AwsSesEmailProvider : IEmailProvider
{
	private const string SenderAddress = "ervinsonong@gmail.com";
	private const string ConfigurationSet = "ohtp-configuration-set-9e6ce29";
	private const string TemplateName = "OTPTemplate";

	private readonly JsonSerializerOptions _jsonSerializerOptions;
	private readonly ILogger _logger;

	private readonly IAsyncPolicy _retryPolicy = 
		Policy.Handle<TooManyRequestsException>()
			.Or<LimitExceededException>()
			.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3));

	public AwsSesEmailProvider(JsonSerializerOptions jsonSerializerOptions, ILogger logger)
	{
		_jsonSerializerOptions = jsonSerializerOptions;
		_logger = logger;
	}

	public async Task<string> Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		using var client = new AmazonSimpleEmailServiceV2Client(RegionEndpoint.APSoutheast1);
		var sendRequest = new SendEmailRequest
		{
			FromEmailAddress = SenderAddress,
			Destination = new Destination
			{
				ToAddresses =
					new List<string> { request.Recipient }
			},
			Content = new EmailContent
			{
				Template = new Template
				{
					TemplateName = TemplateName,
					TemplateData = JsonSerializer.Serialize(new TemplateData(request.Code,
							@"https://pages.getpostman.com/rs/067-UMD-991/images/pm-logo-horiz%402x.png",
							request.Recipient),
						_jsonSerializerOptions)
				}
			},
			EmailTags = new List<MessageTag> { new() { Name = nameof(request.AppId), Value = request.AppId.ToString() } },
			ConfigurationSetName = ConfigurationSet
		};

		try
		{
			_logger.Information("Sending email using Amazon SES...");
			
			var response = await _retryPolicy.ExecuteAsync(() => client.SendEmailAsync(sendRequest, cancellationToken));
			
			_logger.Information("The email was sent successfully");
			return response.MessageId;
		}
		catch (Exception ex)
		{
			throw new ChannelProviderException("Failed to send email", ex);
		}
	}

	private record TemplateData(string Code, string LogoUrl, string ReceiverAddress);
}