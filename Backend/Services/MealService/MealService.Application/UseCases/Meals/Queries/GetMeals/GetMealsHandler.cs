using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMeals
{
    public class GetMealsHandler : IRequestHandler<GetMealsQuery, List<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMealsHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MealDto>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
        {
            var meals = await _unitOfWork.MealRepository.ListAllAsync(cancellationToken);
            var mealDtos = _mapper.Map<List<MealDto>>(meals);

            return mealDtos;
        }
    }
}
