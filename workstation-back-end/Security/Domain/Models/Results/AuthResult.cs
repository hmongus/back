namespace workstation_back_end.Security.Domain.Models.Results;

public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public Guid Id { get; set; } 
}