using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure.Persistence;
using Otp.Infrastructure.Services;
using Otp.Infrastructure.Services.ChannelProviders;
using Otp.Infrastructure.Services.ChannelProviders.EmailProviders;
using Otp.Infrastructure.Services.ChannelProviders.SmsProviders;

namespace Otp.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		if (configuration.GetValue<bool>("UseInMemoryDatabase"))
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
		}
		else
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration
						.GetConnectionString("ApplicationDb"),
					b =>
						b.MigrationsAssembly(typeof(
								ApplicationDbContext)
							.Assembly
							.FullName)));
		}
		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
		services.TryAddEnumerable(new[]
		{
			ServiceDescriptor.Transient<IChannelProviderFactory, SmsChannelProviderFactory>(),
			ServiceDescriptor.Transient<IChannelProviderFactory, EmailChannelProviderFactory>()
		});
		services.TryAddEnumerable(new[]
		{
			ServiceDescriptor.Transient<ISmsProvider, PhilippinesSmsProvider>(),
			ServiceDescriptor.Transient<ISmsProvider, AustraliaSmsProvider>()
		});
		services.AddTransient<IChannelProviderService, ChannelProviderService>();
		services.TryAddEnumerable(new[]
		{
			// ServiceDescriptor.Transient<IEmailProvider, SendGridEmailProvider>(),
			ServiceDescriptor.Transient<IEmailProvider, AwsSesEmailProvider>()
		});
		services.AddTransient<IBlobStorageService, AzureBlobStorageService>();
	}
}