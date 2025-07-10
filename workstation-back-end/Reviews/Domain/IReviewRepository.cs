using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Reviews.Domain;

public interface IReviewRepository : IBaseRepository<Review>
{
    /**
     * <summary>
     * Encuentra todas las reseñas escritas sobre una agency específica.
     * </summary>
     * <param name="agencyId">El ID de la agency.</param>
     * <returns>Una colección de reseñas para la agency.</returns>
     */
    Task<IEnumerable<Review>> FindByAgencyUserIdAsync(Guid agencyUserId);

    Task<Review?> FindByAgencyAndTouristUserAsync(Guid agencyUserId, Guid touristUserId);

    Task<IEnumerable<Review>> FindAllReviewsForAgency(Guid agencyUserId); 
    
    Task<Review?> FindByIdAsync(int id);
    
}