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
    void SaveToken(int idUser, string token);
    Task<User> IsTokenValid(int userId, string token);

    bool Logout(int userId, string token);
  }
}
