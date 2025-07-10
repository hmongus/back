using workstation_back_end.Experience.Interfaces.REST.Resources;

namespace workstation_back_end.Favorites.Interfaces.REST.Resources;

/// <summary>
/// Recurso que representa un favorito para la respuesta de la API.
/// </summary>
/// <param name="Id">ID del favorito.</param>
/// <param name="TouristId">ID del turista que marcó como favorito.</param>
/// <param name="ExperienceId">ID de la experiencia marcada como favorita.</param>
/// <param name="Experience">Información de la experiencia (opcional).</param>
public record FavoriteResource(
    int Id,
    Guid TouristId,
    int ExperienceId,
    ExperienceResource Experience
);