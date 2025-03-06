using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisinesByName
{
    public class GetCuisinesByNameHandler : IRequestHandler<GetCuisinesByNameQuery, List<CuisineDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCuisinesByNameHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CuisineDto>> Handle(GetCuisinesByNameQuery request, CancellationToken cancellationToken)
        {
            var cuisines = await _unitOfWork.CuisineRepository.ListAsync(c => c.Name.StartsWith(request.Name), cancellationToken);
            var cuisinesDtos = _mapper.Map<List<CuisineDto>>(cuisines);

            return cuisinesDtos;
        }
    }
}
