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

public partial class ShortBlog_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CKUser.IsLogin)
        {
            Server.Transfer("user.aspx");
        }
        lblLogin.Text = Print.LoginBox().Replace("width:160px;", "width:130px;").Replace("width:140px;", "width:110px;");
        PrintData();
        BindHot();
    }

    protected void PrintData()
    {
        int pageIndex = 0, pageSize = 20, pageCount = 1;
        int pageNumber = 9;

        if (int.TryParse(Request["p"], out pageIndex))
            pageIndex -= 1;
        else pageIndex = 0;

        string sqlc = "select count(*) from [short_blog]";

        int cnt = (int)DB.GetValue(sqlc);
        if (cnt == 0)
        {
            return;
        }
        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        string sql = String.Format("exec getShortBlog '{0}',{1},{2},{3}", "", 3, pageIndex, pageSize);

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
            sb.AppendFormat("回复（<span id='sp_r_{0}'>{1}</span>）</a></span></div>", row["sb_id"], row["sb_reply_cnt"]);
            sb.Append("<div class='clear'></div></div>");
        }
        lblDataList.Text = sb.ToString();
        if (pageCount > 1)
        {
            string url = "/shortblog/?p={0}";
            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
    private void BindHot()
    {
        string sql = "select top 8 [sb_id],[sb_user_name],[sb_content],[sb_uptime] from [short_blog]";
        sql += " where datediff(day,[sb_uptime],getdate())=0 order by [sb_reply_cnt] desc";
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='item'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' title='{0}'><img alt='{0}' src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["sb_user_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a href='/shortblog/sblog.aspx?q={0}'>{1}</a></div>", row["sb_id"], Tools.HtmlEncode(row["sb_content"].ToString()));
            sb.Append("<div class='clear'></div></div>");
        }
        lblHotSB.Text = sb.ToString();
    }
}
