using AutoMapper;
using UserService.BusinessLogic.DTOs;
using UserService.BusinessLogic.DTOs.Requests;
using UserService.DataAccess.Models;

namespace UserService.BusinessLogic.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, ApplicationUser>().ReverseMap();
            CreateMap<RegistrationRequest,ApplicationUser>().ReverseMap();
        }
    }
}
