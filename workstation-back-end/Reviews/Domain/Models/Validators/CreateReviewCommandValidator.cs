using FluentValidation;
using workstation_back_end.Reviews.Domain.Models.Commands;

namespace workstation_back_end.Reviews.Domain.Models.Validators;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    private const int MaxCommentLength = 200;
    private const int MinRating = 1;
    private const int MaxRating = 5;
    public CreateReviewCommandValidator()
    {
        RuleFor(command => command.TouristUserId)
            .NotEmpty().WithMessage("TouristUserId is required.");

        RuleFor(command => command.AgencyUserId)
            .NotEmpty().WithMessage("AgencyUserId is required.")
            .NotEqual(command => command.TouristUserId)
            .WithMessage("A user cannot review their own agency.");

        RuleFor(command => command.Rating)
            .InclusiveBetween(MinRating, MaxRating)
            .WithMessage($"Rating must be between {MinRating} and {MaxRating}.");

        RuleFor(command => command.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(MaxCommentLength)
            .WithMessage($"Comment cannot exceed {MaxCommentLength} characters.")
            .Must(comment => !comment.Contains("http://") && !comment.Contains("https://"))
            .WithMessage("Links are not allowed in the comment.")
            .Must(comment => !comment.Contains("<script>"))
            .WithMessage("Scripts or unsafe content are not allowed.");

        RuleFor(command => command)
            .Must(c => !string.IsNullOrWhiteSpace(c.Comment) || c.Rating < 3)
            .WithMessage("Low ratings must include a meaningful comment.");
    }
}