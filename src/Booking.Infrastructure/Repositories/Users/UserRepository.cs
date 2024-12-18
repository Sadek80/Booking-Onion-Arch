using Booking.Infrastructure.Repositories;
using Booking.Domain.Users;
using Booking.Domain.Users.Repositories;
using Booking.Domain.Users.ValueObjects;

namespace Booking.Infrastructure.Repositories.Users
{
    internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
