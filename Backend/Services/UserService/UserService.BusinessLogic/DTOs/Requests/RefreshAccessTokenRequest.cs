namespace UserService.BusinessLogic.DTOs.Requests
{
    public class RefreshAccessTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
