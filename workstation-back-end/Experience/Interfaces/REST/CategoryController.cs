using Microsoft.AspNetCore.Mvc;
using workstation_back_end.Experience.Domain.Models.Commands;
using workstation_back_end.Experience.Domain.Models.Entities;
using workstation_back_end.Experience.Domain.Services;
using workstation_back_end.Experience.Interfaces.REST.Transform;
using workstation_back_end.Shared.Domain.Repositories;

namespace workstation_back_end.Experience.Interfaces;

/// <summary>
/// API Controller for managing experience categories.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Gets all available categories.
    /// </summary>
    /// <response code="200">Returns the list of categories</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.ListAsync();
        var response = categories.Select(CategoryResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(response);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/v1/Category
    ///     {
    ///         "name": "Adventure"
    ///     }
    /// </remarks>
    /// <param name="resource">Category name</param>
    /// <response code="201">Returns the newly created category</response>
    /// <response code="400">If the name is missing or invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryResource resource)
    {
        if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
            return BadRequest("Category name is required.");

        var category = new Category { Name = resource.Name };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        var resourceResponse = CategoryResourceFromEntityAssembler.ToResourceFromEntity(category);
        return CreatedAtAction(nameof(GetAll), new { id = category.Id }, resourceResponse);
    }
}

