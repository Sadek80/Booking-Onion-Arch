using ErrorOr;

namespace Booking.Domain.Abstractions
{ 
    public record DomainError(string Code, string Description)
    {
        public static DomainError None = new(string.Empty, string.Empty);

        public static DomainError NullValue = new("Error.NullValue", "Null value was provided");

        public static Error Conflict(DomainError domainError)
        {
            return Error.Conflict(domainError.Code, domainError.Description);
        }

        public static Error Validation(DomainError domainError)
        {
            return Error.Validation(domainError.Code, domainError.Description);
        }

        public static Error NotFound(DomainError domainError)
        {
            return Error.NotFound(domainError.Code, domainError.Description);
        }

        public static Error Failure(DomainError domainError)
        {
            return Error.Failure(domainError.Code, domainError.Description);
        }

        public static Error Unexpected(DomainError domainError)
        {
            return Error.Unexpected(domainError.Code, domainError.Description);
        }

        public static Error Custom(int type, DomainError domainError)
        {
            return Error.Custom(type, domainError.Code, domainError.Description);
        }
    }
}
