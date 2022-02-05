using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class OtpRequestConfiguration : BaseEntityConfiguration<OtpRequest>
{
	public override void Configure(EntityTypeBuilder<OtpRequest> builder)
	{
		base.Configure(builder);
		builder.Property(otpRequest => otpRequest.State)
				.HasConversion<string>();
		builder.Property(otpRequest => otpRequest.Status)
				.HasConversion<string>();
		builder.Property(otpRequest => otpRequest.ExpiresOn).ValueGeneratedNever();
		builder.Property(otpRequest => otpRequest.Channel)
				.HasConversion<string>();
		builder.Ignore(otpRequest => otpRequest.RequestPath);
	}
}