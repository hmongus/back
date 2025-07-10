using workstation_back_end.Security.Domain.Models.Commands;
using workstation_back_end.Security.Domain.Models.Results;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Security.Domain.Services;

public interface IAuthService
{
    Task<User> SignUpAsync(SignUpCommand command);
    Task<AuthResult> SignInAsync(SignInCommand command);
}