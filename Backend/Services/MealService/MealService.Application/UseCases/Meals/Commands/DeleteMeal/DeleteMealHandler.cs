using AutoMapper;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MealService.Application.Specifications.Services;
using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.DeleteMeal
{
    public class DeleteMealHandler : IRequestHandler<DeleteMealCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteMealHandler(IMapper mapper, IUnitOfWork unitOfWork,IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(DeleteMealCommand request, CancellationToken cancellationToken)
        {
            var meal = await _unitOfWork.MealRepository.GetByIdAsync(request.MealId, cancellationToken);

            if (meal is null)
            {
                throw new NotFoundException($"Meal with id {request.MealId} dosen't exist.");
            }

            if (meal.ImageUrl != _imageService.GetDefaultImageUrl())
            {
                var imagePublicId = _imageService.GetPublicIdFromUrl(meal.ImageUrl);
                await _imageService.DeleteImageAsync(imagePublicId);
            }

            meal.MealTags.Clear();

            await _unitOfWork.MealRepository.Delete(meal);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
