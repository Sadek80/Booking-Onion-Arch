namespace Booking.Contracts.Bookings;

public record CancelBookingCommand(Guid BookingId);