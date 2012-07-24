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
using System.Text;
using DLL;

public partial class Groups_GroupInfo : UserPage
{
    protected int groupID;

    protected string script;

    protected void Page_Load(object sender, EventArgs e)
    {
        groupID = 0;
        if (Request.QueryString["q"] != null)
        {
            int.TryParse(Request.QueryString["q"], out groupID);
        }
        if (groupID < 10000)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        BindData();
    }

    private void BindData()
    {
        Group group = new Group(groupID);
        if (!group.Exist)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        string sql = "select count(*) from [group_member] where [gm_g_id]={0} and [gm_user_name]='{1}' and [gm_class]=1";
        sql = String.Format(sql, groupID, CKUser.Username);
        int cnt = (int)DB.GetValue(sql);

        if (cnt==0)
        {
            Server.Transfer("~/note.aspx?q=NoAccess");
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("var _init_ = function(){");
        sb.AppendFormat("el('txtName').value='{0}';", group.Name.Replace("'", "\\'"));
        sb.AppendFormat("bindCat({0});", group.gCategory.ID);
        sb.AppendFormat("el('txtTags').value='{0}';", group.Tags.Replace("'", "\\'"));
        sb.AppendFormat("el('txtJianjie').value='{0}';", group.Description.Replace("'", "\\'"));
        sb.AppendFormat("el('txtGonggao').value='{0}';", group.Gonggao.Replace("'", "\\'"));
        sb.Append("};addLoad(_init_);");
        script = sb.ToString();
    }
}
