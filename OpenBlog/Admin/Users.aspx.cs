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

public partial class Admin_Users : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        string sql = "select top 100 * from [users] order by [_id] desc";
        DataTable dt = null;
        if (gridView1.DataSource != null)
            dt = (DataTable)gridView1.DataSource;
        else dt = DB.GetTable(sql);

        gridView1.DataSource = dt;
        gridView1.DataBind();
    }
    protected void gridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridView1.PageIndex = e.NewPageIndex;
        BindData();
    }
}
