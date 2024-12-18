using Booking.Domain.Abstractions;
using Booking.Domain.Users.ValueObjects;

namespace Booking.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User, UserId>
    {
    }
}
