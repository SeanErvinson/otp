using MediatR;
using Otp.Application.Otp.Commands.RequestOtp;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Application.Otp.Commands.RequestEmailOtp;

/// <summary>
/// Email Otp request
/// </summary>
public sealed record RequestEmailOtp : IRequest<RequestOtpResponse>
{
	/// <summary>The user's email address</summary>
	public string EmailAddress { get; init; } = default!;
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

public sealed class RequestEmailOtpHandler : IRequestHandler<RequestEmailOtp, RequestOtpResponse>
{
	private readonly IMediator _mediator;

	public RequestEmailOtpHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task<RequestOtpResponse> Handle(RequestEmailOtp request, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(new RequestOtp.RequestOtp
		{
			Channel = Channel.Email,
			Contact = request.EmailAddress,
			SuccessUrl = request.SuccessUrl,
			CancelUrl = request.CancelUrl
		},
			cancellationToken);
		return response;
	}
}