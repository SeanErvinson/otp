using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Otp.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(Assembly.GetExecutingAssembly());
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	}
}