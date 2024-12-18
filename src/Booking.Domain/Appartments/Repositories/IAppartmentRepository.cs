using Booking.Domain.Abstractions;
using Booking.Domain.Appartments.ValueObjects;

namespace Booking.Domain.Appartments.Repositories
{
    public interface IAppartmentRepository : IRepository<Appartment, AppartmentId>
    {
    }
}
