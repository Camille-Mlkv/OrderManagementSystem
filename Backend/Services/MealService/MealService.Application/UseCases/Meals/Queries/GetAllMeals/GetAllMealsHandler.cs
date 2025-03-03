using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetAllMeals
{
    public class GetAllMealsHandler : IRequestHandler<GetAllMealsQuery, List<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMealsHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MealDto>> Handle(GetAllMealsQuery request, CancellationToken cancellationToken)
        {
            var meals = await _unitOfWork.MealRepository.ListAllAsync(cancellationToken);
            var mealDtos = _mapper.Map<List<MealDto>>(meals);

            return mealDtos;
        }
    }
}
