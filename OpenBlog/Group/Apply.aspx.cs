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

public partial class Group_Apply : UserPage
{
    protected int groupID;
    protected string groupName;

    protected void Page_Load(object sender, EventArgs e)
    {
        groupID = 0;
        if (Request.QueryString["q"] != null)
        {
            int.TryParse(Request.QueryString["q"], out groupID);
        }
        if (groupID < 10000)
        {
            Server.Transfer("~/note.aspx?q=NoGroup");
        }
        BindData();
    }

    private void BindData()
    {
        string sql = "select [g_name] from [group] where [g_id]={0};select [gm_class] from [group_member] where [gm_g_id]={0} and [gm_user_name]='{1}';";
        sql = String.Format(sql, groupID, CKUser.Username);
        DataSet ds = DB.GetDataSet(sql);
        DataTable dt1 = ds.Tables[0];
        if (dt1.Rows.Count == 0)
        {
            Server.Transfer("~/note.aspx?q=NoGroup");
        }
        groupName = dt1.Rows[0][0].ToString();

        DataTable dt2 = ds.Tables[1];
        
        if (dt2.Rows.Count==0)
        {
            Tools.PrintScript(Page, "alert('您还没有加入该群。');window.history.go(-1);");
            return;
        }
        int cla = (int)dt2.Rows[0][0];
        if (cla == 1)
        {
            Tools.PrintScript(Page, "alert('您已经是该群的管理员了。');window.history.go(-1);");
            return;
        }
        lblHotGroup.Text = Print.HotGroups(6, 2);
    }
}
