using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Inquiry.Domain.Models.Queries;
using workstation_back_end.Inquiry.Domain.Services.Services;

namespace workstation_back_end.Inquiry.Application.QueryServices;

public class ResponseQueryService : IResponseQueryService
{
    private readonly IResponseRepository _repository;

    public ResponseQueryService(IResponseRepository repo) => _repository = repo;

    public async Task<Response?> Handle(GetResponseByInquiryIdQuery query)
    {
        return await _repository.FindByInquiryIdAsync(query.InquiryId);
    }    
}