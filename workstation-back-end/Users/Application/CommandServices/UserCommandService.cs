using FluentValidation;
using workstation_back_end.Shared.Domain;
using workstation_back_end.Users.Domain;
using workstation_back_end.Users.Domain.Models.Commands;
using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Users.Domain.Models.Validadors;
using workstation_back_end.Users.Domain.Services;
using workstation_back_end.Users.Infrastructure;
using  workstation_back_end.Shared.Domain.Repositories;
namespace workstation_back_end.Users.Application.CommandServices;


public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;

    public UserCommandService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateUserCommand> validator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<User> Handle(CreateUserCommand command)
    {
        var user = new User
        {
            UserId = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Number = command.Number,
            Email = command.Email,
            Password = command.Password,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        if (command.Rol == "agency")
        {
            var agency = new Agency
            {
                UserId = user.UserId,
                AgencyName = command.AgencyName ?? "",
                Ruc = command.Ruc ?? "",
                Description = command.Description ?? "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAgencyAsync(agency);
        }
        else if (command.Rol == "tourist")
        {
            var tourist = new Tourist
            {
                UserId = user.UserId,
                AvatarUrl = command.AvatarUrl ?? "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddTouristAsync(tourist);
        }

        await _unitOfWork.CompleteAsync();
        return user;
    }
    
    public async Task UpdateAgencyAsync(Guid userId, UpdateAgencyCommand command)
    {
        var user = await _userRepository.FindByGuidAsync(userId);

        if (user == null || user.Agency == null)
            throw new Exception("No se encontró el user o la agency.");

        user.Agency.AgencyName = command.AgencyName;
        user.Agency.Ruc = command.Ruc;
        user.Agency.Description = command.Description;
        if (command.Rating.HasValue)
            user.Agency.Rating = command.Rating.Value;

        if (command.ReviewCount.HasValue)
            user.Agency.ReviewCount = command.ReviewCount.Value;

        if (command.ReservationCount.HasValue)
            user.Agency.ReservationCount = command.ReservationCount.Value;
        user.Agency.AvatarUrl = command.AvatarUrl;
        user.Agency.ContactEmail = command.ContactEmail;
        user.Agency.ContactPhone = command.ContactPhone;
        user.Agency.SocialLinkFacebook = command.SocialLinkFacebook;
        user.Agency.SocialLinkInstagram = command.SocialLinkInstagram;
        user.Agency.SocialLinkWhatsapp = command.SocialLinkWhatsapp;

        _userRepository.UpdateAgency(user.Agency);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateTouristAsync(Guid userId, UpdateTouristCommand command)
    {
        var user = await _userRepository.FindByGuidAsync(userId);

        if (user == null || user.Tourist == null)
            throw new Exception("No se encontró el user o el tourist.");

        user.Tourist.AvatarUrl = command.AvatarUrl;

        _userRepository.UpdateTourist(user.Tourist);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.FindByGuidAsync(userId);
        if (user == null)
            throw new Exception("User no encontrado");

        _userRepository.Remove(user);
        await _unitOfWork.CompleteAsync();
    }
    public async Task<User> Handle(Guid userId, UpdateUserCommand command)
    {
        var user = await _userRepository.FindByGuidAsync(userId);
        if (user == null)
        {
            throw new Exception("User no encontrado");
        }

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.Number = command.Number;
        user.ModifiedDate = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();
        return user;
    }
    
}