using FluentValidation;
using workstation_back_end.Bookings.Domain.Models.Commands;
using workstation_back_end.Experience.Domain.Models.Queries;
using workstation_back_end.Experience.Domain.Services;

namespace workstation_back_end.Bookings.Domain.Models.Validators;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    private readonly IExperienceQueryService _experienceQueryService;
    private const int MaxPeople = 10;
    private const int MinPeople = 1;

    public CreateBookingCommandValidator(IExperienceQueryService experienceQueryService)
    {
        _experienceQueryService = experienceQueryService;

        RuleFor(x => x.TouristId)
            .NotEmpty().WithMessage("Tourist ID is required.");

        RuleFor(x => x.NumberOfPeople)
            .InclusiveBetween(MinPeople, MaxPeople)
            .WithMessage($"You must book between {MinPeople} and {MaxPeople} people.");

        RuleFor(x => x.BookingDate)
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("Booking date cannot be in the past.");

        RuleFor(x => x.ExperienceId)
            .NotEmpty().WithMessage("Experience ID is required.")
            .MustAsync(ExperienceExists).WithMessage("Experience not found.");

        RuleFor(x => x.time)
            .NotEmpty().WithMessage("Time must be provided.")
            .MustAsync(IsValidTime).WithMessage("The selected time is not valid for this experience.");

        RuleFor(x => x)
            .MustAsync(IsValidFrequency).WithMessage("The selected date is not valid based on the experience frequency.");
    }

    private async Task<bool> ExperienceExists(int id, CancellationToken _)
    {
        var exp = await _experienceQueryService.Handle(new GetExperienceByIdQuery(id));
        return exp != null;
    }
    private async Task<bool> IsValidTime(CreateBookingCommand command, string time, CancellationToken _)
    {
        var exp = await _experienceQueryService.Handle(new GetExperienceByIdQuery(command.ExperienceId));
        if (exp?.Schedules == null) return false;

        var input = time.Trim().ToLowerInvariant();
        return exp.Schedules.Any(s =>
            !string.IsNullOrWhiteSpace(s.Time) &&
            s.Time.Trim().ToLowerInvariant().StartsWith(input)
        );
    }
    private async Task<bool> IsValidFrequency(CreateBookingCommand command, CancellationToken _)
    {
        var experience = await _experienceQueryService.Handle(new GetExperienceByIdQuery(command.ExperienceId));
        if (experience == null) return false;

        var selectedDay = command.BookingDate.DayOfWeek;
        return experience.Frequencies switch
        {
            "weekdays" => selectedDay is >= DayOfWeek.Monday and <= DayOfWeek.Friday,
            "weekends" => selectedDay is DayOfWeek.Saturday or DayOfWeek.Sunday,
            "daily" => true,
            _ => false
        };
    }

}