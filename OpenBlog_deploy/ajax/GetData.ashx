<%@ WebHandler Language="C#" Class="GetData" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using DLL;

public class GetData : IHttpHandler
{
    HttpContext http = null;
    
    public void ProcessRequest(HttpContext context)
    {
        http = context;
        http.Response.ContentType = "text/plain";

        AjaxRequest();
    }

    private void AjaxRequest()
    { 
        if (Par("getsubcat") != null)
        {
            BindSubcat();
            return;
        }
        if (Par("resetsignbox") != null)
        {
            ResetSignbox();
            return;
        }
        if (Par("getblogdata") != null)
        {
            GetBlogData();
            return;
        }
        if (Par("getoneclassblogs") != null)
        {
            GetOneClassBlogs();
            return;
        }
        if (Par("gettopgroup") != null)
        {
            GetTopGroup();
            return;
        }
        if (Par("gethuati") != null)
        {
            GetHuati();
            return;
        }
        if (Par("getlikegroup") != null)
        {
            GetLikeGroup();
            return;
        }
        if(Par("getuserdata")!=null)
        {
            GetUserData();
            return;
        }
        if (Par("getshortblog") != null)
        {
            GetShortBlog();
            return;
        }
        if (Par("getshortblogreply") != null)
        {
            GetShorBlogReply();
            return;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string Par(string key)
    {
        return http.Request[key];
    }
    private void Print(string s)
    {
        http.Response.Write(s);
    }

    private void BindSubcat()
    {
        int cat = 0;
        if (!int.TryParse(Par("cat"), out cat))
        {
            return;
        }
        string sql = "select [_id],[_name] from [subcategory] where [f_id]=" + cat.ToString();
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("${0}*{1}", row["_id"], row["_name"]);
        }
        if (sb.Length == 0) sb.Append("$null");

        Print(sb.Remove(0, 1).ToString());
    }

    private void ResetSignbox()
    {
        if (CKUser.Username != "")
        {
            string str = "";

            str = "<table cellpadding=0 cellspacing=0 class='log22'><tr><td class='log22left'></td>";
            str += "<td><a id='login_a' href='/{0}{1}' title='个人主页'>{0}</a></td><td class='dot'></td><td><a href='/baseinfo.aspx' title='个人管理'>管 理</a></td><td class='dot'></td><td><a href='/login.aspx?logout=1' title='注销账户'>注 销</a></td>";
            str += "<td class='log22right'></td></tr></table>";
            str += "<a href='/shortblog/' title='一句话博客'><b>迷你博客</b></a><a href='/group/' title='博友群组'><b>群 组</b></a><a href='/100{1}' title='查看最新的文章'><b>最新文章</b></a><a href='/' title='cnOpenBlog首页'><b>首 页</b></a>";
            str = String.Format(str, CKUser.Username, Settings.Ext);

            Print(str);
        }
        else Print("null");
    }

    private void GetBlogData()
    {
        int blogID = int.Parse(Par("getblogdata"));

        int setread = 0;
        if (Cookie.GetCookie("setread") != blogID.ToString())
        {
            setread = 1;
            Cookie.SetCookie("setread", blogID.ToString(), DateTime.Now.AddMinutes(1));
        }
        string sql = String.Format("exec getBlogData {0},{1}", blogID, setread);
        DataSet ds = DB.GetDataSet(sql);

        StringBuilder sb = new StringBuilder();
        
        DataTable dt = ds.Tables[0];//read_cnt,comment_cnt,grade,no_comment
        sb.AppendFormat("{0}|{1}|{2}|{3}$#$", dt.Rows[0]["read_cnt"], dt.Rows[0]["comment_cnt"], dt.Rows[0]["grade"], dt.Rows[0]["no_comment"]);

        dt = ds.Tables[1];//comment table(fullname,url,content,uptime)
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("{0}<col>{1}<col>{2}<col>{3}<row>", row["fullname"], row["url"], Tools.HtmlEncode(row["content"].ToString()).Replace("$#$", "$ # $"), Tools.DateString(row["uptime"].ToString()));
        }
        if (dt.Rows.Count > 0) sb.Remove(sb.Length - 5, 5);
        sb.Append("$#$");

        dt = ds.Tables[2];//newest blog(title,filepath,subcat_id,uc_name)
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("{0}<col>{1}<col>{2}<col>{3}<row>", Tools.HtmlEncode(row["title"].ToString()).Replace("$#$", "$ # $"), row["filepath"], row["subcat_id"], Tools.HtmlEncode(row["uc_name"].ToString()));
        }
        if (dt.Rows.Count > 0) sb.Remove(sb.Length - 5, 5);
        sb.Append("$#$");

