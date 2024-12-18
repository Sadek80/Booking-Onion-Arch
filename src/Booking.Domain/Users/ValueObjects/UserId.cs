using Booking.Domain.Abstractions;

namespace Booking.Domain.Users.ValueObjects
{
    public record UserId(Guid Value) : IEntityId<UserId>
    {
        public static UserId New => new UserId(Guid.NewGuid());
    }
}
