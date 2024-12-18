namespace Booking.Domain.Abstractions
{
    public interface IEntityId<T>
    {
        public abstract static T New { get; }
    }
}
