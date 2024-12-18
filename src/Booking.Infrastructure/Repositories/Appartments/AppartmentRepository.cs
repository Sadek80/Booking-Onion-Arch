using Booking.Domain.Appartments;
using Booking.Domain.Appartments.Repositories;
using Booking.Domain.Appartments.ValueObjects;

namespace Booking.Infrastructure.Repositories.Appartments
{
    internal sealed class AppartmentRepository : Repository<Appartment, AppartmentId>, IAppartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
