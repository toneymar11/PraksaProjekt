using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Utilities;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class RoundRepository : Repository, IRoundRepository
  {

    #region ctor
    public RoundRepository() : base() {}
    #endregion

    #region implementation
    public async Task<Round> GetReadyRound()
    {
      Round round = null;
      cmd = CreateProcedure("GetReadyRound");

      await sql.OpenAsync();

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETER
      cmd.Parameters.Add(responseMessage);

      reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        round = HelpFunctions.MapToRound(reader);
      }
      await reader.CloseAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      return round;
    }

    public async Task<Round> GetRunningRound()
    {
      Round round = null;
      cmd = CreateProcedure("GetRunningRound");

      await sql.OpenAsync();

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETER
      cmd.Parameters.Add(responseMessage);

      reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        round = HelpFunctions.MapToRound(reader);
      }
      await reader.CloseAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      return round;
    }

    #endregion
  }
}
