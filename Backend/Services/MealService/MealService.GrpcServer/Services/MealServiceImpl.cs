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

        public override async Task<GetMealByIdReply> CheckIfMealExists(GetMealByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GRPC meal service request started");

            Guid mealGuidId;
            var isIdValid = Guid.TryParse(request.MealId, out mealGuidId);

            if (!isIdValid)
            {
                _logger.LogError("Failed to parse meal id into GUID.");

                throw new RpcException(new Status(StatusCode.InvalidArgument, "Meal id is not valid"));
            }

            var meal = await _unitOfWork.MealRepository.GetByIdAsync(mealGuidId, context.CancellationToken);

            if (meal is null)
            {
                _logger.LogError("GRPC meal service request failed: Meal not found");

                return new GetMealByIdReply
                {
                    Exists = false
                };
            }

            return new GetMealByIdReply
            {
                Exists = true
            };
        }
    }
}
