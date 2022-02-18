using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class AppConfiguration : BaseEntityConfiguration<App>
{
	public override void Configure(EntityTypeBuilder<App> builder)
	{
		base.Configure(builder);
		builder.Property(c => c.Description).HasMaxLength(500);
		builder.Property(c => c.Status)
				.HasConversion<string>();
		builder.OwnsOne(c => c.Branding);
		builder.Property(c => c.Tags)
				.HasConversion(
					v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
					v => JsonSerializer.Deserialize<IReadOnlyCollection<string>>(v, (JsonSerializerOptions)null!)!,
					new ValueComparer<IReadOnlyCollection<string>>(
						(c1, c2) => c1.SequenceEqual(c2),
						c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
						c => c.ToList()));
		builder.HasIndex(c => new { c.Name, c.PrincipalId }).IsUnique();
	}
}