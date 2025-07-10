using workstation_back_end.Bookings.Domain.Models.Entities;
using workstation_back_end.Bookings.Interfaces.REST.Resources;
using workstation_back_end.Experience.Interfaces.REST.Resources;
using workstation_back_end.Experience.Interfaces.REST.Transform;

namespace workstation_back_end.Bookings.Interfaces.REST.Transform;

/**
 * <summary>
 * Clase estática para transformar la entidad Booking a su recurso correspondiente.
 * </summary>
 */
public static class BookingAssembler
{
    public static BookingResource ToResourceFromEntity(Booking entity)
    {

        ExperienceResource experienceResource = null;
        if (entity.Experience != null)
        {
            experienceResource = ExperienceResourceFromEntityAssembler.ToResourceFromEntity(entity.Experience); 
        }

        return new BookingResource(
            entity.Id,
            entity.BookingDate,
            entity.NumberOfPeople,
            entity.Price,
            entity.Status,
            entity.ExperienceId,
            entity.TouristId,
            entity.Time,
            experienceResource 
        );
    }
}