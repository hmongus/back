
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Users.Domain;

namespace workstation_back_end.Experience.Domain.Models.Queries
{
    public class ExperienceQueryService(IExperienceRepository experienceRepository, IUserRepository userRepository) : IExperienceQueryService
    {
        private readonly IExperienceRepository _experienceRepository = experienceRepository;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<IEnumerable<Domain.Models.Entities.Experience>> Handle(GetAllExperiencesQuery query)
        {
            return await experienceRepository.ListAsync();
        }
        public async Task<IEnumerable<Entities.Experience>> Handle(GetExperiencesByCategoryQuery query)
        {
            return await experienceRepository.ListByCategoryIdAsync(query.CategoryId);
        }
        public async Task<Entities.Experience?> Handle(GetExperienceByIdQuery query)
        {
            return await _experienceRepository.FindByIdAsync(query.ExperienceId);
        }
        
        public async Task<IEnumerable<Entities.Experience>> Handle(GetExperiencesByAgencyQuery query)
        {
            var agencyUser = await _userRepository.FindByGuidAsync(query.AgencyUserId);
            if (agencyUser == null || agencyUser.Agency == null)
            {
                return new List<Entities.Experience>(); 
            }
            
            return await _experienceRepository.FindByAgencyUserIdAsync(query.AgencyUserId);
        }
    }
    
}

