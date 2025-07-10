using workstation_back_end.Users.Domain.Models.Commands;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Users.Domain.Services;


public interface IUserCommandService
{
    Task<User> Handle(CreateUserCommand command);
    Task UpdateAgencyAsync(Guid userId, UpdateAgencyCommand command);
    Task UpdateTouristAsync(Guid userId, UpdateTouristCommand command);
    Task DeleteUserAsync(Guid userId);
    Task<User> Handle(Guid userId, UpdateUserCommand command);

}