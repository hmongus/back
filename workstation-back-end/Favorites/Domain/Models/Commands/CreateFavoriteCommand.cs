namespace workstation_back_end.Favorites.Domain.Models.Commands;

/**
 * <summary>
 * Comando para crear un favorito.
 * Relaciona a un turista con una experiencia.
 * </summary>
 * <param name="TouristId">ID del user turista que marca como favorito.</param>
 * <param name="ExperienceId">ID de la experiencia marcada como favorita.</param>
 */
public record CreateFavoriteCommand(
    Guid TouristId,
    int ExperienceId
);