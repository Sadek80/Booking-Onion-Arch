namespace Booking.ApplicationServices.Abstractions.Clock
{
    public interface IDateTimeProvider
    {
        DateTime UTCNow { get; }
    }
}
