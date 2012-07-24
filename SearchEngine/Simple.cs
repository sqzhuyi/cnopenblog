using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Lucene.Net.Index;
using Lucene.Net.Documents;

using SearchEngine.Index;
using SearchEngine.Search;
using SearchEngine.Store;
using SearchEngine.Configuration;
using SearchEngine.Analysis;

namespace SearchEngine
{
    /// <summary>
    /// 搜索引擎简易接口的简易封装
    /// </summary>
    public sealed class Simple
    {
        private static Indexer indexer = new Indexer();
        private static Searcher seacher = new Searcher();

        /// <summary>
        /// 向索引中添加文档
        /// </summary>
        /// <param name="id">文档ID号</param>
        /// <param name="author">作者</param>
        /// <param name="cat">文章类别(大类ID)</param>
        /// <param name="title">文章标题</param>
        /// <param name="body">文章正文</param>
        /// <param name="tag">文章标签关键字</param>
        public static void AddDocument(string id, string author, string cat, string title, string body, string tag, string path)
        {
            body = Util.ParseText(body);

            Lucene.Net.Documents.Document doc = Util.CreateDocument(id, author, cat, title, body, tag, path);

            indexer.Write(doc);
        }

        /// <summary>
        /// 删除文档索引
        /// </summary>
        /// <param name="id">文档ID号</param>
        public static void Delete(string id)
        {
            indexer.Delete(new Lucene.Net.Index.Term("id", id));
        }

        /// <summary>
        /// 更新/添加索引中的文档
        /// </summary>
        /// <param name="id">文档ID号</param>
        /// <param name="author">作者</param>
        /// <param name="cat">文章类别(大类ID)</param>
        /// <param name="title">文章标题</param>
        /// <param name="body">文章正文</param>
        /// <param name="tag">文章标签关键字</param>
        public static void Update(string id, string author, string cat, string title, string body, string tag, string path)
        {
            body = Util.ParseText(body);
            Lucene.Net.Documents.Document doc = Util.CreateDocument(id, author, cat, title, body, tag, path);
            indexer.Update(doc);
        }

        /// <summary>
        /// 搜索包含关键字的文档
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns></returns>
        public static Documents Search(string keyword)
        {
            return seacher.Search(Util.KeywordSplit(keyword));
        }

        /// <summary>
        /// 搜索包含关键字的文档
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public static Documents Search(string keyword, bool onlyTitle)
        {
            return seacher.Search(Util.KeywordSplit(keyword), onlyTitle);
        }

        /// <summary>
        /// 搜索包含关键字的文档
        /// </summary>
        /// <param name="author">作者</param>
        /// <param name="cat">类别(ID)</param>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public static Documents Search(string author, int cat, string keyword, bool onlyTitle)
        {
            return seacher.Search(author, cat, Util.KeywordSplit(keyword), onlyTitle);
        }

        /// <summary>
        /// 搜索相关tag
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Documents SearchTag(string keyword)
        {
            return seacher.SearchTag(Util.KeywordSplit(keyword));
        }
    }
}
