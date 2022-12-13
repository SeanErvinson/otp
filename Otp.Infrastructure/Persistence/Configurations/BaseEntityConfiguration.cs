using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otp.Core.Domains.Common.Models;

namespace Otp.Infrastructure.Persistence.Configurations;

public class BaseEntityConfiguration<T, TKey> : IEntityTypeConfiguration<T> where T : BaseEntity<TKey>
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.HasKey(c => c.Id);
		builder.HasIndex(c => c.Id).IsUnique();
		builder.Ignore(c => c.DomainEvents);
	}
}