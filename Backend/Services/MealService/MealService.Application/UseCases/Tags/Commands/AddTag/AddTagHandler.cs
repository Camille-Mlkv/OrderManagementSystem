using AutoMapper;
using MealService.Application.DTOs.Tags;
using MealService.Application.Specifications;
using MealService.Domain.Entities;
using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.AddTag
{
    public class AddTagHandler : IRequestHandler<AddTagCommand, TagDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddTagHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TagDto> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            var newTag = _mapper.Map<Tag>(request.Tag);
            newTag.Id = Guid.NewGuid();

            await _unitOfWork.TagRepository.AddAsync(newTag, cancellationToken);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            var addedTag = _mapper.Map<TagDto>(newTag);

            return addedTag;
        }
    }
}
