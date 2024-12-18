using Booking.Domain.Abstractions;
using MediatR;

namespace Booking.Infrastructure.Scheduling
{
    internal sealed class ProcessCoreEventJob
    {
        private readonly IPublisher _publisher;

        public ProcessCoreEventJob(IPublisher publisher)
        {
            _publisher = publisher;
        }
        public async Task Process(IDomainEvent domainEvent)
        {
            await _publisher.Publish(domainEvent);
        }

        public async Task Process(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}
