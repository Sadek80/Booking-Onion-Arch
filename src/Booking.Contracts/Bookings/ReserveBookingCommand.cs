namespace Booking.Contracts.Bookings
{
    public sealed record ReserveBookingCommand(Guid userId,
                                               Guid appartmentId,
                                               DateOnly startDate,
                                               DateOnly endDate);
}
