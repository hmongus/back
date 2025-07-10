using workstation_back_end.Shared.Domain.Model.Entities;

namespace workstation_back_end.Experience.Domain.Models.Entities {

    public class Experience : BaseEntity
    {
        
        public Experience(string title, string description, string location, int duration,
            decimal price, string frequencies, int categoryId, Guid agencyUserId)
        {
            Title = title;
            Description = description;
            Location = location;
            Duration = duration;
            Price = price;
            Frequencies = frequencies;
            CategoryId = categoryId;
            AgencyUserId = agencyUserId;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;

            ExperienceImages = new List<ExperienceImage>();
            Includes = new List<Include>();
            Schedules = new List<Schedule>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Frequencies { get; set; }
        
        public int Id { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public Guid AgencyUserId { get; set; } 
        public Users.Domain.Models.Entities.Agency Agency { get; set; } 
        public List<Schedule> Schedules { get; } = new();
        public List<ExperienceImage> ExperienceImages { get; } = new();
        public List<Include> Includes { get; } = new();
    }
};