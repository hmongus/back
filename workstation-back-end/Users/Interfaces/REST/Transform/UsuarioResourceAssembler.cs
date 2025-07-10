using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Users.Interfaces.REST.Resources;

namespace workstation_back_end.Users.Interfaces.REST.Transform;


public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResource(User user)
    {
        return new UserResource
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Number = user.Number,
            Email = user.Email,
            EsAgency = user.Agency != null,
            EsTourist = user.Tourist != null
        };
    }
}