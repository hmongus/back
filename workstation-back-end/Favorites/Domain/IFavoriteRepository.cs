using workstation_back_end.Favorites.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Favorites.Domain
{
    public interface IFavoriteRepository : IBaseRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> FindByTouristIdAsync(Guid touristId);
        Task<bool> ExistsAsync(Guid touristId, int experienceId);
        
        Task<Favorite?> FindByTouristIdAndExperienceIdAsync(Guid touristId, int experienceId);
    }
}