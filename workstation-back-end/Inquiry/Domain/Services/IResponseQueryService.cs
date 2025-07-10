using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Inquiry.Domain.Models.Queries;

namespace workstation_back_end.Inquiry.Domain.Services.Services;

public interface  IResponseQueryService
{
    Task<Response?> Handle(GetResponseByInquiryIdQuery query);
}