namespace Booking.Domain.Shared.ValueObjects
{
    public record Currency
    {
        internal static readonly Currency None = new("");
        public static readonly Currency USD = new("USD");
        public static readonly Currency EUR = new("USD");

        private Currency(string code) => Code = code;

        public string Code { get; init; }

        public static Currency FromCode(string code)
        {
            return All.FirstOrDefault(x => x.Code == code) ??
                   throw new ApplicationException("Currency Code is not Valid");
        }

        public static readonly IReadOnlyCollection<Currency> All = new[]
        {
            USD,
            EUR
        };
    }
}
