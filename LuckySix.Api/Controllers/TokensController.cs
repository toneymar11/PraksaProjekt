using AutoMapper;
using LuckySix.Api.Cookies;
using LuckySix.Api.Models;
using LuckySix.Api.Token;
using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TokensController : ControllerBase
  {

    private readonly ITokenRepository tokenRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public TokensController(ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration)
    {
      this.tokenRepository = tokenRepository;
      this.mapper = mapper;
      this.configuration = configuration;
     
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] User user)
    {
      var loginUser = await tokenRepository.LogIn(user);

      if (loginUser == null)  return BadRequest("Invalid username or password, please try again");

      string token = TokenActions.GenerateToken(loginUser, configuration);

      loginUser.Token = token;
      tokenRepository.SaveToken(loginUser.IdUser, token);

      // SET COOKIES USER ID AND USER TOKEN
      Cookie cookieToken = CookieActions.SetCookie("session-id", token, 1);
      Response.Cookies.Append(cookieToken.Key, cookieToken.Value, cookieToken.Option);

      Cookie cookieUserId = CookieActions.SetCookie("user-id", loginUser.IdUser.ToString(), 1);
      Response.Cookies.Append(cookieUserId.Key, cookieUserId.Value, cookieUserId.Option);

      var userLogin = mapper.Map<UserDto>(loginUser);

      return Ok(userLogin);
    }

    [HttpPut]
    public async Task<IActionResult> Logout()
    {
      int userId = GetUserFromCookie();
      string token = GetTokenFromCookie();
      if (userId == 0 || token == "no")
      {
        return BadRequest("You are already logged out");
      }
      var isTokenValid = await tokenRepository.IsTokenValid(userId, token);
      if (isTokenValid == null)
      {
        return BadRequest("You are already logged out");
      }

      bool isLogOut = tokenRepository.Logout(userId, token);

      if (!isLogOut)
      {
        return BadRequest("You are already logged out");
      }
      // DELETE COOKIES
      Response.Cookies.Delete("session-id");
      Response.Cookies.Delete("user-id");

      return Ok("You are logged out");
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
