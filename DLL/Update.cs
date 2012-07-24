using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace DLL
{
    public class Update
    {
        public static string EditBlog(int blogID, string title, int cat, int subcat, string zhaiyao, string tag, string body, int nocomment)
        {
            tag = TagTools.Filter(tag);

            SqlParameter[] pars = new SqlParameter[7];

            pars[0] = new SqlParameter("@_id", blogID);
            pars[1] = new SqlParameter("@cat_id", cat);
            pars[2] = new SqlParameter("@subcat_id", subcat);
            pars[3] = new SqlParameter("@title", title);
            pars[4] = new SqlParameter("@zhaiyao", zhaiyao);
            pars[5] = new SqlParameter("@tags", tag);
            pars[6] = new SqlParameter("@no_comment", nocomment);

            object obj = DB.Procedure2("editBlog", pars);
            if (obj == null) return "";

            string filepath = obj.ToString();

            StringBuilder sb;
            string sql;
            DataTable dt;

            string html = IO.LoadFile(Settings.BasePath + filepath.TrimStart('/'), Encoding.GetEncoding("GB2312"));

            html = Regex.Replace(html, @"<title>(.+?)</title>", "<title>" + Tools.HtmlEncode(title) + " - cnOpenBlog</title>");
            html = Regex.Replace(html, @"(<meta name=""keywords"" content="")(.+)("" />)", "$1 " + Tools.HtmlEncode(title).Replace("\"","") + ", " + tag + "$3");
            html = Regex.Replace(html, @"(<!--title start-->)(.+)(<!--title end-->)", "$1 " + Tools.HtmlEncode(title) + "$3");


            sb = new StringBuilder();
            
            sql = "select [uc_name] from [user_category] where [uc_id]={0};";
            sql += "select [g_id],[g_name] from [group],[group_member] where [gm_g_id]=[g_id] and [gm_user_name]='{1}';";
            sql = String.Format(sql, subcat, CKUser.Username);
            DataSet ds = DB.GetDataSet(sql);

            dt = ds.Tables[0];

            sb.AppendFormat("<a href='{0}'>首页</a> &gt;&gt; <a href='/{1}{6}'>{2}</a> &gt;&gt; <a href='/{3}/{4}{6}'>{5}</a>", Settings.BaseURL, cat, CategoryB.GetNameById(cat), CKUser.Username, subcat, dt.Rows[0][0], Settings.Ext);
            html = Regex.Replace(html, @"(<!--dao hang start-->)(.+)(<!--dao hang end-->)", "$1" + sb.ToString() + "$3", RegexOptions.Singleline);

            dt = ds.Tables[1];//group
            
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                sb = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendFormat("<div class='groupitem'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
                    sb.AppendFormat("<p><a href='/group/{0}{1}'>{2}</a></p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
                    if (++i % 2 == 0) sb.Append("<div class='clear'></div>");
                }
                sb.Append("<div class='clear'></div>");
                html = Regex.Replace(html, @"(<!--group start-->)(.+?)(<!--group end-->)", "$1" + sb.ToString() + "$3");
            }
            html = Regex.Replace(html, "(<!--tags start-->)(.+?)(<!--tags end-->)", "$1" + TagTools.ToLinks(tag) + "$3", RegexOptions.Singleline);

            body = Regex.Replace(body, @"<img ([^>]*)>", new MatchEvaluator(delegate(Match match) { if (match.Value.Contains("onload") || match.Value.Contains("OutliningIndicators"))return match.Value; else return "<img " + Strings.ImageOnload + " " + match.Groups[1].Value + ">"; }), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, @"<!--(.+?)-->", "", RegexOptions.Singleline);

            html = Regex.Replace(html, @"(<!--body start-->)(.+?)(<!--body end-->)", "<!--body start-->" + body + "$3", RegexOptions.Singleline);

            //更新博客文件
            IO.WriteFile(Settings.BasePath + filepath.TrimStart('/'), html, Encoding.GetEncoding("GB2312"));

            //更新索引
            SearchEngine.Simple.Update(blogID.ToString(), CKUser.Username, cat.ToString(), title, body, tag, filepath);

            return filepath;
        }
    }
}
