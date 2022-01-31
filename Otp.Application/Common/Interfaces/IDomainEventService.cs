using Otp.Core.Domains.Common;

namespace Otp.Application.Common.Interfaces;

public interface IDomainEventService
{
	Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}