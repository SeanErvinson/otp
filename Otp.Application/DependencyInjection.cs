using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otp.Application.PipelineBehaviors;

namespace Otp.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddMediatR(Assembly.GetExecutingAssembly());
	}
}