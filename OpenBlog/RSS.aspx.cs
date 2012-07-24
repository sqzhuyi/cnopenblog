using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using DLL;

public partial class RSS : System.Web.UI.Page
{
    protected string q;
    protected int size = 30;

    protected void Page_Load(object sender, EventArgs e)
    {
        q = "";
        if (Request.QueryString["q"] != null)
        {
            q = Request.QueryString["q"].ToLower();
        }
        Response.ContentType = "text/xml";
        BindData();
    }

    private void BindData()
    {
        if (q.StartsWith("group/"))
        {
            q = q.Substring(6);
            BindGroup();
        }
        else if (q.StartsWith("city/"))
        {
            BindUsers("city", q.Substring(5));
        }
        else if (q.StartsWith("industry/"))
        {
            BindUsers("industry", q.Substring(9));
        }
        else if (q.StartsWith("tag/"))
        {
            q = q.Substring(4);
            BindTag();
        }
        else
        {
            int id = 0;
            if (int.TryParse(q.Split('/')[0], out id)) BindBlog();
            else BindUserBlog();
        }
    }

    private void BindBlog()
    {
        int catID = 0;
        int.TryParse(q.Split('/')[0], out catID);
        string sort = "date";
        if (q.Contains("/")) sort = q.Split('/')[1];

        string where = "";
        if (catID > 100) where = " where [cat_id]=" + catID.ToString();

        string sql = "select top " + size.ToString() + " [_id],[user_name],[title],[cat_id],[zhaiyao],[uptime],[filepath] from [blog]";

        sql += where;

        if (sort == "read") sql += " order by [read_cnt] desc";
        else if (sort == "comment") sql += " order by [comment_cnt] desc";
        else sql += " order by [_id] desc";

        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append(RSSHead);
        sb.Append("<channel>");
        sb.Append("<title>博客订阅 - cnOpenBlog.com</title>");
        sb.AppendFormat("<link>{0}{1}/feed{2}</link>", Settings.BaseURL, q, Settings.Ext);
        sb.Append("<description></description>");
        sb.AppendFormat("<pubDate>{0}</pubDate>", DateTime.Now.ToLongTimeString());
        sb.AppendFormat("<generator>{0}{1}{2}</generator>", Settings.BaseURL, q, Settings.Ext);
        Response.Write(sb.ToString());

        foreach (DataRow row in dt.Rows)
        {
            sb = new StringBuilder();
            sb.Append("<item>");
            sb.AppendFormat("<title><![CDATA[{0}]]></title>", row["title"]);
            sb.AppendFormat("<link>{0}{1}</link>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.AppendFormat("<description><![CDATA[{0}]]></description>", row["zhaiyao"]);
            sb.AppendFormat("<author>{0}</author>", row["user_name"]);
            sb.AppendFormat("<category>{0}</category>", CategoryB.GetNameById((int)row["cat_id"]));
            sb.AppendFormat("<pubDate>{0}</pubDate>", row["uptime"]);
            sb.AppendFormat("<guid>{0}{1}</guid>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.Append("</item>");
            Response.Write(sb.ToString());
        }
        Response.Write("</channel>");
        Response.Write("</rss>");
    }

    private void BindUserBlog()
    {
        string un = q.Split('/')[0].Replace("'", "");
        string sql = "select top " + size.ToString() + " [_id],[user_name],[title],[cat_id],[zhaiyao],[uptime],[filepath] from [blog] ";
        sql += "where [user_name]='" + un + "'";
        int subid = 0;
        if (q.Split('/').Length > 2)
        {
            if (int.TryParse(q.Split('/')[1], out subid))
                sql += " and [subcat_id]=" + subid.ToString();
        }
        string cat = "";
        if (subid > 0)
        {
            cat = DB.GetValue("select [uc_name] from [user_category] where [uc_id]=" + subid.ToString()).ToString();
            cat = "[" + cat + "]";
        }
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append(RSSHead);
        sb.Append("<channel>");
        sb.AppendFormat("<title>订阅{0}的博客{1} - cnOpenBlog.com</title>", un, cat);
        sb.AppendFormat("<link>{0}{1}/feed{2}</link>", Settings.BaseURL, q, Settings.Ext);
        sb.Append("<description></description>");
        sb.AppendFormat("<pubDate>{0}</pubDate>", DateTime.Now.ToLongTimeString());
        sb.AppendFormat("<generator>{0}{1}{2}</generator>", Settings.BaseURL, q, Settings.Ext);
        Response.Write(sb.ToString());

        foreach (DataRow row in dt.Rows)
        {
            sb = new StringBuilder();
            sb.Append("<item>");
            sb.AppendFormat("<title><![CDATA[{0}]]></title>", row["title"]);
            sb.AppendFormat("<link>{0}{1}</link>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.AppendFormat("<description><![CDATA[{0}]]></description>", row["zhaiyao"]);
            sb.AppendFormat("<author>{0}</author>", un);
            sb.AppendFormat("<category>{0}</category>", CategoryB.GetNameById((int)row["cat_id"]));
            sb.AppendFormat("<pubDate>{0}</pubDate>", row["uptime"]);
            sb.AppendFormat("<guid>{0}{1}</guid>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.Append("</item>");
            Response.Write(sb.ToString());
        }
        Response.Write("</channel>");
        Response.Write("</rss>");
    }

    private void BindGroup()
    {
        int catID = 0;
        if (!int.TryParse(q.Split('/')[0], out catID))
            catID = 0;

        string sql = "select top " + size.ToString() + " [g_id],[g_name],[g_description],[g_user_name],[g_uptime] from [group]";
        if (catID > 0) sql += " where [g_cat_id]=" + catID.ToString();
        sql += " order by [g_id] desc";

        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        sb.Append(RSSHead);
        sb.Append("<channel>");
        sb.AppendFormat("<title>{0}群组订阅 - cnOpenBlog.com</title>", Category.GetNameById(catID));
        sb.AppendFormat("<link>{0}group/{1}/feed{2}</link>", Settings.BaseURL, q, Settings.Ext);
        sb.Append("<description></description>");
        sb.AppendFormat("<pubDate>{0}</pubDate>",DateTime.Now.ToLongTimeString());
        sb.AppendFormat("<generator>{0}group/{1}</generator>", Settings.BaseURL, (catID > 0 ? catID.ToString() + Settings.Ext : ""));
        Response.Write(sb.ToString());

        foreach (DataRow row in dt.Rows)
        {
            sb = new StringBuilder();
            sb.Append("<item>");
            sb.AppendFormat("<title><![CDATA[{0}]]></title>", row["g_name"]);
            sb.AppendFormat("<link>{0}group/{1}{2}</link>", Settings.BaseURL, row["g_id"], Settings.Ext);
            sb.AppendFormat("<description><![CDATA[{0}]]></description>", row["g_description"]);
            sb.AppendFormat("<author>{0}</author>", row["g_user_name"]);
            sb.AppendFormat("<pubDate>{0}</pubDate>", row["g_uptime"]);
            sb.AppendFormat("<guid>{0}group/{1}{2}</guid>", Settings.BaseURL, row["g_id"], Settings.Ext);
            sb.Append("</item>");
            Response.Write(sb.ToString());
        }
        Response.Write("</channel>");
        Response.Write("</rss>");
    }

    private void BindUsers(string kind, string key)
    {
        key = System.Text.RegularExpressions.Regex.Replace(key, @"[\W]+", "");

        string fields = "[_name],[fullname],[jianjie],[uptime]";
        string where = "";
        if (kind == "city")
        {
            if (key.Length == 2 || key.EndsWith("市")) where = "[city]='" + key + "'";
            else where = "[state]='" + key + "'";
        }
        else where = "[hangye]='" + key + "'";
        string sql = "select top 30 {0} from [users] where {1} order by [uptime]";
        sql = String.Format(sql, fields, where);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        sb.Append(RSSHead);
        sb.Append("<channel>");
        sb.AppendFormat("<title>{0}:{1} 用户订阅 - cnOpenBlog.com</title>", kind == "city" ? "城市" : "行业", key);
        sb.AppendFormat("<link>{0}{1}/{2}/feed{3}</link>", Settings.BaseURL, kind, key, Settings.Ext);
        sb.AppendFormat("<description>{0}选择为{1}的用户列表</description>", kind == "city" ? "城市" : "行业", key);
        sb.AppendFormat("<pubDate>{0}</pubDate>", DateTime.Now.ToLongTimeString());
        sb.AppendFormat("<generator>{0}{1}/{2}{3}</generator>", Settings.BaseURL, kind, key, Settings.Ext);
        Response.Write(sb.ToString());

        foreach (DataRow row in dt.Rows)
        {
            sb = new StringBuilder();
            sb.Append("<item>");
            sb.AppendFormat("<title><![CDATA[{0} ({1})]]></title>", row["_name"], row["fullname"]);
            sb.AppendFormat("<link>{0}{1}{2}</link>", Settings.BaseURL, row["_name"], Settings.Ext);
            sb.AppendFormat("<description><![CDATA[{0}]]></description>", row["jianjie"]);
            sb.AppendFormat("<author>{0}</author>", row["_name"]);
            sb.AppendFormat("<category>{0}</category>", "user");
            sb.AppendFormat("<pubDate>{0}</pubDate>", row["uptime"]);
            sb.AppendFormat("<guid>{0}{1}{2}</guid>", Settings.BaseURL, row["_name"], Settings.Ext);
            sb.Append("</item>");
            Response.Write(sb.ToString());
        }
        Response.Write("</channel>");
        Response.Write("</rss>");
    }

    private void BindTag()
    {
        string sql = "select top " + size.ToString() + " [_id],[user_name],[title],[cat_id],[zhaiyao],[uptime],[filepath] from [blog]";
        sql += " where [tags] like '%" + TagTools.Filter(q).Replace(",", "") + "%'";
        sql += " order by [_id] desc";

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        sb.Append(RSSHead);
        sb.Append("<channel>");
        sb.AppendFormat("<title>标签:{0} 博客订阅 - cnOpenBlog.com</title>", q);
        sb.AppendFormat("<link>{0}tag/{1}/feed{2}</link>", Settings.BaseURL, q, Settings.Ext);
        sb.Append("<description></description>");
        sb.AppendFormat("<pubDate>{0}</pubDate>", DateTime.Now.ToLongTimeString());
        sb.AppendFormat("<generator>{0}tag/{1}{2}</generator>", Settings.BaseURL, q, Settings.Ext);
        Response.Write(sb.ToString());

        foreach (DataRow row in dt.Rows)
        {
            sb = new StringBuilder();
            sb.Append("<item>");
            sb.AppendFormat("<title><![CDATA[{0}]]></title>", row["title"]);
            sb.AppendFormat("<link>{0}{1}</link>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.AppendFormat("<description><![CDATA[{0}]]></description>", row["zhaiyao"]);
            sb.AppendFormat("<author>{0}</author>", row["user_name"]);
            sb.AppendFormat("<category>{0}</category>",Category.GetNameById((int)row["cat_id"]));
            sb.AppendFormat("<pubDate>{0}</pubDate>", row["uptime"]);
            sb.AppendFormat("<guid>{0}{1}</guid>", Settings.BaseURL.TrimEnd('/'), row["filepath"]);
            sb.Append("</item>");
            Response.Write(sb.ToString());
        }
        Response.Write("</channel>");
        Response.Write("</rss>");
    }

    private string RSSHead
    {
        get { return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><rss version=\"2.0\">"; }
    }
}
