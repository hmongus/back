using workstation_back_end.Shared.Domain.Model.Entities;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Reviews.Domain.Models.Entities;

public class Review : BaseEntity
{
    public int Id { get; set; }
    public Guid TouristUserId { get; set; } 
    public Guid AgencyUserId { get; set; } 
    public int Rating { get; set; } 
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; } 
    
    public User TouristUser { get; set; } = null!; 
    public Agency Agency { get; set; } = null!; 

    public Review() { }

    public Review(
        Guid touristUserId, 
        Guid agencyUserId, 
        int rating, 
        string comment)
    {
        TouristUserId = touristUserId;
        AgencyUserId = agencyUserId;
        Rating = rating;
        Comment = comment;
        ReviewDate = DateTime.UtcNow;
    }
}
