using workstation_back_end.Inquiry.Domain.Models.Queries;
using workstation_back_end.Inquiry.Domain.Services;
using workstation_back_end.Inquiry.Domain.Services.Services;

namespace workstation_back_end.Inquiry.Application.QueryServices;

public class InquiryQueryService : IInquiryQueryService
{
    private readonly IInquiryRepository _repository;

    public InquiryQueryService(IInquiryRepository repo) => _repository = repo;

    public async Task<IEnumerable<Domain.Models.Entities.Inquiry>> Handle(GetAllInquiriesQuery query)
    {
        return await _repository.ListAsync();
    }

    public async Task<IEnumerable<Domain.Models.Entities.Inquiry>> Handle(GetAllInquiriesByExperienceQuery query)
    {
        return await _repository.FindByExperienceIdAsync(query.ExperienceId);
    }    
}