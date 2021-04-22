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

 
    public UserRepository(): base()
    {

    }

    public async Task<User> GetUser(int userId)
    {
      User user = null;

      SqlCommand cmd = new SqlCommand("GetUser", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETER
      SqlParameter IdUser = new SqlParameter("@pIdUser", SqlDbType.Int) { Value = userId };

      // OUTPUT PARAMETER
      SqlParameter ResponseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 250, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(IdUser);
      cmd.Parameters.Add(ResponseMessage);

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
      SqlCommand cmd = new SqlCommand("CheckUsername", sql) { CommandType = CommandType.StoredProcedure };
      await sql.OpenAsync();
      //INPUT PARAMETER
      SqlParameter Username = new SqlParameter("@pusername", SqlDbType.VarChar) { Value = username };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

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

      SqlCommand cmd = new SqlCommand("RegisterUser", sql) { CommandType = CommandType.StoredProcedure };

      User newUser = null;

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter UserName = new SqlParameter("@pUserName", SqlDbType.VarChar) { Value = user.Username };
      SqlParameter Password = new SqlParameter("@pPassword", SqlDbType.VarChar) { Value = user.Password };
      SqlParameter FirstName = new SqlParameter("@pFirstName", SqlDbType.VarChar) { Value = user.FirstName };
      SqlParameter LastName = new SqlParameter("@pLastName", SqlDbType.VarChar) { Value = user.LastName };


      // OUTPUT PARAMETERS
      SqlParameter NewId = new SqlParameter("@newId", SqlDbType.Int) { Direction = ParameterDirection.Output };
      SqlParameter Balance = new SqlParameter("@balance", SqlDbType.Decimal) { Direction = ParameterDirection.Output };
      SqlParameter ResponseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 250, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(UserName);
      cmd.Parameters.Add(Password);
      cmd.Parameters.Add(FirstName);
      cmd.Parameters.Add(LastName);
      cmd.Parameters.Add(NewId);
      cmd.Parameters.Add(Balance);
      cmd.Parameters.Add(ResponseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();
      if (ResponseMessage.Value.ToString().Equals("Success"))
      {

        newUser = await GetUser((int)NewId.Value);

      }


      return newUser;
    }


    public async Task<User> UpdateUser(User user, int idUser)
    {
      SqlCommand cmd = new SqlCommand("UpdateUserDetails", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();
      // INPUT PARAMETERS
      SqlParameter userId = new SqlParameter("@piduser", SqlDbType.Int) { Value = idUser };
      SqlParameter username = new SqlParameter("@pusername", SqlDbType.VarChar) { Value = user.Username };
      SqlParameter password = new SqlParameter("@ppassword", SqlDbType.VarChar) { Value = user.Password };
      SqlParameter firstname = new SqlParameter("@pfirstname", SqlDbType.VarChar) { Value = user.FirstName };
      SqlParameter lastname = new SqlParameter("@plastname", SqlDbType.VarChar) { Value = user.LastName };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(username);
      cmd.Parameters.Add(password);
      cmd.Parameters.Add(firstname);
      cmd.Parameters.Add(lastname);
      cmd.Parameters.Add(responseMessage);

      await cmd.ExecuteNonQueryAsync();

      await sql.CloseAsync();

      if (!responseMessage.Value.ToString().Equals("Success"))
      {
        return null;
      }

      return await GetUser(idUser);
    }

    public async Task<bool> MakeADeposit(decimal balance, int idUser)
    {
      
      SqlCommand cmd = new SqlCommand("MakeADeposit", sql) { CommandType = CommandType.StoredProcedure };

      await sql.OpenAsync();

      // INPUT PARAMETERS
      SqlParameter userBalance = new SqlParameter("@pbalance", SqlDbType.Decimal) { Value = balance };
      SqlParameter userId = new SqlParameter("@puserId", SqlDbType.Int) { Value = idUser };

      // OUTPUT PARAMETER
      SqlParameter responseMessage = new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };

      // ADDING PARAMETERS
      cmd.Parameters.Add(userBalance);
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
