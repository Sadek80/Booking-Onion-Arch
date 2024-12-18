using System.Data;

namespace Booking.ApplicationServices.Abstractions.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}