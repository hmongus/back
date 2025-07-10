namespace workstation_back_end.Inquiry.Interfaces.REST.Resources;

public record ResponseResource(int Id, int InquiryId, Guid ResponderId, string Answer, DateTime AnsweredAt);