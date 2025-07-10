using Microsoft.AspNetCore.Identity;
using workstation_back_end.Security.Domain.Models.Commands;
using workstation_back_end.Security.Domain.Models.Results;
using workstation_back_end.Security.Domain.Services;
using workstation_back_end.Users.Domain;
using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Users.Infrastructure;


namespace workstation_back_end.Security.Application.SecurityCommandServices;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> SignUpAsync(SignUpCommand command)
    {

        var existingUser = await _userRepository.FindByEmailAsync(command.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Este correo electrónico ya está registrado.");

        var hasher = new PasswordHasher<User>();


        var user = new User
        {
            UserId = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Number = command.Number,
            Email = command.Email,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        user.Password = hasher.HashPassword(user, command.Password);
        await _userRepository.AddAsync(user);

        // Crear perfil según rol
        if (command.Rol?.ToLower() == "agency")
        {
            var agency = new Agency
            {
                UserId = user.UserId,
                AgencyName = command.AgencyName ?? "",
                Ruc = command.Ruc ?? "",
                Description = "",
                Rating = 0,
                ReviewCount = 0,
                ReservationCount = 0,
                AvatarUrl = "",
                ContactEmail = "",
                ContactPhone = "",
                SocialLinkFacebook = "",
                SocialLinkInstagram = "",
                SocialLinkWhatsapp = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAgencyAsync(agency);
        }
        else if (command.Rol?.ToLower() == "tourist")
        {
            var tourist = new Tourist
            {
                UserId = user.UserId,
                AvatarUrl = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddTouristAsync(tourist);
        }

        await _unitOfWork.CompleteAsync();
        return user;
    }

    public async Task<AuthResult> SignInAsync(SignInCommand command)
    {
        var user = await _userRepository.FindByEmailAsync(command.Email);
        if (user == null)
            throw new InvalidOperationException("User no encontrado.");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, command.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Contraseña incorrecta.");

        var token = _tokenService.GenerateToken(user);
        
        string rol = user.Agency != null ? "agency" :
            user.Tourist != null ? "tourist" :
            "user";

        return new AuthResult
        {
            Token = token,
            Email = user.Email,
            Rol = rol,
            Id = user.UserId
        };
    }
}