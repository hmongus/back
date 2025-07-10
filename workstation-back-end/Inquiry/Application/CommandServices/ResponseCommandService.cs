using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Models.Entities;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Inquiry.Application.CommandServices;

public class ResponseCommandService : IResponseCommandService
{
    private readonly IResponseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateResponseCommand> _validator;

    public ResponseCommandService(IResponseRepository repo, IUnitOfWork unit, IValidator<CreateResponseCommand> val)
    {
        _repository = repo;
        _unitOfWork = unit;
        _validator = val;
    }

    public async Task<Response> Handle(CreateResponseCommand command)
    {
        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var response = new Response
        {
            InquiryId = command.InquiryId,
            ResponderId = command.ResponderId,
            Answer = command.Answer,
            AnsweredAt = command.AnsweredAt,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        await _repository.AddAsync(response);
        await _unitOfWork.CompleteAsync();
        return response;
    }   
}