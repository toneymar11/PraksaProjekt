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
  public class TokenRepository : ITokenRepository
  {
    public SqlConnection sql;
    private readonly IUserValidation userValidation;

    public TokenRepository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.connectionString);
      userValidation = new UserValidation();
    }

    public async Task<User> LogIn(User user)
    {
      if (!userValidation.CheckLogin(user.Username, user.Password)) return null;


      User loginUser = null;
      SqlCommand cmd = new SqlCommand("ValidateLoginInput", sql) { CommandType = CommandType.StoredProcedure };
      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter userName = new SqlParameter("@pUserName", SqlDbType.VarChar) { Value = user.Username };
      SqlParameter password = new SqlParameter("@pPassword", SqlDbType.VarChar) { Value = user.Password };

      // OUTPUT PARAMETERS
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(userName);
      cmd.Parameters.Add(password);
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

    public async Task SaveToken(int idUser, string token)
    {
      SqlCommand cmd = new SqlCommand("SavingToken", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter userId = new SqlParameter("@pUserId", SqlDbType.Int) { Value = idUser };
      SqlParameter userToken = new SqlParameter("@pToken", SqlDbType.VarChar) { Value = token };

      // ADDING PARAMETERS
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(userToken);


      await cmd.ExecuteNonQueryAsync();


      await sql.CloseAsync();
    }

    public async Task<User> IsTokenValid(int userId, string token)
    {
      SqlCommand cmd = new SqlCommand("IsTokenValid", sql) { CommandType = CommandType.StoredProcedure };

      User user = null;

      await sql.OpenAsync();

      //INPUT PARAMETERS
      SqlParameter IdUser = new SqlParameter("@pidUser", SqlDbType.Int) { Value = userId };
      SqlParameter UserToken = new SqlParameter("@ptoken", SqlDbType.VarChar) { Value = token };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(UserToken);
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
      SqlCommand cmd = new SqlCommand("DeleteToken", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter idUser = new SqlParameter("@pidUser", SqlDbType.Int) { Value = userId };
      SqlParameter userToken = new SqlParameter("@ptoken", SqlDbType.VarChar) { Value = token };

      // OUTPUT PARAMETERS
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(idUser);
      cmd.Parameters.Add(userToken);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql .CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return false;
      }

      return true;
    }
  }
}
