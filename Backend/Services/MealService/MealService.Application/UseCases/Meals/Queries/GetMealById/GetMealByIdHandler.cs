using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.DTOs.Tags;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealById
{
    public class GetMealByIdHandler: IRequestHandler<GetMealByIdQuery,MealDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMealByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MealDto> Handle(GetMealByIdQuery request, CancellationToken cancellationToken)
        {
            var meal = await _unitOfWork.MealRepository.GetByIdAsync(request.MealId, cancellationToken,m=>m.MealTags);

            if (meal is null)
            {
                throw new NotFoundException($"Meal with id {request.MealId} doesn't exist.");
            }

            var tags = await _unitOfWork.TagRepository.ListAsync(t => meal.MealTags.Select(mt => mt.TagId).Contains(t.Id), cancellationToken);

            var mealDto = _mapper.Map<MealDto>(meal);
            mealDto.Tags = _mapper.Map<List<TagDto>>(tags);

            return mealDto;
        }
    }
}
