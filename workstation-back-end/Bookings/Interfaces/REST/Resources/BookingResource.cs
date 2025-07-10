using workstation_back_end.Experience.Interfaces.REST.Resources;

namespace workstation_back_end.Bookings.Interfaces.REST.Resources;

/**
 * <summary>
 * Recurso que representa una reserva para las respuestas de la API.
 * </summary>
 * <param name="Id">ID de la reserva.</param>
 * <param name="BookingDate">Fecha de la reserva.</param>
 * <param name="NumberOfPeople">Número de personas.</param>
 * <param name="Price">Precio total.</param>
 * <param name="Status">Estado actual de la reserva.</param>
 * <param name="ExperienceId">ID de la experiencia reservada.</param>
 * <param name="ExperienceTitle">Título de la experiencia .</param>
 * <param name="TouristId">ID del tourist que hizo la reserva.</param>
 */
public record BookingResource(
    int Id,
    DateTime BookingDate,
    int NumberOfPeople,
    decimal Price,
    string Status,
    int ExperienceId,
    Guid TouristId,
    string Time,
    ExperienceResource Experience
    
);