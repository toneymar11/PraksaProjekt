using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Core.Entities
{
  public class User
  {
    public int IdUser { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public decimal Balance { get; set; }
    public int Status { get; set; }
    public string Token { get; set; }
  }
}
