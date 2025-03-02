using MealService.Application.DTOs;
using MealService.Application.UseCases.Meals.Commands.CreateMeal;
using MealService.Application.UseCases.Meals.Commands.DeleteMeal;
using MealService.Application.UseCases.Meals.Commands.UpdateMeal;
using MealService.Application.UseCases.Meals.Queries.GetAvailableMeals;
using MealService.Application.UseCases.Meals.Queries.GetMeals;
using MealService.Application.UseCases.Meals.Queries.GetMealsByCategory;
using MealService.Application.UseCases.Meals.Queries.GetMealsPerPage;
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
        public async Task<IActionResult> GetMeals(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving meals.");

            var meals = await _mediator.Send(new GetMealsQuery(), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("{pageNo}")]
        public async Task<IActionResult> GetMealsPerPage(CancellationToken cancellationToken, int pageNo = 1, int pageSize = 3)
        {
            _logger.LogInformation($"Start retrieving meals per page {pageNo}.");

            var meals = await _mediator.Send(new GetMealsPerPageQuery(pageNo,pageSize), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("{categoryId}/{pageNo}")]
        public async Task<IActionResult> GetMealsByCategory(CancellationToken cancellationToken,Guid categoryId, int pageNo = 1, int pageSize = 3)
        {
            _logger.LogInformation($"Start retrieving meals per page {pageNo} per category.");

            var meals = await _mediator.Send(new GetMealsByCategoryQuery(categoryId,pageNo, pageSize), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableMeals(CancellationToken cancellationToken, int pageNo = 1, int pageSize = 3)
        {
            _logger.LogInformation($"Start retrieving available meals per page {pageNo}.");

            var meals = await _mediator.Send(new GetAvailableMealsQuery(pageNo,pageSize), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [HttpPost("meal/create")]
        public async Task<IActionResult> CreateMeal([FromForm] MealRequestDto meal, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start adding new meal.");

            var newMeal = await _mediator.Send(new CreateMealCommand(meal), cancellationToken);
            _logger.LogInformation("New meal added.");

            return CreatedAtAction(nameof(CreateMeal), new { id = newMeal.Id }, newMeal);
        }

        [HttpDelete("meal/delete/{id}")]
        public async Task<IActionResult> DeleteMeal(Guid mealId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting meal {mealId}.");

            await _mediator.Send(new DeleteMealCommand(mealId), cancellationToken);
            _logger.LogInformation($"Meal {mealId} deleted.");

            return StatusCode(204);
        }

        [HttpPut("meal/update/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] MealRequestDto meal, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating meal {id}.");

            var updatedMeal = await _mediator.Send(new UpdateMealCommand(id, meal), cancellationToken);

            _logger.LogInformation($"Meal {id} updated.");

            return Ok(updatedMeal);
        }

    }
}
