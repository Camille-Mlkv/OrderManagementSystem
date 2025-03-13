using AutoMapper;
using MealService.Application.DTOs.Tags;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.UpdateTag
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, TagDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTagHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TagDto> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var foundTag = await _unitOfWork.TagRepository.GetByIdAsync(request.TagId,cancellationToken);

            if (foundTag is null)
            {
                throw new NotFoundException($"Tag with id {request.TagId} doesn't exist.");
            }

            _mapper.Map(request.Tag, foundTag);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return _mapper.Map<TagDto>(foundTag);
        }
    }
}
