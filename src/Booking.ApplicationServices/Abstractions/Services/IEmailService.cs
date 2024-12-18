using Booking.Domain.Users.ValueObjects;

namespace Booking.ApplicationServices.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(Email email, string subject, string body);
    }
}
