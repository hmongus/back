using workstation_back_end.Inquiry.Domain.Models.Queries;

namespace workstation_back_end.Inquiry.Domain.Services.Services;

public interface IInquiryQueryService
{
    Task<IEnumerable<Domain.Models.Entities.Inquiry>> Handle(GetAllInquiriesQuery query);
    Task<IEnumerable<Domain.Models.Entities.Inquiry>> Handle(GetAllInquiriesByExperienceQuery query);
}