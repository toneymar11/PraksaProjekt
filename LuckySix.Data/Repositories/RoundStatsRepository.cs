using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class RoundStatsRepository : Repository, IRoundStatsRepository
  {

    #region ctor
    public RoundStatsRepository() : base() { }
    #endregion


    #region implementation
    public RoundStats CalculateNumbersCount(List<Round> rounds)
    {
      RoundStats roundStats = new RoundStats();
      // define counter for 48 numbers
      int[] counter = Enumerable.Repeat(0, 48).ToArray();

      for (int i = 0; i < rounds.Count; i++)
      {
        var round = rounds[i].DrawnNum.Split(',').Select(Int32.Parse).ToList();

        for (int j = 0; j < round.Count; j++)
        {
          counter[round[j] - 1]++;
        }
      }

      for (int i = 0; i < counter.Length; i++)
      {
        roundStats.numbersCount.Add((i + 1).ToString(), counter[i]);
      }

      return roundStats;
    }

    public async Task<RoundStats> GetRoundStats(int nround)
    {
      //RoundStats roundStats = new RoundStats();
      List<Round> rounds = new List<Round>();

      cmd = CreateProcedure("GetLastRounds");

      // INPUT PARAMETERS
      nRound = IntegerParameter("@nRound", nround);

      // ADD PARAMETER
      cmd.Parameters.Add(nRound);

      await sql.OpenAsync();

      reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        rounds.Add(HelpFunctions.MapToRoundForStats(reader));
      }
      await reader.CloseAsync();

      await sql.CloseAsync();

      var roundStats = CalculateNumbersCount(rounds);
      return roundStats;
    }
    #endregion

  }
}
