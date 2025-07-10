using workstation_back_end.Inquiry.Domain.Models.Commands;

namespace workstation_back_end.Inquiry.Domain.Services.Services;

public interface IInquiryCommandService
{
    Task<Domain.Models.Entities.Inquiry> Handle(CreateInquiryCommand command);
}