using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Otp.Api.Extensions;
using Otp.Api.Middlewares;
using Otp.Api.PipelineBehaviors;
using Otp.Api.Services;
using Otp.Application;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
	options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddHealthChecks();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRouting(option =>
{
	option.LowercaseUrls = true;
	option.LowercaseQueryStrings = true;
});

builder.Services.AddControllers()
		.AddJsonOptions(option =>
		{
			option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		})
		.AddFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddJwtBearerAuthentication(builder.Configuration, builder.Environment);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseCors(config => config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomExceptionMiddleware>();

app.MapControllers();

app.Run();