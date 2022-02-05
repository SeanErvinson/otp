using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Entities;

namespace Otp.Core.Domains.Events;

public class OtpRequestedEvent : DomainEvent
{
	public OtpRequest OtpRequest { get; }

	public OtpRequestedEvent(OtpRequest otpRequest)
	{
		OtpRequest = otpRequest;
	}
}