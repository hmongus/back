namespace workstation_back_end.Users.Domain.Models.Commands;


public record CreateAgencyCommand(
    Guid UserId,
    string Ruc,
    string Description,
    string LinkFacebook,
    string LinkInstagram
);