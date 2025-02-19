namespace UserService.BusinessLogic.Exceptions
{
    public class UnauthorizedException:CustomException
    {
        public UnauthorizedException(string message, string? details = null) : base(message, details) { }
    }
}
