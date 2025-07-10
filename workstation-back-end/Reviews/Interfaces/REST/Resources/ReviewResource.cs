namespace workstation_back_end.Reviews.Interfaces.REST.Resources;

/**
 * <summary>
 * Recurso que representa una reseña para las respuestas de la API.
 * </summary>
 * <param name="Id">ID de la reseña.</param>
 * <param name="Rating">Calificación dada.</param>
 * <param name="Comment">Comentario.</param>
 * <param name="Date">Fecha de la reseña.</param>
 * <param name="TouristId">ID del tourist que escribió la reseña.</param>
 * <param name="TouristName">Nombre del tourist (para conveniencia).</param>
 * <param name="TouristAvatarUrl">URL del avatar del tourist (para conveniencia).</param>
 * <param name="AgencyId">ID de la agency reseñada.</param>
 */
public record ReviewResource(
    int Id,
    int Rating,
    string Comment,
    DateTime ReviewDate,        
    Guid TouristUserId,       
    string TouristName,
    string? TouristAvatarUrl,
    Guid AgencyUserId   
    );