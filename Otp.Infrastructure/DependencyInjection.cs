using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure.Persistence;

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
	}
}