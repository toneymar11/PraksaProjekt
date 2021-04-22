using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Database;
using LuckySix.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class RoundRepository : Repository, IRoundRepository
  {


    public RoundRepository() : base()
    {

    }

    public async Task<Round> GetReadyRound()
    {
      Round round = null;
      cmd = CreateProcedure("GetReadyRound");

      await sql.OpenAsync();

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETER
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
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
      cmd = new SqlCommand("GetRunningRound", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETER
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        round = HelpFunctions.MapToRound(reader);
      }
      await reader.CloseAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      return round;
    }
  }
}
