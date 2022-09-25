using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class PrincipalConfiguration : BaseEntityConfiguration<Principal, Guid>
{
	public override void Configure(EntityTypeBuilder<Principal> builder)
	{
		base.Configure(builder);
		builder.Property(e => e.Name).HasMaxLength(128);
		builder.HasIndex(e => e.UserId).IsUnique().HasFilter(null);
		builder.Property(e => e.UserId).HasMaxLength(36).IsFixedLength();
		builder.Property(e => e.Status).HasConversion<string>();
	}
}