namespace UserService.BusinessLogic.Specifications.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        public Task SaveAllAsync(CancellationToken cancellationToken = default);
    }
}
