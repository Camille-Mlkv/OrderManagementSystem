using Grpc.Core;
using CartService.Application.Specifications;

namespace CartService.GrpcServer.Services
{
    public class CartServiceImpl : CartService.CartServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartServiceImpl> _logger;
        public CartServiceImpl(IUnitOfWork unitOfWork, ILogger<CartServiceImpl> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public override async Task<GetCartByUserIdReply> GetCartContent(GetCartByUserIdRequest request, ServerCallContext context)
        {
            Guid userGuidId;
            var isIdValid = Guid.TryParse(request.UserId, out userGuidId);

            if (!isIdValid)
            {
                _logger.LogError("Failed to parse user id into GUID.");

                throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is not valid"));
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
    }
}
