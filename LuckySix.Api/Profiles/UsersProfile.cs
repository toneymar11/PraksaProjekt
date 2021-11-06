using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Entities;

namespace LuckySix.Api.Profiles
{
  public class UsersProfile : Profile
  {
    public UsersProfile()
    {
      CreateMap<User, UserDto>();
      CreateMap<UserLogin, User>();
      CreateMap<UserRegister, User>();

    }
  }
}
