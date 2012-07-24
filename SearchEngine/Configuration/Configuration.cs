using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SearchEngine.Configuration
{
    /// <summary>
    /// 搜索引擎目录配置
    /// </summary>
    public static class Directorys
    {
        /// <summary>
        /// 获取索引目录
        /// </summary>
        public static string IndexDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["IndexDirectory"];
            }
        }

        /// <summary>
        /// 获取文档存放目录
        /// </summary>
        public static string StoreDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["StoreDirectory"];
            }
        }
    }
}
