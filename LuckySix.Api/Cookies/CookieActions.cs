using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuckySix.Api.Cookies
{
  public class CookieActions
  {
    public static Cookie SetCookie(string key, string value, int? expireTime)
    {
      CookieOptions option = new CookieOptions();

      if (expireTime.HasValue)
        option.Expires = DateTime.Now.AddDays(expireTime.Value);
      else
        option.Expires = DateTime.Now.AddMilliseconds(10);

      return new Cookie() { Key = key, Value = value, Option = option };

    }

   
  }
}
