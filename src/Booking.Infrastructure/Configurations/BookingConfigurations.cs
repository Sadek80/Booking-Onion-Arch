using Booking.Domain.Appartments;
using Booking.Domain.Bookings;
using Booking.Domain.Bookings.ValueObjects;
using Booking.Domain.Shared.ValueObjects;
using Booking.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations
{
    internal sealed class BookingConfigurations : IEntityTypeConfiguration<Booking.Domain.Bookings.Booking>
    {
        public void Configure(EntityTypeBuilder<Booking.Domain.Bookings.Booking> builder)
        {
            builder.ToTable(nameof(Booking));

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                   .HasConversion(id => id.Value, value => new BookingId(value));

            builder.Property(p => p.CreatedOnUTC);

            builder.Property(p => p.Status);

            builder.OwnsOne(p => p.Duration);

            builder.OwnsOne(p => p.PriceForPeriod, priceForPeriodBuilder =>
            {
                priceForPeriodBuilder.Property(s => s.Currency)
                                     .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(p => p.CleaningFee, cleaningFeeBuilder =>
            {
                cleaningFeeBuilder.Property(s => s.Currency)
                                  .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(p => p.AmenitiesUpCharge, amenitiesUpChargeBuilder =>
            {
                amenitiesUpChargeBuilder.Property(s => s.Currency)
                                        .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(p => p.TotalPrice, totalPriceBuilder =>
            {
                totalPriceBuilder.Property(s => s.Currency)
                                 .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.HasOne<Appartment>()
                   .WithMany()
                   .HasForeignKey(f => f.AppartmentId);

            builder.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(f => f.UserId);
        }
    }
}
