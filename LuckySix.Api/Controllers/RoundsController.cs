using AutoMapper;
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

    [HttpGet("{idRound}")]
    public async Task<IActionResult> GetRound([FromRoute] int idRound)
    {
      if (!roundRepository.IsRoundExists(idRound)) return NotFound("Round doesn't exist");
      var round = await roundRepository.GetRound(idRound);
      if (round == null) return NotFound("Round doesn't exist");

      return Ok(round);

    }

  }
}
