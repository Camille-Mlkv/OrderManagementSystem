﻿using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Application.Specifications.Services;
using MealService.Application.Specifications;
using MediatR;
using MealService.Application.Exceptions;

namespace MealService.Application.UseCases.Cuisines.Commands.UpdateCuisine
{
    public class UpdateCuisineHandler : IRequestHandler<UpdateCuisineCommand, CuisineDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public UpdateCuisineHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task<CuisineDto> Handle(UpdateCuisineCommand request, CancellationToken cancellationToken)
        {
            var foundCuisine = await _unitOfWork.CuisineRepository.GetByIdAsync(request.Id, cancellationToken);

            if (foundCuisine is null)
            {
                throw new NotFoundException($"Cuisine with id {request.Id} doesn't exist.");
            }

            if (request.Cuisine.ImageFile != null)
            {
                if (foundCuisine.ImageUrl != _imageService.GetDefaultImageUrl())
                {
                    var imagePublicId = GetPublicIdFromUrl(foundCuisine.ImageUrl);
                    await _imageService.DeleteImageAsync(imagePublicId);
                }

                var imageUrl = await _imageService.UploadImageAsync(request.Cuisine.ImageFile);
                foundCuisine.ImageUrl = imageUrl;
            }

            _mapper.Map(request.Cuisine, foundCuisine);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return _mapper.Map<CuisineDto>(foundCuisine);
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var urlParts = imageUrl.Split('/');
            var publicId = urlParts[urlParts.Length - 1].Split('.')[0];
            return publicId;
        }
    }
}
