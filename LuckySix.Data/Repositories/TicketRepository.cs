using LuckySix.Core.Entities;
using LuckySix.Core.Interfaces;
using LuckySix.Data.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LuckySix.Data.Repositories
{
  public class TicketRepository : ITicketRepository
  {

    public SqlConnection sql;
    public TicketRepository()
    {
      DatabaseConnection databaseConnection = new DatabaseConnection();
      sql = new SqlConnection(databaseConnection.connectionString);

    }
    public Task<Ticket> CreateTicket(Ticket ticket)
    {
      throw new NotImplementedException();
    }
  }
}
