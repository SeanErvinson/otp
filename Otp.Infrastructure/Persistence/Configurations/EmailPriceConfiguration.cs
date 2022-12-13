using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class EmailPriceConfiguration : BaseEntityConfiguration<EmailPrice, int>
{
	public override void Configure(EntityTypeBuilder<EmailPrice> builder)
	{
		base.Configure(builder);
		builder.ToTable("EmailPricingTable");
	}
}