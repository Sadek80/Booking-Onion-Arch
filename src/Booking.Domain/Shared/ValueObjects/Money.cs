namespace Booking.Domain.Shared.ValueObjects
{
    public record Money(Currency Currency, decimal Amount)
    {
        public static Money operator +(Money first, Money second)
        {
            if (first.Currency != second.Currency)
            {
                throw new InvalidOperationException("non-matched Currencies");
            }

            return new(first.Currency, first.Amount + second.Amount);
        }

        public static Money Zero() => new(Currency.None, 0);
        public static Money Zero(Currency currency) => new(currency, 0);

        public bool IsZero() => this == Zero(Currency);
    }
}
