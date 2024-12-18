using Booking.Domain.Abstractions;

namespace Booking.Domain.Users
{
    public static partial class DomainErrors
    {
        public static class UserErrors
        {
            public static DomainError NotFound = new(
               "User.NotFound",
               "The user with the specified identifier was not found");

            public static DomainError InvalidCredentials = new(
               "User.InvalidCredentials",
               "Invalid Credentials.");

            public static DomainError AuthenticationFailed = new(
               "User.AuthenticationFailed",
               "Failed to Authenticate User.");
        }
    }
}
