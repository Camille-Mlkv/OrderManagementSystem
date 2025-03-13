using MealService.Application.DTOs.Categories;
using MealService.Application.UseCases.Categories.Commands.AddCategory;
using MealService.Application.UseCases.Categories.Commands.DeleteCategory;
using MealService.Application.UseCases.Categories.Commands.UpdateCategory;
using MealService.Application.UseCases.Categories.Queries.GetCategories;
using MealService.Application.UseCases.Categories.Queries.GetCategoriesByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealService.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving categories.");

            var categories=await _mediator.Send(new GetCategoriesQuery(),cancellationToken);

            _logger.LogInformation("Categories retrieved.");

            return Ok(categories);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCategoriesByName(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving categories by name.");

            var categories = await _mediator.Send(new GetCategoriesByNameQuery(name), cancellationToken);

            _logger.LogInformation("Categories retrieved.");

            return Ok(categories);
        }

        [Authorize(Policy ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto category,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start adding new category.");

            var newCategory=await _mediator.Send(new AddCategoryCommand(category), cancellationToken);

            _logger.LogInformation("New category added.");

            return CreatedAtAction(nameof(AddCategory), new { id = newCategory.Id }, newCategory);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryRequestDto category,CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating category {id}.");

            var updatedCategory =await _mediator.Send(new UpdateCategoryCommand(id,category),cancellationToken);

            _logger.LogInformation($"Category {id} updated.");

            return Ok(updatedCategory);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting category {id}.");

            await _mediator.Send(new DeleteCategoryCommand(id),cancellationToken);

            _logger.LogInformation($"Category {id} deleted.");

            return StatusCode(204);
        }
    }
}
