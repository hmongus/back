using Microsoft.AspNetCore.Mvc;
using workstation_back_end.Users.Domain.Models.Commands;
using workstation_back_end.Users.Domain.Models.Entities;
using workstation_back_end.Users.Domain.Models.Queries;
using workstation_back_end.Users.Domain.Services;
using workstation_back_end.Users.Interfaces.REST.Resources;
using workstation_back_end.Users.Interfaces.REST.Transform;

namespace workstation_back_end.Users.Interfaces.REST;

[ApiController]
[Route("api/v1/tourists")]
public class TouristController : ControllerBase
{
    private readonly IUserQueryService _queryService;
    private readonly IUserCommandService _commandService;

    public TouristController(IUserQueryService queryService, IUserCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    /// <summary>
    /// Get tourist profile by user ID
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var user = await _queryService.Handle(new GetUserByIdQuery(userId));
        if (user?.Tourist == null) return NotFound();

        var resource = TouristResourceAssembler.ToResource(user.Tourist);
        return Ok(resource);
    }

    /// <summary>
    /// Update tourist profile
    /// </summary>
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateTouristCommand command)
    {
        var user = await _queryService.Handle(new GetUserByIdQuery(userId));
        if (user?.Tourist == null) return NotFound();

        var tourist = user.Tourist;

        tourist.AvatarUrl = command.AvatarUrl ?? tourist.AvatarUrl;

        await _commandService.UpdateTouristAsync(userId,command);
        return Ok(TouristResourceAssembler.ToResource(tourist));
    }
}