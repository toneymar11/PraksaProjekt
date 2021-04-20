using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Core.Entities
{
  public class Round
  {
    public int IdRound { get; set; }
    public string DrawnNum { get; set; }
    public DateTime StartRoundTime { get; set; }
    public DateTime FinishRoundTime { get; set; }
    public string Status { get; set; }
  }
}
