using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;

namespace workstation_back_end.Inquiry.Domain.Services.Models.Validators;

public class CreateResponseCommandValidator : AbstractValidator<CreateResponseCommand>
{
    private const int MaxAnswerLength = 500;
    public CreateResponseCommandValidator()
    {
        RuleFor(r => r.InquiryId)
            .GreaterThan(0).WithMessage("A valid Inquiry ID is required.");

        RuleFor(r => r.ResponderId)
            .NotEmpty().WithMessage("Responder ID is required.");

        RuleFor(r => r.Answer)
            .NotEmpty().WithMessage("Answer is required.")
            .MaximumLength(MaxAnswerLength).WithMessage($"Answer must be {MaxAnswerLength} characters or fewer.")
            .Must(a => !a.Contains("<script>"))
            .WithMessage("Scripts are not allowed in the answer.");

        RuleFor(r => r.AnsweredAt)
            .NotEmpty().WithMessage("Answer date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Answer date cannot be in the future.");
    }
}