using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace DLL
{
    /// <summary>
    /// 数据库插入操作
    /// </summary>
    public class Insert
    {
        /// <summary>
        /// 向数据库插入一个用户
        /// </summary>
        public static void User(string name, string pwd, string fullname, string sex, DateTime birthday, string email)
        {
            SqlParameter[] pars = new SqlParameter[6];

            pars[0] = new SqlParameter("@name", name);
            pars[1] = new SqlParameter("@pwd",pwd);
            pars[2] = new SqlParameter("@fullname", fullname);
            pars[3] = new SqlParameter("@sex", sex);
            pars[4] = new SqlParameter("@birthday", birthday);
            pars[5] = new SqlParameter("@email", email);

            DB.Procedure("addUser", pars);
        }

        public static string CreateBlog(string title, int cat, int subcat, string zhaiyao, string tag, string body, int nocomment)
        {
            string username = CKUser.Username;

            tag = TagTools.Filter(tag);

            SqlParameter[] pars = new SqlParameter[7];

            pars[0] = new SqlParameter("@user_name", username);
            pars[1] = new SqlParameter("@cat_id", cat);
            pars[2] = new SqlParameter("@subcat_id", subcat);
            pars[3] = new SqlParameter("@title", title);
            pars[4] = new SqlParameter("@zhaiyao", zhaiyao);
            pars[5] = new SqlParameter("@tags", tag);
            pars[6] = new SqlParameter("@no_comment", nocomment);

            int blogID = DB.Procedure("addBlog", pars);

            string filepath = "blog\\" + DateTime.Now.ToString("yyyyMM") + "\\" + blogID.ToString() + ".html";

            foreach (string _t in tag.ToLower().Split(','))
            {
                Tag.Add(_t);
            }

            DBUser dbuser = new DBUser(username);

            string html = IO.LoadFile(Settings.BlogMasterPath, Encoding.GetEncoding("GB2312"));

            StringBuilder sb;

            html = html.Replace("<title></title>", "<title>" + Tools.HtmlEncode(title) + " - cnOpenBlog</title>");
            html = html.Replace("<meta name=\"keywords\" content=\"\" />", "<meta name=\"keywords\" content=\"" + Tools.HtmlEncode(title).Replace("\"", "") + ", " + tag + "\" />");
            html = html.Replace("<!--title start--><!--title end-->", "<!--title start-->" + Tools.HtmlEncode(title) + "<!--title end-->");

            sb = new StringBuilder();
            sb.AppendFormat("<h1 class='pagetitle'>{0}</h1><h5 class='pagesubtitle'>{1}</h5>", Tools.HtmlEncode(dbuser.BlogTitle), Tools.HtmlEncode(dbuser.BlogSubtitle));
            html = html.Replace("<!--blog title start--><!--blog title end-->", "<!--blog title start-->" + sb.ToString() + "<!--blog title end-->");

            sb = new StringBuilder();
            sb.AppendFormat("<p style='text-align:center;'><a href='/{0}{1}'><img id='author_img' class='photo' src='/upload/photo/{0}.jpg' onerror='this.src=\"/upload/photo/nophoto.jpg\";' alt='{0}' /></a></p>", username,Settings.Ext);
            string age = "";
            if (dbuser.ShowBirthday) age = ", " + Convert.ToString(DateTime.Now.Year - dbuser.Birthday.Year);
            sb.AppendFormat("<p><a href='/{0}{4}'>{1}</a>，{2}{3}</p>", username, dbuser.Fullname, dbuser.Sex, age,Settings.Ext);
            if (dbuser.State != "")
            {
                sb.Append("<p>城市：");
                if (dbuser.City.Length == 2 || dbuser.State.Substring(0, 2) == dbuser.City.Substring(0, 2))
                {
                    sb.AppendFormat("<a href='/city/{0}{1}'>{0}</a>", dbuser.City, Settings.Ext);
                }
                else
                {
                    sb.AppendFormat("<a href='/city/{2}{1}'>{2}</a>，<a href='/city/{0}{1}'>{0}</a>", dbuser.City, Settings.Ext, dbuser.State);
                }
                sb.Append("</p>");
            }
            if (dbuser.Hangye != "") sb.AppendFormat("<p>行业：<a href='/industry/{0}{1}'>{0}</a></p>", dbuser.Hangye, Settings.Ext);
            if (dbuser.ShowEmail) sb.AppendFormat("<p>Email：<a href='mailto:{0}'>{0}</a></p>", dbuser.Email);
            if (dbuser.ShowQQ && dbuser.QQ != "") sb.AppendFormat("<p>QQ：<a href='http://wpa.qq.com/msgrd?V=1&Uin={0}&Site=cnopenblog.com&Menu=yes'>{0}</a></p>", dbuser.QQ);
            if (dbuser.ShowMSN && dbuser.MSN != "") sb.AppendFormat("<p>MSN：<a href='mailto:{0}'>{0}</a></p>", dbuser.MSN);
            if (dbuser.Url != "")
            {
                sb.AppendFormat("<p>主页：<a href='{0}'>", dbuser.Url);
                if (dbuser.Url.Length > 24) sb.AppendFormat("{0}<br />{1}", dbuser.Url.Substring(0, 22), dbuser.Url.Substring(22));
                else sb.Append(dbuser.Url);
                sb.Append("</a></p>");
            }
            sb.AppendFormat("<br /><p>个人简介：</p><p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(dbuser.Jianjie));
            if (dbuser.Xingqu != "") sb.AppendFormat("<br /><p>兴趣爱好：</p><p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(dbuser.Xingqu));

            html = html.Replace("<!--author info start--><!--author info end-->", "<!--author info start-->" + sb.ToString() + "<!--author info end-->");

            string sql = "select top 8 [title],[filepath],[subcat_id],[uc_name] from [blog],[user_category] where [subcat_id]=[uc_id] and [user_name]='{0}' order by [blog].[_id] desc;";
            sql += "select [cnt],[uc_id],[uc_name] from (select count(*) [cnt],[subcat_id] from [blog] where [user_name]='{0}' group by [subcat_id]) tb1,[user_category] where [subcat_id]=[uc_id];";
            sql += "select top 1 [title],[filepath] from [blog] where [_id]<{3} and [user_name]='{0}' order by [_id] desc;";
            sql += "select [g_id],[g_name] from [group],[group_member] where [gm_g_id]=[g_id] and [gm_user_name]='{0}';";

            sql = String.Format(sql, username, cat, subcat, blogID);

            DataSet ds = DB.GetDataSet(sql);

            DataTable dt = ds.Tables[0];//new blog

            sb = new StringBuilder();
            sb.Append("<ol>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<li><a href='{0}'>{1}</a> - <a href='/{5}/{2}{4}' class='hui'>{3}</a></li>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()), row["subcat_id"], row["uc_name"], Settings.Ext, username);
            }
            sb.Append("</ol>");

            html = html.Replace("<!--new blog start--><!--new blog end-->", "<!--new blog start-->" + sb.ToString() + "<!--new blog end-->");

            dt = ds.Tables[1];//blog category

            sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<li><a href='/{0}/{1}{2}'>{3}（{4}）</a> - <a href='/{0}/{1}/feed{2}' target=_blank>RSS</a></li>", username, row["uc_id"], Settings.Ext, row["uc_name"], row["cnt"]);
            }
            sb.Append("</ul>");

            html = html.Replace("<!--category start--><!--category end-->", "<!--category start-->" + sb.ToString() + "<!--category end-->");
            
            sb = new StringBuilder();
            string subcatname = "未分类";
            DataRow[] rows = dt.Select("uc_id=" + subcat);
            if (rows.Length > 0) subcatname = rows[0]["uc_name"].ToString();
            sb.AppendFormat("<a href='{0}'>首页</a> &gt;&gt; <a href='/{1}{6}'>{2}</a> &gt;&gt; <a href='/{3}/{4}{6}'>{5}</a>", Settings.BaseURL, cat, CategoryB.GetNameById(cat), username, subcat, subcatname, Settings.Ext);

            html = html.Replace("<!--dao hang start--><!--dao hang end-->", "<!--dao hang start-->" + sb.ToString() + "<!--dao hang end-->");

            dt = ds.Tables[2];//prev next blog
            if (dt.Rows.Count > 0)
            {
                sb = new StringBuilder();
                sb.AppendFormat("上一篇：<a href='{0}'>{1}</a>", dt.Rows[0]["filepath"], Tools.HtmlEncode(dt.Rows[0]["title"].ToString()));
                html = html.Replace("<!--prev blog-->", sb.ToString());
            }

            dt = ds.Tables[3];//group
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
                html = html.Replace("<!--group start--><!--group end-->", "<!--group start-->" + sb.ToString() + "<!--group end-->");
            }

            html = html.Replace("<!--pubdate start--><!--pubdate end-->", "<!--pubdate start-->" + DateTime.Now.ToString(Settings.DateFormat) + "<!--pubdate end-->");

            html = html.Replace("<!--tags start--><!--tags end-->", "<!--tags start-->" + TagTools.ToLinks(tag) + "<!--tags end-->");

            //body = Tools.FormatBody(body);

            body = Regex.Replace(body, @"<img ([^>]*)>", new MatchEvaluator(delegate(Match match) { if (match.Value.Contains("OutliningIndicators"))return match.Value; else return "<img " + Strings.ImageOnload + " " + match.Groups[1].Value + ">"; }), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, @"<!--(.+?)-->", "", RegexOptions.Singleline);

            html = html.Replace("<!--body start--><!--body end-->", "<!--body start-->" + body + "<!--body end-->");

            //创建静态页
            IO.WriteFile(Settings.BasePath + filepath, html, Encoding.GetEncoding("GB2312"));

            filepath = "/" + filepath.Replace("\\", "/");
            //创建索引
            SearchEngine.Simple.Update(blogID.ToString(), username, cat.ToString(), title, body, tag, filepath);

            return filepath;
        }

        public static void AddComment(int blogID, string username, string fullname, string url, int grade, string content, string ip)
        {
            SqlParameter[] pars = new SqlParameter[7];

            pars[0] = new SqlParameter("@blog_id", blogID);
            pars[1] = new SqlParameter("@username", username);
            pars[2] = new SqlParameter("@fullname", fullname);
            pars[3] = new SqlParameter("@url", url);
            pars[4] = new SqlParameter("@grade", grade);
            pars[5] = new SqlParameter("@content", content);
            pars[6] = new SqlParameter("@ip", ip);

            DB.Procedure("addComment", pars);
        }

        public static void AddFavorite(int catID, string newCat, int blogID, string username)
        {
            SqlParameter[] pars = new SqlParameter[4];

            pars[0] = new SqlParameter("@cat_id", catID);
            pars[1] = new SqlParameter("@new_cat", newCat);
            pars[1].IsNullable = true;
            pars[2] = new SqlParameter("@blog_id", blogID);
            pars[3] = new SqlParameter("@username", username);

            DB.Procedure("addFavorite", pars);
        }

        /// <summary>
        /// 创建一个group，返回该group得ID
        /// </summary>
        public static int CreateGroup(string gname, string username, int catID, string description, string tags)
        {
            SqlParameter[] pars = new SqlParameter[5];

            pars[0] = new SqlParameter("@g_name", gname);
            pars[1] = new SqlParameter("@username", username);
            pars[2] = new SqlParameter("@cat_id", catID);
            pars[3] = new SqlParameter("@description", description);
            pars[4] = new SqlParameter("@tags", tags);

            return DB.Procedure("addGroup", pars);
        }

        public static int AddTopic(string username, int groupID, string title, string body)
        {
            SqlParameter[] pars = new SqlParameter[4];

            pars[0] = new SqlParameter("@user_name", username);
            pars[1] = new SqlParameter("@g_id", groupID);
            pars[2] = new SqlParameter("@title", title);
            pars[3] = new SqlParameter("@content", body);

            return DB.Procedure("addTopic", pars);
        }
        /// <summary>
        /// 添加一个回复
        /// </summary>
        /// <returns>省,城市</returns>
        public static string AddReply(string username, int topicID, string body)
        {
            SqlParameter[] pars = new SqlParameter[3];

            pars[0] = new SqlParameter("@user_name", username);
            pars[1] = new SqlParameter("@t_id", topicID);
            pars[2] = new SqlParameter("@content", body);

            return DB.Procedure2("addReply", pars).ToString();
        }
    }
}
