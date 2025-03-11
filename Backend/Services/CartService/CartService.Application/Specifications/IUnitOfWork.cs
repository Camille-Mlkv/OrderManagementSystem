using CartService.Application.Specifications.Repositories;

namespace CartService.Application.Specifications
{
    public interface IUnitOfWork
    {
        ICartRepository CartRepository { get; }

        IJobRepository CartJobRepository { get; }
    }
}
