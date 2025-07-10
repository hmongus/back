using Microsoft.EntityFrameworkCore;
using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

namespace workstation_back_end.Inquiry.Infraestructure;

public class ResponseRepository : BaseRepository<Response>, IResponseRepository
{
    public ResponseRepository(TripMatchContext context) : base(context) { }

    public async Task<Response?> FindByInquiryIdAsync(int inquiryId)
    {
        return await Context.Set<Response>()
            .FirstOrDefaultAsync(r => r.InquiryId == inquiryId);
    }
}