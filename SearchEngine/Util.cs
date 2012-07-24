using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;

using SearchEngine.Analysis;
using SearchEngine.Index;
using SearchEngine.Search;
using SearchEngine.Store;
using SearchEngine.Configuration;
using ThesaurusAnalysis.Rule;

namespace SearchEngine
{
    public class Util
    {
        static Analyzer analyzer = new WhitespaceAnalyzer();

        /// <summary>
        /// 解析文本内容，消除无效字符
        /// </summary>
        /// <param name="text">待解析的内容</param>
        /// <returns></returns>
        public static string ParseText(string text)
        {
            text = Regex.Replace(text, "<br[^>]*>", "\n", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<[^>]*>", " ");
            text = HttpUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ");
            text = Regex.Replace(text, @"([^\w\d])(\1+){2,}",
                delegate(Match m)
                {
                    string s = m.Value;
                    if (s == "++" || s == "--")
                    {
                        return s;
                    }
                    return s[0].ToString();
                });

            text = Regex.Replace(text, @"[\u0000-\u0008\u000B\u000C\u000E-\u001A\uD800-\uDFFF]",
                delegate(Match m)
                {
                    int code = (int)m.Value.ToCharArray()[0];
                    return (code > 9 ? "&#" + code.ToString() : "&#0" + code.ToString()) + ";";
                }
            );
            return text;
        }

        /// <summary>
        /// 创建索引档
        /// </summary>
        /// <param name="id">文档ID号</param>
        /// <param name="author">作者</param>
        /// <param name="cat">文章类别(大类ID)</param>
        /// <param name="title">文章标题</param>
        /// <param name="body">文章正文</param>
        /// <param name="tag">标签</param>
        /// <param name="path">文档路径</param>
        /// <returns></returns>
        public static Lucene.Net.Documents.Document CreateDocument(string id, string author, string cat, string title, string body, string tag, string path)
        {
            Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();

            doc.Add(new Field("id", id, Field.Store.YES, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("author", author, Field.Store.NO, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("cat", cat, Field.Store.NO, Field.Index.UN_TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("title", title, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("body", body, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.WITH_OFFSETS));
            doc.Add(new Field("tag", tag, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("path", path, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO));
            doc.Add(new Field("date", DateField.DateToString(DateTime.Now), Field.Store.YES, Field.Index.NO, Field.TermVector.NO));

            //设置权重，越靠后的文章权重越大，在搜索结果中的位置靠前的机会就越大
            float boost = Single.Parse(DateTime.Now.ToString("0.yyyyMMddhh"));
            doc.SetBoost(boost);

            //确定保存文档压缩包的路径
            string fpath = Directorys.StoreDirectory + Math.Ceiling(Double.Parse(id) / 10000D).ToString("f0");
            if (!System.IO.Directory.Exists(fpath))
            {
                System.IO.Directory.CreateDirectory(fpath);
            }

            //将文档以gzip方式保存到相应位置
            StoreWriter store = new StoreWriter(fpath + @"\" + id + ".gz");
            store.WriteLine(author);
            store.WriteLine(cat);
            store.WriteLine(tag);
            store.WriteLine(title);
            store.WriteLine(path);
            store.WriteLine(body);
            store.Close();

            return doc;
        }

        /// <summary>
        /// 关键字拆分并分组
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public static string KeywordSplit(string keyword)
        {
            //将搜索的词语按照空格分组
            string[] keyGroups = keyword.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder keywords = new StringBuilder(/*keyword*/);

            for (int i = 0; i < keyGroups.Length; i++)
            {
                ThesaurusSpliter ts = new ThesaurusSpliter(new StringReader(keyGroups[i]));
                for (string word = ts.Next(); word != null; word = ts.Next())
                {
                    if (word.Length > 0 && !StopWord.Test(word))
                    {
                        keywords.Append(" ");
                        keywords.Append(word);
                    }
                }

                keywords.Append("+");
            }

            string trem = keywords.ToString().Trim(new char[] { ' ', '+' });

            if (trem.Length > 0)
            {
                return trem;
            }

            return keyword.Trim();
        }

        /// <summary>
        /// 创建条件查询定义
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public static Query BuildQuery(string keyword)
        {
            return BuildQuery(null, 0, keyword, false);
        }

        /// <summary>
        /// 创建条件查询定义
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public static Query BuildQuery(string keyword, bool onlyTitle)
        {
            return BuildQuery(null, 0, keyword, onlyTitle);
        }

        /// <summary>
        /// 创建条件查询定义
        /// </summary>
        /// <param name="author">作者</param>
        /// <param name="cat">类别（ID）</param>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyTitle">是否只搜索标题</param>
        /// <returns></returns>
        public static Query BuildQuery(string author, int cat, string keyword, bool onlyTitle)
        {
            BooleanQuery query = new BooleanQuery();

            QueryParser parser;

            if (!String.IsNullOrEmpty(author))
            {
                parser = new QueryParser("author", analyzer);
                query.Add(parser.Parse(author), BooleanClause.Occur.MUST);
            }
            if (cat > 0)
            {
                parser = new QueryParser("cat", analyzer);
                query.Add(parser.Parse(cat.ToString()), BooleanClause.Occur.MUST);
            }
            //只能以“+”分割，否则不对
            string[] trems = keyword.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in trems)
            {
                if (onlyTitle)
                {
                    parser = new QueryParser("title", analyzer);
                    query.Add(parser.Parse(key), BooleanClause.Occur.MUST);
                }
                else
                {
                    BooleanQuery qu = new BooleanQuery();

                    parser = new QueryParser("title", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    parser = new QueryParser("tag", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    parser = new QueryParser("body", analyzer);
                    qu.Add(parser.Parse(key), BooleanClause.Occur.SHOULD);

                    query.Add(qu, BooleanClause.Occur.MUST);
                }
            }
            return query;
        }

        /// <summary>
        /// 创建tag查询定义
        /// </summary>
        /// <param name="keyword">tag</param>
        /// <returns></returns>
        public static Query BuildTagQuery(string keyword)
        {
            BooleanQuery query = new BooleanQuery();

            QueryParser parser;

            string[] trems = keyword.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string key in trems)
            {
                parser = new QueryParser("tag", analyzer);
                query.Add(parser.Parse(key), BooleanClause.Occur.MUST);
            }
            return query;
        }
    }
}
