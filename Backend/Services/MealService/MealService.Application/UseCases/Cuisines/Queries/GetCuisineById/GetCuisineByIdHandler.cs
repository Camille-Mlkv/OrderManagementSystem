using AutoMapper;
using MealService.Application.DTOs.Cuisines;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisineById
{
    public class GetCuisineByIdHandler : IRequestHandler<GetCuisineByIdQuery, CuisineDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCuisineByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CuisineDto> Handle(GetCuisineByIdQuery request, CancellationToken cancellationToken)
        {
            var cuisine = await _unitOfWork.CuisineRepository.GetByIdAsync(request.Id, cancellationToken);
            var cuisineDto = _mapper.Map<CuisineDto>(cuisine);

            return cuisineDto;
        }
    }
}
