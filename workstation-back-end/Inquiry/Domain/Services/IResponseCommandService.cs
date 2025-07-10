using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Models.Entities;

namespace workstation_back_end.Inquiry.Domain.Services.Services;

public interface IResponseCommandService
{ 
    Task<Response> Handle(CreateResponseCommand command);  
}