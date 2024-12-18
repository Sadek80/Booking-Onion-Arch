namespace Booking.Contracts.Bookings;

public sealed record RejectBookingCommand(Guid BookingId);