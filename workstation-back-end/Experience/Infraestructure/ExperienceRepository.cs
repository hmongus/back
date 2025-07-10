using Microsoft.EntityFrameworkCore;
using workstation_back_end.Experience.Domain;
using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Domain.Models.Queries;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

public class ExperienceRepository : BaseRepository<Experience>, IExperienceRepository
{
    private readonly TripMatchContext _context;

    public ExperienceRepository(TripMatchContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Experience>> ListAsync()
    {
        return await _context.Experiences
            .Include(e => e.Agency)
            .Include(e => e.ExperienceImages)
            .Include(e => e.Includes)
            .Include(e => e.Schedules)
            .Include(e => e.Agency)
            .Include(e => e.Category)
            .ToListAsync();
    }
    

    public async Task<IEnumerable<Experience>> ListByCategoryIdAsync(int categoryId)
    {
        return await _context.Set<Experience>()
            .Where(e => e.CategoryId == categoryId && e.IsActive)
            .Include(e => e.Agency) 
            .Include(e => e.ExperienceImages)
            .Include(e => e.Includes)
            .Include(e => e.Schedules)
            .ToListAsync();
    }
    
    public async Task<Experience?> FindByIdAsync(int id)
    {
        return await _context.Experiences
            .Include(e => e.Schedules)
            .Include(e => e.Agency)
            .Include(e => e.ExperienceImages)
            .Include(e => e.Includes)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<IEnumerable<Experience>> FindByAgencyUserIdAsync(Guid agencyUserId)
    {
        return await _context.Experiences
            .Where(e => e.AgencyUserId == agencyUserId && e.IsActive) // Se añadió el filtro IsActive
            .Include(e => e.ExperienceImages) // Incluir datos relacionados
            .Include(e => e.Includes)
            .Include(e => e.Schedules)
            .ToListAsync();
    }
}