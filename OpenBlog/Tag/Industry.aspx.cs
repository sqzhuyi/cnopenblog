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

public partial class Tag_Industry : BasePage
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

        Page.Title = "行业:" + tag + " - cnOpenBlog";

    }

    private void BindData()
    {
        string fields = "[_name],[fullname],[state],[city],[jianjie]";
        string where = "[hangye]='" + tag + "'";
        string sql = "select {0} from [users] where {1} order by [_name]";
        sql = String.Format(sql, fields, where);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();

        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div style='margin-top:16px;margin-left:14px;'>");
            sb.AppendFormat("<div class='ileft'><a href='/{0}{1}' title='{0}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='iright'><a class='bold' href='/{0}{1}' target=_blank>{0}</a> ({2})", row["_name"], Settings.Ext, row["fullname"]);

            if (row["state"].ToString() != "")
            {
                if (row["state"].ToString().Substring(0, 2) == row["city"].ToString().Substring(0, 2))
                {
                    sb.AppendFormat(" <a href='/city/{0}{1}'>{0}</a>", row["city"], Settings.Ext);
                }
                else
                {
                    sb.AppendFormat(" <a href='/city/{0}{1}'>{0}</a>，<a href='/city/{2}{1}'>{2}</a>", row["state"], Settings.Ext, row["city"]);
                }
            }
            sb.AppendFormat("<p><span style='color:#888888;'>简介：</span>{0}</p>", Tools.HtmlEncode(Tools.CutString(row["jianjie"].ToString(), 126)));
            sb.Append("</div><div class='clear'></div></div>");
        }
        lblDataList.Text = sb.ToString();
    }
}
