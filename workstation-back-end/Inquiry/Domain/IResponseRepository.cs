using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Inquiry.Domain.Services.Services;

public interface IResponseRepository : IBaseRepository<Response>
{
    Task<Response?> FindByInquiryIdAsync(int inquiryId);
}