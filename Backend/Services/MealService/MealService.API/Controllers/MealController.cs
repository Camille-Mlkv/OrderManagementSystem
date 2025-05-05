using MealService.Application.DTOs.Meals;
using MealService.Application.UseCases.Meals.Commands.AddMeal;
using MealService.Application.UseCases.Meals.Commands.DeleteMeal;
using MealService.Application.UseCases.Meals.Commands.UpdateMeal;
using MealService.Application.UseCases.Meals.Queries.GetFilteredMeals;
using MealService.Application.UseCases.Meals.Queries.GetMealById;
using MealService.Application.UseCases.Meals.Queries.GetMealsByName;
using MediatR;
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

        [HttpGet]
        public async Task<IActionResult> GetMeals([FromQuery] MealFilterDto filter,
                                                   CancellationToken cancellationToken,
                                                   int pageNo = 1,
                                                   int pageSize = 3)
        {
            var meals = await _mediator.Send(new GetFilteredMealsQuery(filter, pageNo, pageSize), cancellationToken);

            _logger.LogInformation($"Filtered meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("{mealId}")]
        public async Task<IActionResult> GetMealById(Guid mealId, CancellationToken cancellationToken)
        {
            var mealDto = await _mediator.Send(new GetMealByIdQuery(mealId), cancellationToken);

            _logger.LogInformation($"Data for meal {mealId} is loaded.");

            return Ok(mealDto);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetMealsByName(string name, CancellationToken cancellationToken)
        {
            var meals = await _mediator.Send(new GetMealsByNameQuery(name), cancellationToken);

            _logger.LogInformation($"Meals retrieved by name {name}.");

            return Ok(meals);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromBody] MealRequestDto meal, CancellationToken cancellationToken)
        {
            var newMeal = await _mediator.Send(new AddMealCommand(meal), cancellationToken);

            _logger.LogInformation($"New meal {newMeal.Id} added.");

            return CreatedAtAction(nameof(CreateMeal), new { id = newMeal.Id }, newMeal);
        }

        [HttpDelete("{mealId}")]
        public async Task<IActionResult> DeleteMeal(Guid mealId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMealCommand(mealId), cancellationToken);

            _logger.LogInformation($"Meal {mealId} deleted.");

            return StatusCode(204);
        }

        [HttpPut("{mealId}")]
        public async Task<IActionResult> UpdateMeal(Guid mealId, [FromBody] MealRequestDto meal, CancellationToken cancellationToken)
        {
            var updatedMeal = await _mediator.Send(new UpdateMealCommand(mealId, meal), cancellationToken);

            _logger.LogInformation($"Meal {mealId} updated.");

            return Ok(updatedMeal);
        }
    }
}
