using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Inquiry.Domain.Services;

public interface IInquiryRepository : IBaseRepository<Domain.Models.Entities.Inquiry>
{
    Task<IEnumerable<Domain.Models.Entities.Inquiry>> FindByExperienceIdAsync(int experienceId);
}