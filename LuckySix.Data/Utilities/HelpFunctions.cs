using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LuckySix.Data.Utilities
{
  public class HelpFunctions
  {
    public static User MapToUser(SqlDataReader reader)
    {
      return new User()

      {
        IdUser = (int)reader["id_user"],
        Username = reader["username"].ToString(),
        FirstName = reader["first_name"].ToString(),
        LastName = reader["last_name"].ToString(),
        Balance = (decimal)reader["balance"],
        Token = (string)reader["token"]
      };

    }

    public static Round MapToRound(SqlDataReader reader)
    {
      return new Round()

      {
        IdRound = (int)reader["id_round"],
        DrawnNum = reader["drawn_num"].ToString(),
        StartRoundTime = (DateTime)reader["start_round_time"],
        FinishRoundTime = (DateTime)reader["finish_round_time"]
      };

    }
  }
}
