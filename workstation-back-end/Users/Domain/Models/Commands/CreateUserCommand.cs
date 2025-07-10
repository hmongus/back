namespace workstation_back_end.Users.Domain.Models.Commands;



public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Number,
    string Email,
    string Password,

    // Rol puede ser "agency" o "tourist"
    string Rol,

    // Datos opcionales para Agency
    string? AgencyName,
    string? Ruc,
    string? Description,
    string? ContactEmail,
    string? ContactPhone,
    string? SocialLinkFacebook,
    string? SocialLinkInstagram,
    string? SocialLinkWhatsapp,

    // Datos opcionales para Tourist
    string? AvatarUrl
);
