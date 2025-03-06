using MealService.Application.DTOs.Meals;
using MealService.Application.UseCases.Meals.Commands.AddMeal;
using MealService.Application.UseCases.Meals.Commands.DeleteMeal;
using MealService.Application.UseCases.Meals.Commands.UpdateMeal;
using MealService.Application.UseCases.Meals.Queries.GetFilteredMealsByCuisine;
using MealService.Application.UseCases.Meals.Queries.GetMealById;
using MealService.Application.UseCases.Meals.Queries.GetMealsByCuisine;
using MealService.Application.UseCases.Meals.Queries.GetMealsByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealService.API.Controllers
{
    [ApiController]
    [Route("api/meals")]
    public class MealController : ControllerBase
    {
        private readonly ILogger<MealController> _logger;
        private readonly IMediator _mediator;

        public MealController(IMediator mediator, ILogger<MealController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("cuisine/{cuisineId}")]
        public async Task<IActionResult> GetMealsByCuisine(Guid cuisineId, bool? isAvailable, int pageNo=1,int pageSize=3)
        {
            _logger.LogInformation($"Start retrieving meals for cuisine {cuisineId}.");

            var meals = await _mediator.Send(new GetMealsByCuisineQuery(cuisineId, pageNo, pageSize, isAvailable));

            _logger.LogInformation($"Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("cuisine/{cuisineId}/filtered")]
        public async Task<IActionResult> GetFilteredMealsByCuisine(Guid cuisineId,[FromQuery] MealFilterDto filter,int pageNo=1,int pageSize=3)
        {
            _logger.LogInformation($"Start retrieving filtered meals for cuisine {cuisineId}.");

            var meals = await _mediator.Send(new GetFilteredMealsByCuisineQuery(cuisineId, filter, pageNo, pageSize));

            _logger.LogInformation($"Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("meal/{mealId}")]
        public async Task<IActionResult> GetMealById(Guid mealId)
        {
            _logger.LogInformation($"Load data for meal {mealId}.");

            var mealDto = await _mediator.Send(new GetMealByIdQuery(mealId));

            _logger.LogInformation($"Data for meal {mealId} is loaded.");

            return Ok(mealDto);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetMealsByName(string name)
        {
            _logger.LogInformation($"Start retrieving meals for with name {name}.");

            var meals = await _mediator.Send(new GetMealsByNameQuery(name));

            _logger.LogInformation($"Meals retrieved.");

            return Ok(meals);
        }

        [Authorize(Policy ="Admin")]
        [HttpPost("meal/create")]
        public async Task<IActionResult> CreateMeal([FromForm] MealRequestDto meal, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start adding new meal.");

            var newMeal = await _mediator.Send(new AddMealCommand(meal), cancellationToken);

            _logger.LogInformation("New meal added.");

            return CreatedAtAction(nameof(CreateMeal), new { id = newMeal.Id }, newMeal);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("meal/delete/{mealId}")]
        public async Task<IActionResult> DeleteMeal(Guid mealId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting meal {mealId}.");

            await _mediator.Send(new DeleteMealCommand(mealId), cancellationToken);

            _logger.LogInformation($"Meal {mealId} deleted.");

            return StatusCode(204);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("meal/update/{id}")]
        public async Task<IActionResult> UpdateMeal(Guid id, [FromForm] MealRequestDto meal, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating meal {id}.");

            var updatedMeal = await _mediator.Send(new UpdateMealCommand(id, meal), cancellationToken);

            _logger.LogInformation($"Meal {id} updated.");

            return Ok(updatedMeal);
        }

    }
}
