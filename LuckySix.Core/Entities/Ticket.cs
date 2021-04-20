using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Core.Entities
{
  public class Ticket
  {
    public int IdTicket { get; set; }

    public int IdUser { get; set; }
    public int IdRound { get; set; }

    public string SelectedNum { get; set; }

    public string SelectedNumDrawn { get; set; }

    public decimal Stake { get; set; }
    public decimal Payout { get; set; }

    public byte Won { get; set; }
  }
}
