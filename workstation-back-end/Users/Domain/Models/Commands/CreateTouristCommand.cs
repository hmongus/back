namespace workstation_back_end.Users.Domain.Models.Commands;


public record CreateTouristCommand(
    Guid UserId,
    int Age,
    string Gender,
    string Language,
    string Preferences
);