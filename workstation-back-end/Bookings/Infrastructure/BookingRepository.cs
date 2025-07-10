using Microsoft.EntityFrameworkCore;
using workstation_back_end.Bookings.Domain;
using workstation_back_end.Bookings.Domain.Models.Entities;
using workstation_back_end.Bookings.Domain.Models.Queries;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;
namespace workstation_back_end.Bookings.Infrastructure;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    private readonly TripMatchContext _context;

    public BookingRepository(TripMatchContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> FindByTouristIdAsync(Guid touristId)
    {
        return await _context.Bookings
            .Where(b => b.TouristId == touristId)
            .Include(b => b.Experience) 
                .ThenInclude(e => e.ExperienceImages) 
            .Include(b => b.Experience) 
                .ThenInclude(e => e.Schedules) 
            .Include(b => b.Experience)
                .ThenInclude(e => e.Category) 
            .Include(b => b.Experience)
                .ThenInclude(e => e.Agency)
            .ToListAsync();
    }

    public async Task<Booking?> FindByIdWithExperienceAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Experience)
                .ThenInclude(e => e.ExperienceImages)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Schedules)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Category)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Agency)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public async Task<IEnumerable<Booking>> ListAllWithExperienceAsync()
    {
        return await _context.Bookings
            .Include(b => b.Tourist) 
            .Include(b => b.Experience)
                .ThenInclude(e => e.ExperienceImages)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Schedules)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Category)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Agency)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Booking>> FindByAgencyIdAsync(Guid agencyId)
    {
        return await _context.Bookings
            .Include(b => b.Experience)
                .ThenInclude(e => e.ExperienceImages)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Schedules)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Category)
            .Include(b => b.Experience)
                .ThenInclude(e => e.Agency)
            .Where(b => b.Experience.Agency.UserId == agencyId) 
            .ToListAsync();
    }

    public async Task<bool> HasCompletedBookingForAgencyViaExperienceAsync(Guid touristUserId, Guid agencyUserId)
    {
        return await _context.Bookings
            .Include(b => b.Experience)
            .ThenInclude(e => e.Agency)
            .AnyAsync(
                b => b.TouristId == touristUserId &&
                     b.Experience.Agency.UserId == agencyUserId &&
                     b.Status == "Confirmada"
            );
    }

    
}