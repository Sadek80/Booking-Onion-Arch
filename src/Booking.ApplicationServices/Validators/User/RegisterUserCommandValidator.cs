using Booking.ApplicationServices.Abstractions.Validators;
using Booking.Contracts.Users;
using ErrorOr;
using FluentValidation;
using MediatR;
using System.Text;

namespace Booking.ApplicationServices.Validators.User;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();

        RuleFor(c => c.LastName).NotEmpty();

        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).SetValidator(new PasswordValidator());
    }

    public class PasswordValidator : AbstractValidator<string>
    {
        private readonly bool _requireLowerCase;
        private readonly bool _requireDigit;
        private readonly bool _requireUpperCase;
        private readonly bool _requireSpecialCharacter;
        private readonly int _minimumLength;

        public PasswordValidator(bool requireLowerCase = true,
                                 bool requireDigit = true,
                                 bool requireUpperCase = true,
                                 bool requireSpecialCharacter = true,
                                 int minimumLength = 8)
        {
            _requireLowerCase = requireLowerCase;
            _requireDigit = requireDigit;
            _requireUpperCase = requireUpperCase;
            _requireSpecialCharacter = requireSpecialCharacter;
            _minimumLength = minimumLength;

            RuleFor(c => c)
                .Must(password =>
                {
                    if (password.Length < minimumLength)
                    {
                        return false;
                    }

                    if (requireDigit && password.All(c => !IsDigit(c)))
                    {
                        return false;
                    }

                    if (requireLowerCase && password.All(c => !IsLower(c)))
                    {
                        return false;
                    }

                    if (requireUpperCase && password.All(c => !IsUpper(c)))
                    {
                        return false;
                    }

                    if (requireSpecialCharacter && password.All(c => IsLetterOrDigit(c)))
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage(GenerateErrorMessage());
        }

        private bool FitMinimumLength(string password, int minimumLength)
        {
            return password.Length >= minimumLength;
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        private bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        private bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }

        private string GenerateErrorMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"Password Should be at least {_minimumLength} characters.");

            if (_requireLowerCase)
            {
                stringBuilder.Append("At Least one Lower case.");
            }

            if (_requireUpperCase)
            {
                stringBuilder.Append("At Least one Upper case.");
            }

            if (_requireSpecialCharacter)
            {
                stringBuilder.Append("At Least one Special Character.");
            }

            if (_requireDigit)
            {
                stringBuilder.Append("At Least one Number.");
            }

            return stringBuilder.ToString();
        }
    }
}