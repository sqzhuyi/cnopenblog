using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DLL;

public partial class Group_Create : UserPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionFacade.ConfirmCode = Tools.RandomCode();
        imgCaptcha.Alt = SessionFacade.ConfirmCode;
    }
}
