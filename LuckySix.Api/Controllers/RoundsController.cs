using AutoMapper;
using LuckySix.Api.Models;
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
  public class RoundsController : ControllerBase
  {
    private readonly IRoundRepository roundRepository;
    private readonly IMapper mapper;

    public RoundsController(IRoundRepository roundRepository, IMapper mapper)
    {
      this.roundRepository = roundRepository;
      this.mapper = mapper;
    }

    [HttpGet("{status}")]
    public async Task<IActionResult> GetRound([FromRoute] string status)
    {
      if (status == "ready")
      {
        var readyRound = await roundRepository.GetReadyRound();
        if (readyRound == null) return BadRequest("This round doesn't exist");

        return Ok(mapper.Map<ReadyRound>(readyRound));
      }
      else if (status == "running")
      {
        var runningRound = await roundRepository.GetRunningRound();
        if(runningRound == null) return BadRequest("This round doesn't exist");

        return Ok(mapper.Map<RunningRound>(runningRound));
      }
      else
      {
        return BadRequest("This round doesn't exist");
      }


    }

  }
}
