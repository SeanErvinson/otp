// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Otp.Core.Domains.Common.Enums;
// using Otp.Core.Domains.Entities;
//
// namespace Otp.Infrastructure.Persistence.Configurations;
//
// public class SubscriptionConfiguration : BaseEntityConfiguration<Subscription>
// {
// 	public override void Configure(EntityTypeBuilder<Subscription> builder)
// 	{
// 		base.Configure(builder);
// 		builder.ToTable("Subscriptions", tableBuilder => { tableBuilder.IsTemporal(); });
// 		builder.Property(p => p.TieredPlan)
// 				.HasConversion(
// 					p => p.Value,
// 					p => TieredPlan.FromValue(p));
// 	}
// }

