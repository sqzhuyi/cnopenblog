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

public partial class Register : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionFacade.ConfirmCode = Tools.RandomCode();
            imgCaptcha.Alt = SessionFacade.ConfirmCode;

            BindUsers();
        }

    }

    protected void BindUsers()
    {
        string sql = "select top 19 [_name] from [users] order by [_id] desc";
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<a href='/{0}{1}' title='{0}'><img alt='{0}' src='/upload/photo/{0}-s.jpg' {2} /></a>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            if (++i % 7 == 0) sb.Append("<div class='clear'></div>");
        }
        lblWho.Text = sb.ToString();
    }
}
