using workstation_back_end.Experience.Domain.Models.Queries;

namespace workstation_back_end.Experience.Domain.Services;

public interface IExperienceQueryService
{
    Task<IEnumerable<Models.Entities.Experience>> Handle(GetAllExperiencesQuery query);
    Task<IEnumerable<Models.Entities.Experience>> Handle(GetExperiencesByCategoryQuery query);
    Task<Models.Entities.Experience?> Handle(GetExperienceByIdQuery query);
    Task<IEnumerable<Models.Entities.Experience>> Handle(GetExperiencesByAgencyQuery query);
    
}
