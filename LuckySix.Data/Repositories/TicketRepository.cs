using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Core.TicketCalculation;
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

      var ticketStatus = IsUserWinTicket(newTicket.SelectedNum, newTicket.DrawnNum);


      newTicket.Payout = ticketStatus.Coefficient * newTicket.Stake;
      if (newTicket.SelectedNum.Length != ticketStatus.SelectedNumDrawn.Length)
      {
        ticketStatus.SelectedNumDrawn = ticketStatus.SelectedNumDrawn.Remove(ticketStatus.SelectedNumDrawn.Length - 1, 1);
        newTicket.SelectedNumDrawn = ticketStatus.SelectedNumDrawn;
        newTicket.Won = 0;
      }
      else
      {
        newTicket.SelectedNumDrawn = ticketStatus.SelectedNumDrawn;
        newTicket.Won = 1;
      }


      bool update = await UpdateTicket(newTicket);

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

    public TicketStatus IsUserWinTicket(string selectedNum, string DrawnNum)
    {
      var selectedNumbers = selectedNum.Split(',').Select(Int32.Parse).ToList();

      var drawnNumbers = DrawnNum.Split(',').Select(Int32.Parse).ToList();

      TicketStatus ticket = new TicketStatus
      {
        SelectedNumDrawn = "",
        Index = 0
      };

      // control variable
      int k = 0;

      //string selectedNumDrawn = "";
      for (int i = 0; i < drawnNumbers.Count; i++)
      {
        for (int j = 0; j < selectedNumbers.Count; j++)
        {
          if (drawnNumbers[i] == selectedNumbers[j])
          {

            //Console.WriteLine(selectedNumbers[j]);
            if (selectedNumbers.Count == 1)
            {
              ticket.SelectedNumDrawn = string.Concat(ticket.SelectedNumDrawn, selectedNumbers[j].ToString());
              k = 1;
              // last number index
              ticket.Index = i;
              ticket.Update();
            }
            else
            {
              ticket.SelectedNumDrawn = string.Concat(ticket.SelectedNumDrawn, selectedNumbers[j].ToString() + ",");
            }
            selectedNumbers.RemoveAt(j);
            break;

          }
        }

        if (k == 1)
        {
          break;
        }
      }

      return ticket;
    }

    public async Task<bool> UpdateTicket(Ticket ticket)
    {
      SqlCommand cmd = new SqlCommand("UpdateTicket", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter ticketId = new SqlParameter("@ticketId", SqlDbType.Int) { Value = ticket.IdTicket };
      SqlParameter selectedNumDrawn = new SqlParameter("@SelectedNumDrawn", SqlDbType.VarChar) { Value = ticket.SelectedNumDrawn };
      SqlParameter won = new SqlParameter("@Won", SqlDbType.TinyInt) { Value = ticket.Won };
      SqlParameter payout = new SqlParameter("@Payout", SqlDbType.Decimal) { Value = ticket.Payout };


      //ADDING PARAMETERS
      cmd.Parameters.Add(ticketId);
      cmd.Parameters.Add(selectedNumDrawn);
      cmd.Parameters.Add(won);
      cmd.Parameters.Add(payout);


      await cmd.ExecuteNonQueryAsync();


      await sql.CloseAsync();


      return true;
    }
  }
}
