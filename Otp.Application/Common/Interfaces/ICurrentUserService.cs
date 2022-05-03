namespace Otp.Application.Common.Interfaces;

public interface ICurrentUserService
{
	string Email { get; }
	string UserId { get; }
	string UserAgent { get; }
	string IpAddress { get; }
	Guid PrincipalId { get; }
}