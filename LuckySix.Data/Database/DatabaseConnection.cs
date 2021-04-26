using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySix.Data.Database
{
    public class DatabaseConnection
    {
        public string ConnectionString { get; set; }

        public DatabaseConnection()
        {
            ConnectionString = "Server=uk.sql01.yourwebservers.com,1786;Database=bingo;User Id=bingo;Password=bingo;";
        }
    }
}
