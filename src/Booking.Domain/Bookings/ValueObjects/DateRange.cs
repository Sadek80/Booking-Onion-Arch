namespace Booking.Domain.Bookings.ValueObjects
{
    public record DateRange
    {
        private DateRange()
        {
        }

        public DateOnly Start { get; private set; }
        public DateOnly End { get; private set; }

        public int LengthInDays => End.DayNumber - Start.DayNumber;

        public static DateRange Create(DateOnly start, DateOnly end)
        {
            if (start > end)
            {
                throw new ApplicationException("End date precedes start date");
            }

            return new DateRange
            {
                Start = start,
                End = end
            };
        }
    }
}
