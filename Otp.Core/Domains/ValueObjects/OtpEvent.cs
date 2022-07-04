using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

public class OtpEvent : ValueObject
{
	public EventState State { get; private set; }
	public DateTime OccuredAt { get; private set; }
	public string? Response { get; private set; }
	public EventStatus Status { get; private set; }

	public static OtpEvent Success(EventState state, string? response = null) => new(state, response, EventStatus.Success);

	public static OtpEvent Fail(EventState state, string? response = null) => new(state, response, EventStatus.Fail);

	private OtpEvent(EventState state, string? response, EventStatus status)
	{
		State = state;
		OccuredAt = DateTime.UtcNow;
		Response = response;
		Status = status;
	}
}

public enum EventState
{
	Request,
	Send,
	Deliver
}

public enum EventStatus
{
	Success,
	Fail
}