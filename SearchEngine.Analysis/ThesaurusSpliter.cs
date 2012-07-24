using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using Lucene.Net.Analysis;
using SearchEngine.Analysis.Thesaurus;
using SearchEngine.Analysis.Rule;
using SearchEngine.Analysis.Match;
using SearchEngine.Analysis.Coder;
using System.Text.RegularExpressions;

namespace SearchEngine.Analysis
{
	/// <summary>
    /// ThesaurusSpliter 分词处理器
	/// </summary>
    public class ThesaurusSpliter
    {
        private TextReader input;

        private AssociateStream assoStream;

        //当前到达的流的位置
        private int offset = 0;

        //当前到达的缓冲区位置
        private int bufferIndex = 0;

        //当前缓冲区的内容长度
        private int dataLen = 0;

        //最大词长，超过这个长度则强行输出该词条
        private static int MAX_WORD_LEN = 24;

        //缓冲长度
        private static int IO_BUFFER_SIZE = 1024;

        //当前词的长度
        private int length = 0;

        //预读内容长度
        private int prepLength = 0;

        //当前词内容
        private char[] buffer = new char[MAX_WORD_LEN];

        //预读取内容
        private char[] prepBuffer = new char[MAX_WORD_LEN];

        //预读缓冲区
        private char[] ioBuffer = new char[IO_BUFFER_SIZE];

        //将前一次读取结果的最后 MAX_WORD_LEN 个字备份，以备回溯。
        private char[] backBuffer = new char[MAX_WORD_LEN];

        /// <summary>
        /// 分词处理器构造函数
        /// </summary>
        /// <param name="input">文本读取流</param>
        public ThesaurusSpliter(TextReader input)
        {
            this.input = input;
            //创建联想流.模仿输入法的联想词语的功能
            assoStream = new AssociateStream();
            ReadBuffer();
        }

        //读取留中的内容到缓冲区
        private void ReadBuffer()
        {
            //备份缓冲区中的最后 MAX_WORD_LEN 个字备份，以备回溯。
            Array.Copy(ioBuffer, IO_BUFFER_SIZE - MAX_WORD_LEN, backBuffer, 0, MAX_WORD_LEN);
            dataLen = input.Read(ioBuffer, 0, ioBuffer.Length);
            CodeMap.Escape(ioBuffer, 0, dataLen);
            bufferIndex = 0;
        }

        //从缓冲区读取下一个字符
        private Char GetNextChar()
        {
            //前到达的流的位置前进一个字符
            offset++;
            //如果读取的字符在之前的缓冲区则在备份缓冲区读取
            if (bufferIndex < 0)
            {
                return Char.ToLower(backBuffer[backBuffer.Length + (bufferIndex++)]);
            }
            //如果缓冲区的字符不足则读取下IO_BUFFER_SIZE个字符早缓冲区
            else if (bufferIndex >= IO_BUFFER_SIZE)
            {
                ReadBuffer();
            }
            return Char.ToLower(ioBuffer[bufferIndex++]);
        }

        //将字符压入输出字符数组并提升一个长度
        private void Push(char c)
        {
            buffer[length++] = c;
        }

        private void Prep(char c)
        {
            prepBuffer[prepLength++] = c;
        }
        //当内容不能和前一个字符组成词语时回朔一个字符
        private void Back()
        {
            bufferIndex--;
            offset--;
        }

        /// <summary>
        /// 获取当前分词的偏移量
        /// </summary>
        public int Offset
        {
            get
            {
                return this.offset - this.length;
            }
        }

        //返回当前词语
        private string Flush()
        {
            //如果输出字符数组里有内容时返回TOKEN,否则关闭本次匹配(内容已经到尾部)
            if (length > 0)
            {
                string token = new String(buffer, 0, length);

                //如果预读缓冲里有内容则将预读缓存复制到输出缓冲以备下次处理
                if (prepLength > 0)
                {
                    prepBuffer.CopyTo(buffer, 0);
                    length = prepLength;
                    prepLength = 0;
                }
                else
                {
                    length = 0;
                }
                return token;
            }
            else
            {
                return null;
            }
        }

