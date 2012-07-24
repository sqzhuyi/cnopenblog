using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SearchEngine.Analysis.Configuration
{
    /// <summary>
    /// ×ÖµäÄ¿Â¼ÅäÖÃ
    /// </summary>
    public static class Directorys
    {
        /// <summary>
        /// »ñÈ¡×ÖµäÄ¿Â¼
        /// </summary>
        public static string DictsDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["DictsDirectory"];
            }
        }
    }
}
