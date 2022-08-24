using System.Reflection;
using Amazon;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otp.Application.Consumers;

namespace Otp.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(Assembly.GetExecutingAssembly());
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddMassTransit(configurator =>
		{
			configurator.SetKebabCaseEndpointNameFormatter();
			configurator.AddConsumers(Assembly.GetExecutingAssembly());
			configurator.AddDelayedMessageScheduler();
			configurator.UsingAmazonSqs((context, config) =>
			{
				config.UseDelayedMessageScheduler();
				config.ReceiveEndpoint("ohtp-ses-events",
					c =>
					{
						c.ConfigureConsumer<SesEventConsumer>(context);
					});
				config.ConfigureEndpoints(context);
			});
		});
	}
}