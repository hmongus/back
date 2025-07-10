using workstation_back_end.Favorites.Domain.Models.Queries;
using workstation_back_end.Favorites.Domain.Models.Entities;

namespace workstation_back_end.Favorites.Domain.Services
{
    public interface IFavoriteQueryService
    {
        Task<IEnumerable<Favorite>> Handle(GetFavoritesByUserIdQuery query);
    }
}