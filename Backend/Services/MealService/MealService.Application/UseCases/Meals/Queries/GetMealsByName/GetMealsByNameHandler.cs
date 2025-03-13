using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByName
{
    public class GetMealsByNameHandler: IRequestHandler<GetMealsByNameQuery,List<MealDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMealsByNameHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MealDto>> Handle(GetMealsByNameQuery request, CancellationToken cancellationToken)
        {
            var meals = await _unitOfWork.MealRepository.ListAsync(m => m.Name.StartsWith(request.Name.ToLower()), cancellationToken);
            var mealDtos = _mapper.Map<List<MealDto>>(meals);

            return mealDtos;
        }
    }
}
