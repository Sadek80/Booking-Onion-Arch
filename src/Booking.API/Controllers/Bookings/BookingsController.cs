using Booking.ApplicationServices.Abstractions.Services;
using Booking.Contracts.Bookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers.Bookings;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ApiController
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await _bookingService.GetBookingAsync(query, cancellationToken);

        return result.Match(success => Ok(result.Value),
                            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await _bookingService.ReserveBookingAsync(command, cancellationToken);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}
