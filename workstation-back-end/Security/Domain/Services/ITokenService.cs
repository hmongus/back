using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Security.Domain.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}