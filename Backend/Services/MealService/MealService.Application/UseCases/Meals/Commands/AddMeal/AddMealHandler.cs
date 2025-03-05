using AutoMapper;
using MealService.Application.DTOs.Meals;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MealService.Application.Specifications.Services;
using MealService.Domain.Entities;
using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.AddMeal
{
    public class AddMealHandler : IRequestHandler<AddMealCommand, MealDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public AddMealHandler(IMapper mapper, IUnitOfWork unitOfWork,IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<MealDto> Handle(AddMealCommand request, CancellationToken cancellationToken)
        {
            var category=await _unitOfWork.CategoryRepository.GetByIdAsync(request.Meal.CategoryId,cancellationToken);
            if (category is null)
            {
                throw new BadRequestException("Foreign key constraint for category is violated.");
            }

            var cuisine = await _unitOfWork.CuisineRepository.GetByIdAsync(request.Meal.CuisineId, cancellationToken);
            if (cuisine is null)
            {
                throw new BadRequestException("Foreign key constraint for cuisine is violated.");
            }

            var newMeal = _mapper.Map<Meal>(request.Meal);
            newMeal.Id = Guid.NewGuid();

            if (request.Meal.ImageFile != null)
            {
                var imageUrl = await _imageService.UploadImageAsync(request.Meal.ImageFile);
                newMeal.ImageUrl = imageUrl;
            }
            else
            {
                newMeal.ImageUrl = _imageService.GetDefaultImageUrl();
            }

            await _unitOfWork.MealRepository.AddAsync(newMeal, cancellationToken);

            
            if (request.Meal.TagIds.Count != 0)
            {
                var tags = await _unitOfWork.TagRepository.ListAsync(t => request.Meal.TagIds.Contains(t.Id), cancellationToken);
                newMeal.SetTags(tags);
            }

            await _unitOfWork.SaveAllAsync(cancellationToken);

            var addedMeal = _mapper.Map<MealDto>(newMeal);

            return addedMeal;
        }
    }
}
