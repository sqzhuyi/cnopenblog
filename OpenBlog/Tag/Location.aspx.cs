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

public partial class Tag_Location : BasePage
{
    protected string tag;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        tag = "";
        if (Request.QueryString["q"] != null)
        {
            tag = System.Text.RegularExpressions.Regex.Replace(Request.QueryString["q"], @"[\W]+", "");
        }
        if (tag == "")
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        BindData();

        lblLogin.Text = Print.LoginBox();

        lblHotTags.Text = Data.GetHotTags();

        Page.Title = "城市:" + tag + " - cnOpenBlog";

    }

    private void BindData()
    {
        string fields = "[_name],[fullname],[hangye],[city],[jianjie]";
        string where = "";
        bool iscity = tag.EndsWith("市") || tag.Length == 2;
        if (iscity) where = "[city]='" + tag + "'";
        else where = "[state]='" + tag + "'";
        string sql = "select {0} from [users] where {1} order by [_name]";
        sql = String.Format(sql, fields, where);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();

        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div style='margin-top:16px;margin-left:14px;'>");
            sb.AppendFormat("<div class='ileft'><a href='/{0}{1}' title='{0}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='iright'><a class='bold' href='/{0}{1}' target=_blank>{0}</a> ({2})", row["_name"], Settings.Ext, row["fullname"]);
            if (!iscity) 
            {
                sb.AppendFormat(" <a href='/city/{0}{1}'>{0}</a>", row["city"], Settings.Ext);
            }
            if (row["hangye"].ToString() != "")
            {
                sb.AppendFormat(" <span style='color:#888888;'>行业：</span><a href='/industry/{0}{1}'>{0}</a>",row["hangye"],Settings.Ext);
            }
            sb.AppendFormat("<p><span style='color:#888888;'>简介：</span>{0}</p>", Tools.HtmlEncode(Tools.CutString(row["jianjie"].ToString(), 126)));
            sb.Append("</div><div class='clear'></div></div>");
        }
        lblDataList.Text = sb.ToString();
    }
}
