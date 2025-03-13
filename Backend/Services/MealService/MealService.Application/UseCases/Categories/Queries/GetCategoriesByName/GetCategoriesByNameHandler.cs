using AutoMapper;
using MealService.Application.DTOs.Categories;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategoriesByName
{
    public class GetCategoriesByNameHandler : IRequestHandler<GetCategoriesByNameQuery, List<CategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesByNameHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesByNameQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.ListAsync(c=>c.Name.StartsWith(request.Name),cancellationToken);
            var categoriesDtos = _mapper.Map<List<CategoryDto>>(categories);

            return categoriesDtos;
        }
    }
}
