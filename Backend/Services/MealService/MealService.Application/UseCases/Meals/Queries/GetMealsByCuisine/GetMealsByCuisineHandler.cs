using AutoMapper;
using MealService.Application.DTOs;
using MealService.Application.DTOs.Meals;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByCuisine
{
    public class GetMealsByCuisineHandler : IRequestHandler<GetMealsByCuisineQuery, PagedList<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMealsByCuisineHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<MealDto>> Handle(GetMealsByCuisineQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var mealsList = await _unitOfWork.MealRepository.GetPagedListAsync(
                request.PageNo,
                request.PageSize,
                cancellationToken,
                m => m.CuisineId == request.CuisineId && (request.IsAvailable == null || m.IsAvailable == request.IsAvailable)
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
