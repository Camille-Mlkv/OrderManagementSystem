using AutoMapper;
using MealService.Application.Exceptions;
using MealService.Application.Specifications;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category is null)
            {
                throw new NotFoundException($"Category with id {request.Id} dosen't exist.");
            }

            bool hasMeals = await _unitOfWork.MealRepository.AnyAsync(m => m.CategoryId == request.Id, cancellationToken);
            if (hasMeals)
            {
                throw new ConflictException("Delete operation failed.", $"Category with id {request.Id} can't be deleted as it has associated meals.");
            }

            await _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveAllAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
