using Booking.Contracts.Bookings;
using Booking.Domain.Bookings.ValueObjects;
using ErrorOr;

namespace Booking.ApplicationServices.Abstractions.Services
{
    public interface IBookingService
    {
        Task<ErrorOr<bool>> CancelBookingAsync(CancelBookingCommand request, CancellationToken cancellationToken);
        Task<ErrorOr<bool>> CompleteBookingAsync(CompleteBookingCommand request, CancellationToken cancellationToken);
        Task<ErrorOr<bool>> ConfirmBookingAsync(ConfirmBookingCommand request, CancellationToken cancellationToken);
        Task<ErrorOr<BookingResponse>> GetBookingAsync(GetBookingQuery request, CancellationToken cancellationToken);
        Task<ErrorOr<bool>> RejectBookingAsync(RejectBookingCommand request, CancellationToken cancellationToken);
        Task<ErrorOr<BookingId>> ReserveBookingAsync(ReserveBookingCommand request, CancellationToken cancellationToken);
    }
}
