using System;
using System.Web.UI;

public class UserPage : BasePage
{
    protected override void InitializeCulture()
    {
        if (!DLL.CKUser.IsLogin)
        {
            Response.Redirect("~/login.aspx?from=" + DLL.Tools.UrlEncode(Request.RawUrl));
        }
    }
}
