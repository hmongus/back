using workstation_back_end.Shared.Domain.Model.Entities;

namespace workstation_back_end.Experience.Domain.Models.Entities;

public class Category 
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Experience> Experiences { get; set; } = new();
}