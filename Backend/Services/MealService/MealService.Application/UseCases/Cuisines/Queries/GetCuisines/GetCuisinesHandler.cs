using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisines
{
    public class GetCuisinesHandler : IRequestHandler<GetCuisinesQuery, List<CuisineDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCuisinesHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CuisineDto>> Handle(GetCuisinesQuery request, CancellationToken cancellationToken)
        {
            var cuisines = await _unitOfWork.CuisineRepository.ListAllAsync(cancellationToken);
            var cuisinesDtos = _mapper.Map<List<CuisineDto>>(cuisines);

            return cuisinesDtos;
        }
    }
}
