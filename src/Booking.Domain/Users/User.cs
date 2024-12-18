using Booking.Domain.Abstractions;
using Booking.Domain.Users.Events;
using Booking.Domain.Users.ValueObjects;

namespace Booking.Domain.Users
{
    public sealed class User : Entity<UserId>
    {
        private User(UserId Id,
                    FirstName firstName,
                    LastName lastName,
                    Email email)
            : base(Id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        private User()
        {
        }
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public string IdentityId { get; private set; } = string.Empty;

        public static User Create(FirstName firstName,
                                  LastName lastName,
                                  Email email)
        {
            var user = new User(UserId.New, firstName, lastName, email);

            user.RaisDomainEvent(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public void SetIdentityId(string identityId)
        {
            IdentityId = identityId;
        }
    }
}
