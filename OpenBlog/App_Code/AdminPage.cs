using System;
using System.Web.UI;

public class AdminPage : Page
{
    protected override void InitializeCulture()
    {
        if (DLL.CKUser.Username != "admin")
        {
            Response.Write("<script>top.location='/';</script>");
            Response.End();
        }
    }
}
