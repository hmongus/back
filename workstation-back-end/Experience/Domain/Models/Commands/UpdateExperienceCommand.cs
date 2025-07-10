namespace workstation_back_end.Experience.Domain.Models.Commands;

public record UpdateExperienceCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string Frequencies { get; set; }
    public int CategoryId { get; set; }
}