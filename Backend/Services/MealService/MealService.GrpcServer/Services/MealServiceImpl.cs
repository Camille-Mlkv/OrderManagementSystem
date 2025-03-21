using Grpc.Core;
using MealService.Application.Specifications;

namespace MealService.GrpcServer.Services
{
    public class MealServiceImpl : MealService.MealServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MealServiceImpl> _logger;
        public MealServiceImpl(IUnitOfWork unitOfWork, ILogger<MealServiceImpl> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task<GetMealByIdReply> GetMealById(GetMealByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.MealId, out var mealGuid))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid meal ID format"));
            }

            var meal = await _unitOfWork.MealRepository.GetByIdAsync(mealGuid, CancellationToken.None);

            if (meal is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Meal not found"));
            }

            return new GetMealByIdReply
            {
                MealId = meal.Id.ToString(),
                Name = meal.Name,
                Price = meal.Price
            };
        }
    }
}
