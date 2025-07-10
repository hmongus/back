using workstation_back_end.Shared.Domain.Model.Entities;
using workstation_back_end.Users.Domain.Models.Entities;

namespace workstation_back_end.Inquiry.Domain.Models.Entities;

public class Response : BaseEntity
{
    public int Id { get; set; }
    public int InquiryId { get; set; }
    public Inquiry Inquiry { get; set; }

    public Guid ResponderId { get; set; }               
    public User Responder { get; set; }            

    public string Answer { get; set; } = string.Empty;
    public DateTime AnsweredAt { get; set; }
}