using workstation_back_end.Experience.Domain.Models.Commands;
using workstation_back_end.Users.Interfaces.REST.Resources;

namespace workstation_back_end.Experience.Interfaces.REST.Resources;

public record ExperienceResource (    
    int Id,
    string Title,
    string Description,
    string Location,
    int Duration,
    decimal Price,
    string Frequencies,
    int CategoryId,
    List<ExperienceImageResource> ExperienceImages,
    List<IncludeResource> Includes, 
    List<ScheduleResource> Schedule,
    CategoryResource Category,
    AgencyResource Agency)
{
    
}