using Bogus;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Infrastructure.Persistence;

namespace Otp.Api.Startup;

public class DbInitializer : IDbInitializer
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public DbInitializer(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public void Migrate()
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.Migrate();
	}

	public void Seed()
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

		if (!dbContext.Principals.Any())
		{
			var adminPrincipal = new Principal("Bob", "a07f580c-026a-4669-8f8d-c3684bc65646", Guid.NewGuid().ToString());
			dbContext.Principals.Add(adminPrincipal);
			dbContext.SaveChanges();
		}

		if (!dbContext.Apps.Any())
		{
			var principal = dbContext.Principals.First();
			var fakerApps = new Faker<App>()
				.CustomInstantiator(f => new App(principal.Id, new Randomizer().Word(), "test", null))
				.RuleFor(app => app.PrincipalId, _ => principal.Id)
				.RuleFor(app => app.Description, fake => fake.Lorem.Sentence());
			var seedApps = fakerApps.Generate(3);
			dbContext.Apps.AddRange(seedApps);
			dbContext.SaveChanges();
		}
	}
}

public interface IDbInitializer
{
	void Migrate();
	void Seed();
}