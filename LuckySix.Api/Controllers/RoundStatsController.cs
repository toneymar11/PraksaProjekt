using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RoundStatsController : ControllerBase
  {

    public readonly IRoundStatsRepository roundStatsRepository;

    #region ctor
    public RoundStatsController(IRoundStatsRepository roundStatsRepository)
    {
      this.roundStatsRepository = roundStatsRepository;
    }
    #endregion

    #region implementation
    [HttpGet("{nRound}")]
    public async Task<IActionResult> GetRoundsStatistic([FromRoute] int nRound)
    {
      var roundStats = await roundStatsRepository.GetRoundStats(nRound);
      var sortedDict = from entry in roundStats.numbersCount orderby entry.Value descending select entry;

      return Ok(sortedDict);
    }
    #endregion
  }
}
