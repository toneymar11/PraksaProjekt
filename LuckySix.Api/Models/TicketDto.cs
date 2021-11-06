using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Models
{
  public class TicketDto
  {
    public int IdRound { get; set; }

    public string SelectedNum { get; set; }


    public decimal Stake { get; set; }

  }
}
