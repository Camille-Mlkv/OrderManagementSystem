namespace UserService.BusinessLogic.Exceptions
{
    public class NotFoundException:CustomException
    {
        public NotFoundException(string message, string? details = null) : base(message, details) { }
    }
}
