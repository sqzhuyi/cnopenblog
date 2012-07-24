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

public partial class PostList : UserPage
{
    protected string username;
    protected string script;

    protected int subcatID;

    private int pageIndex = 0, pageSize = 10, pageCount = 1;
    private int pageNumber = 9;

    private DataTable dtsubcat;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        if (!IsPostBack)
        {
            BindCategory();
            
            BindBlog();

            dtsubcat.Dispose();

            Page.Title = "管理我的文章 - cnOpenBlog";
        }
    }

    private void BindCategory()
    {
        string q = Request.QueryString["q"] != null ? Request.QueryString["q"] : "";

        string subcat = q.Split('/')[0];

        subcatID = 0;

        int.TryParse(subcat, out subcatID);

        if (q.Contains("/"))
        {
            if (int.TryParse(q.Split('/')[1], out pageIndex))
                pageIndex -= 1;
        }

        string sql = "select a.[cnt],[uc_id],[uc_name] from [user_category],(select count([_id]) [cnt],[subcat_id] from [blog] where [user_name]='"+username+"' group by [subcat_id]) a where [uc_id]=[subcat_id]";
        dtsubcat = DB.GetTable(sql);

        if (subcatID > 0)
        {
            if (dtsubcat.Select("uc_id=" + subcatID.ToString()).Length == 0)
                subcatID = 0;
        }

        StringBuilder sb = new StringBuilder();
        bool find = false;
        sb.Append("<ul>");
        foreach (DataRow row in dtsubcat.Rows)
        {
            if (subcatID == (int)row["uc_id"])
            {
                sb.AppendFormat("<li><a class='curr' href='/postlist/{0}.aspx'>{1}（{2}）</a></li>", row["uc_id"], Tools.HtmlEncode(row["uc_name"].ToString()), row["cnt"]);
                find = true;
            }
            else sb.AppendFormat("<li><a href='/postlist/{0}.aspx'>{1}（{2}）</a></li>", row["uc_id"], Tools.HtmlEncode(row["uc_name"].ToString()), row["cnt"]);
        }
        sb.Append("</ul>");
        if (!find) subcatID = 0;

        if (subcatID > 0) sb.Append("<p><a href='/postlist.aspx'>显示全部文章</a></p>");
        else sb.Append("<p><b>显示全部文章</b></p>");

        lblCat.Text = sb.ToString();
    }

    private void BindBlog()
    {
        string where = "[user_name]='" + username + "'";
        if (subcatID > 0) where += " and [subcat_id]=" + subcatID.ToString();

        string sqlc = "select count(*) from [blog] where " + where;

        int cnt = (int)DB.GetValue(sqlc);

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount-1, pageIndex);

        DataTable dt = Data.Blogs("uptime", pageIndex, pageSize, where, true);

        StringBuilder sb = new StringBuilder();

        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div>");
            sb.AppendFormat("<p class='title'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat("<a class='edit' href='/write.aspx?blog={0}'>修改</a><a class='delete' href='#' onclick='javascript:deleteBlog(this, {0});return false;'>删除</a></p>", row["_id"]);
            sb.AppendFormat("<p class='zhaiyao'>摘要: {0} - <a href='{1}'>阅读全文</a></p>", Tools.HtmlEncode(row["zhaiyao"].ToString()), row["filepath"]);
            sb.Append("<p class='bot'>");
            if (subcatID == 0)
                sb.AppendFormat("类别: <a href='/{0}{1}'>{2}</a> ", row["subcat_id"], Settings.Ext, Tools.HtmlEncode(dtsubcat.Select("uc_id=" + row["subcat_id"])[0]["uc_name"].ToString()));
            sb.AppendFormat("【评论:{0}】【阅读:{1}】 {2}</p>", row["comment_cnt"], row["read_cnt"], Tools.DateString(row["uptime"].ToString()));
            sb.Append("</div>");
        }
        lblBlogList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = "/postlist/" + subcatID.ToString() + "/{0}.aspx";

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
}
