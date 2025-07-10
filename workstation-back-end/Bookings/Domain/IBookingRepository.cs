using workstation_back_end.Bookings.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Bookings.Domain;

public interface IBookingRepository : IBaseRepository<Booking>
{
    /**
     * <summary>
     * Encuentra todas las reservas realizadas por un tourist específico.
     * </summary>
     * <param name="touristId">El ID del user tourist.</param>
     * <returns>Una colección de reservas del tourist.</returns>
     */
    Task<IEnumerable<Booking>> FindByTouristIdAsync(Guid  touristId);

    /**
     * <summary>
     * Encuentra todas las reservas asociadas a las experiencias de una agency específica.
     * </summary>
     * <param name="agencyId">El ID del user agency.</param>
     * <returns>Una colección de reservas para la agency.</returns>
     */
    Task<IEnumerable<Booking>> FindByAgencyIdAsync(Guid agencyId);
    
    Task<Booking?> FindByIdWithExperienceAsync(int bookingId);
    Task<IEnumerable<Booking>> ListAllWithExperienceAsync();
    
    Task<bool> HasCompletedBookingForAgencyViaExperienceAsync(Guid touristUserId, Guid agencyUserId);
}