using FluentValidation;
using Microsoft.CodeAnalysis.CodeActions;
using workstation_back_end.Experience.Domain;
using workstation_back_end.Experience.Domain.Models.Commands;
using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Users.Domain.Models.Queries;
using workstation_back_end.Users.Domain.Services;

namespace workstation_back_end.Experience.Application.ExperienceCommandServices;

public class ExperienceCommandService(
    IExperienceRepository experienceRepository,
    IUnitOfWork unitOfWork, 
    ICategoryRepository categoryRepository,
    IUserQueryService userQueryService,
    IValidator<CreateExperienceCommand> validator) : IExperienceCommandService
{
    private readonly IExperienceRepository _experienceRepository =
            experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
    
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    private readonly IUserQueryService _userQueryService = userQueryService;
    private readonly IValidator<CreateExperienceCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    
    public async Task<Domain.Models.Entities.Experience> Handle(CreateExperienceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var category = await _categoryRepository.FindByIdAsync(command.CategoryId);
        if (category == null)
            throw new ArgumentException($"Category with ID {command.CategoryId} does not exist.");


        var user = await _userQueryService.Handle(new GetUserByIdQuery(command.AgencyUserId)); 
        if (user?.Agency == null) 
            throw new ArgumentException($"Agency with User ID {command.AgencyUserId} does not exist or is not an agency.");
        
        var experience = new Domain.Models.Entities.Experience(
            command.Title,
            command.Description,
            command.Location,
            command.Duration,
            command.Price,
            command.Frequencies,
            command.CategoryId,          
            command.AgencyUserId
            );
        
        command.ExperienceImages?.ForEach(img =>
        {
            experience.ExperienceImages.Add(new ExperienceImage
            {
                Url = img.Url,
                Experience = experience
            });
        });
        
        command.Includes?.ForEach(inc =>
        {
            experience.Includes.Add(new Include
            {
                Description = inc.Description,
                Experience = experience
            });
        });
        
        command.Schedules?.ForEach(inc =>
        {
            experience.Schedules.Add(new Schedule
            {
                Time = inc.Time,
                Experience = experience
            });
        });

        await experienceRepository.AddAsync(experience);
        await unitOfWork.CompleteAsync();

        return experience;
    }
    
    public async Task<bool> Handle(DeleteExperienceCommand command)
    {
        var experience = await _experienceRepository.FindByIdAsync(command.ExperienceId);
        if (experience == null) throw new ArgumentException("Experience not found.");

        _experienceRepository.Remove(experience);
        await _unitOfWork.CompleteAsync();
        return true;
    }
    
    public async Task<Domain.Models.Entities.Experience> Handle(UpdateExperienceCommand command)
    {
        var experience = await _experienceRepository.FindByIdAsync(command.Id);
        if (experience == null) throw new ArgumentException("Experience not found.");
        
        experience.Title = command.Title;
        experience.Description = command.Description;
        experience.Location = command.Location;
        experience.Duration = command.Duration;
        experience.Price = command.Price;
        experience.Frequencies = command.Frequencies;
        experience.CategoryId = command.CategoryId;
        experience.ModifiedDate = DateTime.UtcNow;

        _experienceRepository.Update(experience);
        await _unitOfWork.CompleteAsync();

        return experience;
    }
    
}