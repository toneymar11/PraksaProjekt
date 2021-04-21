using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface IUserRepository
  {

    Task<User> RegisterUser(User user);
    Task<User> GetUser(int userId);
    Task<User> UpdateUser(User user, int idUser);

    Task<bool> MakeADeposit(decimal balance, int idUser);
    Task<bool> IsUserExists(string username);
  }
}
