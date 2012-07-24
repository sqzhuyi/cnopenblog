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

public partial class do_EditUserCat : UserPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCategory();
            btnAdd.Attributes.Add("onclick", @"if(!document.getElementById('txtCat').value.replace(/\s/g, ''))return false;");
        }
    }

    private void BindCategory()
    {
        DataTable dt = null;
        if (GridView1.DataSource != null)
        {
            dt = (DataTable)GridView1.DataSource;
        }
        else
        {
            string sql = "select [uc_id],[uc_name] from [user_category] where [uc_user_name]='" + CKUser.Username + "' order by [uc_id]";
            dt = DB.GetTable(sql);
        }
        GridView1.DataSource = dt;
        GridView1.DataKeyNames = new string[] { "uc_id" };
        GridView1.PageSize = dt.Rows.Count;
        GridView1.DataBind();

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("$|${0}|{1}", row["uc_id"], row["uc_name"].ToString().Replace("$|$", "$#$"));
        }
        sb.Remove(0, 3);
        hd_cat.Value = sb.ToString();
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindCategory();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string id = GridView1.DataKeys[e.RowIndex][0].ToString();
        string cat = ((TextBox)GridView1.Rows[e.RowIndex].Cells[0].Controls[0]).Text;

        string sql = "update [user_category] set [uc_name]='{0}' where [uc_id]={1}";
        sql = String.Format(sql, cat, id);
        DB.Execute(sql);

        RebindGrid();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindCategory();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id0 = GridView1.DataKeys[0][0].ToString();
        string id = GridView1.DataKeys[e.RowIndex][0].ToString();

        string sql = "update [blog] set [subcat_id]={0} where [subcat_id]={1};delete from [user_category] where [uc_id]={1};";
        sql = String.Format(sql, id0, id);
        DB.Execute(sql);

        RebindGrid();
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        string cat = txtCat.Value.Replace("'", "''").Trim();
        txtCat.Value = "";
        string sql = "insert into [user_category]([uc_user_name],[uc_name]) values('{0}','{1}')";
        sql = String.Format(sql, CKUser.Username, cat);
        DB.Execute(sql);

        RebindGrid();
    }

    protected void RebindGrid()
    {
        GridView1.DataSource = null;

        GridView1.EditIndex = -1;
        BindCategory();
    }
}
