using Booking.Domain.Shared.ValueObjects;

namespace Booking.Domain.Bookings.Services
{
    public record BookingPricingDetails(Money PriceForPeriod,
                                        Money CleaningFee,
                                        Money AmenitiesUpCharge,
                                        Money TotalPrice);
}
