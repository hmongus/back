using System.Net.Mime;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using workstation_back_end.Favorites.Domain.Models.Commands;
using workstation_back_end.Favorites.Domain.Models.Queries;
using workstation_back_end.Favorites.Domain.Services;
using workstation_back_end.Favorites.Interfaces.REST.Transform;

namespace workstation_back_end.Favorites.Interfaces.REST
{
    /// <summary>
    /// API Controller for managing favorites.
    /// Allows tourists to mark or unmark experiences as favorites, and retrieve their favorite list.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteCommandService _favoriteCommandService;
        private readonly IFavoriteQueryService _favoriteQueryService;

        public FavoriteController(
            IFavoriteCommandService favoriteCommandService,
            IFavoriteQueryService favoriteQueryService)
        {
            _favoriteCommandService = favoriteCommandService;
            _favoriteQueryService = favoriteQueryService;
        }

        /// <summary>
        /// Gets all favorites for a specific tourist.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/Favorite/tourist/4fb6c84d-8f1a-4f22-a7d6-cd39e7c933cd
        ///
        /// </remarks>
        /// <param name="touristId">The unique identifier of the tourist (Guid).</param>
        /// <response code="200">Returns the list of favorites for the tourist.</response>
        [HttpGet("tourist/{touristId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoritesByTourist(Guid touristId)
        {
            var query = new GetFavoritesByUserIdQuery(touristId);
            var favorites = await _favoriteQueryService.Handle(query);
            var resources = favorites.Select(FavoriteAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        /// <summary>
        /// Adds a new experience to the tourist's favorites.
        /// </summary>
        /// <remarks>
        /// Requires authentication with a valid JWT token. The `touristId` is taken from the authenticated user.
        ///
        /// Sample request:
        ///
        ///     POST /api/v1/Favorite
        ///     {
        ///         "experienceId": 1
        ///     }
        /// </remarks>
        /// <param name="command">Command containing the experience ID.</param>
        /// <response code="201">Returns the newly created favorite.</response>
        /// <response code="400">If validation fails or input is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFavorite([FromBody] CreateFavoriteCommand command)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var touristId))
                    return Unauthorized("Invalid or missing user identity.");

                var commandWithUser = command with { TouristId = touristId };
                var favorite = await _favoriteCommandService.Handle(commandWithUser);

                if (favorite is null)
                    return BadRequest("Favorite could not be created.");

                var resource = FavoriteAssembler.ToResourceFromEntity(favorite);
                return CreatedAtAction(nameof(GetFavoritesByTourist), new { touristId = resource.TouristId }, resource);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                });

                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Application error when creating favorite: {ex.Message}");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error creating favorite: {ex.Message}");
                return Problem(detail: "An internal server error occurred.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Removes an experience from the tourist's favorites.
        /// </summary>
        /// <remarks>
        /// Requires authentication. The authenticated user's ID will be used.
        ///
        /// Sample request:
        ///
        ///     DELETE /api/v1/Favorite?experienceId=1
        /// </remarks>
        /// <param name="experienceId">The ID of the experience to remove from favorites.</param>
        /// <response code="204">If the favorite was removed successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the favorite does not exist.</response>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFavorite([FromQuery] int experienceId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var touristId))
                return Unauthorized("Invalid or missing user identity.");

            var command = new DeleteFavoriteCommand(touristId, experienceId);
            var deleted = await _favoriteCommandService.Handle(command);

            return deleted ? NoContent() : NotFound("Favorite not found.");
        }
    }
}
