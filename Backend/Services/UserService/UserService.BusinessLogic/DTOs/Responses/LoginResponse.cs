namespace UserService.BusinessLogic.DTOs.Responses
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
