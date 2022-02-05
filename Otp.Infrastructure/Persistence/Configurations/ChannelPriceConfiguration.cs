using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class ChannelPriceConfiguration : BaseEntityConfiguration<ChannelPrice>
{
	public override void Configure(EntityTypeBuilder<ChannelPrice> builder)
	{
		base.Configure(builder);
		builder.Property(c => c.Country);
		builder.Property(c => c.Origin);
	}
}