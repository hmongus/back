using Microsoft.EntityFrameworkCore;
using workstation_back_end.Favorites.Domain;
using workstation_back_end.Favorites.Domain.Models.Entities;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
namespace workstation_back_end.Favorites.Infrastructure
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        private readonly TripMatchContext _context;

        public FavoriteRepository(TripMatchContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Favorite?> FindByTouristIdAndExperienceIdAsync(Guid touristId, int experienceId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.TouristId == touristId && f.ExperienceId == experienceId);
        }

        public async Task<IEnumerable<Favorite>> FindByTouristIdAsync(Guid touristId)
        {
            return await _context.Favorites
                .Include(f => f.Experience)
                .Where(f => f.TouristId == touristId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid touristId, int experienceId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.TouristId == touristId && f.ExperienceId == experienceId);
        }
    }
}