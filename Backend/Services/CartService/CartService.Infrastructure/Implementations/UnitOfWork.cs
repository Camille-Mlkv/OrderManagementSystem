using CartService.Application.Specifications;
using CartService.Application.Specifications.Repositories;

namespace CartService.Infrastructure.Implementations
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ICartRepository _cartRepository;
        private readonly IJobRepository _jobRepository;

        public UnitOfWork(ICartRepository cartRepository, IJobRepository jobRepository)
        {
            _cartRepository = cartRepository;
            _jobRepository = jobRepository;
        }
        public ICartRepository CartRepository => 
           _cartRepository;

        public IJobRepository CartJobRepository => 
           _jobRepository;
    }
}
