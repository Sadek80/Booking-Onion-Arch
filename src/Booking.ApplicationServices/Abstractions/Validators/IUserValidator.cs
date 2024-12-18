using Booking.Contracts.Users;
using ErrorOr;

namespace Booking.ApplicationServices.Abstractions.Validators
{
    public interface IUserValidator
    {
        List<Error> ValidateRegisterUser(RegisterUserCommand request);
    }
}