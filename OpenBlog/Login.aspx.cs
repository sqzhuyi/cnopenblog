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
using DLL;

public partial class Login : BasePage
{
    protected string redirect = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetRedirect();

            if (Request.QueryString["logout"] != null)
            {
                CKUser.Logout();
                if (redirect == "") redirect = "login.aspx";
                Response.Redirect(redirect);
            }
            if (Request.QueryString["n"] != null && Request.QueryString["p"] != null)
            {
                SignIn(Request.QueryString["n"].Trim(), Request.QueryString["p"], Request.QueryString["r"] != null);
            }
            if (CKUser.Username != "")
            {
                if (redirect == "null") redirect = "";

                if (redirect != "") Response.Redirect(redirect);
                else Response.Redirect(CKUser.Username + Settings.Ext);
            }
        }
    }

    private void GetRedirect()
    {
        if (Request.QueryString["redirect"] != null)
        {
            redirect = Request.QueryString["redirect"];
        }
        else if (Request.QueryString["from"] != null)
        {
            redirect = Request.QueryString["from"];
        }
        else if (Request.UrlReferrer != null)
        {
            redirect = Request.UrlReferrer.ToString();
        }
        if (redirect.ToLower().Contains("register.aspx"))
        {
            redirect = "me.aspx";
        }
    }

    private void SignIn(string u, string p, bool remember)
    {
        DBUser user = new DBUser(u);
        bool ok = false;
        if (user.Username != "")
        {
            ok = p.ToLower() == user.Password.ToLower();
        }
        if (ok)
        {
            CKUser.Login(user.Username, user.Fullname, user.Email, remember);

            if (redirect == "null") redirect = "";

            if (redirect != "") Response.Redirect(redirect);
            else Response.Redirect(CKUser.Username + Settings.Ext);
        }
        else
        {
            Response.Redirect("login.aspx?er=1");
        }
    }
}
