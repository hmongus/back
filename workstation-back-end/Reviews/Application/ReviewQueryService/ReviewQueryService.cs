using workstation_back_end.Reviews.Domain;
using workstation_back_end.Reviews.Domain.Models.Entities;
using workstation_back_end.Reviews.Domain.Models.Queries;
using workstation_back_end.Reviews.Domain.Services;

namespace workstation_back_end.Reviews.Application.ReviewQueryService;

public class ReviewQueryService : IReviewQueryService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewQueryService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<Review>> Handle(GetReviewsByAgencyIdQuery query)
    {

        return await _reviewRepository.FindByAgencyUserIdAsync(query.AgencyUserId); 
    }

    public async Task<Review?> Handle(GetReviewByIdQuery query)
    {
        return await _reviewRepository.FindByIdAsync(query.ReviewId);
    }
}