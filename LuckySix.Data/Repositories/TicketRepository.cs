using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Database;
using LuckySix.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class TicketRepository : ITicketRepository
  {

    public SqlConnection sql;
    public TicketRepository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.connectionString);

    }
    public async Task<Ticket> CreateTicket(Ticket ticket)
    {
     
      SqlCommand cmd = new SqlCommand("CreateTicket", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter userId = new SqlParameter("@p_userId", SqlDbType.Int) { Value = ticket.IdUser };
      SqlParameter selectedNum = new SqlParameter("@p_selectedNum", SqlDbType.VarChar) { Value = ticket.SelectedNum };
      SqlParameter stake = new SqlParameter("@p_stake", SqlDbType.Decimal) { Value = ticket.Stake };

      // OUTPUT PARAMETERS
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };
      SqlParameter newId = new SqlParameter("@newId", SqlDbType.Int) { Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(selectedNum);
      cmd.Parameters.Add(stake);
      cmd.Parameters.Add(responseMessage);
      cmd.Parameters.Add(newId);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      Ticket newTicket = await GetTicket((int)newId.Value);

      string selectedNumDrawn = IsUserWinTicket(newTicket.SelectedNum, newTicket.DrawnNum);

      Console.WriteLine(selectedNumDrawn);

      return newTicket;
      
    }

    public async Task<Ticket> GetTicket(int ticketId)
    {
      Ticket ticket = null;
      SqlCommand cmd = new SqlCommand("GetTicket", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();
      // INPUT PARAMETER
      SqlParameter idTicket = new SqlParameter("@pidTicket", SqlDbType.Int) { Value = ticketId };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(idTicket);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        ticket = HelpFunctions.MaptoTicket(reader);
      }
      await reader.CloseAsync();


      await sql.CloseAsync();

      return ticket;
    }
    // NEED TO FIX THIS FUNCTION
    public string IsUserWinTicket(string selectedNum, string DrawnNum)
    {
      var selectedNumbers = selectedNum.Split(',').Select(Int32.Parse).ToList(); 

      var drawnNumbers = DrawnNum.Split(',').Select(Int32.Parse).ToList();

      string selectedNumDrawn = "";
      for(int i=0; i<drawnNumbers.Count; i++)
      {
        for(int j=0; j<selectedNumbers.Count; j++)
        {
          if(drawnNumbers[i] == selectedNumbers[j])
          {
            Console.WriteLine(selectedNumbers[j]);
            if (j != selectedNumbers.Count - 1)
            {
              selectedNumDrawn += drawnNumbers[i].ToString() + ",";
            }
            selectedNumDrawn += drawnNumbers[i].ToString();
            selectedNumbers.RemoveAt(j);
            continue;

          }
        }
      }

      return selectedNumDrawn;
    }
  }
}
