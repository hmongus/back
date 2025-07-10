namespace workstation_back_end.Reviews.Domain.Models.Queries;

/**
 * <summary>
 * Consulta para obtener todas las reseñas de una agency específica.
 * </summary>
 * <param name="AgencyId">El ID de la agency cuyas reseñas se quieren obtener.</param>
 */
public record GetReviewsByAgencyIdQuery(Guid AgencyUserId);