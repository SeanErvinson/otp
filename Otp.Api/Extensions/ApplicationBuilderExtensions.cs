using Otp.Api.Options;

namespace Otp.Api.Extensions;

public static class ApplicationBuilderExtensions
{
	public static void UseSwaggerApi(this IApplicationBuilder app, IConfiguration configuration)
	{
		var swaggerOptions = new SwaggerOptions();
		configuration.GetSection(SwaggerOptions.Section).Bind(swaggerOptions);
		
		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint($"/swagger/{swaggerOptions.Version}/swagger.json", options.DocumentTitle);
		});
	}
}