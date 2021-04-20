using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Cookies
{
  public class Cookie
  {
    public string Key { get; set; }
    public string Value { get; set; }
    public CookieOptions Option { get; set; }
  }
}
