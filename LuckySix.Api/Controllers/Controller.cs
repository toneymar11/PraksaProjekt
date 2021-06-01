using AutoMapper;
using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{

  public class Controller : ControllerBase
  {
    #region fields
    public readonly IRoundRepository roundRepository;
    public readonly IMapper mapper;
    public readonly ITicketRepository ticketRepository;
    public readonly ITicketValidation ticketValidation;
    public readonly ITokenRepository tokenRepository;
    public readonly IConfiguration configuration;
    public readonly IUserRepository userRepository;
    public readonly IUserValidation userValidation;
    #endregion


    #region ctor
    public Controller(IRoundRepository roundRepository, IMapper mapper)
    {
      this.roundRepository = roundRepository;
      this.mapper = mapper;
    }
    #endregion

    #region ctor
    public Controller(ITicketRepository ticketRepository, IMapper mapper, ITicketValidation ticketValidation, ITokenRepository tokenRepository)
    {
      this.mapper = mapper;
      this.ticketRepository = ticketRepository;
      this.ticketValidation = ticketValidation;
      this.tokenRepository = tokenRepository;
    }
    #endregion

    #region ctor
    public Controller(ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration)
    {
      this.mapper = mapper;
      this.tokenRepository = tokenRepository;
      this.configuration = configuration;
    }
    #endregion


    #region ctor
    public Controller(IUserRepository userRepository, IMapper mapper, IUserValidation userValidation, ITokenRepository tokenRepository)
    {
      this.mapper = mapper;
      this.tokenRepository = tokenRepository;
      this.userRepository = userRepository;
      this.userValidation = userValidation;

    }
    #endregion


    #region helpFunctions

    [ApiExplorerSettings(IgnoreApi = true)]
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

    [ApiExplorerSettings(IgnoreApi = true)]

    public async Task<User> IsUserLoggedNow()
    {
      int userId = GetUserFromHeader();
      string token = GetTokenFromHeader();

      if (userId == 0 || token == "no")
      {
        return null;
      }

      var userValid = await tokenRepository.IsTokenValid(userId, token);
      if (userValid == null) return null;

      return userValid;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public int GetUserFromHeader()
    {
      string userid = Request.Headers["userid"];


      if (userid == null || userid.Equals("null")) return 0;

      int number = Int32.Parse(userid);

      return number;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetTokenFromHeader()
    {
      string auth = Request.Headers["authorization"];


      if (auth == null || auth.Equals("null")) return "no";

      return auth;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public void ResponseHeaders()
    {
      Response.Headers["authorization"] = GetTokenFromHeader();
      Response.Headers["userid"] = GetUserFromHeader().ToString();
    }

    #endregion
  }
}
