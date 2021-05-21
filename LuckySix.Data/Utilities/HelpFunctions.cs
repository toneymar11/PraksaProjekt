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

    public static Ticket MaptoTicket(SqlDataReader reader)
    {
      return new Ticket()
      {
        IdTicket = (int)reader["id_ticket"],
        IdUser = (int)reader["user_id"],
        Balance = (decimal) reader["balance"],
        DrawnNum = reader["drawn_num"].ToString(),
        IdRound = (int)reader["round_id"],
        SelectedNum = reader["selected_num"].ToString(),
        SelectedNumDrawn = reader["selected_num_drawn"].ToString(),
        Stake = (decimal)reader["stake"],
        Payout = (decimal)reader["payout"],
        Won = (byte)reader["won"]

      };
    }

    public static Round MapToRoundForStats(SqlDataReader reader)
    {
      return new Round()

      {
        IdRound = (int)reader["id_round"],
        DrawnNum = reader["drawn_num"].ToString(),
       
      };

    }
  }
}
