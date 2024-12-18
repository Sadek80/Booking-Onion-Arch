using Booking.Domain.Abstractions;
using Booking.Domain.Users.ValueObjects;

namespace Booking.Domain.Users.Events
{
    public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
}
