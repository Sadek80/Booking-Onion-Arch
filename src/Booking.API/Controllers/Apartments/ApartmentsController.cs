using Booking.ApplicationServices.Abstractions.Services;
using Booking.Contracts.Appartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Apartments
{
    [ApiController]
    [Route("api/apartments")]
    public class ApartmentsController : ApiController
    {
        private readonly IAppartmentService _appartmentService;

        public ApartmentsController(IAppartmentService appartmentService)
        {
            _appartmentService = appartmentService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchApartments(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken)
        {
            var query = new SearchApartmentsQuery(startDate, endDate);

            var result = await _appartmentService.SearchAppartments(query, cancellationToken);

            return result.Match(success => Ok(result.Value),
                                Problem);
        }
    }
}