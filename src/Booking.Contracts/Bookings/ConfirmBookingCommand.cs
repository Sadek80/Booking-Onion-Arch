namespace Booking.Contracts.Bookings;

public sealed record ConfirmBookingCommand(Guid BookingId);