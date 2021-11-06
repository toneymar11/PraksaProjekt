using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RoundsController : Controller
  {

    #region ctor
    public RoundsController(IRoundRepository roundRepository, IMapper mapper) : base(roundRepository, mapper) {  }
    #endregion


    #region implementation
    [HttpGet("{status}")]
    public async Task<IActionResult> GetRound([FromRoute] string status)
    {
      switch (status)
      {
        case "ready":
          var readyRound = await roundRepository.GetReadyRound();
          if (readyRound == null) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "This round doesn't exist" });

          return Ok(mapper.Map<ReadyRound>(readyRound));

        case "running":
          var runningRound = await roundRepository.GetRunningRound();
          if (runningRound == null) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "This round doesn't exist" });

          return Ok(mapper.Map<RunningRound>(runningRound));

        case "last":
          var lastRound = await roundRepository.GetLastRound();
          return Ok(lastRound);

        default:
          return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "This round doesn't exist" });

      };
    }


    [HttpOptions]
    public IActionResult GetOptions()
    {
      Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");
      return Ok();
    }

    #endregion
  }
}
