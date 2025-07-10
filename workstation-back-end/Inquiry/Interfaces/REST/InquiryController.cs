using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Models.Queries;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Inquiry.Interfaces.REST.Transform;

namespace workstation_back_end.Inquiry.Interfaces.REST;

/// <summary>
/// API Controller for managing user inquiries related to tourism experiences.
/// Allows creating inquiries and retrieving them globally or by experience.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class InquiryController : ControllerBase
{
    private readonly IInquiryCommandService _commandService;
    private readonly IInquiryQueryService _queryService;

    public InquiryController(IInquiryCommandService commandService, IInquiryQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    /// <summary>
    /// Retrieves all inquiries made by users.
    /// </summary>
    /// <response code="200">Returns a list of all inquiries</response>
    /// <response code="404">No inquiries found</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _queryService.Handle(new GetAllInquiriesQuery());
        var resources = result.Select(InquiryResourceFromEntityAssembler.ToResourceFromEntity);
        return resources.Any() ? Ok(resources) : NotFound("No inquiries found.");
    }

    /// <summary>
    /// Retrieves all inquiries related to a specific experience.
    /// </summary>
    /// <param name="experienceId">ID of the experience</param>
    /// <response code="200">Returns inquiries for the specified experience</response>
    /// <response code="404">No inquiries found for the experience</response>
    [HttpGet("experience/{experienceId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByExperience(int experienceId)
    {
        var result = await _queryService.Handle(new GetAllInquiriesByExperienceQuery(experienceId));
        var resources = result.Select(InquiryResourceFromEntityAssembler.ToResourceFromEntity);
        return resources.Any() ? Ok(resources) : NotFound($"No inquiries found for experience {experienceId}.");
    }

    /// <summary>
    /// Creates a new inquiry about an experience.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/Inquiry
    ///     {
    ///         "experienceId": 2,
    ///         "userId": "4e0e1e06-3073-45a3-a732-36484e666ca1",
    ///         "question": "Is this experience available in Spanish?",
    ///         "askedAt": "2025-06-20T12:00:00Z"
    ///     }
    /// </remarks>
    /// <response code="201">Inquiry created successfully</response>
    /// <response code="400">Validation error or invalid request</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInquiryCommand command)
    {
        try
        {
            var inquiry = await _commandService.Handle(command);
            var resource = InquiryResourceFromEntityAssembler.ToResourceFromEntity(inquiry);
            return CreatedAtAction(nameof(GetAll), new { id = resource.Id }, resource);
        }
        catch (ValidationException e)
        {
            return BadRequest(new
            {
                message = "Validation failed",
                errors = e.Errors.Select(err => new
                {
                    field = err.PropertyName,
                    error = err.ErrorMessage
                })
            });
        }
    }
}