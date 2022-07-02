using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence.Configurations;

public class OtpRequestConfiguration : BaseEntityConfiguration<OtpRequest>
{
	public override void Configure(EntityTypeBuilder<OtpRequest> builder)
	{
		base.Configure(builder);
		builder.Property(otpRequest => otpRequest.SuccessUrl);
		builder.Property(otpRequest => otpRequest.CancelUrl);
		builder.Property(otpRequest => otpRequest.AuthenticityKey);
		builder.Property(otpRequest => otpRequest.ExpiresOn);
		builder.Property(otpRequest => otpRequest.Recipient);

		builder.HasIndex(otpRequest => new
		{
			otpRequest.CreatedAt,
			otpRequest.Id
		});
		
		builder.Property(otpRequest => otpRequest.Availability)
				.HasConversion<string>();
		builder.OwnsMany(otpRequest => otpRequest.Timeline,
			childBuilder =>
			{
				childBuilder.ToTable(nameof(OtpRequest.Timeline));
				childBuilder.Property(child => child.State).HasConversion<string>();
				childBuilder.Property(child => child.Status).HasConversion<string>();
			});
		builder.Property(otpRequest => otpRequest.Channel)
				.HasConversion<string>();
		builder.Ignore(otpRequest => otpRequest.RequestPath);
		builder.OwnsMany(otpRequest => otpRequest.OtpAttempts, attempts =>
		{
			attempts.ToTable(nameof(OtpRequest.OtpAttempts));
			attempts.Property(attempt => attempt.AttemptStatus).HasConversion<string>();
		});
		builder.OwnsOne(otpRequest => otpRequest.ClientInfo);
	}
}