using AutoMapper;
using MealService.Application.Specifications.Services;
using MealService.Application.Specifications;
using MediatR;
using MealService.Application.Exceptions;

namespace MealService.Application.UseCases.Cuisines.Commands.DeleteCuisine
{
    public class DeleteCuisineHandler : IRequestHandler<DeleteCuisineCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteCuisineHandler(IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(DeleteCuisineCommand request, CancellationToken cancellationToken)
        {
            var cuisine = await _unitOfWork.CuisineRepository.GetByIdAsync(request.Id, cancellationToken);

            if (cuisine is null)
            {
                throw new NotFoundException($"Cuisine with id {request.Id} dosen't exist.");
            }

            bool hasMeals = await _unitOfWork.MealRepository.AnyAsync(m => m.CuisineId == request.Id, cancellationToken);

            if (hasMeals)
            {
                throw new ConflictException("Delete operation failed.", $"Cuisine with id {request.Id} can't be deleted as it has associated meals.");
            }

            if (cuisine.ImageUrl != _imageService.GetDefaultImageUrl())
            {
                var imagePublicId = _imageService.GetPublicIdFromUrl(cuisine.ImageUrl);
                await _imageService.DeleteImageAsync(imagePublicId);
            }

            await _unitOfWork.CuisineRepository.Delete(cuisine);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
