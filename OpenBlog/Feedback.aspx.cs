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

public partial class Feedback : UserPage
{
    protected string username;
    private int pageIndex = 0, pageSize = 20, pageCount = 1, pageNumber = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            username = CKUser.Username;

            BindComment();

            Page.Title = "管理评论 - cnOpenBlog";
        }
    }

    private void BindComment()
    {
        if (Request.QueryString["p"] != null)
        {
            if (int.TryParse(Request.QueryString["p"], out pageIndex))
                pageSize -= 1;
            else pageSize = 0;
        }
        string where = "[blog_id]=[blog].[_id] and [blog].[user_name]='" + username + "'";
        string sql = "select count(*) from [blog_comment],[blog] where " + where;
        int cnt = (int)DB.GetValue(sql);

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        DataTable dt = Data.FeedBack(pageIndex, pageSize, where);

        StringBuilder sb = new StringBuilder();

        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='msgbox'><div class='msgleft'>");
            sb.AppendFormat("<a href='{0}'>", row["url"]);
            if (row["user_name"].ToString() == "")
                sb.Append("<img src='/upload/photo/nophoto-s.jpg' /></a>");
            else 
                sb.AppendFormat("<img src='/upload/photo/{0}-s.jpg' onerror='this.src=\"/upload/photo/nophoto-s.jpg\";' /></a>", row["user_name"]);
            sb.Append("</div><div class='msgright'>");
            sb.AppendFormat("<p class='msgtit'><a href='{0}'>{1}</a> 于 {2} &nbsp; {3} ",row["url"],row["fullname"],Tools.DateString(row["uptime"].ToString()),Tools.IPString(row["ip"].ToString()));
            sb.AppendFormat("<a class='delmsg' href='#' onclick='javascript:deleteComment(this,{0});return false;' title='删除此条评论'></a></p>", row["_cid"]);
            sb.AppendFormat("<p>文章：<a href='{0}'>{1}</a><br />评论：{2}</p>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()), Tools.HtmlEncode(row["content"].ToString()));
            sb.Append("</div><div class='clear'></div></div>");
        }
        lblCommentList.Text = sb.ToString();
        if (pageCount > 1)
        {
            string url = "/feedback.aspx?p={0}";

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
}
