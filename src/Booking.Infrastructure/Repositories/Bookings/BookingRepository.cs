using AutoMapper;
using AutoMapper.QueryableExtensions;
using Booking.Infrastructure.Repositories;
using Booking.Domain.Appartments;
using Booking.Domain.Bookings;
using Booking.Domain.Bookings.Enums;
using Booking.Domain.Bookings.Repositories;
using Booking.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories.Bookings
{
    internal sealed class BookingRepository : Repository<Domain.Bookings.Booking, BookingId>, IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        private static readonly BookingStatus[] ActiveBookingStatuses =
        {
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
        };


        public BookingRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TResult?> GetBookingDetails<TResult>(BookingId bookingId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Domain.Bookings.Booking>()
                                   .Where(f => f.Id == bookingId)
                                   .AsNoTracking()
                                   .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                                   .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsOverlappingAsync(Appartment apartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await _dbContext
                      .Set<Domain.Bookings.Booking>()
                      .AnyAsync(
                          booking =>
                              booking.AppartmentId == apartment.Id &&
                              booking.Duration.Start <= duration.End &&
                              booking.Duration.End >= duration.Start &&
                              ActiveBookingStatuses.Contains(booking.Status),
                              cancellationToken);
        }
    }
}
