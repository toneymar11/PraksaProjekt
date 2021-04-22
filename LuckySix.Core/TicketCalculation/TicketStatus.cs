using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Core.TicketCalculation
{
  public class TicketStatus
  {
    public string SelectedNumDrawn { get; set; }
    public int Index { get; set; }

    public decimal Coefficient { get; set; }
  
    public List<decimal> Coefficients = new List<decimal> { 0, 0, 0, 0, 0, 10000, 7500, 5000, 2500, 1000, 500, 300, 200,
          150, 100, 90, 80, 70, 60, 50, 40, 30, 25, 20, 15, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};



    public void Update()
    {
      Coefficient = Coefficients[Index];
    }

  }
}
