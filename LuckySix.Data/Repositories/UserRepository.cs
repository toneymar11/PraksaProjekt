using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Database;
using LuckySix.Data.Utilities;
using LuckySix.Data.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class UserRepository : Repository, IUserRepository
  {
    public UserRepository() : base() { }

    public async Task<User> GetUser(int userId)
    {
      User user = null;

      cmd = CreateProcedure("GetUser");

      await sql.OpenAsync();

      // INPUT PARAMETER
      IdUser = IntegerParameter("@pIdUser", userId);

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        user = HelpFunctions.MapToUser(reader);

      }
      await sql.CloseAsync();

      return user;
    }

    public async Task<bool> IsUserExists(string username)
    {
      cmd = CreateProcedure("CheckUsername");
      await sql.OpenAsync();

      //INPUT PARAMETER 
      Username = StringParameter("@pusername", username);

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(Username);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      return (!responseMessage.Value.ToString().Equals("Success"));

    }


    public async Task<User> RegisterUser(User user)
    {

      if (await IsUserExists(user.Username)) return null;

      cmd = CreateProcedure("RegisterUser");

      User newUser = null;

      await sql.OpenAsync();

      // INPUT PARAMETERS

      Username = StringParameter("@pUserName", user.Username);
      Password = StringParameter("@pPassword", user.Password);
      FirstName = StringParameter("@pFirstName", user.FirstName);
      LastName = StringParameter("@pLastName", user.LastName);


      // OUTPUT PARAMETERS

      NewId = IntegerOutput("@newId");
      Balance = DecimalOutput("@balance");
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(Username);
      cmd.Parameters.Add(Password);
      cmd.Parameters.Add(FirstName);
      cmd.Parameters.Add(LastName);
      cmd.Parameters.Add(NewId);
      cmd.Parameters.Add(Balance);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();
      if (responseMessage.Value.ToString().Equals("Success"))
      {

        newUser = await GetUser((int)NewId.Value);

      }


      return newUser;
    }


    public async Task<User> UpdateUser(User user, int userId)
    {
      cmd = CreateProcedure("UpdateUserDetails");

      await sql.OpenAsync();
      // INPUT PARAMETERS

      IdUser = IntegerParameter("@piduser", userId);
      Username = StringParameter("@pusername", user.Username);
      Password = StringParameter("@ppassword", user.Password);
      FirstName = StringParameter("@pfirstname", user.FirstName);
      LastName = StringParameter("@plastname", user.LastName);


      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(Username);
      cmd.Parameters.Add(Password);
      cmd.Parameters.Add(FirstName);
      cmd.Parameters.Add(LastName);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return null;
      }

      return await GetUser(userId);
    }

    public async Task<bool> MakeADeposit(decimal balance, int userId)
    {

      cmd = CreateProcedure("MakeADeposit");

      await sql.OpenAsync();

      // INPUT PARAMETERS
      Balance = DecimalParameter("@pbalance", balance);
      IdUser = IntegerParameter("@puserId", userId); ;

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(Balance);
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();
      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return false;
      }

      return true;
    }
  }
}
