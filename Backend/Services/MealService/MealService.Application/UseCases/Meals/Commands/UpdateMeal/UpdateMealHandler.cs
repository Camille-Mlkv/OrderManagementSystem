using AutoMapper;
using MealService.Application.DTOs;
using MealService.Application.Specifications.Services;
using MealService.Application.Specifications;
using MediatR;
using MealService.Application.Exceptions;
using MealService.Domain.Entities;

namespace MealService.Application.UseCases.Meals.Commands.UpdateMeal
{
    public class UpdateMealHandler : IRequestHandler<UpdateMealCommand, MealDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UpdateMealHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task<MealDto> Handle(UpdateMealCommand request, CancellationToken cancellationToken)
        {
            var foundMeal = await _unitOfWork.MealRepository.GetByIdAsync(request.Id, cancellationToken);

            if (foundMeal is null)
            {
                throw new NotFoundException($"Meal with id {request.Id} doesn't exist.");
            }

            if (request.UpdatedMeal.ImageFile != null)
            {
                if (foundMeal.ImageUrl != _imageService.GetDefaultImageUrl())
                {
                    var imagePublicId = GetPublicIdFromUrl(foundMeal.ImageUrl); // get rid of old image
                    await _imageService.DeleteImageAsync(imagePublicId);
                }

                var imageUrl = await _imageService.UploadImageAsync(request.UpdatedMeal.ImageFile);
                foundMeal.ImageUrl = imageUrl;

            }

            _mapper.Map(request.UpdatedMeal, foundMeal);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return _mapper.Map<MealDto>(foundMeal);
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var urlParts = imageUrl.Split('/');
            var publicId = urlParts[urlParts.Length - 1].Split('.')[0];
            return publicId;
        }
    }
}
