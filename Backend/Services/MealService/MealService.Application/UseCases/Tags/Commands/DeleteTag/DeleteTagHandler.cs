using AutoMapper;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.DeleteTag
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.TagRepository.GetByIdAsync(request.Id, cancellationToken);

            if (tag is null)
            {
                throw new NotFoundException($"Tag with id {request.Id} doesn't exist.");
            }

            await _unitOfWork.TagRepository.Delete(tag);

            await _unitOfWork.SaveAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
