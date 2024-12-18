using Booking.ApplicationServices.Abstractions.Services;
using Booking.Domain.Users.ValueObjects;

namespace Booking.Infrastructure.Services
{
    internal sealed class EmailService : IEmailService
    {
        public async Task SendAsync(Email email, string subject, string body)
        {
            await Task.CompletedTask;
        }
    }
}
