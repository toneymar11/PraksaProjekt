using LuckySix.Core.Interfaces;
using LuckySix.Data.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LuckySix.Data.Repositories
{
  public class RoundStatsRepository : Repository, IRoundStatsRepository
  {
   
    public RoundStatsRepository() : base()
    {
      
    }

  }
}
