using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings
{
    public static partial class DomainErrors
    {
        public static class BookingErrors
        {
            public static DomainError NotFound = new(
               "Booking.NotFound",
               "The booking with the specified identifier was not found");

            public static DomainError Overlap = new(
                "Booking.Overlap",
                "The current booking is overlapping with an existing one");

            public static DomainError NotReserved = new(
                "Booking.NotReserved",
                "The booking is not pending");

            public static DomainError NotConfirmed = new(
                "Booking.NotReserved",
                "The booking is not confirmed");

            public static DomainError AlreadyStarted = new(
                "Booking.AlreadyStarted",
                "The booking has already started");
        }
    }

}
