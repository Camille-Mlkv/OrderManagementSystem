using UserService.DataAccess.Specifications.Repositories;

namespace UserService.DataAccess.Specifications
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public Task SaveAllAsync(CancellationToken cancellationToken = default);
    }
}
