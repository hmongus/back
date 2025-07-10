namespace workstation_back_end.Users.Domain.Models.Commands;

public class UpdateAgencyCommand
{
    public string? AgencyName { get; set; }
    public string? Ruc { get; set; }
    public string? Description { get; set; }
    public string? AvatarUrl { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    public string? SocialLinkFacebook { get; set; }
    public string? SocialLinkInstagram { get; set; }
    public string? SocialLinkWhatsapp { get; set; }
    
    public float? Rating { get; set; }                
    public int? ReviewCount { get; set; }         
    public int? ReservationCount { get; set; }    
}