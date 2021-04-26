using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface ITicketValidation
  {

    bool IsValidSelectedNumbers(string numbers);

    bool IsValidStake(decimal stake);

    Task<bool> IsPossibleBetting(decimal stake, int userId);

    bool IsStringInFormat(string str);
  }
}
