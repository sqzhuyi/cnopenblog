using System;
using System.Collections.Generic;
using System.Text;
using SearchEngine.Analysis.Thesaurus;
using SearchEngine.Analysis.Rule;

namespace SearchEngine.Analysis.Match
{
    /// <summary>
    /// 生词检测
    /// </summary>
    public class StrangeWord
    {
        /// <summary>
        /// 对字符序列进行生词检测
        /// </summary>
        /// <param name="word">字符序列</param>
        /// <param name="length">有效字符数量</param>
        /// <returns></returns>
        public static int Match(Char[] word, int length)
        {
            int nStart = 0;
            //姓氏检测
            if (FamilyNameRule.Test(((int)word[0] << 2) + (int)word[1]))
            {
                nStart = 2;
            }
            else if (FamilyNameRule.Test((int)word[0]) && !LinkRule.Test(word[1]))
            {
                nStart = 1;
            }
            else
            {
                AssociateStream assoStream = new AssociateStream();
                //连词检测,如果不是连词则向后进行词语匹配
                if (!LinkRule.Test(word[1]))
                {
                    for (nStart = 1; nStart < length; )
                    {
                        assoStream.Associate(word[nStart++]);
                        if (!assoStream.IsBegin && assoStream.Associate(word[nStart]))
                        {
                            return nStart - 1;
                        }
                        else
                        {
                            assoStream.Reset();
                        }
                    }
                }
                //地名检测
                for (nStart = 0; nStart < length - 1;)
                {
                    assoStream.Reset();
                    if (!LinkRule.Test(word[nStart++]) && PlaceRule.Test(word[nStart]))
                    {
                        return nStart + 1;
                    }
                }
                return 1;
            }
            //如果检测到姓氏并且剩余字符数小于3个字符则整个串是一个完整姓名
            if(length - nStart <= 2)
            {
                return length;
            }
            //如果串的长度大于姓氏的长度则开始检测有效的名字长度
            if (length > nStart)
            {
                int nEnd = nStart + 1;
                AssociateStream assoStream = new AssociateStream();

                //如果下一个字是连接字或者当前字符和下一个字无法组合成一个已知词则将当前字被确认为时姓名的一个字
                if (LinkRule.Test(word[nEnd + 1]) || !(assoStream.Associate(word[nEnd]) && assoStream.Associate(word[nEnd + 1])))
                {
                    nEnd++;
                }
                nStart = nEnd;
            }
            return nStart;
        }
    }
}