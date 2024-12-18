using Bogus;
using Booking.ApplicationServices.Abstractions.Data;
using Booking.Domain.Appartments.Enums;
using Booking.Infrastructure;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> apartments = new();
        for (var i = 0; i < 100; i++)
        {
            apartments.Add(new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = "Amazing view",
                Country = faker.Address.Country(),
                State = faker.Address.State(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(50, 1000),
                PriceCurrency = "USD",
                CleaningFeeAmount = faker.Random.Decimal(25, 200),
                CleaningFeeCurrency = "USD",
                Amenities = new List<int> { (int)Amenity.Parking, (int)Amenity.WiFi },
                LastBookedOn = DateTime.MinValue
            });
        }

        const string sql = """
            INSERT INTO public."Appartment"
            ("Id", "Name", "Description", "Address_Country", "Address_State", "Address_ZipCode", "Address_City", "Address_Street", "Price_Amount", "Price_Currency", "CleaningFee_Amount", "CleaningFee_Currency", "Amenities", "LastBookedOnUTC")
            VALUES(@Id, @Name, @Description, @Country, @State, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @Amenities, @LastBookedOn);
            """;

        connection.Execute(sql, apartments);
    }


    //public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    //{
    //    app.UseMiddleware<ExceptionHandlingMiddleware>();
    //}
}
