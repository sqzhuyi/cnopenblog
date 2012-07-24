using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DLL
{
    public class Tag
    {
        string _name;
        int _cnt = 1;

        public Tag()
        {
        }
        public Tag(string name)
        {
            _name = name;
        }
        public Tag(string name, int count)
        {
            _name = name;
            _cnt = count;
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Count
        {
            get { return _cnt; }
            set { _cnt = value; }
        }

        private static List<string> _alltags = new List<string>();

        public static void Add(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            tag = tag.ToLower();
            if (!_alltags.Contains(tag))
                _alltags.Add(tag);
        }
        public static List<string> AllTags
        {
            get { return _alltags; }
            set { _alltags = value; }
        }
    }

    public class TagTools
    {
        /// <summary>
        /// 过滤tags中非法字符（结果可入库）
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string Filter(string tags)
        {
            if (tags == null || tags.Trim() == "") return "";

            tags = Regex.Replace(tags, @"[^\w, \.]+", ",");
            string _tags = "";
            foreach (string tag in tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (tag.Trim().Length > 1) _tags += tag.Trim() + ",";
            }
            return _tags.TrimEnd(',');
        }
        /// <summary>
        /// 把tags转换成连接形式
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string ToLinks(string tags)
        {
            return ToLinks(tags, false);
        }
        /// <summary>
        /// 把tags转换成连接形式
        /// </summary>
        /// <param name="tags">标签组字符串</param>
        /// <param name="isGroup">是否是group的标签</param>
        /// <returns></returns>
        public static string ToLinks(string tags, bool isGroup)
        {
            if (tags.Trim().Length < 1) return "";

            string[] taglist = tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            string str = ", <a href='/tag/{0}{1}'>{0}</a>";
            if (isGroup) str = ", <a href='/group/tag/{0}{1}'>{0}</a>";
            foreach (string tag in taglist)
            {
                sb.AppendFormat(str, tag, Settings.Ext);
            }
            if (sb.Length > 0) sb.Remove(0, 2);

            return sb.ToString();
        }
        /// <summary>
        /// 通过一个tags数组返回格式化后的Tag类数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Tag> Format(List<string> list)
        {
            if (list.Count == 0) return new List<Tag>();

            list.Sort();

            List<Tag> tags = new List<Tag>();
            Tag tag = new Tag("");
            foreach (string s in list)
            {
                if (tag.Name.ToLower() != s.ToLower())
                {
                    if (tag.Name != null) tags.Add(tag);
                    tag = new Tag(s);
                }
                else
                {
                    tag.Count++;
                }
            }
            return tags;
        }
    }
}
