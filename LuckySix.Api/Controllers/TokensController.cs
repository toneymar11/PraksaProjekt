using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Api.Token;
using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{

  [ApiController]
  [Route("api/[controller]")]

  public class TokensController : Controller
  {
    #region ctor
    public TokensController(ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration) : base(tokenRepository, mapper, configuration){}
    #endregion


    #region implementation

    [HttpGet]
    public async Task<IActionResult> ChechIfUserLogged()
    {
      var user = await IsUserLoggedNow();
      if (user == null)
      {
        return BadRequest(new ErrorMessage() { StatusCode = "401", ErrorText = "You need to login" });
      }

      return Ok(mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] UserLogin user)
    {

      if (await IsUserLogged()) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are already logged in" });
      var entityUser = mapper.Map<User>(user);
      var loginUser = await tokenRepository.LogIn(entityUser);

      if (loginUser == null) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Invalid username or password, please try again" });


      string token = TokenActions.GenerateToken(loginUser, configuration);
      //Console.WriteLine(token);

      loginUser.Token = token;
      await tokenRepository.SaveToken(loginUser.IdUser, token);


      HttpContext.Response.Headers["authorization"] = token.ToString();
      HttpContext.Response.Headers["userId"] = loginUser.IdUser.ToString();

      var userLogin = mapper.Map<UserDto>(loginUser);

      return Ok(userLogin);
    }


    [HttpPut]
    public async Task<IActionResult> Logout()
    {
      int userId = GetUserFromHeader();
      string token = GetTokenFromHeader();
      if (userId == 0 || token == "no")
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are already logged out" });
      }
      var isTokenValid = await tokenRepository.IsTokenValid(userId, token);
      if (isTokenValid == null)
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are already logged out" });
      }

      bool isLogOut = await tokenRepository.Logout(userId, token);

      if (!isLogOut)
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are already logged out" });
      }
      // DELETE HEADERS
      Response.Headers["authorization"] = "null";
      Response.Headers["userid"] = "null";

      return Ok(new { Message = "You are logged out" });
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
