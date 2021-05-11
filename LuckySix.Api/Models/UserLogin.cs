using LuckySix.Api.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Models
{
   
    public class UserLogin
    {
        public string Username { get; set; }    
        public string Password { get; set; }
    }
}
