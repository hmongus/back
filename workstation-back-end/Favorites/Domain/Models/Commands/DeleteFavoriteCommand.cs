namespace workstation_back_end.Favorites.Domain.Models.Commands;

/**
 * <summary>
 * Comando para eliminar un favorito existente.
 * </summary>
 * <param name="TouristId">ID del user turista que quiere eliminar el favorito.</param>
 * <param name="ExperienceId">ID de la experiencia que ya no desea como favorita.</param>
 */
public record DeleteFavoriteCommand(
    Guid TouristId,
    int ExperienceId
);