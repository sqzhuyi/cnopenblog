using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using SearchEngine.Configuration;

namespace SearchEngine.Store
{
    /// <summary>
    /// 文档存根写入器
    /// </summary>
    internal class StoreWriter : StreamWriter
    {
        /// <summary>
        /// 文档存根写入器的构造函数
        /// </summary>
        /// <param name="stream">压缩流</param>
        public StoreWriter(GZipOutputStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// 文档存根写入器的构造函数
        /// </summary>
        /// <param name="stream">输出流</param>
        public StoreWriter(Stream stream)
            : this(new GZipOutputStream(stream))
        {
        }

        /// <summary>
        /// 文档存根写入器的构造函数
        /// </summary>
        /// <param name="stream">存档ID号</param>
        public StoreWriter(string path)
            : this(new FileStream(path, FileMode.Create, FileAccess.Write))
        {
        }
    }
}
