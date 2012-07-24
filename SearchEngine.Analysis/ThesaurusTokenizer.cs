using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.IO;
using ThesaurusAnalysis.Rule;

namespace SearchEngine.Analysis
{
    /// <summary>
    /// ThesaurusTokenizer
    /// </summary>
    public class ThesaurusTokenizer : Tokenizer
    {
        private ThesaurusSpliter spliter;

        /// <summary>
        /// ThesaurusTokenizer构造函数
        /// </summary>
        /// <param name="input"></param>
        public ThesaurusTokenizer(TextReader input)
        {
            spliter = new ThesaurusSpliter(input);
        }

        /// <summary>
        /// 返回下一个Token
        /// </summary>
        /// <returns></returns>
        public override Token Next()
        {
            for (string word = spliter.Next(); word != null; word = spliter.Next())
            {
                if (word.Length > 0 && !StopWord.Test(word))
                {
                    return new Token(word, spliter.Offset - word.Length, spliter.Offset);
                }
            }

            //关闭文本流
            Close();
            return null;
        }
    }
}
