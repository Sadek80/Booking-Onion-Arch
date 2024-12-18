using Booking.Domain.Abstractions;
using Booking.Domain.Appartments;
using Booking.Domain.Bookings.ValueObjects;

namespace Booking.Domain.Bookings.Repositories
{
    public interface IBookingRepository : IRepository<Booking, BookingId>
    {
        Task<TResult?> GetBookingDetails<TResult>(BookingId bookingId, CancellationToken cancellationToken = default);
        Task<bool> IsOverlappingAsync(Appartment apartment,
                                      DateRange duration,
                                      CancellationToken cancellationToken = default);
    }
}
