using Booking.ApplicationServices.Abstractions.Authentication;
using Booking.ApplicationServices.Abstractions.Data;
using Booking.ApplicationServices.Abstractions.Services;
using Booking.Contracts.Users;
using Booking.Domain.Abstractions;
using Booking.Domain.Users.Repositories;
using Booking.Domain.Users.ValueObjects;
using Booking.Domain.Users;
using Dapper;
using ErrorOr;
using static Booking.Domain.Users.DomainErrors;
using Booking.ApplicationServices.Abstractions.Validators;

namespace Booking.ApplicationServices.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IUserContext _userContext;
        private readonly IJwtService _jwtService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserValidator _userValidator;

        public UserService(ISqlConnectionFactory sqlConnectionFactory,
                           IUserContext userContext,
                           IJwtService jwtService,
                           IAuthenticationService authenticationService,
                           IUserRepository userRepository,
                           IUnitOfWork unitOfWork,
                           IUserValidator userValidator)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _userContext = userContext;
            _jwtService = jwtService;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public async Task<ErrorOr<UserResponse>> GetUserAsync(CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                "Id" AS Id,
                "FirstName" AS FirstName,
                "LastName" AS LastName,
                "Email" AS Email
            FROM "User"
            WHERE "IdentityId" = @IdentityId
            """;

            var user = await connection.QuerySingleAsync<UserResponse>(
                sql,
                new
                {
                    _userContext.IdentityId
                });

            return user;
        }

        public async Task<ErrorOr<AccessTokenResponse>> LoginUserAsync(LogInUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _jwtService.GetAccessTokenAsync(
                       request.Email,
                       request.Password,
                       cancellationToken);

            if (result.IsError)
            {
                return DomainError.Failure(UserErrors.InvalidCredentials);
            }

            return new AccessTokenResponse(result.Value);
        }

        public async Task<ErrorOr<RegisterUserResponse>> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validationErrors = _userValidator.ValidateRegisterUser(request);

            if(validationErrors != null && validationErrors.Count != 0)
            {
                return validationErrors;
            }

            var user = User.Create(
                        new FirstName(request.FirstName),
                        new LastName(request.LastName),
                        new Email(request.Email));

            var identityId = await _authenticationService.RegisterAsync(user,
                                                                        request.Password,
                                                                        cancellationToken);

            user.SetIdentityId(identityId);

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return new RegisterUserResponse(user.Id);
        }
    }
}
