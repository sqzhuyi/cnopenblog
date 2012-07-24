using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace DLL
{
    public class Cookie
    {
        /// <summary>
        /// 写入cookie，默认时间一天
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valu"></param>
        public static void SetCookie(string name, string valu)
        {
            SetCookie(name, valu, DateTime.Now.AddDays(1));
        }

        public static void SetCookie(string name, string valu, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(name, HttpUtility.UrlEncode(valu, Encoding.GetEncoding("GB2312")));
            cookie.Expires = expires;
            //cookie.Domain = "cnopenblog.com";

            HttpContext context = HttpContext.Current;
            context.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string name)
        {
            string valu = "";
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                valu = HttpUtility.UrlDecode(cookie.Value, Encoding.GetEncoding("GB2312"));
            }
            return valu;
        }

        public static void RemoveCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name, "");
            cookie.Expires = DateTime.Now.AddDays(-1);

            HttpContext context = HttpContext.Current;
            context.Response.Cookies.Add(cookie);
        }
    }
}
