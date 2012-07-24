using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// 连接字规则
    /// </summary>
    public class LinkRule
    {
        private static Char[] LINK_CHAR = new Char[] {
            '的', '地', '还', '有', '在', '为', '来', '及',
            '下', '可', '到', '由', '把', '与', '也', '乃',
            '并', '所', '哪', '那', '已', '起', '最', '再',
            '去', '好', '只', '亦', '又', '或', '很', '和',
            '是', '对', '将', '给', '算', '致', '至', '用',
            '向', '往', '随', '虽', '使', '若', '让', '却',
            '且', '凭', '其', '拿', '另', '么', '吗', '嘛',
            '了', '啦', '靠', '距', '看', '既', '几', '即',
            '何', '各', '该', '打', '得', '从', '趁', '别',
            '比', '被', '吧', '呗', '之', '呢', '啊', '呀',
            '会', '帮', '约', '玩', '因', '怔', '说'
        };
        private static Hashtable m_LinkCharTbl = new Hashtable();

        /// <summary>
        /// 连接字符检测
        /// </summary>
        /// <param name="c">待检测的字符</param>
        /// <returns></returns>
        public static bool Test(Char c)
        {
            if (m_LinkCharTbl.Count == 0)
            {
                foreach (Char linkChar in LINK_CHAR)
                {
                    m_LinkCharTbl.Add(linkChar, null);
                }
            }
            return m_LinkCharTbl.ContainsKey(c);
        }
    }
}
