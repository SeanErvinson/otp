using Otp.Core.Domains.Common.Models;

namespace Otp.Application.Common.Interfaces;

public interface IDomainEventService
{
	Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}