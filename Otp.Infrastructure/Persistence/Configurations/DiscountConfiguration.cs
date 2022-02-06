using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class DiscountConfiguration : BaseEntityConfiguration<Discount>
{
	public override void Configure(EntityTypeBuilder<Discount> builder)
	{
		base.Configure(builder);
		builder.Property(c => c.Channel)
				.HasConversion<string>();
	}
}