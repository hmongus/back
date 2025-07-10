using FluentValidation;
using workstation_back_end.Bookings.Domain; 
using workstation_back_end.Reviews.Domain;
using workstation_back_end.Reviews.Domain.Models.Commands;
using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Reviews.Domain.Services;
using workstation_back_end.Shared.Domain.Repositories;
using workstation_back_end.Users.Domain;

namespace workstation_back_end.Reviews.Application.ReviewCommandService;

public class ReviewCommandService : IReviewCommandService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateReviewCommand> _createReviewCommandValidator;

    public ReviewCommandService( IReviewRepository reviewRepository,
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Review?> Handle(CreateReviewCommand command)
    {
        var touristUserMain = await _userRepository.FindByGuidAsync(command.TouristUserId);
        if (touristUserMain == null || touristUserMain.Tourist == null) 
        {
            throw new ArgumentException("Tourist user not found or is not a valid tourist profile.");
        }

        var agencyUserMain = await _userRepository.FindByGuidAsync(command.AgencyUserId);
        if (agencyUserMain == null || agencyUserMain.Agency == null) 
        {
            throw new ArgumentException("Agency user not found or is not a valid agency profile.");
        }
        var agency = agencyUserMain.Agency; 
        

        var hasCompletedBookingWithAgency = await _bookingRepository.HasCompletedBookingForAgencyViaExperienceAsync( 
            command.TouristUserId, 
            command.AgencyUserId 
        );

        if (!hasCompletedBookingWithAgency)
        {
            throw new InvalidOperationException("Tourist must have completed at least one booking with an experience from this agency to leave a review."); // Mensaje más descriptivo
        }

        var existingReview = await _reviewRepository.FindByAgencyAndTouristUserAsync(
            command.AgencyUserId, 
            command.TouristUserId
        );

        if (existingReview != null)
        {
            throw new InvalidOperationException("Tourist has already reviewed this agency. Please update the existing review instead.");
        }
        
        if (command.Rating < 1 || command.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }

        var newReview = new Review(
            command.TouristUserId,
            command.AgencyUserId, 
            command.Rating,
            command.Comment
        );

        try
        {
            await _reviewRepository.AddAsync(newReview);
            await _unitOfWork.CompleteAsync(); 

            await RecalculateAgencyAverageRatingAndCount(command.AgencyUserId);
            
            return newReview;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the review: {e.Message}");
            throw; 
        }
    }
    
    private async Task RecalculateAgencyAverageRatingAndCount(Guid agencyUserId)
    {

        var agencyUserMain = await _userRepository.FindByGuidAsync(agencyUserId); 
        var agency = agencyUserMain?.Agency; 

        if (agency == null) return; 

        var reviews = await _reviewRepository.FindAllReviewsForAgency(agencyUserId);

        if (reviews.Any())
        {
            agency.Rating = (float)reviews.Average(r => r.Rating); 
            agency.ReviewCount = reviews.Count(); 
        }
        else
        {
            agency.Rating = 0.0f; 
            agency.ReviewCount = 0;
        }
        
        _userRepository.UpdateAgency(agency); 
        await _unitOfWork.CompleteAsync(); 
    }
}