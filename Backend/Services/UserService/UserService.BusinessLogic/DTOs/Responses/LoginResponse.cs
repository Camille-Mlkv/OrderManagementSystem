﻿namespace UserService.BusinessLogic.DTOs.Responses
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
