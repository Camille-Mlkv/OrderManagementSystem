using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsPerPage
{
    public class GetMealsPerPageHandler : IRequestHandler<GetMealsPerPageQuery, List<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMealsPerPageHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<MealDto>> Handle(GetMealsPerPageQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNo < 1 || request.PageSize < 1)
            {
                throw new BadRequestException("Failed to load data.", "Provide valid data for page number and page size.");
            }

            var meals = await _unitOfWork.MealRepository.GetPagedListAsync(request.PageNo, request.PageSize, cancellationToken);
            var mealsDtos = _mapper.Map<List<MealDto>>(meals);
            return mealsDtos;
        }
    }
}
