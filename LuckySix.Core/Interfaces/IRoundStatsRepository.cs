

using LuckySix.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface IRoundStatsRepository
  {

    Task<RoundStats> GetRoundStats(int nround);

    RoundStats CalculateNumbersCount(List<Round> rounds);

  }
}
