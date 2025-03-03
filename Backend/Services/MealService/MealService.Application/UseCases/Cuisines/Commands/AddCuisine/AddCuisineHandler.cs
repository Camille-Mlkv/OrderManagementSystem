using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Application.Specifications.Services;
using MealService.Application.Specifications;
using MediatR;
using MealService.Domain.Entities;

namespace MealService.Application.UseCases.Cuisines.Commands.AddCuisine
{
    public class AddCuisineHandler : IRequestHandler<AddCuisineCommand, CuisineDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public AddCuisineHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<CuisineDto> Handle(AddCuisineCommand request, CancellationToken cancellationToken)
        {
            var newCuisine = _mapper.Map<Cuisine>(request.Cuisine);
            newCuisine.Id = Guid.NewGuid();

            if (request.Cuisine.ImageFile != null)
            {
                var imageUrl = await _imageService.UploadImageAsync(request.Cuisine.ImageFile);
                newCuisine.ImageUrl = imageUrl;
            }
            else
            {
                newCuisine.ImageUrl = _imageService.GetDefaultImageUrl();
            }

            await _unitOfWork.CuisineRepository.AddAsync(newCuisine, cancellationToken);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            var addedCuisine = _mapper.Map<CuisineDto>(newCuisine);

            return addedCuisine;
        }
    }
}
