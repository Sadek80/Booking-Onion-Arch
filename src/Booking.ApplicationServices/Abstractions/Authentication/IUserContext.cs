﻿namespace Booking.ApplicationServices.Abstractions.Authentication;

public interface IUserContext
{
    string IdentityId { get; }
}