namespace workstation_back_end.Reviews.Domain.Models.Commands;

/**
 * <summary>
 * Comando para crear una nueva reseña sobre una agency.
 * </summary>
 * <param name="TouristId">ID del user tourist que escribe la reseña.</param>
 * <param name="AgencyId">ID de la agency que está siendo reseñada.</param>
 * <param name="Rating">Calificación en estrellas (ej. 1-5).</param>
 * <param name="Comment">Comentario de la reseña.</param>
 */
public record CreateReviewCommand(
    Guid TouristUserId,    
    Guid AgencyUserId,
    int Rating,
    string Comment
);