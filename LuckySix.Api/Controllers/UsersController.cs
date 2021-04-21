﻿using AutoMapper;
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
  public class UsersController : ControllerBase
  {
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IUserValidation userValidation;
    private readonly ITokenRepository tokenRepository;

    public UsersController(IUserRepository userRepository, IMapper mapper, IUserValidation userValidation, ITokenRepository tokenRepository)
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.userValidation = userValidation;
      this.tokenRepository = tokenRepository;
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<IActionResult> GetUser([FromRoute] int userId)
    {
      if (!(await IsUserLogged())) return Unauthorized("You need to login");

      if (userId != GetUserFromCookie()) return Unauthorized("You can't see this page");

      var userEntity = await userRepository.GetUser(userId);
      if (userEntity == null)
      {
        return NotFound("User doesn't exist");
      }

      var userDto = mapper.Map<UserDto>(userEntity);


      return Ok(userDto);
    }


    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {

      if (await IsUserLogged())
      {
        return BadRequest("You can't register, you are already logged in");
      }
   

      bool validation = userValidation.CheckFirstNameAndLastName(user.FirstName, user.LastName);
      if (!validation) return BadRequest("Please enter a valid first name or last name");

      validation = userValidation.CheckLogin(user.Username, user.Password);
      if (!validation) return BadRequest("Password must be at least 8 characters or username is empty");

      var userEntity = await userRepository.RegisterUser(user);
      if (userEntity == null)
      {
        return NotFound("Username already exists");
      }

      var userDto = mapper.Map<UserDto>(userEntity);



      // 201 Code Status Created
      return CreatedAtRoute("GetUser",new { userId = userEntity.IdUser} ,userDto);  
    }


    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserDetails([FromBody] User user, [FromRoute] int userId)
    {

      if (!( await IsUserLogged())) return BadRequest("You are not authorized, please log in");
      
      if (userId != GetUserFromCookie()) return BadRequest("You are not authorized for this operation");
     
      bool validation = userValidation.CheckFirstNameAndLastName(user.FirstName, user.LastName);
      if (!validation) return BadRequest("Please enter a valid first name or last name");

      validation = userValidation.CheckLogin(user.Username, user.Password);
      if (!validation) return BadRequest("Password must be at least 8 characters or username is empty");

      var updatedUser = await userRepository.UpdateUser(user, userId);
      if (updatedUser == null)
      {
        return BadRequest("User with that username already exists");
      }


      return Ok(mapper.Map<UserDto>(updatedUser));
    }

    [HttpPut("balance")]
    public async Task<IActionResult> MakeADeposit([FromBody] User user)
    {
      int userId = GetUserFromCookie();
      if (!(await IsUserLogged()))
      {
        return BadRequest("You are not authorized, please log in");
      }

      if (!userValidation.IsValidBalance(user.Balance)) return BadRequest("You balance is not valid");
     
      bool isDepositSuccesfull = await userRepository.MakeADeposit(user.Balance, userId);

      if (!isDepositSuccesfull)
      {
        return BadRequest("Payment failed");
      }
      var updatedUser = await userRepository.GetUser(userId);



      return Ok(mapper.Map<UserDto>(updatedUser));
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