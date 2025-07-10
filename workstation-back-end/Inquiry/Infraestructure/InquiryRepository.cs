using Microsoft.EntityFrameworkCore;
using workstation_back_end.Inquiry.Domain.Services;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

namespace workstation_back_end.Inquiry.Infraestructure;

public class InquiryRepository: BaseRepository<Domain.Models.Entities.Inquiry>, IInquiryRepository
{
    public InquiryRepository(TripMatchContext context) : base(context) { }

    public async Task<IEnumerable<Domain.Models.Entities.Inquiry>> FindByExperienceIdAsync(int experienceId)
    {
        return await Context.Inquiries.Where(i => i.ExperienceId == experienceId).ToListAsync();
    }
}