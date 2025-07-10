using workstation_back_end.Favorites.Domain.Models.Commands;
using workstation_back_end.Favorites.Domain.Models.Entities;

namespace workstation_back_end.Favorites.Domain.Services
{
    public interface IFavoriteCommandService
    {
        Task<Favorite?> Handle(CreateFavoriteCommand command);
        Task<bool> Handle(DeleteFavoriteCommand command);
    }
}