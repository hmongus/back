using workstation_back_end.Shared.Domain.Model.Entities;

namespace workstation_back_end.Experience.Domain.Models.Entities;

public class Schedule : BaseEntity
{
    public String Time { get; set; }= string.Empty;
    
    public int Id { get; set; }
    public int ExperienceId { get; set; }
    public Experience Experience { get; set; }
}