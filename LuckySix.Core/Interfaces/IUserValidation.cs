using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Core.Interfaces
{
  public interface IUserValidation
  {

    bool CheckLogin(string username, string password);
    bool CheckPasswordLength(string password);

    bool CheckFirstNameAndLastName(string FirstName, string LastName);

    bool IsValidBalance(decimal balance);

  }
}
