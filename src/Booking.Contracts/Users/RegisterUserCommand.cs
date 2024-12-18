namespace Booking.Contracts.Users;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password);