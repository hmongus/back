namespace workstation_back_end.Security.Interfaces.REST.Resources;

public class AuthResponseResource
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    
    public string Id { get; set; }
}