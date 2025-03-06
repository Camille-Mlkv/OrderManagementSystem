using AutoMapper;
using MealService.Application.DTOs.Tags;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Tags.Queries.GetTags
{
    public class GetTagsHandler : IRequestHandler<GetTagsQuery, List<TagDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetTagsHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _unitOfWork.TagRepository.ListAllAsync(cancellationToken);
            var tagDtos = _mapper.Map<List<TagDto>>(tags);

            return tagDtos;
        }
    }
}
