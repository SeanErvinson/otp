using MediatR;
using Otp.Core.Domains;
using Otp.Core.Domains.Common;

namespace Otp.Application.Otp.Commands.RequestOtp;

public record RequestOtpCommand : IRequest<RequestOtpDto>
{
	public Mode Mode { get; init; }
	public string Contact { get; init; } = default!;
	public string SuccessUrl { get; init; } = default!;
	public string CancelUrl { get; init; } = default!;

	/*
	 * TODO: Consider adding an additional options for stuff like
	 * maxRetries, allowResend, and etc
	*/

	public static RequestOtpCommand Sms(string contact, string successUrl, string cancelUrl)
	{
		return new RequestOtpCommand
		{
			Mode = Mode.SMS,
			Contact = contact,
			SuccessUrl = successUrl,
			CancelUrl = cancelUrl
		};
	}

	public static RequestOtpCommand Email(string contact, string successUrl, string cancelUrl)
	{
		return new RequestOtpCommand
		{
			Mode = Mode.Email,
			Contact = contact,
			SuccessUrl = successUrl,
			CancelUrl = cancelUrl
		};
	}

	public class Handler : IRequestHandler<RequestOtpCommand, RequestOtpDto>
	{
		public async Task<RequestOtpDto> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
		{
			var newOtpRequest = new OtpRequest(request.Contact, request.Mode, request.SuccessUrl, request.CancelUrl);

			//TODO: Save to database
			//TODO: Get the base url of the frontend append /ui/ <- as a configurable

			return new RequestOtpDto(newOtpRequest.Id, new Uri(new Uri("http://localhost:8080/"), newOtpRequest.RequestPath));
		}
	}
}

public record RequestOtpDto(Guid Id, Uri RedirectUri);