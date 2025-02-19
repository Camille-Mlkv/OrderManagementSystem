using UserService.BusinessLogic.Specifications.Repositories;
using UserService.DataAccess.Data;

namespace UserService.BusinessLogic.Implementations.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public IUserRepository UserRepository => _userRepository;
        public async Task SaveAllAsync(CancellationToken cancellationToken = default) => 
            await _context.SaveChangesAsync(cancellationToken);
    }
}
