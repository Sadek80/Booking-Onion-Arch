using Booking.Domain.Appartments;
using Booking.Domain.Appartments.ValueObjects;
using Booking.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Configurations
{
    internal sealed class AppartmentConfigurations : IEntityTypeConfiguration<Appartment>
    {
        public void Configure(EntityTypeBuilder<Appartment> builder)
        {
            builder.ToTable(nameof(Appartment));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                   .HasConversion(id => id.Value, value => new AppartmentId(value));

            builder.Property(p => p.LastBookedOnUTC);

            builder.Property(x => x.Name)
                .HasMaxLength(400)
                .HasConversion(name => name.Value, value => new Name(value));

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .HasConversion(description => description.Value, value => new Description(value));

            builder.OwnsOne(p => p.Address);

            builder.OwnsOne(p => p.Price, priceBuilder =>
            {
                priceBuilder.Property(s => s.Currency)
                            .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(p => p.CleaningFee, cleaningFeeBuilder =>
            {
                cleaningFeeBuilder.Property(s => s.Currency)
                                  .HasConversion(c => c.Code, code => Currency.FromCode(code));
            });

            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
