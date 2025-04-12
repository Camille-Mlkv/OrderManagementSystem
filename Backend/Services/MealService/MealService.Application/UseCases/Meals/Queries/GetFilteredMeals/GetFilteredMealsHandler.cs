using MealService.Application.DTOs.Meals;
using MealService.Application.DTOs;
using MediatR;
using AutoMapper;
using MealService.Application.Specifications;
using LinqKit;
using MealService.Application.Exceptions;
using MealService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MealService.Application.UseCases.Meals.Queries.GetFilteredMeals
{
    public class GetFilteredMealsHandler : IRequestHandler<GetFilteredMealsQuery, PagedList<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetFilteredMealsHandler> _logger;

        public GetFilteredMealsHandler(IMapper mapper, IUnitOfWork unitOfWork, ILogger<GetFilteredMealsHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PagedList<MealDto>> Handle(GetFilteredMealsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var filter = request.Filter;
            var predicate = PredicateBuilder.New<Meal>(true);

            if (filter.IsAvailable.HasValue)
            {
                predicate = predicate.And(m => m.IsAvailable == filter.IsAvailable);
                _logger.LogInformation("Is available if");
            }

            if (filter.CuisineId.HasValue)
            {
                predicate = predicate.And(m => m.CuisineId == filter.CuisineId);
            }

            if (filter.CategoryId.HasValue)
            {
                predicate = predicate.And(m => m.CategoryId == filter.CategoryId);
            }

            if (filter.TagIds.Any())
            {
                predicate = predicate.And(m => m.MealTags.Any(mt => filter.TagIds.Contains(mt.TagId)));
            }

            if (filter.MaxPrice.HasValue)
            {
                predicate = predicate.And(m => m.Price <= filter.MaxPrice);
            }

            if (filter.MinPrice.HasValue)
            {
                predicate = predicate.And(m => m.Price >= filter.MinPrice);
            }

            if (filter.MaxCalories.HasValue)
            {
                predicate = predicate.And(m => m.Calories <= filter.MaxCalories);
            }

            if (filter.MinCalories.HasValue)
            {
                predicate = predicate.And(m => m.Calories >= filter.MinCalories);
            }

            var mealsList = await _unitOfWork.MealRepository.GetPagedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken,
                predicate
            );

            var mealDtos = _mapper.Map<List<MealDto>>(mealsList.Items);

            return new PagedList<MealDto>
            {
                Items = mealDtos,
                TotalCount = mealsList.TotalCount
            };
        }
    }
}
