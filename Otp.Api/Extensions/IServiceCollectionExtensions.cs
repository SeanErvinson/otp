using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Otp.Api.Options;

namespace Otp.Api.Extensions;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services,
																IConfiguration configuration,
																IWebHostEnvironment hostEnvironment)
	{
		var azureB2COptions = configuration.GetSection(AzureB2COptions.Section).Get<AzureB2COptions>();
		services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.Authority = azureB2COptions.Authority;
					options.Audience = azureB2COptions.ClientId;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateLifetime = !hostEnvironment.IsDevelopment()
					};
					options.RequireHttpsMetadata = !hostEnvironment.IsDevelopment();
				});

		services.AddAuthorization(options =>
		{
			var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
			defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
			options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
		});
		return services;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "Otp", Version = "v1" });
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.Http,
				In = ParameterLocation.Header,
				Description = "Provide a bearer token to access api endpoints",
				Name = "Bearer Authorization",
				BearerFormat = "JWT",
				Scheme = "bearer"
			});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Scheme = "oauth2",
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					new List<string>()
				}
			});
		});
		return services;
	}
}