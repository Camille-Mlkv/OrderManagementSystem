using AutoMapper;
using MealService.Application.DTOs.Categories;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }
    }
}
