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
using System.Text.RegularExpressions;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Request.Url.ToString();
        if (url.Contains("?404;"))
        {
            url = url.Substring(url.IndexOf("?404;")).ToLower().Replace("http://", "");
            string q = url.Substring(url.IndexOf("/") + 1);
            string name = q.Split('?')[0];
            Server.Transfer("~/blogger.aspx?q=" + Server.UrlEncode(q), false);
        }
    }
}
