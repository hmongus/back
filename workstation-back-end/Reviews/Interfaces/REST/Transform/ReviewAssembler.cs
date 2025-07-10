using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Reviews.Interfaces.REST.Resources;

namespace workstation_back_end.Reviews.Interfaces.REST.Transform;

public static class ReviewAssembler
{
    public static ReviewResource ToResourceFromEntity(Review entity)
    {
        var touristName = entity.TouristUser != null ? $"{entity.TouristUser.FirstName} {entity.TouristUser.LastName}" : "User Anónimo"; 
        var touristAvatarUrl = entity.TouristUser?.Tourist?.AvatarUrl; 

        return new ReviewResource(
            entity.Id,
            entity.Rating,
            entity.Comment,
            entity.ReviewDate,      
            entity.TouristUserId,  
            touristName,
            touristAvatarUrl,
            entity.AgencyUserId   
        );
    }
}