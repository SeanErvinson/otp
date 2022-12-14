using Microsoft.Extensions.Hosting;

namespace Otp.Infrastructure.Utils;

public static class EnvironmentMapper
{
	public static string Convert(IHostEnvironment environment)
	{
		if (environment.IsProduction())
		{
			return Environments.Production[..4].ToLower();
		}

		if (environment.IsStaging())
		{
			return Environments.Staging.ToLower();
		}

		return Environments.Development[..3].ToLower();
	}
}