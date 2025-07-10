using FluentValidation;
using workstation_back_end.Experience.Domain.Models.Commands;

namespace workstation_back_end.Experience.Domain.Models.Validators;

public class CreateExperienceCommandValidator  : AbstractValidator<CreateExperienceCommand>
{
    
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 800;
    private const int MaxLocationLength = 60;
    private const int MaxFrequenciesLength = 100;
    private const int MinImagesCount = 1;
    private const int MaxImagesCount = 3;
    public CreateExperienceCommandValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(MaxTitleLength).WithMessage($"Title must be {MaxTitleLength} characters or fewer.");

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(MaxDescriptionLength).WithMessage($"Description must be {MaxDescriptionLength} characters or fewer.");

        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(MaxLocationLength).WithMessage($"Location must be {MaxLocationLength} characters or fewer.");

        RuleFor(e => e.Frequencies)
            .NotEmpty().WithMessage("At least one frequency is required.")
            .MaximumLength(MaxFrequenciesLength).WithMessage($"Frequencies must be {MaxFrequenciesLength} characters or fewer.");

        RuleFor(e => e.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0.");

        RuleFor(e => e.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");

        RuleFor(e => e.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(e => e.Schedules)
            .NotNull().WithMessage("At least one schedule is required.")
            .Must(s => s.Any()).WithMessage("At least one schedule is required.");

        RuleForEach(e => e.Schedules)
            .ChildRules(schedule =>
            {
                schedule.RuleFor(s => s.Time).NotEmpty().WithMessage("Schedule time is required.");
            });

        RuleFor(e => e.ExperienceImages)
            .NotNull().WithMessage("At least one image is required.")
            .Must(images => images.Count >= MinImagesCount && images.Count <= MaxImagesCount)
            .WithMessage($"You must provide between {MinImagesCount} and {MaxImagesCount} images.");

        RuleForEach(e => e.ExperienceImages)
            .ChildRules(img =>
            {
                img.RuleFor(i => i.Url).NotEmpty().WithMessage("Image URL is required.");
            });

        RuleForEach(e => e.Includes)
            .ChildRules(inc =>
            {
                inc.RuleFor(i => i.Description).NotEmpty().WithMessage("Include description is required.");
            });
    }
}