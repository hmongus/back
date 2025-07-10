using FluentValidation;
using workstation_back_end.Experience.Domain.Models.Queries;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Favorites.Domain;
using workstation_back_end.Favorites.Domain.Models.Commands;
using workstation_back_end.Favorites.Domain.Models.Entities;
using workstation_back_end.Favorites.Domain.Services;
using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Users.Domain.Models.Queries;
using workstation_back_end.Users.Domain.Services;

namespace workstation_back_end.Favorites.Application.FavoriteCommandService;

public class FavoriteCommandService : IFavoriteCommandService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IExperienceQueryService _experienceQueryService;
    private readonly IUserQueryService _userQueryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateFavoriteCommand> _validator;

    public FavoriteCommandService(
        IFavoriteRepository favoriteRepository,
        IExperienceQueryService experienceQueryService,
        IUserQueryService userQueryService,
        IUnitOfWork unitOfWork,
        IValidator<CreateFavoriteCommand> validator)
    {
        _favoriteRepository = favoriteRepository;
        _experienceQueryService = experienceQueryService;
        _userQueryService = userQueryService;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Favorite?> Handle(CreateFavoriteCommand command)
    {
        // Validación del comando
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Verificar existencia del turista
        var user = await _userQueryService.Handle(new GetUserByIdQuery(command.TouristId));
        if (user is null)
        {
            throw new ArgumentException("The specified tourist does not exist.");
        }

        // Verificar existencia de la experiencia
        var experience = await _experienceQueryService.Handle(new GetExperienceByIdQuery(command.ExperienceId));
        if (experience is null)
        {
            throw new ArgumentException("The specified experience does not exist.");
        }
        // Validar si ya existe un favorito con el mismo turista y experiencia
        var existingFavorite = await _favoriteRepository.FindByTouristIdAndExperienceIdAsync(command.TouristId, command.ExperienceId);
        if (existingFavorite != null)
        {
            throw new ApplicationException("This experience is already in your favorites.");
        }
        // Crear favorito
        var favorite = new Favorite(command.TouristId, command.ExperienceId);

        try
        {
            await _favoriteRepository.AddAsync(favorite);
            await _unitOfWork.CompleteAsync();
            return favorite;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating favorite: {ex.Message}");
            throw new ApplicationException("Could not create the favorite due to a database error.");
        }
    }

    public async Task<bool> Handle(DeleteFavoriteCommand command)
    {
        var favorite = await _favoriteRepository.FindByTouristIdAndExperienceIdAsync(command.TouristId, command.ExperienceId);
        if (favorite is null) return false;

        try
        {
            _favoriteRepository.Remove(favorite);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting favorite: {ex.Message}");
            return false;
        }
    }
}