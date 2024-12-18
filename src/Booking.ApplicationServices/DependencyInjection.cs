using Booking.ApplicationServices.Abstractions.Services;
using Booking.ApplicationServices.Abstractions.Validators;
using Booking.ApplicationServices.Services;
using Booking.ApplicationServices.Validators.User;
using Booking.Domain.Bookings.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.ApplicationServices
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IAppartmentService, AppartmentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<BookingPricingService>();

            return services;
        }
    }
}
