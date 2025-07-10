using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using workstation_back_end.Inquiry.Domain.Models.Commands;
using workstation_back_end.Inquiry.Domain.Models.Queries;
using workstation_back_end.Inquiry.Domain.Services.Services;
using workstation_back_end.Inquiry.Interfaces.REST.Transform;

namespace workstation_back_end.Inquiry.Interfaces.REST;

/// <summary>
/// API Controller for managing responses to inquiries.
/// Supports creating a response and retrieving it by inquiry ID.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class ResponseController : ControllerBase
{
    private readonly IResponseCommandService _commandService;
    private readonly IResponseQueryService _queryService;

    public ResponseController(IResponseCommandService command, IResponseQueryService query)
    {
        _commandService = command;
        _queryService = query;
    }

    /// <summary>
    /// Creates a response for a specific inquiry.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/v1/Response
    ///     {
    ///        "inquiryId": 1,
    ///        "responderId": "4e0e1e06-3073-45a3-a732-36484e666ca1",
    ///        "answer": "Thank you for your question! The experience starts at 9am.",
    ///        "answeredAt": "2025-06-20T14:00:00Z"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created response</response>
    /// <response code="400">Validation error or bad request</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateResponseCommand command)
    {
        try
        {
            var response = await _commandService.Handle(command);
            var resource = ResponseResourceFromEntityAssembler.ToResourceFromEntity(response);
            return CreatedAtAction(nameof(GetByInquiry), new { inquiryId = response.InquiryId }, resource);
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
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Gets the response associated with a specific inquiry.
    /// </summary>
    /// <param name="inquiryId">The ID of the inquiry</param>
    /// <response code="200">Returns the response for the inquiry</response>
    /// <response code="404">No response found for the inquiry</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("inquiry/{inquiryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByInquiry(int inquiryId)
    {
        try
        {
            var result = await _queryService.Handle(new GetResponseByInquiryIdQuery(inquiryId));
            if (result == null) return NotFound($"No response found for inquiry with ID {inquiryId}.");

            return Ok(ResponseResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}