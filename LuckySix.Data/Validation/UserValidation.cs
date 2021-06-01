using LuckySix.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckySix.Data.Validation
{
  public class UserValidation : IUserValidation
  {

    public UserValidation()
    {

    }
    public bool CheckFirstNameAndLastName(string FirstName, string LastName)
    {
      if (FirstName.Equals("") || LastName.Equals("")) return false;

      if (FirstName.Any(char.IsDigit) || LastName.Any(char.IsDigit)) return false;

      return !(FirstName.Equals(LastName));
    }
    public bool CheckLogin(string username, string password)
    {
     if(username.Equals("") || username == null || password.Equals("") || password == null)
      {
        return false;
      }
      return CheckPasswordLength(password);
    }

    public bool CheckPasswordLength(string password)
    {
      return (password.Length >= 8);
    }

    public bool IsValidBalance(decimal balance)
    {
      return (balance >= 5 && balance <= 100) && !balance.Equals(null);
    }
  }
}
