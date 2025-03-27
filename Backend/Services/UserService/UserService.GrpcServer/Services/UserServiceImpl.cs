using Grpc.Core;
using UserService.DataAccess.Specifications;

namespace UserService.GrpcServer.Services
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<UserReply> GetUserEmailById(UserRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out _))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID format"));
            }

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId, CancellationToken.None);

            if (user is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User with given id doesn't exist."));
            }

            var response = new UserReply()
            {
                Email = user.Email,
            };

            return response;
        }
    }
}
