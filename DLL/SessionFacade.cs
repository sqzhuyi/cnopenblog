using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace DLL
{
    public class SessionFacade
    {
        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static string Get(string key)
        {
            object obj = Session[key];
            if (obj != null) return obj.ToString();
            else return "";
        }

        public static void Set(string key, string valu)
        {
            Session[key] = valu;
        }

        /// <summary>
        /// ÑéÖ¤Âë
        /// </summary>
        public static string ConfirmCode
        {
            get { return Get("confirmcode"); }
            set { Set("confirmcode", value); }
        }
    }
}
