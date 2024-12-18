using Booking.Domain.Abstractions;

namespace Booking.Domain.Appartments.ValueObjects
{
    public record AppartmentId(Guid Value) : IEntityId<AppartmentId>
    {
        public static AppartmentId New => new AppartmentId(Guid.NewGuid());
    }
}
