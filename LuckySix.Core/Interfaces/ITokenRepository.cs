using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface ITokenRepository
  {

    Task<User> LogIn(User user);
    Task SaveToken(int userId, string token);
    Task<User> IsTokenValid(int userId, string token);

    Task<bool> Logout(int userId, string token);
  }
}
