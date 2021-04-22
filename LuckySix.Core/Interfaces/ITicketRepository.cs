using LuckySix.Core.Entities;
using LuckySix.Core.TicketCalculation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface ITicketRepository
  {
    Task<Ticket> CreateTicket(Ticket ticket);
    Task<Ticket> GetTicket(int ticketId);

    TicketStatus IsUserWinTicket(string selectedNum, string DrawnNum);

    Task<bool> UpdateTicket(Ticket ticket);
  }
}
