using AutoMapper;
using MealService.Application.DTOs;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.ListAllAsync(cancellationToken);
            var categoriesDtos = _mapper.Map<List<CategoryDto>>(categories);

            return categoriesDtos;
        }
    }
}
