using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Otp.Api.Options;
using Otp.Infrastructure.Options;
using Otp.Infrastructure.Persistence;

namespace Otp.Api.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddJwtBearerAuthentication(this IServiceCollection services,
		IConfiguration configuration,
		IWebHostEnvironment hostEnvironment)
	{
		services.AddOptions<AzureB2COptions>().BindConfiguration(AzureB2COptions.Section);
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
				options.TokenValidationParameters =
					new TokenValidationParameters { ValidateLifetime = !hostEnvironment.IsDevelopment() };
				options.RequireHttpsMetadata = !hostEnvironment.IsDevelopment();
			});
		services.AddAuthorization(options =>
		{
			var defaultAuthorizationPolicyBuilder =
				new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
			defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
			options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
		});
	}

	public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
	{
		var swaggerOptions = new SwaggerOptions();
		configuration.GetSection(SwaggerOptions.Section).Bind(swaggerOptions);
		services.AddFluentValidationRulesToSwagger();
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc(swaggerOptions.Version,
				new OpenApiInfo
				{
					Title = swaggerOptions.Title,
					Version = swaggerOptions.Version,
					Description = swaggerOptions.Description,
					Contact = new OpenApiContact
					{
						Name = "Support",
						Email = $"support@{swaggerOptions.Domain}",
					},
					License = new OpenApiLicense
					{
						Name = "License",
						Url = new Uri($"https://{swaggerOptions.Domain}/license")
					},
					TermsOfService = new Uri($"https://{swaggerOptions.Domain}/tos")
				});
			c.AddSecurityDefinition("api-key",
				new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.ApiKey,
					In = ParameterLocation.Header,
					Description = "API key of your application",
					Name = "api-key",
					Scheme = "token"
				});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "api-key" },
					},
					new[] {"writeAccess"}
				}
			});
			var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
			xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
		});
	}

	public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		var storageBlobOptions = configuration.GetSection(StorageAccountOptions.Section).Get<StorageAccountOptions>();
		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>()
			.AddAzureBlobStorage(storageBlobOptions.BlobStorage.ConnectionString);
	}
}