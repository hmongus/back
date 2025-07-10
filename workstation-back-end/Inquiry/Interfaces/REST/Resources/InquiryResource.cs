namespace workstation_back_end.Inquiry.Interfaces.REST.Resources;

public record InquiryResource(int Id, int ExperienceId, Guid UserId, string? Question, DateTime? AnsweredAt);