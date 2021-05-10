using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Api.Token;
using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]

    public class TokensController : Controller
    {

        public TokensController(ITokenRepository tokenRepository, IMapper mapper, IConfiguration configuration) : base(tokenRepository, mapper, configuration)
        {

        }

       
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] User user)
        {
            if (user == null) return BadRequest("Invalid type");
            if (await IsUserLogged()) return BadRequest("You are already logged in");
            var loginUser = await tokenRepository.LogIn(user);

            if (loginUser == null) return BadRequest("Invalid username or password, please try again");


            string token = TokenActions.GenerateToken(loginUser, configuration);
            Console.WriteLine(token);

            loginUser.Token = token;
            await tokenRepository.SaveToken(loginUser.IdUser, token);

            //// SET COOKIES USER ID AND USER TOKEN
            //Cookie cookieToken = CookieActions.SetCookie("session-id", token, 1);
            //Response.Cookies.Append(cookieToken.Key, cookieToken.Value, cookieToken.Option);

            //Cookie cookieUserId = CookieActions.SetCookie("user-id", loginUser.IdUser.ToString(), 1);
            //Response.Cookies.Append(cookieUserId.Key, cookieUserId.Value, cookieUserId.Option);


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
                return BadRequest("You are already logged out");
            }
            var isTokenValid = await tokenRepository.IsTokenValid(userId, token);
            if (isTokenValid == null)
            {
                return BadRequest("You are already logged out");
            }

            bool isLogOut = await tokenRepository.Logout(userId, token);

            if (!isLogOut)
            {
                return BadRequest("You are already logged out");
            }
            // DELETE HEADERS
            Response.Headers.Remove("authorization");
            Response.Headers.Remove("userid");

            return Ok("You are logged out");
        }

        
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT");

            return Ok();
        }


    }
}
