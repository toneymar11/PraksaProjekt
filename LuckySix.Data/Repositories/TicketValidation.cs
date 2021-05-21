using LuckySix.Core.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class TicketValidation : ITicketValidation
  {
    private readonly IUserRepository userRepository;

    #region ctor
    public TicketValidation()
    {
      userRepository = new UserRepository();
    }
    #endregion

    #region implementation
    public async Task<bool> IsPossibleBetting(decimal stake, int userId)
    {
      var user = await userRepository.GetUser(userId);
      decimal balance = user.Balance;
      if (stake > balance)
      {
        return false;
      }

      return true;
    }

    public bool IsValidSelectedNumbers(string numbers)
    {

      if (!IsStringInFormat(numbers)) return false;
      if (numbers.Length < 11) return false;

      var selectedNum = numbers.Split(',').Select(Int32.Parse).ToList();
      if (selectedNum.Count != 6) return false;
      for (int i = 0; i < selectedNum.Count - 1; i += 1)
      {
        if (selectedNum[i] < 1 || selectedNum[i] > 48) return false;
        for (int j = i + 1; j < selectedNum.Count; j += 1)
        {
          if (selectedNum[i] == selectedNum[j]) return false;
        }
      }

      return true;
    }

    public bool IsStringInFormat(string str)
    {

      int counter = 0, counter1 = 0;

      Regex rgx = new Regex(@"\D");

      foreach (Match match in rgx.Matches(str))
      {
        counter1++;

      }

      for (int i = 0; i < str.Length; i++)
      {
        if (str[i] == ',')
        {
          if (Char.IsDigit(str[i - 1]) && Char.IsDigit(str[i + 1]))
          {
            counter++;
          }

        }
      }

      if (counter != 5 || counter1 != 5)
      {
        return false;
      }

      return true;
    }

    #endregion

  }
}

