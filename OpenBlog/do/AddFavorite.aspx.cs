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

public partial class do_AddFavorite : System.Web.UI.Page
{
    protected string script;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            script = "";

            string username = CKUser.Username;
            if (username == "")
            {
                script = "changediv(2);";
                return;
            }

            int blogID = int.Parse(Request.QueryString["blog"]);
            string sql = "select [title],[filepath] from [blog] where [_id]={0};";
            sql += "select [_id],[cat_name] from [favorite_category] where [user_name]='{1}';";
            sql = String.Format(sql, blogID, username);
            DataSet ds = DB.GetDataSet(sql);
            if (ds == null) Response.Redirect("/default.html");
            DataTable dt = ds.Tables[0];
            
            script += "el('txtTitle').value='" + dt.Rows[0]["title"].ToString().Replace("'", "\\'") + "';";
            script += "el('txtUrl').value='" + Settings.BaseURL.TrimEnd('/') + dt.Rows[0]["filepath"] + "';";
            dt = ds.Tables[1];
            foreach (DataRow row in dt.Rows)
            {
                selCat.Items.Add(new ListItem(row["cat_name"].ToString(), row["_id"].ToString()));
            }
        }
    }
}
