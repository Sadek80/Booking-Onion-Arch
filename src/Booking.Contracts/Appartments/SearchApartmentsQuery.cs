namespace Booking.Contracts.Appartments;

public sealed record SearchApartmentsQuery(DateOnly StartDate,
                                           DateOnly EndDate);