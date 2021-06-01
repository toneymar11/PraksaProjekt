using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Entities;
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
  public class TicketsController : Controller
  {
    #region ctor
    public TicketsController(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository) : base(ticketRepository, mapper, ticketValidation, tokenRepository) { }
    #endregion



    #region implementation
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] TicketPost ticket)
    {
      if (!(await IsUserLogged())) return Unauthorized(new ErrorMessage() { StatusCode = "401", ErrorText = "You need to login" });

      ticket.SelectedNum = String.Concat(ticket.SelectedNum.Where(c => !Char.IsWhiteSpace(c)));

      if (!ticketValidation.IsValidSelectedNumbers(ticket.SelectedNum)) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Selected numbers are not valid" });



      int userId = GetUserFromHeader();

      var ticketEntity = mapper.Map<Ticket>(ticket);
      if (!(await ticketValidation.IsPossibleBetting(ticket.Stake, userId))) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Insufficient Funds" });

      ticketEntity.IdUser = userId;
      var newTicket = await ticketRepository.CreateTicket(ticketEntity);

      if (newTicket == null) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Ticket is invalid" });

      return Ok(mapper.Map<TicketDto>(newTicket));
    }


    [HttpGet]
    public async Task<IActionResult> GetTicketsRound()
    {
      if (!(await IsUserLogged())) return Unauthorized(new ErrorMessage() { StatusCode = "400", ErrorText = "You need to login" });

      var tickets = await ticketRepository.GetTicketsRound(GetUserFromHeader());

      if (tickets == null) return NotFound(new ErrorMessage() { StatusCode = "404", ErrorText = "Tickets don't exist" });

      return Ok(mapper.Map<IEnumerable<TicketsRound>>(tickets));
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
