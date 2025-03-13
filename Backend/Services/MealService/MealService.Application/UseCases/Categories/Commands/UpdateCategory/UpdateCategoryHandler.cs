using AutoMapper;
using MealService.Application.DTOs.Categories;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var foundCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (foundCategory is null)
            {
                throw new NotFoundException($"Category with id {request.Id} doesn't exist.");
            }

            _mapper.Map(request.Category, foundCategory); 

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(foundCategory);
        }
    }
}
