namespace Otp.Application.Common.Interfaces;

public interface ICurrentUserService
{
	string Email { get; }
	string UserId { get; }
	Guid PrincipalId { get; }
}