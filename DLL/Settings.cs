using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public class Settings
    {
        public static string BasePath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        public static string BlogMasterPath
        {
            get { return BasePath + @"blog\master.html"; }
        }
        public static string BaseURL
        {
            get { return "http://www.cnopenblog.com/"; }
        }

        public static string DateFormat
        {
            get { return "yyyy-MM-dd HH:mm"; }
        }

        public static string Ext
        {
            get { return ".htm"; }
        }

        public static string UserFormat
        {
            get { return @"@([a-z0-9_]+)"; }
        }

        public static string GroupFormat
        {//[[group:id#name]]
            get { return @"\[\[group:([\d]{5,6})#([^\]]+)\]\]"; }
        }

        public static string TopicFormat
        {//[[topic:id#title]]
            get { return @"\[\[topic:([\d]{5,6})#([^\]]+)\]\]"; }
        }
        
        public static string ApplyFormat
        {//[[apply:gid#uname]]
            get { return @"\[\[apply:([\d]{5,6})#([a-z0-9_]+)\]\]"; }
        }
    }
}