        //生词匹配
        private void StrangeWordMacth()
        {
            Char c;
            //将缓冲区填满5个字符
            while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < 6)
            {
                Push(c);
            }

            //对缓冲区中的内容进行生词匹配
            int len = StrangeWord.Match(buffer, length);
            //如果匹配失败则开始大写数字匹配
            if (len == 1 && NumberRule.Test(buffer[0]))
            {
                for (int i = 1; i < MAX_WORD_LEN; i++)
                {
                    if (i > length && Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter)
                    {
                        Push(c);
                    }
                    if (NumberRule.Test(buffer[i]))
                    {
                        len++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            bufferIndex = bufferIndex - length + len;
            offset = offset - length + len;
            length = len;
        }
        //歧义消除
        private void ClearDifferentMeanings()
        {
            int len = length - 1;
            //如果当前词语中含有更短的词语时进行再分词处理，否则继续向后进行歧义检测
            if (assoStream.BackToLastWordEnd())
            {
                int start = assoStream.Step;
                int end = start;
                assoStream.Reset();

                char c = buffer[end];
                //对多出的词继续进行词语匹配
                while (end < length && assoStream.Associate(c = buffer[end]))
                {
                    Prep(c);
                    end++;
                }

                //如果缓冲区中的内容全部匹配完后继续中文本流中读取
                if (end == length)
                {
                    while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                    {
                        Prep(c);
                    }
                }

                //如果预处理的词出现过词语则回朔到最后一个词语
                if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                {
                    assoStream.BackToLastWordEnd();
                    bufferIndex = bufferIndex - prepLength + assoStream.Step;
                    offset = offset - prepLength + assoStream.Step;
                    prepLength = assoStream.Step;
                }

                ///如果存在一个完整的词语并且词语长度比缓冲区中的上一个词语更长时将缓冲区切断至词语长度，并将多余字符放到预读缓冲区，否则回朔本次处理
                if (assoStream.IsWordEnd && (prepLength > len || prepLength > start && len > start + 1 || prepLength >= start && Char.GetUnicodeCategory(c) != UnicodeCategory.OtherLetter))
                {
                    len = start;
                }
                else
                {
                    if (end == length)
                    {
                        bufferIndex = bufferIndex + len - prepLength - start;
                        offset = offset + len - prepLength - start;
                    }
                    prepLength = 0;
                }
            }
            else if (len == 2)
            {
                //保存词性
                //WordPart wp1 = assoStream.GetWordPart();
                assoStream.Reset();
                char c = buffer[1];
                //从词语的第二个词开始依次进行词语匹配
                if (assoStream.Associate(c) && assoStream.Associate(buffer[2]))
                {
                    Prep(c);
                    Prep(buffer[2]);
                    while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                    {
                        Prep(c);
                    }
                    if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                    {
                        assoStream.BackToLastWordEnd();
                        bufferIndex = bufferIndex - prepLength + assoStream.Step;
                        offset = offset - prepLength + assoStream.Step;
                        prepLength = assoStream.Step;
                    }
                    if (assoStream.IsWordEnd)
                    {
                        if (prepLength == 2)
                        {
                            //词性组合规则，尚未实现
                            //WordPart wp2 = assoStream.GetWordPart();
                            //if(WordPart.Combo(wp1, wp2)
                            //    len = 2;
                            //else
                            //    len = 1;

                            //临时方案 
                            assoStream.Reset();
                            if (assoStream.Associate(prepBuffer[1]) && assoStream.Associate(c))
                            {
                                char[] tmp = new char[MAX_WORD_LEN];
                                tmp[0] = prepBuffer[1];
                                tmp[1] = c;
                                int tmpLength = 2;
                                while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && assoStream.Associate(c))
                                {
                                    tmp[tmpLength++] = c;
                                }
                                if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                                {
                                    assoStream.BackToLastWordEnd();
                                    bufferIndex = bufferIndex - tmpLength + assoStream.Step;
                                    offset = offset - tmpLength + assoStream.Step;
                                    tmpLength = assoStream.Step;
                                }
                                if (assoStream.IsWordEnd)
                                {
                                    tmp.CopyTo(prepBuffer, 0);
                                    prepLength = tmpLength;
                                }
                                else
                                {
                                    bufferIndex = bufferIndex - tmpLength;
                                    offset = offset - tmpLength;
                                    prepLength = 0;
                                }
                            }
                            else if (LinkRule.Test(buffer[0]))
                                len = 1;
                            else
                            {
                                bufferIndex = bufferIndex - prepLength + 1;
                                offset = offset - prepLength + 1;
                                prepLength = 0;
                            }
                        }
                        else
                        {
                            len = 1;
                        }
                    }
                    else
                    {
                        bufferIndex = bufferIndex - prepLength + 1;
                        offset = offset - prepLength + 1;
                        prepLength = 0;
                    }
                }
            }
            length = len;
        }

        /// <summary>
        /// 下一个词语
        /// </summary>
        /// <returns></returns>
        public string Next()
        {
            if (length > 0)
            {
                return Flush();
            }
            //重置联合器
            assoStream.Reset();

            //读取下一个字符
            char c = GetNextChar();

            //如果缓冲区里已经没有内容则终止当前读取
            if (dataLen < bufferIndex)
                return Flush();

            //将字符放入输出数组
            Push(c);

            //根据首字符的类型选择不同的读取过程
            switch (Char.GetUnicodeCategory(c))
            {
                //如果是数字则读取之后的全部数字直到遇到非数字字符
                case UnicodeCategory.DecimalDigitNumber:
                    while (
                            (
                                Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.DecimalDigitNumber
                                || c == '.' && buffer[length - 1] != '.'
                            )
                            && length < MAX_WORD_LEN
                        )
                    {
                        Push(c);
                    }
                    Back();
                    return Flush().Trim('.');
                //如果是英文字符则读取之后的全部英文字符直到遇到非英文字符
                case UnicodeCategory.LowercaseLetter:
                    while (
                            (
                                Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.LowercaseLetter
                                || c == '+'
                                || c == '#'
                            )
                            && length < MAX_WORD_LEN
                        )
                    {
                            Push(c);
                    }
                    Back();
                    return Flush();
                //如果是中文字符则开始中文分词过程
                case UnicodeCategory.OtherLetter:
                    if (c > 19967 && c < 40870 || c > 12353 && c < 12436)
                    {
                        assoStream.Associate(c);
                        //读取并检测下一个字符是否是中文字符
                        while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < MAX_WORD_LEN)
                        {
                            //通过字典树向下匹配词语直至无法匹配
                            if (!assoStream.IsBegin && assoStream.HasChildren && assoStream.Associate(c))
                            {
                                Push(c);
                                continue;
                            }
                            //如果出现过的匹配成功的词语则昭会上一个词语
                            if (!assoStream.IsWordEnd && assoStream.IsOccurWord)
                            {
                                assoStream.BackToLastWordEnd();
                                bufferIndex = bufferIndex - length + assoStream.Step;
                                offset = offset - length + assoStream.Step;
                                length = assoStream.Step;
                            }
                            //如果正好是一个完整的词语则中断处理
                            if (assoStream.IsWordEnd)
                            {
                                Push(c);
                                ClearDifferentMeanings();
                            }
                            //否则进行人名的匹配
                            else
                            {
                                if (!LinkRule.Test(buffer[0]))
                                {
                                    Push(c);
                                    StrangeWordMacth();
                                }
                                else
                                {
                                    bufferIndex = bufferIndex - length;
                                    offset = offset - length;
                                    length = 1;
                                    return Flush();
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        while (Char.GetUnicodeCategory(c = GetNextChar()) == UnicodeCategory.OtherLetter && length < MAX_WORD_LEN)
                        {
                            Push(c);
                        }
                    }
                    Back();
                    return Flush();
                //如果是非可读字符(包括标点符号,空格等)则直接进入下一个处理过程
                default:
                    length = 0;
                    return String.Empty;
            }
        }
    }
}
