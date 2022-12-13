using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class SmsPriceConfiguration : BaseEntityConfiguration<SmsPrice, int>
{
	public override void Configure(EntityTypeBuilder<SmsPrice> builder)
	{
		base.Configure(builder);
		builder.ToTable("SmsPricingTable");
	}
}