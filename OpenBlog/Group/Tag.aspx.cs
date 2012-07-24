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

public partial class Group_Tag : BasePage
{
    protected string tag;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        tag = "";
        if (Request.QueryString["q"] != null)
        {
            tag = TagTools.Filter(Request.QueryString["q"]).Replace(",", "");
        }
        if (tag == "")
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        BindGroups();
        BindHotGroups();

        lblLogin.Text = Print.LoginBox();

        Page.Title = "群组Tag:" + tag + " - cnOpenBlog";

    }

    private void BindGroups()
    {
        string sql = "select * from [group] where [g_tags] like '%" + tag + "%'";
        DataTable dt = DB.GetTable(sql);

        int cnt = dt != null ? dt.Rows.Count : 0;

        if (cnt == 0) return;

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div style='margin-top:16px;margin-left:14px;'>");
            sb.AppendFormat("<div class='ileft'><a href='/{0}{1}' title='{0}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<div class='iright'><a class='bold' href='/group/{0}{1}' target=_blank>{2}</a>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(row["g_description"].ToString()));
            sb.AppendFormat("<span class='em'>标签：{0}</span></div>", TagTools.ToLinks(row["g_tags"].ToString(), true));
            sb.Append("<div class='clear'></div></div>");
        }
        lblGroupList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = String.Format("/group/tag/{0}/{1}{2}", tag, "{0}", Settings.Ext);
            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }

    private void BindHotGroups()
    {
        DataTable dt = Data.GetHotGroup(8);

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='hotitem'><a href='/group/{0}{1}' target=_blank><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<p><a href='/group/{0}{1}' target=_blank>{2}</a></p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            if (++i % 2 == 0) sb.Append("<div class='clear'></div>");
        }
        lblHotGroups.Text = sb.ToString();
    }
}
