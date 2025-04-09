using MealService.Application.DTOs.Meals;
using MealService.Application.UseCases.Meals.Commands.AddMeal;
using MealService.Application.UseCases.Meals.Commands.DeleteMeal;
using MealService.Application.UseCases.Meals.Commands.UpdateMeal;
using MealService.Application.UseCases.Meals.Queries.GetFilteredMealsByCuisine;
using MealService.Application.UseCases.Meals.Queries.GetMealById;
using MealService.Application.UseCases.Meals.Queries.GetMealsByCuisine;
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

        [HttpGet("cuisine/{cuisineId}")]
        public async Task<IActionResult> GetMealsByCuisine(Guid cuisineId,
                                                           bool? isAvailable, 
                                                           CancellationToken cancellationToken,
                                                           int pageNo=1,
                                                           int pageSize=3)
        {
            var meals = await _mediator.Send(new 
               GetMealsByCuisineQuery(cuisineId, pageNo, pageSize, isAvailable), cancellationToken);

            _logger.LogInformation($"Meals retrieved.");

            return Ok(meals);
        }

        [HttpGet("cuisine/{cuisineId}/filtered")]
        public async Task<IActionResult> GetFilteredMealsByCuisine(Guid cuisineId,
                                                                   [FromQuery] MealFilterDto filter,
                                                                   CancellationToken cancellationToken,
                                                                   int pageNo=1, 
                                                                   int pageSize=3)
        {
            var meals = await _mediator.Send(new 
               GetFilteredMealsByCuisineQuery(cuisineId, filter, pageNo, pageSize), cancellationToken);

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
        public async Task<IActionResult> CreateMeal([FromForm] MealRequestDto meal, 
                                                    IFormFile? imageFile, 
                                                    CancellationToken cancellationToken)
        {
            if (imageFile != null)
            {
                await ProcessImageFileAsync(imageFile, meal, cancellationToken);
            }

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
        public async Task<IActionResult> UpdateMeal(Guid mealId, 
                                                    [FromForm] MealRequestDto meal, 
                                                    IFormFile? imageFile, 
                                                    CancellationToken cancellationToken)
        {
            if (imageFile != null)
            {
                await ProcessImageFileAsync(imageFile, meal, cancellationToken);
            }

            var updatedMeal = await _mediator.Send(new UpdateMealCommand(mealId, meal), cancellationToken);

            _logger.LogInformation($"Meal {mealId} updated.");

            return Ok(updatedMeal);
        }

        private async Task ProcessImageFileAsync(IFormFile imageFile, MealRequestDto meal, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream, cancellationToken);

            meal.ImageData = memoryStream.ToArray();
            meal.ImageContentType = imageFile.ContentType;
        }
    }
}
