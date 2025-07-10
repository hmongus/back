using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;

namespace workstation_back_end.Inquiry.Domain.Services.Models.Validators;

public class CreateInquiryCommandValidator : AbstractValidator<CreateInquiryCommand>
{
    private const int MaxQuestionLength = 500;
    public CreateInquiryCommandValidator()
    {
        RuleFor(i => i.Question)
            .NotEmpty().WithMessage("Question is required.")
            .MaximumLength(MaxQuestionLength).WithMessage($"Question must be {MaxQuestionLength} characters or fewer.")
            .Must(q => !q.Contains("http://") && !q.Contains("https://"))
            .WithMessage("Links are not allowed in the question.")
            .Must(q => !q.Contains("<script>"))
            .WithMessage("Scripts are not allowed in the question.");

        RuleFor(i => i.ExperienceId)
            .GreaterThan(0).WithMessage("A valid Experience ID is required.");

        RuleFor(i => i.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(i => i.AskedAt)
            .NotEmpty().WithMessage("Question date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Question date cannot be in the future.");
    }
}