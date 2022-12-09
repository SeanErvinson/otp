using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Api.Policies;

public record IsActiveUser : IAuthorizationRequirement;

public class IsActiveUserHandler : AuthorizationHandler<IsActiveUser>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public IsActiveUserHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsActiveUser requirement)
	{
		var user = context.User;

		if (user is null)
		{
			context.Fail(new AuthorizationFailureReason(this, "User must be logged in"));
			return;
		}

		var principal = await _applicationDbContext.Principals.FirstOrDefaultAsync(p => p.UserId == _currentUserService.UserId);

		if (principal is { Status: PrincipalStatus.Deleted or PrincipalStatus.Inactive })
		{
			context.Fail(new AuthorizationFailureReason(this, "User is not available"));
			return;
		}

		context.Succeed(requirement);
	}
}