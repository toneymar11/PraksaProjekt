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
            ConnectionString = "";
        }
    }
}
