using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class CallbackEventConfiguration : BaseEntityConfiguration<CallbackEvent>
{
	public override void Configure(EntityTypeBuilder<CallbackEvent> builder)
	{
		base.Configure(builder);
		builder.Property(c => c.Type)
				.HasConversion<string>();
		builder.Property(c => c.Channel)
				.HasConversion<string>();
	}
}