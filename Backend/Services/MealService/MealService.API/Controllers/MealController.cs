using MealService.Application.DTOs.Meals;
using MealService.Application.UseCases.Meals.Commands.AddMeal;
using MealService.Application.UseCases.Meals.Commands.DeleteMeal;
using MealService.Application.UseCases.Meals.Commands.UpdateMeal;
using MealService.Application.UseCases.Meals.Queries.GetAllMeals;
using MealService.Application.UseCases.Meals.Queries.GetFilteredMeals;
using MealService.Application.UseCases.Meals.Queries.GetMealsPerPage;
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

        [HttpGet]
        public async Task<IActionResult> GetMeals(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving meals.");

            var meals = await _mediator.Send(new GetAllMealsQuery(), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [Authorize(Policy = "Admin")] // admins can see paginated meals without specified cuisine
        [HttpGet("{pageNo}")]
        public async Task<IActionResult> GetMealsPerPage(CancellationToken cancellationToken, int pageNo = 1, int pageSize = 3)
        {
            _logger.LogInformation($"Start retrieving meals per page {pageNo}.");

            var meals = await _mediator.Send(new GetMealsPerPageQuery(pageNo,pageSize), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredMeals([FromQuery] MealFilterDto filter, CancellationToken cancellationToken, int pageNo = 1, int pageSize = 3)
        {
            _logger.LogInformation($"Start retrieving filtered meals per page {pageNo}.");

            var meals = await _mediator.Send(new GetFilteredMealsQuery(filter,pageNo, pageSize), cancellationToken);

            _logger.LogInformation("Meals retrieved.");

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
