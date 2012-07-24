using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Rule
{
    /// <summary>
    /// 地名规则
    /// </summary>
    public class PlaceRule
    {
        private static Char[] PLACE_CHAR = new Char[] {
            '省', '市', '城', '州',
            '区', '县', '镇', '乡',
            '村', '社', '营', '街',
            '路', '巷', '道', '弄',
            '栋', '幢', '号', '组',
            '山', '海', '江', '湖',
            '河', '潭', '溪', '坡',
            '桥', '塔', '场', '坝',
            '岛', '屿', '林', '关',
            '泉', '岗', '洲', '港'
        };

        private static Hashtable m_PlaceCharTbl = new Hashtable();

        /// <summary>
        /// 地名尾字符检测
        /// </summary>
        /// <param name="c">待检测字符</param>
        /// <returns></returns>
        public static bool Test(Char c)
        {
            if (m_PlaceCharTbl.Count == 0)
            {
                foreach (Char placeChar in PLACE_CHAR)
                {
                    m_PlaceCharTbl.Add(placeChar, null);
                }
            }

            return m_PlaceCharTbl.ContainsKey(c);
        }
    }
}
