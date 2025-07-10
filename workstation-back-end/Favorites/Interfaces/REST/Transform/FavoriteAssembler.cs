using workstation_back_end.Favorites.Domain.Models.Entities;
using workstation_back_end.Favorites.Interfaces.REST.Resources;
using workstation_back_end.Experience.Interfaces.REST.Resources;
using workstation_back_end.Experience.Interfaces.REST.Transform;

namespace workstation_back_end.Favorites.Interfaces.REST.Transform;

/// <summary>
/// Clase para transformar entidades de favorito a recursos REST.
/// </summary>
public static class FavoriteAssembler
{
    public static FavoriteResource ToResourceFromEntity(Favorite entity)
    {
        ExperienceResource experienceResource = null;
        if (entity.Experience is not null)
        {
            experienceResource = ExperienceResourceFromEntityAssembler.ToResourceFromEntity(entity.Experience);
        }

        return new FavoriteResource(
            entity.Id,
            entity.TouristId,
            entity.ExperienceId,
            experienceResource
        );
    }
}