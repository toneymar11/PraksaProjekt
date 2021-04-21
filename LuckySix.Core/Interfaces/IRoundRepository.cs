using LuckySix.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Core.Interfaces
{
  public interface IRoundRepository
  {
    Task<Round> GetReadyRound();

    Task<Round> GetRunningRound();

    
  }
}
