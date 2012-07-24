using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// 大写数字规则
    /// </summary>
    public class NumberRule
    {
        private static Char[] NUMBER_CHAR = new Char[] {
            '○', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '百', '千', '万', '亿',
            '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖', '零', '拾', '佰', '仟', '数', '两', '俩', '卅'
        };

        private static Hashtable m_NumberCharTbl = new Hashtable();

        /// <summary>
        /// 大写数字检测
        /// </summary>
        /// <param name="number">待检测字符</param>
        /// <returns></returns>
        public static bool Test(Char number)
        {
            if (m_NumberCharTbl.Count == 0)
            {
                foreach (Char numberChar in NUMBER_CHAR)
                {
                    m_NumberCharTbl.Add(numberChar, null);
                }
            }

            return m_NumberCharTbl.ContainsKey(number);
        }
    }
}
