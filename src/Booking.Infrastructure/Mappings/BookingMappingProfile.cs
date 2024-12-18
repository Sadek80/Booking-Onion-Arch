using AutoMapper;
using Booking.Contracts.Bookings;

namespace Booking.Infrastructure.Mappings
{
    internal sealed class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateProjection<Booking.Domain.Bookings.Booking, BookingResponse>()
                .ForMember(d => d.Status, s => s.MapFrom(sf => sf.Status.ToString()))
                .ForMember(d => d.PriceAmount, s => s.MapFrom(sf => sf.PriceForPeriod.Amount))
                .ForMember(d => d.PriceCurrency, s => s.MapFrom(sf => sf.PriceForPeriod.Currency.Code))
                .ForMember(d => d.CleaningFeeAmount, s => s.MapFrom(sf => sf.CleaningFee.Amount))
                .ForMember(d => d.CleaningFeeCurrency, s => s.MapFrom(sf => sf.CleaningFee.Currency.Code))
                .ForMember(d => d.AmenitiesUpChargeAmount, s => s.MapFrom(sf => sf.AmenitiesUpCharge.Amount))
                .ForMember(d => d.AmenitiesUpChargeCurrency, s => s.MapFrom(sf => sf.AmenitiesUpCharge.Currency.Code))
                .ForMember(d => d.TotalPriceAmount, s => s.MapFrom(sf => sf.TotalPrice.Amount))
                .ForMember(d => d.TotalPriceCurrency, s => s.MapFrom(sf => sf.TotalPrice.Currency.Code))
                .ForMember(d => d.DurationStart, s => s.MapFrom(sf => sf.Duration.Start))
                .ForMember(d => d.DurationEnd, s => s.MapFrom(sf => sf.Duration.End));
        }
    }
}
