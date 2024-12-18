using Booking.ApplicationServices.Abstractions.Clock;
using Booking.ApplicationServices.Abstractions.Services;
using Booking.Contracts.Bookings;
using Booking.Domain.Abstractions;
using Booking.Domain.Appartments.Repositories;
using Booking.Domain.Appartments.ValueObjects;
using Booking.Domain.Bookings.Repositories;
using Booking.Domain.Bookings.Services;
using Booking.Domain.Bookings.ValueObjects;
using Booking.Domain.Exceptions;
using Booking.Domain.Users.Repositories;
using Booking.Domain.Users.ValueObjects;
using ErrorOr;
using static Booking.Domain.Appartments.DomainErrors;
using static Booking.Domain.Bookings.DomainErrors;
using static Booking.Domain.Users.DomainErrors;
using static Booking.Domain.Bookings.Booking;

namespace Booking.ApplicationServices.Services
{
    internal sealed class BookingService : IBookingService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IAppartmentRepository _appartmentRepository;
        private readonly BookingPricingService _bookingPricingService;

        public BookingService(IDateTimeProvider dateTimeProvider,
                              IBookingRepository bookingRepository,
                              IUnitOfWork unitOfWork,
                              IUserRepository userRepository,
                              IAppartmentRepository appartmentRepository,
                              BookingPricingService bookingPricingService)
        {
            _dateTimeProvider = dateTimeProvider;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _appartmentRepository = appartmentRepository;
            _bookingPricingService = bookingPricingService;
        }

        public async Task<ErrorOr<bool>> CancelBookingAsync(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);

            if (booking is null)
            {
                return DomainError.NotFound(BookingErrors.NotFound);
            }

            var result = booking.Cancel(_dateTimeProvider.UTCNow);

            if (result.IsError)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<ErrorOr<bool>> CompleteBookingAsync(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);

            if (booking is null)
            {
                return DomainError.NotFound(BookingErrors.NotFound);
            }

            var result = booking.Complete(_dateTimeProvider.UTCNow);

            if (result.IsError)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<ErrorOr<bool>> ConfirmBookingAsync(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);

            if (booking is null)
            {
                return DomainError.NotFound(BookingErrors.NotFound);
            }

            var result = booking.Confirm(_dateTimeProvider.UTCNow);

            if (result.IsError)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<ErrorOr<BookingResponse>> GetBookingAsync(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingDetails<BookingResponse>(new BookingId(request.BookingId), cancellationToken);

            if (booking is null)
            {
                return DomainError.NotFound(BookingErrors.NotFound);
            }

            return booking;
        }

        public async Task<ErrorOr<bool>> RejectBookingAsync(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);

            if (booking is null)
            {
                return DomainError.NotFound(BookingErrors.NotFound);
            }

            var result = booking.Reject(_dateTimeProvider.UTCNow);
            if (result.IsError)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<ErrorOr<BookingId>> ReserveBookingAsync(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(request.userId), cancellationToken);

            if (user is null)
            {
                return DomainError.NotFound(UserErrors.NotFound);
            }

            var appartment = await _appartmentRepository.GetByIdAsync(new AppartmentId(request.appartmentId), cancellationToken);

            if (appartment is null)
            {
                return DomainError.NotFound(AppartmentErrors.NotFound);
            }

            var duration = DateRange.Create(request.startDate, request.endDate);

            if (await _bookingRepository.IsOverlappingAsync(appartment, duration, cancellationToken))
            {
                return DomainError.Conflict(BookingErrors.Overlap);
            }

            try
            {
                var booking = Reserve(new UserId(request.userId),
                                         appartment,
                                         duration,
                                         _bookingPricingService,
                                         _dateTimeProvider.UTCNow);

                _bookingRepository.Add(booking);

                await _unitOfWork.SaveChangesAsync();

                return booking.Id;
            }
            catch (ConcurrencyException)
            {
                return DomainError.Conflict(BookingErrors.Overlap);
            }
        }
    }
}
