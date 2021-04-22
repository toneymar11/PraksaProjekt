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
    private readonly IRoundStatsRepository roundStatsRepository;

    public RoundStatsController(IRoundStatsRepository roundStatsRepository)
    {
      this.roundStatsRepository = roundStatsRepository;
    }
  }
}
