using LuckySix.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class TicketValidation : ITicketValidation
  {
    private readonly IUserRepository userRepository;
    public TicketValidation()
    {
      userRepository = new UserRepository();
    }

    public async Task<bool> IsPossibleBetting(decimal stake, int userId)
    {
      var user = await userRepository.GetUser(userId);
      decimal balance = user.Balance;
      if(stake > balance)
      {
        return false;
      }

      return true;
    }

    public bool IsValidSelectedNumbers(string numbers)
    {
      Console.WriteLine(numbers);
      if (numbers.Length != 11) return false;
      for (int i = 0; i < numbers.Length - 1; i += 2)
      {

        for (int j = i + 2; j < numbers.Length; j += 2)
        {
          if (numbers[i] == numbers[j])
          {
            Console.WriteLine($"I:{i} J:{j}");
            return false;
          }
        }
      }

      return true;
    }

    public bool IsValidStake(decimal stake)
    {
      if (stake < 1 || stake > 100 || stake.Equals(null)) return false;

      return true;

    }
  }
}
