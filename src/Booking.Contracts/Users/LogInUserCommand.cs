namespace Booking.Contracts.Users;

public sealed record LogInUserCommand(string Email, string Password);