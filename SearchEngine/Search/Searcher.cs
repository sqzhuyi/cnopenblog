using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SearchEngine.Analysis;
using SearchEngine.Store;
using SearchEngine.Configuration;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace SearchEngine.Search
{
    /// <summary>
    /// 文档搜索器
    /// </summary>
    public class Searcher
    {
        private Lucene.Net.Search.IndexSearcher searcher;

        /// <summary>
        /// 当索引文件发生变化是重新载入索引到内存中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIndexChanged(object sender, FileSystemEventArgs e)
        {
            OpenIndex();
        }

        /// <summary>
        /// 打开索引
        /// </summary>
        private void OpenIndex()
        {
            //如果不存在索引则创建空白索引
            if (!File.Exists(Directorys.IndexDirectory + "segments.gen"))
            {
                IndexWriter empty = new IndexWriter(Directorys.IndexDirectory, new ThesaurusAnalyzer(), true);
                empty.Optimize();
                empty.Close();
            }

            //如果索引器已经创建则先关闭索引器
            if (searcher != null)
            {
                searcher.Close();
            }
            searcher = new Lucene.Net.Search.IndexSearcher(Directorys.IndexDirectory);
        }

        /// <summary>
        /// 索引器构造函数
        /// </summary>
        public Searcher()
        {
            OpenIndex();
            FileSystemWatcher fsWatcher = new FileSystemWatcher(Directorys.IndexDirectory, "segments.gen");
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.IncludeSubdirectories = false;
            fsWatcher.Changed += new FileSystemEventHandler(OnIndexChanged);
        }

        /// <summary>
        /// 搜索指定条件的文档
        /// 返回：所有符合条件的文档
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public Documents Search(string keywords)
        {
            return Search(null, 0, keywords, false);
        }

        /// <summary>
        /// 搜索指定条件的文档
        /// 返回：所有符合条件的文档
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public Documents Search(string keywords, bool onlyTitle)
        {
            return Search(null, 0, keywords, onlyTitle);
        }

        /// <summary>
        /// 搜索指定条件的文档
        /// 返回：所有符合条件的文档
        /// </summary>
        /// <param name="author">作者</param>
        /// <param name="cat">类别（ID）</param>
        /// <param name="keywords">查询关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public Documents Search(string author, int cat, string keywords, bool onlyTitle)
        {
            Lucene.Net.Search.Hits hits = searcher.Search(Util.BuildQuery(author, cat, keywords, onlyTitle));

            return new SearchEngine.Store.Hits(hits, searcher.Reader, keywords.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public Documents SearchTag(string keywords)
        {
            Lucene.Net.Search.Hits hits = searcher.Search(Util.BuildTagQuery(keywords));

            return new SearchEngine.Store.Hits(hits, searcher.Reader, keywords.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
