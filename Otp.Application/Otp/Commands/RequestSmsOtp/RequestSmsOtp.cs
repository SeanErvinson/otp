using MediatR;
using Otp.Application.Otp.Commands.RequestOtp;
using Otp.Core.Domains.Common.Enums;
using PhoneNumbers;

namespace Otp.Application.Otp.Commands.RequestSmsOtp;

/// <summary>
/// SMS Otp request
/// </summary>
public sealed record RequestSmsOtp : IRequest<RequestOtpResponse>
{
	/// <summary>The user's phone number. Phone number must include country code.</summary>
	public string PhoneNumber { get; init; } = default!;
	/// <summary>
	/// Where to redirect a user after OTP success
	/// </summary>
	public string SuccessUrl { get; init; } = default!;
	/// <summary>
	/// Where to redirect a user after OTP failed
	/// </summary>
	public string CancelUrl { get; init; } = default!;

	/*
	 * TODO: Consider adding an additional options for stuff like
	 * maxRetries, allowResend, and etc
	*/
}

public sealed class RequestSmsOtpHandler : IRequestHandler<RequestSmsOtp, RequestOtpResponse>
{
	private readonly IMediator _mediator;

	public RequestSmsOtpHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task<RequestOtpResponse> Handle(RequestSmsOtp request, CancellationToken cancellationToken)
	{
		var phoneNumberUtil = PhoneNumberUtil.GetInstance();
		var parsedNumber = phoneNumberUtil.Parse(request.PhoneNumber, "ZZ");
		var formattedInternationalNumber = phoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.INTERNATIONAL);
		var response = await _mediator.Send(new RequestOtp.RequestOtp
			{
				Channel = Channel.Sms,
				Contact = formattedInternationalNumber,
				SuccessUrl = request.SuccessUrl,
				CancelUrl = request.CancelUrl
			},
			cancellationToken);

		return response;
	}
}