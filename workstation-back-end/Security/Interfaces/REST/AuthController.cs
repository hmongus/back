using Microsoft.AspNetCore.Mvc;
using workstation_back_end.Security.Domain.Models.Commands;
using workstation_back_end.Security.Domain.Services;
using workstation_back_end.Security.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
namespace workstation_back_end.Security.Interfaces.REST;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/auth/signup
    ///     {
    ///        "firstName": "John",
    ///        "lastName": "Doe",
    ///        "number": "987654321",
    ///        "email": "new.user@example.com",
    ///        "password": "AStrongPassword123!",
    ///        "rol": "agency",
    ///        "agencyName": "Amazing Tours",
    ///        "ruc": "12345678901"
    ///     }
    ///
    /// Notes:
    /// - The role can be "agency" o "tourist"
    /// - If the role is "tourist", those fields can be empty or omitted.
    /// </remarks>
    /// <param name="command">The command with registration data.</param>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">If the input is invalid or the user already exists.</response>
    [AllowAnonymous]
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var user = await _authService.SignUpAsync(command);
        return Ok(new { message = "User registrado con éxito", user.Email });
    }
    
    /// <summary>
    /// Logs in a user and returns a JWT token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/auth/signin
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "YourSecurePassword!"
    ///     }
    ///
    /// </remarks>
    /// <param name="command">The command with login credentials (email and password).</param>
    /// <response code="200">Returns the JWT token and user's email.</response>
    /// <response code="401">Invalid credentials.</response>
    [HttpPost("signin")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var result = await _authService.SignInAsync(command);

        return Ok(new AuthResponseResource
        {
            Token = result.Token,
            Email = result.Email,
            Rol = result.Rol,
            Id = result.Id.ToString()
        });
    }
}