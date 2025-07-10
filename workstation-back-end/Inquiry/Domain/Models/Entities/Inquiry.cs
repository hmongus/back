using workstation_back_end.Shared.Domain.Model.Entities;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Inquiry.Domain.Models.Entities;

public class Inquiry : BaseEntity
{
    public int Id { get; set; }
    public int ExperienceId { get; set; }
    public Experience.Domain.Models.Entities.Experience Experience { get; set; }

    public Guid UserId { get; set; } 
    public User User { get; set; }
    
    public string Question { get; set; } = string.Empty;

    public DateTime? AskedAt { get; set; }
    public Response? Response { get; set; }
}