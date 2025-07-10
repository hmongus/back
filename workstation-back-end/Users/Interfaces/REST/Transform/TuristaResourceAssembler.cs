using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Users.Interfaces.REST.Resources;

namespace workstation_back_end.Users.Interfaces.REST.Transform;


public static class TouristResourceAssembler
{
    public static TouristResource ToResource(Tourist tourist)
    {
        return new TouristResource
        {
            UserId = tourist.UserId,
            AvatarUrl = tourist.AvatarUrl
        };
    }
}