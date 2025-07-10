using workstation_back_end.Inquiry.Interfaces.REST.Resources;

namespace workstation_back_end.Inquiry.Interfaces.REST.Transform;

public class InquiryResourceFromEntityAssembler
{
    public static InquiryResource ToResourceFromEntity(Domain.Models.Entities.Inquiry entity)
    {
        return new InquiryResource(
            entity.Id,
            entity.ExperienceId,
            entity.UserId,
            entity.Question,
            entity.AskedAt);
    }
}