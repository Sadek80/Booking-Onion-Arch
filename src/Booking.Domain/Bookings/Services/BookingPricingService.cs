using Booking.Domain.Appartments;
using Booking.Domain.Appartments.Enums;
using Booking.Domain.Bookings.ValueObjects;
using Booking.Domain.Shared.ValueObjects;

namespace Booking.Domain.Bookings.Services
{
    public sealed class BookingPricingService
    {
        public BookingPricingDetails CalculatePrice(Appartment appartment, DateRange duration)
        {
            var currency = appartment.Price.Currency;

            var pricePerPeriod = new Money(currency, appartment.Price.Amount * duration.LengthInDays);

            var cleaningFee = Money.Zero(currency);

            if (!appartment.CleaningFee.IsZero())
            {
                cleaningFee += appartment.CleaningFee;
            }

            decimal amenitiesChargePercentage = 0.0m;

            foreach (var amenity in appartment.Amenities)
            {
                decimal amenityCharge = amenity switch
                {
                    Amenity.CloseToAirPort => 2.0m,
                    Amenity.CloseToDowTown => 1.5m,
                    Amenity.SwimmingPool or Amenity.PetFriendly => 1.0m,
                    _ => 0.5m,
                };

                amenitiesChargePercentage += amenityCharge;
            }

            var amenityUpCharge = new Money(currency, amenitiesChargePercentage * pricePerPeriod.Amount);

            var totalPrice = pricePerPeriod + cleaningFee + amenityUpCharge;

            return new BookingPricingDetails(pricePerPeriod, cleaningFee, amenityUpCharge, totalPrice);
        }
    }
}
