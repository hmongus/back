using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using workstation_back_end.Reviews.Domain.Models.Commands;
using workstation_back_end.Reviews.Domain.Models.Queries;
using workstation_back_end.Reviews.Domain.Services;
using workstation_back_end.Reviews.Interfaces.REST.Transform;

namespace workstation_back_end.Reviews.Interfaces.REST
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewCommandService _reviewCommandService;
        private readonly IReviewQueryService _reviewQueryService;

        public ReviewController(
            IReviewCommandService reviewCommandService,
            IReviewQueryService reviewQueryService)
        {
            _reviewCommandService = reviewCommandService;
            _reviewQueryService = reviewQueryService;
        }
        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <remarks>
        /// Note: The `CreateReviewCommand` class must have properties that match this JSON structure.
        /// <br/>
        /// Sample request:
        ///
        ///     POST /api/v1/Review
        ///     {
        ///        "touristUserId": "12345678-abcd-abcd-abcd-1234567890ab", 
        ///        "agencyUserId": "a1b2c3d4-e5f6-7890-1234-567890abcdef", 
        ///        "rating": 5, 
        ///        "comment": "Excellent experience, highly recommended!" 
        ///     }
        ///
        /// </remarks>
        /// <param name="command">The command object to create a review.</param>
        /// <response code="201">Returns the newly created review.</response>
        /// <response code="400">If the command is invalid or the review could not be created.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            try
            {
                var review = await _reviewCommandService.Handle(command);
                if (review is null)
                    return BadRequest("Failed to create review for unknown reasons. Please check your request data."); // Mensaje más genérico

                var resource = ReviewAssembler.ToResourceFromEntity(review);
                return CreatedAtAction(nameof(GetReviewById), new { id = resource.Id }, resource);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: "An unexpected error occurred while creating the review: " + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets all registered reviews for a specific agency.
        /// </summary>
        /// <param name="agencyUserId">The agency's User ID (GUID).</param> // *** DESCRIPCIÓN ACTUALIZADA ***
        /// <response code="200">Returns the list of reviews for the agency.</response>
        /// <response code="404">If no reviews are found for the specified agency.</response>
        [HttpGet("agency/{agencyUserId:guid}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewsByAgency(Guid agencyUserId) 
        {
            var query = new GetReviewsByAgencyIdQuery(agencyUserId); 
            var reviews = await _reviewQueryService.Handle(query);
            var resources = reviews.Select(ReviewAssembler.ToResourceFromEntity);
            return resources.Any() ? Ok(resources) : NotFound($"No reviews found for agency with User ID {agencyUserId}.");
        }

        /// <summary>
        /// Gets a review by its ID.
        /// </summary>
        /// <param name="id">The review's ID.</param>
        /// <response code="200">Returns the review.</response>
        /// <response code="404">If the review is not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewById(int id)
        {
 
            var query = new GetReviewByIdQuery(id); 
            var review = await _reviewQueryService.Handle(query);
            
            if (review == null)
                return NotFound($"Review with ID {id} not found.");

            var resource = ReviewAssembler.ToResourceFromEntity(review);
            return Ok(resource);
        }
    }
}