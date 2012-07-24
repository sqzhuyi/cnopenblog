using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SearchEngine.Store;
using SearchEngine.Configuration;
using System.Globalization;

namespace SearchEngine.Store
{
    /// <summary>
    /// 搜索结果文档
    /// </summary>
    public abstract class Document
    {
        protected string id;
        protected string author;
        protected string cat;
        protected string title;
        protected string tag;
        protected string body;
        protected string path;
        protected DateTime lastIndex;

        /// <summary>
        /// 获取结果文档的ID号
        /// </summary>
        public string ID
        {
            get
            {
                return this.id;
            }
        }
        /// <summary>
        /// 获取结果文档的作者
        /// </summary>
        public string Author
        {
            get
            {
                return this.author;
            }
        }
        /// <summary>
        /// 获取结果文档的类别
        /// </summary>
        public int Cat
        {
            get
            {
                int c = 0;
                int.TryParse(this.cat, out c);
                return c;
            }
        }
        /// <summary>
        /// 获取结果文档的标题
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
        }
        /// <summary>
        /// 获取结果文档的标签
        /// </summary>
        public string Tag
        {
            get
            {
                return this.tag;
            }
        }
        /// <summary>
        /// 获取结果文档的正文
        /// </summary>
        public string Body
        {
            get
            {
                return this.body;
            }
        }
        /// <summary>
        /// 获取文档路径
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }
        /// <summary>
        /// 获取结果文档的最后索引日期
        /// </summary>
        public DateTime LastIndex
        {
            get
            {
                return this.lastIndex;
            }
        }
    }

    /// <summary>
    /// 命中结果
    /// </summary>
    internal class Hit : Document
    {
        /// <summary>
        /// 命中结果构造函数
        /// </summary>
        /// <param name="doc">索引档</param>
        /// <param name="offset">关键字在正文中的位置</param>
        internal Hit(Lucene.Net.Documents.Document doc, int offset)
        {
            base.id = doc.Get("id");
            base.lastIndex = Lucene.Net.Documents.DateField.StringToDate(doc.Get("date"));

            //到开外部存放的文档实体
            StoreReader story = new StoreReader(Directorys.StoreDirectory + Math.Ceiling(Double.Parse(base.id) / 10000D).ToString("f0") + @"\" + base.id + ".gz");
            //读取已保存的文章头
            base.author = story.ReadLine();
            base.cat = story.ReadLine();
            base.tag = story.ReadLine();
            base.title = story.ReadLine();
            base.path = story.ReadLine();

            int readed = 0;

            int len = 126;//显示内容长度

            char[] block = new char[offset + len];

            //读取正文至关键字后len个字符
            readed = story.ReadBlock(block, 0, block.Length);

            story.Close();

            int index = offset;

            //如果关键字不在结尾处则摘要起始位置定位于关键字前一个标点符号之后，否则摘要取末尾的len个字符
            if (readed == block.Length)
            {
                UnicodeCategory category;
                for (; index > 0; index--)
                {
                    category = Char.GetUnicodeCategory(Char.ToLower(block[index]));
                    if (category == UnicodeCategory.OtherPunctuation)
                    {
                        index += 1;
                        break;
                    }
                }
            }
            else
            {
                index = Math.Max(0, readed - len);
            }

            //如果摘要不在结尾处则在后面添加“...”
            base.body = (new String(block, index, Math.Min(len - 1, readed))) + ((readed >= index + len) ? "..." : "");
        }
    }
}
