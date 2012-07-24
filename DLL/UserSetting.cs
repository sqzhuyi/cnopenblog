using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public class UserSetting
    {
        private static int _online = 0;

        public static int Online
        {
            get { return _online; }
            set { _online = value; }
        }
    }
}
