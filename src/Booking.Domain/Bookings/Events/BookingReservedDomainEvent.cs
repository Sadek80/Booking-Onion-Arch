using Booking.Domain.Abstractions;
using Booking.Domain.Bookings.ValueObjects;

namespace Booking.Domain.Bookings.Events
{
    public record BookingReservedDomainEvent(BookingId BookingId) : IDomainEvent;
}
