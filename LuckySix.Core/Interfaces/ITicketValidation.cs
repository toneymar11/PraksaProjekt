
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface ITicketValidation
  {

    bool IsValidSelectedNumbers(string numbers);

   

    Task<bool> IsPossibleBetting(decimal stake, int userId);

    bool IsStringInFormat(string str);
  }
}
