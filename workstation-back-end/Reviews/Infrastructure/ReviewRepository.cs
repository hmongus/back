using Microsoft.EntityFrameworkCore;
using workstation_back_end.Reviews.Domain;
using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

namespace workstation_back_end.Reviews.Infrastructure;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    private readonly TripMatchContext _context;
    
    public ReviewRepository(TripMatchContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> FindByAgencyUserIdAsync(Guid agencyUserId)
    {
        return await _context.Reviews
            .Where(r => r.AgencyUserId == agencyUserId)
            .Include(r => r.TouristUser)
            .ThenInclude(u => u.Tourist) 
            .ToListAsync();
    }

    public async Task<Review?> FindByAgencyAndTouristUserAsync(Guid agencyUserId, Guid touristUserId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.AgencyUserId == agencyUserId && r.TouristUserId == touristUserId);
    }

    public async Task<IEnumerable<Review>> FindAllReviewsForAgency(Guid agencyUserId)
    {
        return await _context.Reviews
            .Where(r => r.AgencyUserId == agencyUserId)
            .ToListAsync();
    }
    
    public async Task<Review?> FindByIdAsync(int id)
    {
        return await _context.Reviews
            .Where(r => r.Id == id)
            .Include(r => r.TouristUser)   
            .ThenInclude(u => u.Tourist)   
            .FirstOrDefaultAsync();     
    }
}