using Grpc.Core;
using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;

namespace CartService.GrpcServer.Services
{
    public class CartServiceImpl : CartService.CartServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartServiceImpl> _logger;
        private readonly ICartJobService _jobService;
        public CartServiceImpl(IUnitOfWork unitOfWork, ILogger<CartServiceImpl> logger, ICartJobService jobService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jobService = jobService;
        }

        public override async Task<GetCartByUserIdReply> GetCartContent(GetCartByUserIdRequest request, ServerCallContext context)
        {
            var isIdValid = Guid.TryParse(request.UserId, out Guid userGuidId);

            if (!isIdValid)
            {
                _logger.LogError("Failed to parse user id into GUID.");

                throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is not valid."));
            }

            var cart = await _unitOfWork.CartRepository.GetCartAsync(userGuidId, CancellationToken.None);

            if (cart is null)
            {
                return new GetCartByUserIdReply();
            }

            var reply = new GetCartByUserIdReply();

            reply.Items.AddRange(cart.Items.Select(item => new CartItemReply
            {
                MealId = item.MealId.ToString(),
                Quantity = item.Quantity
            }));

            return reply;
        }

        public override async Task<Empty> ClearCart(ClearCartRequest request, ServerCallContext context)
        {
            var isIdValid = Guid.TryParse(request.UserId, out Guid userGuidId);

            if (!isIdValid)
            {
                _logger.LogError("Failed to parse user id into GUID.");

                throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is not valid."));
            }

            await _unitOfWork.CartRepository.DeleteCartAsync(userGuidId, CancellationToken.None);

            await _jobService.DeleteJobAsync(userGuidId, CancellationToken.None);

            return new Empty();
        }
    }
}
