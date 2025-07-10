namespace workstation_back_end.Security.Domain.Models.Commands;

/// <summary>
/// Comando para registrar un nuevo user (Tourist o Agency)
/// </summary>
public class SignUpCommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; } // "tourist" o "agency"

    // Solo para Agencies
    public string? AgencyName { get; set; }
    public string? Ruc { get; set; }
}