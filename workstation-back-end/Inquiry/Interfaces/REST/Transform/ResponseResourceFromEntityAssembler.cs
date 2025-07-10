using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Inquiry.Interfaces.REST.Resources;

namespace workstation_back_end.Inquiry.Interfaces.REST.Transform;

public class ResponseResourceFromEntityAssembler
{
    public static ResponseResource ToResourceFromEntity(Response entity)
    {
        return new ResponseResource(
            entity.Id,
            entity.InquiryId,
            entity.ResponderId,
            entity.Answer,
            entity.AnsweredAt
        );
    }
    
}