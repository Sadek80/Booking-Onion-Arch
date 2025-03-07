﻿using Booking.Domain.Abstractions;
using Booking.Domain.Bookings.ValueObjects;

namespace Booking.Domain.Bookings.Events
{
    public record BookingCanceledDomianEvent(BookingId BookingId) : IDomainEvent;
}
