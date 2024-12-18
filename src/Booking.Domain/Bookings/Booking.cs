using Booking.Domain.Abstractions;
using Booking.Domain.Appartments;
using Booking.Domain.Appartments.ValueObjects;
using Booking.Domain.Bookings.Enums;
using Booking.Domain.Bookings.Events;
using Booking.Domain.Bookings.Services;
using Booking.Domain.Shared.ValueObjects;
using Booking.Domain.Users.ValueObjects;
using ErrorOr;
using Booking.Domain.Bookings.ValueObjects;

namespace Booking.Domain.Bookings
{
    public sealed class Booking : Entity<BookingId>
    {
        private Booking(BookingId Id,
                        UserId userId,
                        AppartmentId appartmentId,
                        DateRange duration,
                        Money priceForPeriod,
                        Money cleaningFee,
                        Money amenitiesUpCharge,
                        Money totalPrice,
                        BookingStatus bookingStatus,
                        DateTime createdOnUTC)
            : base(Id)
        {
            UserId = userId;
            AppartmentId = appartmentId;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            CleaningFee = cleaningFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            CreatedOnUTC = createdOnUTC;
        }

        private Booking()
        {
        }

        public UserId UserId { get; private set; }
        public AppartmentId AppartmentId { get; private set; }
        public DateRange Duration { get; private set; }
        public Money PriceForPeriod { get; private set; }
        public Money CleaningFee { get; private set; }
        public Money AmenitiesUpCharge { get; private set; }
        public Money TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime CreatedOnUTC { get; private set; }
        public DateTime? ConfirmedOnUTC { get; private set; }
        public DateTime? RejectedOnUTC { get; private set; }
        public DateTime? CompletedOnUTC { get; private set; }
        public DateTime? CanceledOnUTC { get; private set; }

        public static Booking Reserve(UserId userId,
                                     Appartment appartment,
                                     DateRange duration,
                                     BookingPricingService bookingPricingService,
                                     DateTime createdOnUTC)
        {
            var pricingDetails = bookingPricingService.CalculatePrice(appartment, duration);

            var booking = new Booking(BookingId.New,
                                      userId,
                                      appartment.Id,
                                      duration,
                                      pricingDetails.PriceForPeriod,
                                      pricingDetails.CleaningFee,
                                      pricingDetails.AmenitiesUpCharge,
                                      pricingDetails.TotalPrice,
                                      BookingStatus.Reserved,
                                      createdOnUTC);

            booking.RaisDomainEvent(new BookingReservedDomainEvent(booking.Id));

            appartment.LastBookedOnUTC = createdOnUTC;

            return booking;
        }

        public ErrorOr<bool> Confirm(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
            {
                return DomainError.Conflict(DomainErrors.BookingErrors.NotReserved);
            }

            Status = BookingStatus.Confirmed;
            ConfirmedOnUTC = utcNow;

            RaisDomainEvent(new BookingConfirmedDomainEvent(Id));

            return true;
        }

        public ErrorOr<bool> Reject(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
            {
                return DomainError.Conflict(DomainErrors.BookingErrors.NotReserved);
            }

            Status = BookingStatus.Rejected;
            RejectedOnUTC = utcNow;

            RaisDomainEvent(new BookingRejectedDomainEvent(Id));

            return true;
        }

        public ErrorOr<bool> Cancel(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return DomainError.Conflict(DomainErrors.BookingErrors.NotConfirmed);
            }

            if (Duration.Start > DateOnly.FromDateTime(utcNow))
            {
                return DomainError.Validation(DomainErrors.BookingErrors.AlreadyStarted);
            }

            Status = BookingStatus.Cancelled;
            CanceledOnUTC = utcNow;

            RaisDomainEvent(new BookingCanceledDomianEvent(Id));

            return true;
        }

        public ErrorOr<bool> Complete(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return DomainError.Conflict(DomainErrors.BookingErrors.NotConfirmed);
            }

            Status = BookingStatus.Completed;
            CompletedOnUTC = utcNow;

            RaisDomainEvent(new BookingCompletedDomainEvent(Id));

            return true;
        }
    }
}
