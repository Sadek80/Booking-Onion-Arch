using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings.ValueObjects
{
    public record BookingId(Guid Value) : IEntityId<BookingId>
    {
        public static BookingId New => new BookingId(Guid.NewGuid());
    }
}
