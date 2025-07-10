using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Reviews.Domain.Models.Queries;

namespace workstation_back_end.Reviews.Domain.Services;

public interface IReviewQueryService
{
    /**
     * <summary>
     * Maneja la consulta para obtener todas las reseñas de una agency.
     * </summary>
     * <param name="query">La consulta con el ID de la agency.</param>
     * <returns>Una colección de reseñas.</returns>
     */
    Task<IEnumerable<Review>> Handle(GetReviewsByAgencyIdQuery query);
    Task<Review?> Handle(GetReviewByIdQuery query);
}
