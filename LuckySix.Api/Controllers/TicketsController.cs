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

    public TicketsController(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository) : base(ticketRepository, mapper, ticketValidation, tokenRepository) { }



    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
      if (!(await IsUserLogged())) return Unauthorized("You need to login");

      ticket.SelectedNum = String.Concat(ticket.SelectedNum.Where(c => !Char.IsWhiteSpace(c)));

      if (!ticketValidation.IsValidSelectedNumbers(ticket.SelectedNum)) return BadRequest("Selected numbers are not valid");

      if (!ticketValidation.IsValidStake(ticket.Stake)) return BadRequest("You stake value is not valid");

      int userId = GetUserFromCookie();

      if (!( await ticketValidation.IsPossibleBetting(ticket.Stake, userId))) return BadRequest("Insufficient Funds");

      ticket.IdUser = userId;
      var newTicket = await ticketRepository.CreateTicket(ticket);

      if (newTicket == null) return BadRequest("Ticket is invalid");


      return Ok(newTicket);
    }
    [HttpGet]
    public async Task<IActionResult> GetTicketsRound()
    {
      if (!(await IsUserLogged())) return Unauthorized("You need to login");

      var tickets = await ticketRepository.GetTicketsRound(GetUserFromCookie());

      if (tickets == null) return BadRequest("Tickets don't exist");


      return Ok(mapper.Map<IEnumerable<TicketsRound>>(tickets));
    }



  }
}
