using workstation_back_end.Experience.Domain.Models.Commands;

namespace workstation_back_end.Experience.Domain.Services;

public interface IExperienceCommandService
{
    Task<Models.Entities.Experience> Handle(CreateExperienceCommand command);
    Task<Models.Entities.Experience> Handle(UpdateExperienceCommand command);
    Task<bool> Handle(DeleteExperienceCommand command);
}