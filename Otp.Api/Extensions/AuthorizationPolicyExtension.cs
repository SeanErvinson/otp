using Microsoft.AspNetCore.Authorization;
using Otp.Api.Policies;

namespace Otp.Api.Extensions;

public static class AuthorizationPolicyExtension
{
	public static void AddPolicies(this AuthorizationOptions options)
	{
		options.AddPolicy(nameof(IsActiveUser), builder => builder.AddRequirements(new IsActiveUser()));
	}

	public static void AddAuthorizationPolicies(this IServiceCollection services)
	{
		// Register authorization requirements (should have the same count as AddPolicies)
		services.AddSingleton<IAuthorizationRequirement, IsActiveUser>();

		// Register handlers for each authorization requirements
		services.AddSingleton<IAuthorizationHandler, IsActiveUserHandler>();
	}
}