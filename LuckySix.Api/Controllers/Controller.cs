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
      int userId = GetUserFromHeader();
      string token = GetTokenFromHeader();

      if (userId == 0 || token == "no")
      {
        return false;
      }

      var userValid = await tokenRepository.IsTokenValid(userId, token);
      if (userValid == null) return false;

      return true;
    }

    public int GetUserFromHeader()
    {
      if (Request.Headers["userid"].ToString().Equals(""))
      {
        Console.WriteLine("hello");

        return 0;
      }
      string userid = Request.Headers["userid"];

      int number = Int32.Parse(userid);
      Console.WriteLine(number);
      return number;
    }
    public string GetTokenFromHeader()
    {
      if (Request.Headers["authorization"].ToString().Equals(""))
      {
        Console.WriteLine("hello");  
        return "no";
      }

      return Request.Headers["authorization"];
    }

    public void ResponseHeaders()
    {
      Response.Headers["authorization"] = GetTokenFromHeader();
      Response.Headers["userid"] = GetUserFromHeader().ToString();
    }

    
  }
}
