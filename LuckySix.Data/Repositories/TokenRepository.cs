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
  public class TokenRepository : Repository, ITokenRepository
  {

    private readonly IUserValidation userValidation;


    #region ctor
    public TokenRepository() : base()
    {

      userValidation = new UserValidation();
    }
    #endregion


    #region implementation
    public async Task<User> LogIn(User user)
    {
      //if (!userValidation.CheckLogin(user.Username, user.Password)) return null;


      User loginUser = null;

      cmd = CreateProcedure("ValidateLoginInput");
      await sql.OpenAsync();

      // INPUT PARAMETERS

      Username = StringParameter("@pUserName", user.Username);
      Password = StringParameter("@pPassword", user.Password);

      // OUTPUT PARAMETERS
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(Username);
      cmd.Parameters.Add(Password);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();

      while (await reader.ReadAsync())
      {
        loginUser = HelpFunctions.MapToUser(reader);
      }

      await reader.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      await sql.CloseAsync();

      return loginUser;

    }

    public async Task SaveToken(int userId, string token)
    {
     
      cmd = CreateProcedure("SavingToken");

      await sql.OpenAsync();

      // INPUT PARAMETERS
      IdUser = IntegerParameter("@pUserId", userId);
      userToken = StringParameter("@pToken", token);

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(userToken);


      await cmd.ExecuteNonQueryAsync();


      await sql.CloseAsync();
    }

    public async Task<User> IsTokenValid(int userId, string token)
    {
      
      cmd = CreateProcedure("IsTokenValid");

      User user = null;

      await sql.OpenAsync();

      //INPUT PARAMETERS
     
      IdUser = IntegerParameter("@pidUser", userId);
      userToken = StringParameter("@ptoken", token);

      // OUTPUT PARAMETER
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(userToken);
      cmd.Parameters.Add(responseMessage);

      var reader = await cmd.ExecuteReaderAsync();
      while (await reader.ReadAsync())
      {
        user = HelpFunctions.MapToUser(reader);
      }
      await reader.CloseAsync();

      await sql.CloseAsync();


      if (!responseMessage.Value.ToString().Equals("Success")) return null;

      return user;
    }

    public async Task<bool> Logout(int userId, string token)
    {
      
      cmd = CreateProcedure("DeleteToken");

      await sql.OpenAsync();

      // INPUT PARAMETERS
      
      IdUser = IntegerParameter("@pidUser", userId);
      SqlParameter userToken = StringParameter("@ptoken", token);


      // OUTPUT PARAMETERS
      responseMessage = ResponseMessage();

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(userToken);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return false;
      }

      return true;
    }

    #endregion
  }
}
