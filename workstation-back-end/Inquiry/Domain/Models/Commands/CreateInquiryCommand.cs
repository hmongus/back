namespace workstation_back_end.Inquiry.Domain.Models.Commands;

public class CreateInquiryCommand
{
    public int ExperienceId { get; set; }
    public Guid UserId { get; set; }
    public string Question { get; set; } = null!;
    public DateTime AskedAt { get; set; }
}
