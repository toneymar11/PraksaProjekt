using AutoMapper;
using LuckySix.Api.Models;
using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LuckySix.Api.Controllers
{

  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : Controller
  {
    #region ctor
    public UsersController(IUserRepository userRepository, IMapper mapper, IUserValidation userValidation, ITokenRepository tokenRepository) : base(userRepository, mapper, userValidation, tokenRepository) { }
    #endregion



    #region implementation
    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<IActionResult> GetUser([FromRoute] int userId)
    {
      if (!(await IsUserLogged())) return Unauthorized(new ErrorMessage() { StatusCode = "401", ErrorText = "You need to login" });

      if (userId != GetUserFromHeader()) return Unauthorized(new ErrorMessage() { StatusCode = "401", ErrorText = "You can't see this page" });

      var userEntity = await userRepository.GetUser(userId);

      if (userEntity == null) return NotFound(new ErrorMessage() { StatusCode = "404", ErrorText = "User doesn't exist" });


      var userDto = mapper.Map<UserDto>(userEntity);

      return Ok(userDto);
    }


    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegister user)
    {

      if (await IsUserLogged())
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You can't register, you are already logged in" });



      bool validation = userValidation.CheckFirstNameAndLastName(user.FirstName, user.LastName);
      if (!validation) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Please enter a valid first name or last name" });

      validation = userValidation.CheckLogin(user.Username, user.Password);
      if (!validation) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Password must be at least 8 characters or username is empty" });

      var userEntity = await userRepository.RegisterUser(mapper.Map<User>(user));
      if (userEntity == null)
      {
        return NotFound(new ErrorMessage() { StatusCode = "404", ErrorText = "Username already exists" });
      }

      var userDto = mapper.Map<UserDto>(userEntity);



      // 201 Code Status Created
      return CreatedAtRoute("GetUser", new { userId = userEntity.IdUser }, userDto);
    }


    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserDetails([FromBody] UserRegister user, [FromRoute] int userId)
    {

      if (!(await IsUserLogged())) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are not authorized, please log in" });

      if (userId != GetUserFromHeader()) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are not authorized for this operation" });

      bool validation = userValidation.CheckFirstNameAndLastName(user.FirstName, user.LastName);
      if (!validation) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Please enter a valid first name or last name" });

      validation = userValidation.CheckLogin(user.Username, user.Password);
      if (!validation) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Password must be at least 8 characters or username is empty" });

      var updatedUser = await userRepository.UpdateUser(mapper.Map<User>(user), userId);
      if (updatedUser == null)
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "User with that username already exist" });
      }

      return Ok(mapper.Map<UserDto>(updatedUser));
    }


    [HttpPut("balance")]
    public async Task<IActionResult> MakeADeposit([FromBody] UserDeposit userDeposit)
    {
      int userId = GetUserFromHeader();
      if (!(await IsUserLogged()))
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You are not authorized, please log in" });
      }

      if (!userValidation.IsValidBalance(userDeposit.Balance)) return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "You balance is not valid" });

      bool isDepositSuccesfull = await userRepository.MakeADeposit(userDeposit.Balance, userId);

      if (!isDepositSuccesfull)
      {
        return BadRequest(new ErrorMessage() { StatusCode = "400", ErrorText = "Payment failed" });
        }
      var updatedUser = await userRepository.GetUser(userId);

      return Ok(mapper.Map<UserDto>(updatedUser));
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
