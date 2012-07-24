using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using SearchEngine.Configuration;

namespace SearchEngine.Store
{

    /// <summary>
    /// 文档存根读取器
    /// </summary>
    internal class StoreReader : StreamReader
    {
        private Stream baseStream = null;

        /// <summary>
        /// 文档存根读取器的构造函数
        /// </summary>
        /// <param name="stream">解压缩流</param>
        public StoreReader(GZipInputStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// 文档存根读取器的构造函数
        /// </summary>
        /// <param name="stream">输入流</param>
        public StoreReader(Stream stream)
            : this(new GZipInputStream(stream))
        {
            baseStream = stream;
        }

        /// <summary>
        /// 文档存根读取器的构造函数
        /// </summary>
        /// <param name="id">存档ID号</param>
        public StoreReader(string path)
            : this(File.OpenRead(path))
        {
        }

        /// <summary>
        /// 关闭该读取器
        /// </summary>
        public override void Close()
        {
            if (baseStream != null)
                baseStream.Close();
        }
    }
}
