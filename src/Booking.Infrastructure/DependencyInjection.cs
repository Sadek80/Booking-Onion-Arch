using Booking.Infrastructure;
using Booking.Infrastructure.Authentication;
using Booking.Infrastructure.Authentication.Options;
using Booking.Infrastructure.Repositories.Appartments;
using Booking.Infrastructure.Repositories.Bookings;
using Booking.Infrastructure.Repositories.Users;
using Booking.Infrastructure.Scheduling;
using Booking.Infrastructure.Services;
using Booking.Domain.Abstractions;
using Booking.Domain.Appartments.Repositories;
using Booking.Domain.Bookings.Repositories;
using Booking.Domain.Users.Repositories;
using BookingAP.Infrastructure.Authentication.Options;
using BookingAP.Infrastructure.Clock;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Data.Helpers;
using Dapper;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System.Configuration;
using Booking.ApplicationServices.Abstractions.Clock;
using Booking.ApplicationServices.Abstractions.Services;
using Booking.ApplicationServices.Abstractions.Data;
using Booking.ApplicationServices.Abstractions.Scheduling;
using Booking.ApplicationServices.Abstractions.Authentication;
using BookingAP.Infrastructure.Authentication;

namespace Booking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostBuilder hostBuilder, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddPersistence(configuration);

            services.AddBackgroundJobs(configuration);

            services.AddLogging(hostBuilder, configuration);

            services.AddMapper();

            services.AddAuthentication(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException("Database Connection String is Null");

            services.AddEntityFrameworkNpgsql();

            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(connectionString, npgsqlOptionsAction: options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 10,
                                                 maxRetryDelay: TimeSpan.FromSeconds(30),
                                                 errorCodesToAdd: null);
                });

                options.UseInternalServiceProvider(serviceProvider);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAppartmentRepository, AppartmentRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                                new SqlConnectionFactory(connectionString));

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }

        /// <summary>
        /// Add Background Jobs
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>
        /// <returns>Extended IServiceCollection</returns>
        private static void AddBackgroundJobs(this IServiceCollection services,
                                                               IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException("Database Connection String is Null");

            services.AddHangfire(configurations =>
            {
                configurations.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
            });

            services.AddHangfireServer();

            services.AddTransient<IBackgroundJobService, BackgroundJobService>();
            services.AddTransient<ProcessCoreEventJob>();
        }

        /// <summary>
        /// Add Serilog Logging with SEQ
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>
        /// <param name="hostBuilder">Host Builder</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        private static void AddLogging(this IServiceCollection services, IHostBuilder hostBuilder, IConfiguration configuration)
        {
            hostBuilder.UseSerilog((context, config) =>
            {
                config
                .ReadFrom.Configuration(configuration);
            });
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        }

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
