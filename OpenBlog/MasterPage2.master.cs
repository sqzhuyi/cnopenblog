using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DLL;

public partial class MasterPage2 : System.Web.UI.MasterPage
{
    protected string username;
    protected string fullname;
    protected string email;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;
        fullname = CKUser.Fullname;
        email = CKUser.Email;
    }
}
