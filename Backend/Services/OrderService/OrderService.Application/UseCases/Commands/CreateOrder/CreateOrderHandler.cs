using MediatR;
using OrderService.Application.Specifications.Repositories;
using OrderService.Domain.Entities.OrderComponents;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using AutoMapper;
using OrderService.Application.Utilities;

namespace OrderService.Application.UseCases.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly OrderNumberGenerator _orderNumberGenerator;

        public CreateOrderHandler(IOrderRepository orderRepository, IMapper mapper, OrderNumberGenerator orderNumberGenerator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderNumberGenerator = orderNumberGenerator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // get cart via gRPC

            // get cart meals via gRPC

            var address = _mapper.Map<Address>(request.Address);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderNumber = _orderNumberGenerator.GenerateOrderNumber(),
                ClientId = request.ClientId,
                Status = new Status { Name = StatusName.Pending },
                Address = address,
                Meals = new List<OrderMeal>(),
                CreatedAt = DateTime.UtcNow
            };

            order.CalculateTotalPrice();

            await _orderRepository.CreateAsync(order);

            return order.Id;
        }
    }
}
