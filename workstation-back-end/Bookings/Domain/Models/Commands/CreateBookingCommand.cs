namespace workstation_back_end.Bookings.Domain.Models.Commands;

/**
 * <summary>
 * Comando para registrar una nueva reserva.
 * Contiene toda la información necesaria que el frontend debe enviar.
 * </summary>
 * <param name="ExperienceId">ID de la experiencia que se está reservando.</param>
 * <param name="BookingDate">Fecha para la cual se hace la reserva.</param>
 * <param name="NumberOfPeople">Cantidad de personas para la reserva.</param>
 */
public record CreateBookingCommand(
    Guid TouristId, 
    int ExperienceId,
    DateTime BookingDate,
    int NumberOfPeople, 
    string time
);