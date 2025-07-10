using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Services;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Inquiry.Application.CommandServices;

public class InquiryCommandService : IInquiryCommandService
{
    private readonly IInquiryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateInquiryCommand> _validator;

    public InquiryCommandService(IInquiryRepository repo, IUnitOfWork unit, IValidator<CreateInquiryCommand> val)
    {
        _repository = repo;
        _unitOfWork = unit;
        _validator = val;
    }

    public async Task<Domain.Models.Entities.Inquiry> Handle(CreateInquiryCommand command)
    {
        var validation = await _validator.ValidateAsync(command);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var inquiry = new Domain.Models.Entities.Inquiry
        {
            ExperienceId = command.ExperienceId,
            UserId = command.UserId,
            Question = command.Question,
            AskedAt = command.AskedAt,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _repository.AddAsync(inquiry);
        await _unitOfWork.CompleteAsync();
        return inquiry;
    }
}