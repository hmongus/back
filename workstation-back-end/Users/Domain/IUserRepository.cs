using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Users.Domain;


public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByGuidAsync(Guid userId);
    Task AddAgencyAsync(Agency agency);
    Task AddTouristAsync(Tourist tourist);
    Task<User?> FindByEmailAsync(string email);
    void UpdateAgency(Agency agency);
    void UpdateTourist(Tourist tourist);
    void Remove(User entity);
}