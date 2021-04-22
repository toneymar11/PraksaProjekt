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

    public Repository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.connectionString);
      
    }

    public SqlCommand CreateProcedure(string procedureName)
    {
      return new SqlCommand(procedureName, sql) { CommandType = CommandType.StoredProcedure };
    }
  }
}
