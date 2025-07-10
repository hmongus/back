using workstation_back_end.Favorites.Domain;
using workstation_back_end.Favorites.Domain.Models.Entities;
using workstation_back_end.Favorites.Domain.Models.Queries;
using workstation_back_end.Favorites.Domain.Services;

namespace workstation_back_end.Favorites.Application.FavoriteQueryService
{
    public class FavoriteQueryService : IFavoriteQueryService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteQueryService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<Favorite>> Handle(GetFavoritesByUserIdQuery query)
        {
            return await _favoriteRepository.FindByTouristIdAsync(query.TouristId);
        }
    }
}