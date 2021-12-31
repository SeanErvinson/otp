using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Common;

namespace Otp.Infrastructure.Persistence.Configurations;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.HasKey(c => c.Id);
		builder.HasIndex(c => c.Id).IsUnique();
	}
}