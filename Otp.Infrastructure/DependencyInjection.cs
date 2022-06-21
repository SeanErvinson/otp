using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure.Persistence;
using Otp.Infrastructure.Services;
using Otp.Infrastructure.Services.Sender;
using Otp.Infrastructure.Services.Sender.EmailProviders;
using Otp.Infrastructure.Services.Sender.SmsProviders;

namespace Otp.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		if (configuration.GetValue<bool>("UseInMemoryDatabase"))
			services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
		else
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ApplicationDb"),
																						b =>
																							b.MigrationsAssembly(
																								typeof(ApplicationDbContext).Assembly.FullName)));
		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.TryAddEnumerable(new[]
		{
			ServiceDescriptor.Transient<ISenderFactory, SmsSenderFactory>(),
			ServiceDescriptor.Transient<ISenderFactory, EmailSenderFactory>(),
		});

		services.TryAddEnumerable(new[]
		{
			ServiceDescriptor.Transient<ISmsProvider, PhilippinesSmsProvider>(),
			ServiceDescriptor.Transient<ISmsProvider, AustraliaSmsProvider>(),
		});
		services.AddTransient<ISenderService, SenderService>();
		
		services.TryAddEnumerable(new []
		{
			// ServiceDescriptor.Transient<IEmailProvider, SendGridEmailProvider>(),
			ServiceDescriptor.Transient<IEmailProvider, AwsSesEmailProvider>(), 
		});

		services.AddTransient<IBlobStorageService, AzureBlobStorageService>();
	}
}