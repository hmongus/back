namespace workstation_back_end.Inquiry.Domain.Models.Commands;

public class CreateResponseCommand
{
    public int InquiryId { get; set; }
    public Guid ResponderId { get; set; }
    public string Answer { get; set; } = null!;
    public DateTime AnsweredAt { get; set; }
}