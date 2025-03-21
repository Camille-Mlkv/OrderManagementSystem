using MediatR;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Entities.OrderComponents;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using AutoMapper;
using OrderService.Application.Utilities;
using OrderService.Application.Exceptions;
using MealService.GrpcServer;

namespace OrderService.Application.UseCases.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly OrderNumberGenerator _orderNumberGenerator;
        private readonly CartService.GrpcServer.CartService.CartServiceClient _cartClient;
        private readonly MealService.GrpcServer.MealService.MealServiceClient _mealClient;

        public CreateOrderHandler(
            IOrderRepository orderRepository, 
            IMapper mapper, 
            OrderNumberGenerator orderNumberGenerator,
            CartService.GrpcServer.CartService.CartServiceClient cartClient,
            MealService.GrpcServer.MealService.MealServiceClient mealClient)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderNumberGenerator = orderNumberGenerator;
            _cartClient = cartClient;
            _mealClient = mealClient;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // get cart via gRPC

            var clientId = request.ClientId.ToString();

            var cartContentRequest = new CartService.GrpcServer.GetCartByUserIdRequest() { UserId = clientId };

            var response = await _cartClient.GetCartContentAsync(cartContentRequest);

            var cartItems = response.Items.ToList();

            if(cartItems.Count() == 0)
            {
                throw new BadRequestException("Bad request.", "You can't create an order from an empty cart.");
            }

            // get cart meals via gRPC

            var orderMeals = new List<OrderMeal>();

            foreach (var item in cartItems)
            {
                var mealRequest = new GetMealByIdRequest { MealId = item.MealId };
                var mealResponse = await _mealClient.GetMealByIdAsync(mealRequest);

                orderMeals.Add(new OrderMeal
                {
                    Id = Guid.Parse(mealResponse.MealId),
                    Name = mealResponse.Name,
                    Price =(decimal)mealResponse.Price,
                    PortionsAmount = item.Quantity
                });
            }

            var address = _mapper.Map<Address>(request.Address);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = _orderNumberGenerator.GenerateOrderNumber(),
                ClientId = request.ClientId,
                Status = new Status { Name = StatusName.Pending },
                Address = address,
                Meals = orderMeals,
                CreatedAt = DateTime.UtcNow
            };

            order.CalculateTotalPrice();

            await _orderRepository.CreateAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
