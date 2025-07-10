namespace workstation_back_end.Users.Domain.Models.Commands;

public record UpdateUserCommand(
    string FirstName,
    string LastName,
    string Number
);