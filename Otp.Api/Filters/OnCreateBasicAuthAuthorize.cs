using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Otp.Api.Options;

namespace Otp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class OnCreateBasicAuthAuthorize : BasicAuthAuthorize
{
	protected override (string username, string password) GetCredential(AuthorizationFilterContext context)
	{
		var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AzureB2COptions>>();
		var onCreate = options.Value.ApiConnector.OnCreate;
		return (onCreate.Username, onCreate.Password);
	}
}