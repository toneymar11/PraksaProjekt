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
  public class RoundRepository : IRoundRepository
  {

    public SqlConnection sql;

    public RoundRepository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.connectionString);
    }
    public async Task<Round> GetRound(int idRound)
    {
      Round round = null;
      SqlCommand cmd = new SqlCommand("GetRound", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();
      // INPUT PARAMETER
      SqlParameter roundId = new SqlParameter("@roundId", SqlDbType.Int) { Value = idRound };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(roundId);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        round = HelpFunctions.MapToRound(reader);
      }

      await reader.CloseAsync();
      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return null;
      }

      return round;
    }

    public bool IsRoundExists(int idRound)
    {
      SqlCommand cmd = new SqlCommand("IsRoundExists", sql) { CommandType = CommandType.StoredProcedure };

      sql.Open();

      // INPUT PARAMETER
      SqlParameter roundId = new SqlParameter("@pidRound", SqlDbType.Int) { Value = idRound };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(roundId);
      cmd.Parameters.Add(responseMessage);

      cmd.ExecuteNonQuery();

      sql.Close();

      return (responseMessage.Value.ToString().Equals("Round exists"));
    }
  }
}
