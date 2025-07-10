namespace workstation_back_end.Bookings.Domain.Models.Commands;

/**
 * <summary>
 * Comando para eliminar una reserva.
 * </summary>
 * <param name="BookingId">ID de la reserva a eliminar.</param>
 */
public record DeleteBookingCommand(int BookingId);