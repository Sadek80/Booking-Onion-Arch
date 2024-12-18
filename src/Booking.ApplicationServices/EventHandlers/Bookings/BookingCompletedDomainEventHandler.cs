using Booking.ApplicationServices.Abstractions.Services;
using Booking.Domain.Bookings.Events;
using Booking.Domain.Bookings.Repositories;
using Booking.Domain.Users.Repositories;
using MediatR;

namespace Booking.ApplicationServices.EventHandlers.Bookings
{
    internal sealed class BookingCompletedDomainEventHandler : INotificationHandler<BookingCompletedDomainEvent>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public BookingCompletedDomainEventHandler(IBookingRepository bookingRepository,
                                                 IUserRepository userRepository,
                                                 IEmailService emailService)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task Handle(BookingCompletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

            if (booking is null)
            {
                return;
            }

            var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);

            if (user is null) { return; }

            await _emailService.SendAsync(user.Email,
                                          "Booking Completed!",
                                          "You have Completed this booking");
        }
    }
}
