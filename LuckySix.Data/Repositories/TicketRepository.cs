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
  public class TicketRepository : Repository, ITicketRepository
  {
    public TicketRepository() : base() { }

    public async Task<Ticket> CreateTicket(Ticket ticket)
    {

      cmd = CreateProcedure("CreateTicket");

      await sql.OpenAsync();

      // INPUT PARAMETERS
      IdUser = IntegerParameter("@p_userId", ticket.IdUser);
      selectedNum = StringParameter("@p_selectedNum", ticket.SelectedNum);
      stake = DecimalParameter("@p_stake", ticket.Stake);

      // OUTPUT PARAMETERS
      responseMessage = ResponseMessage();
      NewId = IntegerOutput("@newId");

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(selectedNum);
      cmd.Parameters.Add(stake);
      cmd.Parameters.Add(responseMessage);
      cmd.Parameters.Add(NewId);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      Ticket newTicket = await GetTicket((int)NewId.Value);

      var ticketStatus = IsUserWinTicket(newTicket.SelectedNum, newTicket.DrawnNum);

      // Structure data
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

      // Save data to database
      bool update = await UpdateTicket(newTicket);

      return newTicket;

    }

    public async Task<Ticket> GetTicket(int ticketId)
    {
      Ticket ticket = null;
      cmd = CreateProcedure("GetTicket");

      await sql.OpenAsync();

      // INPUT PARAMETER
      idTicket = IntegerParameter("@pidTicket", ticketId);

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(idTicket);
      cmd.Parameters.Add(responseMessage);

      reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        ticket = HelpFunctions.MaptoTicket(reader);
      }
      await reader.CloseAsync();


      await sql.CloseAsync();

      return ticket;
    }



    public async Task<bool> UpdateTicket(Ticket ticket)
    {
      cmd = CreateProcedure("UpdateTicket");

      await sql.OpenAsync();

      // INPUT PARAMETERS
      idTicket = IntegerParameter("@pidTicket", ticket.IdTicket);
      selectedNumDrawn = StringParameter("@SelectedNumDrawn", ticket.SelectedNumDrawn);
      won = TinyIntParameter("@Won", ticket.Won);
      payout = DecimalParameter("@Payout", ticket.Payout);

      //ADDING PARAMETERS
      cmd.Parameters.Add(idTicket);
      cmd.Parameters.Add(selectedNumDrawn);
      cmd.Parameters.Add(won);
      cmd.Parameters.Add(payout);


      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      return true;
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

      for (int i = 0; i < drawnNumbers.Count; i++)
      {
        for (int j = 0; j < selectedNumbers.Count; j++)
        {
          if (drawnNumbers[i] == selectedNumbers[j])
          {

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

    public async Task<IEnumerable<Ticket>> GetTicketsRound(int userId)
    {
      var tickets = new List<Ticket>();
      cmd = CreateProcedure("GetTicketsRound");
      await sql.OpenAsync();

      IdUser = IntegerParameter("@puserId", userId);
      responseMessage = ResponseMessage();

      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        tickets.Add(HelpFunctions.MaptoTicket(reader));
      }

      await sql.CloseAsync();
      await reader.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      return tickets;
    }
  }
}
