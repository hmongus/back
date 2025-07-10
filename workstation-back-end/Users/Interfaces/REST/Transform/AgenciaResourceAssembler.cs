using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Users.Interfaces.REST.Resources;

namespace workstation_back_end.Users.Interfaces.REST.Transform;


public static class AgencyResourceAssembler
{
    public static AgencyResource ToResource(Agency agency)
    {
        return new AgencyResource
        {
            UserId = agency.UserId,
            AgencyName = agency.AgencyName,
            Ruc = agency.Ruc,
            Description = agency.Description,
            Rating = agency.Rating,
            ReviewCount = agency.ReviewCount,
            ReservationCount = agency.ReservationCount,
            AvatarUrl = agency.AvatarUrl,
            ContactEmail = agency.ContactEmail,
            ContactPhone = agency.ContactPhone,
            SocialLinkFacebook = agency.SocialLinkFacebook,
            SocialLinkInstagram = agency.SocialLinkInstagram,
            SocialLinkWhatsapp = agency.SocialLinkWhatsapp
        };
    }
}