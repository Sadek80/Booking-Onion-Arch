using Booking.Contracts.Users;
using ErrorOr;

namespace Booking.ApplicationServices.Abstractions.Services
{
    public interface IUserService
    {
        Task<ErrorOr<UserResponse>> GetUserAsync(CancellationToken cancellationToken);
        Task<ErrorOr<AccessTokenResponse>> LoginUserAsync(LogInUserCommand request, CancellationToken cancellationToken);
        Task<ErrorOr<RegisterUserResponse>> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken);
    }
}
