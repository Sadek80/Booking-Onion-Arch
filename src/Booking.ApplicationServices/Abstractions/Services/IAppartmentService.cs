using ErrorOr;
using Booking.Contracts.Appartments;

namespace Booking.ApplicationServices.Abstractions.Services
{
    public interface IAppartmentService
    {
        Task<ErrorOr<IReadOnlyList<ApartmentResponse>>> SearchAppartments(SearchApartmentsQuery request, CancellationToken cancellationToken);
    }
}
