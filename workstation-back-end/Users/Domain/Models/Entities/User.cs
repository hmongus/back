using workstation_back_end.Shared.Domain.Model.Entities;

namespace workstation_back_end.Users.Domain.Models.Entities;

/// <summary>
/// Representa a un user base en TripMatch. Puede ser una agency o un tourist.
/// </summary>
public class User : BaseEntity
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Tourist? Tourist { get; set; }

    public Agency? Agency { get; set; }
}