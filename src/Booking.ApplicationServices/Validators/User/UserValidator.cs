using Booking.ApplicationServices.Abstractions.Validators;
using Booking.Contracts.Users;
using ErrorOr;
using FluentValidation;

namespace Booking.ApplicationServices.Validators.User
{
    internal sealed class UserValidator : IUserValidator
    {
        private readonly IEnumerable<IValidator<RegisterUserCommand>> _regiserUserCommandValidators;

        public UserValidator(IEnumerable<IValidator<RegisterUserCommand>> regiserUserCommandValidators)
        {
            _regiserUserCommandValidators = regiserUserCommandValidators;
        }

        public List<Error> ValidateRegisterUser(RegisterUserCommand request)
        {
            if (!_regiserUserCommandValidators.Any())
            {
                return null;
            }

            var context = new ValidationContext<RegisterUserCommand>(request);

            var validationErrors = _regiserUserCommandValidators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .ToList()
                .ConvertAll(validationFailure =>
                            Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage));

            return validationErrors;
        }
    }
}
