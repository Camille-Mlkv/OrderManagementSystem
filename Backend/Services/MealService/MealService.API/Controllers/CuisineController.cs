﻿using MealService.Application.DTOs.Cuisines;
using MealService.Application.UseCases.Cuisines.Commands.AddCuisine;
using MealService.Application.UseCases.Cuisines.Commands.DeleteCuisine;
using MealService.Application.UseCases.Cuisines.Commands.UpdateCuisine;
using MealService.Application.UseCases.Cuisines.Queries.GetCuisines;
using MealService.Application.UseCases.Cuisines.Queries.GetCuisinesByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MealService.API.Controllers
{
    [Route("api/cuisines")]
    [ApiController]
    public class CuisineController : ControllerBase
    {
        private readonly ILogger<CuisineController> _logger;
        private readonly IMediator _mediator;

        public CuisineController(IMediator mediator, ILogger<CuisineController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCuisines(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving cuisines.");

            var cuisines = await _mediator.Send(new GetCuisinesQuery(), cancellationToken);

            _logger.LogInformation("Cuisines retrieved.");

            return Ok(cuisines);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCuisinesByName(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start retrieving cuisine by its name.");

            var cuisines = await _mediator.Send(new GetCuisinesByNameQuery(name), cancellationToken);

            _logger.LogInformation("Cuisines retrieved.");

            return Ok(cuisines);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCuisine([FromForm] CuisineRequestDto cuisine, IFormFile? imageFile, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start adding new cuisine.");

            if (imageFile != null)
            {
                await ProcessImageFileAsync(imageFile, cuisine, cancellationToken);
            }

            var newCuisine = await _mediator.Send(new AddCuisineCommand(cuisine), cancellationToken);

            _logger.LogInformation("New cuisine added.");

            return CreatedAtAction(nameof(CreateCuisine), new { id = newCuisine.Id }, newCuisine);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{cuisineId}")]
        public async Task<IActionResult> UpdateCuisine(Guid cuisineId, [FromForm] CuisineRequestDto cuisine, IFormFile? imageFile, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating cuisine {cuisineId}.");

            if (imageFile != null)
            {
                await ProcessImageFileAsync(imageFile, cuisine, cancellationToken);
            }

            var updatedCuisine = await _mediator.Send(new UpdateCuisineCommand(cuisineId, cuisine), cancellationToken);

            _logger.LogInformation($"Cuisine {cuisineId} updated.");

            return Ok(updatedCuisine);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{cuisineId}")]
        public async Task<IActionResult> DeleteCuisine(Guid cuisineId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting cuisine {cuisineId}.");

            await _mediator.Send(new DeleteCuisineCommand(cuisineId), cancellationToken);

            _logger.LogInformation($"cuisine {cuisineId} deleted.");

            return StatusCode(204);
        }

        private async Task ProcessImageFileAsync(IFormFile imageFile, CuisineRequestDto cuisine, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream, cancellationToken);

            cuisine.ImageData = memoryStream.ToArray();
            cuisine.ImageContentType = imageFile.ContentType;
        }
    }
}
