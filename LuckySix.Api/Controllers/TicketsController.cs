using AutoMapper;
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
  public class TicketsController : ControllerBase
  {
    private readonly ITicketRepository ticketRepository;
    private readonly IMapper mapper;
    private readonly ITicketValidation ticketValidation;
    private readonly ITokenRepository tokenRepository;

    public TicketsController(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository)
    {
      this.ticketRepository = ticketRepository;
      this.mapper = mapper;
      this.ticketValidation = ticketValidation;
      this.tokenRepository = tokenRepository;

    }

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



    // Cookies
    public int GetUserFromCookie()
    {
      if (Request.Cookies["user-id"] == null)
      {
        return 0;
      }
      string cookie = Request.Cookies["user-id"];
      int number = Int32.Parse(cookie);

      return number;
    }
    public string GetTokenFromCookie()
    {
      if (Request.Cookies["session-id"] == null)
      {
        return "no";
      }
      return Request.Cookies["session-id"];
    }

    public async Task<bool> IsUserLogged()
    {
      int userId = GetUserFromCookie();
      string token = GetTokenFromCookie();

      if (userId == 0 || token == "no")
      {
        return false;
      }

      var userValid = await tokenRepository.IsTokenValid(userId, token);
      if (userValid == null) return false;

      return true;
    }


  }
}