        dt = ds.Tables[3];//blog category(cnt,uc_id,uc_name)
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("{0}<col>{1}<col>{2}<row>", row["cnt"], row["uc_id"], Tools.HtmlEncode(row["uc_name"].ToString()));
        }
        if (dt.Rows.Count > 0) sb.Remove(sb.Length - 5, 5);

        dt = ds.Tables[4];//next blog(_id,title,filepath)
        if (dt.Rows.Count == 0) sb.Append("$#$0<col>");
        else sb.AppendFormat("$#${0}<col>{1}", Tools.HtmlEncode(dt.Rows[0]["title"].ToString()).Replace("$#$", "$ # $"), dt.Rows[0]["filepath"]);
        sb.Append("$#$");
        
        dt = ds.Tables[5];//group
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("{0}<col>{1}<row>", row["g_id"], Tools.HtmlEncode(row["g_name"].ToString()));
        }
        if (dt.Rows.Count > 0) sb.Remove(sb.Length - 5, 5);
        
        ds.Dispose();

        Print(sb.ToString());
    }

    private void GetOneClassBlogs()
    {
        int blogid = int.Parse(Par("getoneclassblogs"));
        string sql = "select [_id],[title],[filepath],[edit_time] from [blog] where [subcat_id]=(select [subcat_id] from [blog] where [_id]=" + blogid.ToString() + ") order by [edit_time] desc";
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append("<ul>");
        foreach (DataRow row in dt.Rows)
        {
            if ((int)row["_id"] == blogid) sb.AppendFormat("<li><b>{0}</b><span style='color:#999;padding-left:20px;'>{1}</span></li>", Tools.HtmlEncode(row["title"].ToString()), Tools.DateString(row["edit_time"].ToString()));
            else sb.AppendFormat("<li><a href='{0}'>{1}</a><span style='color:#999;padding-left:20px;'>{2}</span></li>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()), Tools.DateString(row["edit_time"].ToString()));
        }
        sb.Append("</ul>");
        
        Print(sb.ToString());
    }

    private void GetTopGroup()
    {
        string kind = Par("gettopgroup");
        int catID = 0;
        if (Par("cat") != null)
        {
            int.TryParse(Par("cat"), out catID);
        }
        SqlParameter[] pars = new SqlParameter[3];
        pars[0] = new SqlParameter("@kind", kind);
        pars[1] = new SqlParameter("@cat_id", catID);
        pars[2] = new SqlParameter("@num", 20);

        DataTable dt = DB.TableFromProcedure("getTopGroup", pars);

        StringBuilder sb = new StringBuilder();

        int i = 1;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div lang={1} class='top20item{0}'>", i % 2 == 0 ? " top20item2" : "", row["g_id"]);
            sb.AppendFormat("<p class='number'>{0}</p>", i.ToString().PadLeft(2, '0'));
            sb.AppendFormat("<p class='name'><a href='/group/{0}{1}'>{2}</a></p>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            sb.AppendFormat("<p class='redu'>{0}</p>", row["g_redu"]);
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        sb.Append("<div class='clear'></div>");//必须 js用

        Print(sb.ToString());
    }

    private void GetHuati()
    {
        string kind = Par("gethuati");
        int catID = 0;
        if (Par("cat") != null)
        {
            int.TryParse(Par("cat"), out catID);
        }
        SqlParameter[] pars = new SqlParameter[3];
        pars[0] = new SqlParameter("@kind", kind);
        pars[1] = new SqlParameter("@cat_id", catID);
        pars[2] = new SqlParameter("@num", 10);

        DataTable dt = DB.TableFromProcedure("getTopTopic", pars);
        
        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='ht_item{0}'>", (i % 2 == 1 ? " ht_item2" : ""));
            sb.AppendFormat("<p class='ht_title'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<p class='ht_author'><a href='/{0}{1}'>{0}</a></p>", row["t_user_name"], Settings.Ext);
            sb.AppendFormat("<p class='ht_time'>{0}</p>", Tools.DateString(row["t_uptime"].ToString()));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }

        Print(sb.ToString());
    }

    private void GetLikeGroup()
    {
        string name = Par("getlikegroup");
        
        name = Regex.Replace(name, @"[\W]+", "%").Trim('%');

        string sql = "select top 8 [g_id],[g_name] from [group] where [g_name] like '%" + name + "%'";
        DataTable dt = DB.GetTable(sql);
        
        StringBuilder sb = new StringBuilder();
        if (dt == null || dt.Rows.Count == 0)
        {
            sb.Append("none");
        }
        else
        {
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<row>{0}<col>{1}", row["g_id"], Tools.HtmlEncode(row["g_name"].ToString()));
            }
            sb.Remove(0, 5);
        }
        Print(sb.ToString());
    }

    private void GetUserData()
    {
        string name = Par("getuserdata").Replace("'", "");

        DBUser u = new DBUser(name);
        if (!u.Exist)
        {
            Print("null");
            return;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(u.Fullname);
        sb.Append("<col>");
        sb.Append(u.Sex);
        sb.Append("<col>");
        if (u.ShowBirthday) sb.Append((DateTime.Now.Year - u.Birthday.Year).ToString());
        sb.Append("<col>");
        sb.Append(u.Hangye);
        sb.Append("<col>");
        if (u.ShowEmail) sb.Append(u.Email);
        sb.Append("<col>");
        if (u.ShowQQ) sb.Append(u.QQ);
        sb.Append("<col>");
        if (u.ShowMSN) sb.Append(u.MSN);
        sb.Append("<col>");
        sb.Append(u.ViewCount.ToString());
        sb.Append("<col>");
        sb.Append(Tools.HtmlEncode(u.Jianjie));

        Print(sb.ToString());
    }

    private void GetShortBlog()
    {
        int pageIndex = 0, pageSize = 20, pageCount = 1;
        int pageNumber = 9;
        int kind = 1;//个人

        if (int.TryParse(Par("p"), out pageIndex))
            pageIndex -= 1;
        else pageIndex = 0;
        
        int.TryParse(Par("getshortblog"), out kind);

        string un = Par("uname") != null ? Par("uname").Replace("'", "") : CKUser.Username;

        string sqlc = "select count(*) from [short_blog]";
        if (kind == 1) sqlc = "select count(*) from [short_blog] where [sb_user_name]='" + un + "'";
        else if (kind == 2) sqlc = "select count([sb_id]) from [short_blog],[friends] where [sb_user_name]=[f_friend_name] and [f_user_name]='" + un + "'";

        int cnt = (int)DB.GetValue(sqlc);
        if (cnt == 0)
        {
            Print("<div class='pagelist'></div>");
            return;
        }
        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        string sql = String.Format("exec getShortBlog '{0}',{1},{2},{3}", un, kind, pageIndex, pageSize);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='item'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' title='查看{0}的博客'><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a href='/shortblog/{0}{1}' title='查看{2}的迷你博客'>{2}</a>", row["_name"], Settings.Ext, row["fullname"]);
            if (row["city"].ToString() != "") sb.AppendFormat("（<a href='/city/{0}{1}'>{0}</a>）", row["city"], Settings.Ext);
            if (row["hangye"].ToString() != "") sb.AppendFormat(" 行业：<a href='/industry/{0}{1}'>{0}</a>", row["hangye"], Settings.Ext);
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(row["sb_content"].ToString()));
            sb.AppendFormat("<span class='em'>{0} <a class='reply' href='{1}' onclick='javascript:replyItem(this,{2});return false;'>", Tools.FormatDate(row["sb_uptime"].ToString()), Strings.JSVoid, row["sb_id"]);
            sb.AppendFormat("回复（<span id='sp_r_{0}'>{1}</span>）</a></span></div>",row["sb_id"], row["sb_reply_cnt"]);
            sb.Append("<div class='clear'></div></div>");
        }
        if (pageCount > 1)
        {
            string url = "javascript:doPager(" + kind.ToString() + ",{0});";
            sb.AppendFormat("<div class='pagelist'>{0}</div>", Tools.GetPager(pageIndex, pageCount, pageNumber, url));
        }
        Print(sb.ToString());
    }

    private void GetShorBlogReply()
    {
        int sb_id = int.Parse(Par("sb_id"));

        string sql = "select top 5 [sbr_id],[sbr_content],[sbr_uptime],[_name],[fullname],[city],[hangye] from [short_blog_reply],[users]";
        sql += " where [sbr_user_name]=[_name] and [sbr_sb_id]=" + sb_id.ToString() + " order by [sbr_id] desc";

        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Select("", "sbr_id asc"))
        {
            sb.Append("<div class='item2'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' title='查看{0}的博客'><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='rcol2'><a href='/shortblog/{0}{1}' title='查看{2}的迷你博客'>{2}</a>", row["_name"], Settings.Ext, row["fullname"]);
            if (row["city"].ToString() != "") sb.AppendFormat("（<a href='/city/{0}{1}'>{0}</a>）", row["city"], Settings.Ext);
            if (row["hangye"].ToString() != "") sb.AppendFormat(" 行业：<a href='/industry/{0}{1}'>{0}</a>", row["hangye"], Settings.Ext);
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(row["sbr_content"].ToString()));
            sb.AppendFormat("<span class='em'>{0}</span></div>", Tools.FormatDate(row["sbr_uptime"].ToString()));
            sb.Append("<div class='clear'></div></div>");
        }
        sb.Append("<p id='rp_line'>");
        if (dt.Rows.Count > 0)
        {
            sb.AppendFormat("<a class='more' href='/shortblog/sblog.aspx?q={0}'>more...</a>", sb_id);
        }
        sb.Append("</p>");
        
        Print(sb.ToString());
    }
}