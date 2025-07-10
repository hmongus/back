using workstation_back_end.Shared.Domain.Model.Entities;

namespace workstation_back_end.Users.Domain.Models.Entities;


/// <summary>
/// Representa a un user con perfil de tourist.
/// </summary>
public class Tourist : BaseEntity
{
    public Guid UserId { get; set; }

    public string? AvatarUrl { get; set; }

    public User? User { get; set; }
}