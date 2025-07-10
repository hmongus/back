namespace workstation_back_end.Bookings.Domain.Models.Commands;

/**
 * <summary>
 * Comando específico para actualizar el estado de una reserva.
 * </summary>
 * <param name="BookingId">ID de la reserva que se va a modificar.</param>
 * <param name="NewStatus">El nuevo estado (ej. "Completada", "Cancelada").</param>
 */
public record UpdateBookingStatusCommand(int BookingId, string NewStatus);