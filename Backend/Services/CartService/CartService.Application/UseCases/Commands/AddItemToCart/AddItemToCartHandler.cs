using AutoMapper;
using CartService.Application.Specifications;
using CartService.Application.Specifications.Jobs;
using CartService.Domain.Entities;
using MediatR;

using CartService.Application.Exceptions;

namespace CartService.Application.UseCases.Commands.AddItemToCart
{
    public class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICartJobService _jobService;
        private readonly MealService.GrpcServer.MealService.MealServiceClient _grpcClient;

        public AddItemToCartHandler(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ICartJobService jobService,
            MealService.GrpcServer.MealService.MealServiceClient client)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jobService = jobService;
            _grpcClient = client;
        }

        public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var mealId = request.Item.MealId.ToString();

            var mealRequest = new MealService.GrpcServer.GetMealByIdRequest{ MealId = mealId };

            var response = await _grpcClient.CheckIfMealExistsAsync(mealRequest);

            if (!response.Exists)
            {
                throw new NotFoundException("Failed to add item to the cart.",$"Meal with Id {mealId} wasn't found.");
            }

            var cart = await _unitOfWork.CartRepository.GetCartAsync(request.UserId, cancellationToken);

            if (cart is null)
            {
                cart = new Cart { UserId = request.UserId, Items = new List<CartItem>() };
            }

            var newItem = _mapper.Map<CartItem>(request.Item);
            cart.AddItem(newItem);
            await _unitOfWork.CartRepository.SaveCartAsync(cart, cancellationToken);

            // delete old job if exists
            await _jobService.DeleteJobAsync(request.UserId, cancellationToken);

            // schedule new job
            await _jobService.ScheduleJobAsync(request.UserId, cancellationToken); 
        }
    }
}
