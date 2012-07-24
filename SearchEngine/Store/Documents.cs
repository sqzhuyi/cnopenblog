using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Index;

namespace SearchEngine.Store
{
    /// <summary>
    /// 搜索结果文档集合
    /// </summary>
    public abstract class Documents
    {
        protected IndexReader reader;
        protected Lucene.Net.Search.Hits _hits;
        protected string[] terms;

        /// <summary>
        /// 获取搜索使用的关键字分词结果字符串
        /// </summary>
        public string Keywords
        {
            get
            {
                return String.Join(" ", this.terms);
            }
        }

        /// <summary>
        /// 获取搜索到的文档数量
        /// </summary>
        public int Count
        {
            get
            {
                return this._hits.Length();
            }
        }

        /// <summary>
        /// 获取当前搜索结果文档集合中指定索引位置的文档
        /// </summary>
        /// <param name="index">文档在搜索结果文档集合中的从零开始的索引</param>
        /// <returns></returns>
        public Document this[int index]
        {
            get
            {
                int offset = 0;

                //条件在索引中的位置向量
                TermPositionVector termPositionVector = (TermPositionVector)this.reader.GetTermFreqVector(this._hits.Id(index), "body");

                //如果存在位置向量
                if (termPositionVector != null)
                {
                    int pos = -1;

                    for (int i = 0; i < terms.Length; i++)
                    {
                        //第一个命中的关键字在索引中的位置
                        pos = System.Array.IndexOf<string>(termPositionVector.GetTerms(), terms[i]);
                        if (pos > -1)
                            break;
                    }

                    //如果在索引中找到对应关键字则取出关键字在正文中的偏移量
                    if (pos > -1)
                    {
                        TermVectorOffsetInfo[] tvois = termPositionVector.GetOffsets(pos);
                        offset = tvois[0].GetStartOffset();
                    }
                }
                return new Hit(this._hits.Doc(index), offset);
            }
        }
    }

    /// <summary>
    /// 命中结果集合
    /// </summary>
    internal class Hits : Documents
    {
        /// <summary>
        /// 命中结果集合
        /// </summary>
        /// <param name="hits">索引结果集合</param>
        /// <param name="reader">索引读取器</param>
        /// <param name="terms">关键字分词结果数组</param>
        public Hits(Lucene.Net.Search.Hits hits, IndexReader reader, string[] terms)
        {
            base.reader = reader;
            base._hits = hits;
            base.terms = terms;
        }
    }
}
