using workstation_back_end.Shared.Domain.Model.Entities;
using workstation_back_end.Users.Domain.Models.Entities;
using ExperienceEntity = workstation_back_end.Experience.Domain.Models.Entities.Experience;
namespace workstation_back_end.Favorites.Domain.Models.Entities;

/// <summary>
/// Entidad que representa un favorito, conectando un turista con una experiencia.
/// </summary>
public class Favorite : BaseEntity
{
    public int Id { get; set; }

    public Guid TouristId { get; set; }
    public int ExperienceId { get; set; }

    public Favorite() { }
    
    public Tourist Tourist { get; set; } = null!;
    
    public ExperienceEntity Experience { get; set; } = null!;

    public Favorite(Guid touristId, int experienceId)
    {
        TouristId = touristId;
        ExperienceId = experienceId;
    }
}