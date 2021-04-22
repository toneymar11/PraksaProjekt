using AutoMapper;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
  public class Controller : ControllerBase
  {
    public readonly IRoundRepository roundRepository;
    public readonly IMapper mapper;
    public readonly ITicketRepository ticketRepository;
    public readonly ITicketValidation ticketValidation;
    public readonly ITokenRepository tokenRepository;
    public readonly IConfiguration configuration;
    public readonly IUserRepository userRepository;
    public readonly IUserValidation userValidation;

    public Controller(IRoundRepository roundRepository, IMapper mapper)
    {
      this.roundRepository = roundRepository;
      this.mapper = mapper;
    }

    public Controller(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository)
    {
      this.mapper = mapper;
      this.ticketRepository = ticketRepository;
      this.ticketValidation = ticketValidation;
      this.tokenRepository = tokenRepository;
    }

    public Controller( ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration)
    {
      this.mapper = mapper;
      this.tokenRepository = tokenRepository;
      this.configuration = configuration;
    }

    public Controller(IUserRepository userRepository, IMapper mapper, IUserValidation userValidation, ITokenRepository tokenRepository)
    {
      this.mapper = mapper;
      this.tokenRepository = tokenRepository;
      this.userRepository = userRepository;
      this.userValidation = userValidation;

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
  }
}
