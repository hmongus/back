
using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Shared.Infraestructure.Persistence.Repositories;

namespace workstation_back_end.Experience.Infraestructure;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(TripMatchContext context) : base(context)
    {
    }
}