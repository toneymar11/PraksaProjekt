using LuckySix.Data.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LuckySix.Data.Repositories
{
  public class Repository
  {
    public SqlConnection sql;
    public SqlCommand cmd;
    public SqlDataReader reader;

    // PARAMETERS
    public SqlParameter responseMessage;
    public SqlParameter idTicket;
    public SqlParameter selectedNumDrawn;
    public SqlParameter won;
    public SqlParameter payout;
    public SqlParameter IdUser;
    public SqlParameter Username;
    public SqlParameter Password;
    public SqlParameter FirstName;
    public SqlParameter LastName;
    public SqlParameter userToken;
    public SqlParameter Balance;
    public SqlParameter selectedNum;
    public SqlParameter stake;
    public SqlParameter NewId;




    public Repository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.ConnectionString);
      
    }

    public SqlCommand CreateProcedure(string procedureName)
    {
      return new SqlCommand(procedureName, sql) { CommandType = CommandType.StoredProcedure };
    }
    public SqlParameter ResponseMessage()
    {
      return new SqlParameter("@responseMessage", SqlDbType.VarChar) { Size = 25, Direction = ParameterDirection.Output };
    }
    public SqlParameter DecimalOutput(string ParameterName)
    {
      return new SqlParameter(ParameterName, SqlDbType.Decimal) { Direction = ParameterDirection.Output };
    }
    public SqlParameter IntegerOutput(string ParameterName)
    {
      return new SqlParameter(ParameterName, SqlDbType.Int) { Direction = ParameterDirection.Output };
    }

    public SqlParameter IntegerParameter(string ParameterName, int Value)
    {
      return new SqlParameter(ParameterName, SqlDbType.Int) { Value = Value };
    }
    public SqlParameter StringParameter(string ParameterName, string Value)
    {
      return new SqlParameter(ParameterName, SqlDbType.VarChar) { Value = Value };
    }

    public SqlParameter TinyIntParameter(string ParameterName, byte Value)
    {
      return new SqlParameter(ParameterName, SqlDbType.TinyInt) { Value = Value };
    }

    public SqlParameter DecimalParameter(string ParameterName, decimal Value)
    {
      return new SqlParameter(ParameterName, SqlDbType.Decimal) { Value = Value };
    }


  }
}
