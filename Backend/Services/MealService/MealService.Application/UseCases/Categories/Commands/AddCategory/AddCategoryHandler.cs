using AutoMapper;
using MealService.Application.DTOs.Categories;
using MealService.Application.Specifications;
using MealService.Domain.Entities;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.AddCategory
{
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddCategoryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = _mapper.Map<Category>(request.Category);
            newCategory.Id = Guid.NewGuid();

            await _unitOfWork.CategoryRepository.AddAsync(newCategory, cancellationToken);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            var addedCategory = _mapper.Map<CategoryDto>(newCategory);

            return addedCategory;
        }
    }
}
