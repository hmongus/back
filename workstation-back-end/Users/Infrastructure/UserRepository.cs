using Microsoft.EntityFrameworkCore;
using workstation_back_end.Shared.Infraestructure.Persistence.Configuration;
using workstation_back_end.Users.Domain;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Users.Infrastructure;


public class UserRepository : IUserRepository
{
    private readonly TripMatchContext _context;

    public UserRepository(TripMatchContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
    }

    public async Task<User?> FindByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> FindByGuidAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.Tourist)
            .Include(u => u.Agency)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public void Remove(User entity)
    {
        _context.Users.Remove(entity);
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
    }

    public async Task<IEnumerable<User>> ListAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAgencyAsync(Agency agency)
    {
        await _context.Set<Agency>().AddAsync(agency);
    }

    public async Task AddTouristAsync(Tourist tourist)
    {
        await _context.Set<Tourist>().AddAsync(tourist);
    }
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Tourist)
            .Include(u => u.Agency)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public void UpdateAgency(Agency agency)
    {
        _context.Set<Agency>().Update(agency);
    }

    public void UpdateTourist(Tourist tourist)
    {
        _context.Set<Tourist>().Update(tourist);
    }
    
}