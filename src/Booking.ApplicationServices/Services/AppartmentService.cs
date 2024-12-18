using Booking.ApplicationServices.Abstractions.Data;
using Booking.ApplicationServices.Abstractions.Services;
using Booking.Contracts.Appartments;
using Booking.Domain.Bookings.Enums;
using Dapper;
using ErrorOr;

namespace Booking.ApplicationServices.Services
{
    internal sealed class AppartmentService : IAppartmentService
    {
        private static readonly int[] ActiveBookingStatuses =
        {
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed
        };

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AppartmentService(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<ErrorOr<IReadOnlyList<ApartmentResponse>>> SearchAppartments(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return new List<ApartmentResponse>();
            }

            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
            SELECT
                a."Id" AS Id,
                a."Name" AS Name,
                a."Description" AS Description,
                a."Price_Amount" AS Price,
                a."Price_Currency" AS Currency,
                a."Address_Country" AS Country,
                a."Address_State" AS State,
                a."Address_ZipCode" AS ZipCode,
                a."Address_City" AS City,
                a."Address_Street" AS Street
            FROM "Appartment" AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM "Booking" AS b
                WHERE
                    b."AppartmentId" = a."Id" AND
                    b."Duration_Start" <= @EndDate AND
                    b."Duration_End" >= @StartDate AND
                    b."Status" = ANY(@ActiveBookingStatuses)
            )
            """;

            var apartments = await connection
                .QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
                    sql,
                    (apartment, address) =>
                    {
                        apartment.Address = address;

                        return apartment;
                    },
                    new
                    {
                        request.StartDate,
                        request.EndDate,
                        ActiveBookingStatuses
                    },
                    splitOn: "Country");

            return apartments.ToList();
        }
    }
}
