using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Interfaces.REST.Resources;
using workstation_back_end.Users.Interfaces.REST.Resources;
using workstation_back_end.Users.Interfaces.REST.Transform;

namespace workstation_back_end.Experience.Interfaces.REST.Transform;

public static class ExperienceResourceFromEntityAssembler
{
    public static ExperienceResource ToResourceFromEntity(Domain.Models.Entities.Experience experience)
    {
        List<ExperienceImageResource> images = new List<ExperienceImageResource>();
        List<IncludeResource> includes = new List<IncludeResource>();
        List<ScheduleResource> schedule = new List<ScheduleResource>();

        foreach (var experienceImage in experience.ExperienceImages)
        {
            images.Add(new ExperienceImageResource(experienceImage.Url));
        }
        foreach (var experienceInclude in experience.Includes)
        {
            includes.Add(new IncludeResource(experienceInclude.Description));
        } 
        
        foreach (var experienceSchedule in experience.Schedules)
        {
            schedule.Add(new ScheduleResource(experienceSchedule.Time));
        } 
        
        CategoryResource categoryResource = null;
        if (experience.Category != null)
        {
            categoryResource = CategoryResourceFromEntityAssembler.ToResourceFromEntity(experience.Category); 
        }

        AgencyResource agencyResource = null;
        if (experience.Agency != null)
        {
            agencyResource = AgencyResourceAssembler.ToResource(experience.Agency); 
        }
        
        return new ExperienceResource(
            experience.Id,
            experience.Title,
            experience.Description,
            experience.Location,
            experience.Duration,
            experience.Price,
            experience.Frequencies,
            experience.CategoryId,
            images,
            includes,
            schedule, 
            categoryResource, 
            agencyResource   
        );
    }
    
}