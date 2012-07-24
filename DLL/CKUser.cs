using System;
using System.Collections.Generic;
using System.Text;
using System.Web.SessionState;

namespace DLL
{
    public class CKUser
    {
        /// <summary>
        /// 获取当前用户名
        /// </summary>
        public static string Username
        {
            get { return Tools.DecryptDES(Cookie.GetCookie("username")).Replace("'", ""); }
        }
        /// <summary>
        /// 获取当前用户姓名
        /// </summary>
        public static string Fullname
        {
            get { return Cookie.GetCookie("fullname"); }
        }
        /// <summary>
        /// 获取当前用户email
        /// </summary>
        public static string Email
        {
            get { return Cookie.GetCookie("email"); }
        }
        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        public static bool IsLogin
        {
            get { return Username != ""; }
        }

        /// <summary>
        /// 写入用户信息到cookie
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="fullname">姓名</param>
        /// <param name="email">email</param>
        /// <param name="remember">是否记住</param>
        public static void Login(string username, string fullname, string email, bool remember)
        {
            username = Tools.EncryptDES(username);
            if (remember)
            {
                DateTime expires = DateTime.Now.AddYears(1);
                Cookie.SetCookie("username", username, expires);
                Cookie.SetCookie("fullname", fullname, expires);
                Cookie.SetCookie("email", email, expires);
            }
            else
            {
                Cookie.SetCookie("username", username);
                Cookie.SetCookie("fullname", fullname);
                Cookie.SetCookie("email", email);
            }
        }

        public static void Logout()
        {
            Cookie.RemoveCookie("username");
            Cookie.RemoveCookie("fullname");
            Cookie.RemoveCookie("email");
        }
    }
}
