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

public partial class ShortBlog_User : UserPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblLogin.Text = Print.LoginBox().Replace("width:160px;", "width:130px;").Replace("width:140px;", "width:110px;");
        BindHot();
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
