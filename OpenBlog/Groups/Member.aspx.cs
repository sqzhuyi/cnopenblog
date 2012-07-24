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

public partial class Groups_Member : UserPage
{
    protected int groupID;

    protected string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        groupID = 0;
        if (Request.QueryString["q"] != null)
        {
            int.TryParse(Request.QueryString["q"], out groupID);
        }
        if (groupID < 10000)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }

        BindMember();
    }

    private void BindMember()
    {
        string sql = "select [gm_id],[gm_user_name],[gm_uptime],[gm_class] from [group_member] where [gm_g_id]=" + groupID.ToString();
        sql += " order by [gm_id]";
        DataTable dt = DB.GetTable(sql);
        if (dt == null || dt.Rows.Count == 0)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        DataRow[] rows = dt.Select("gm_user_name='" + username + "' and gm_class=1");
        if (rows.Length == 0)
        {
            Server.Transfer("~/note.aspx?q=NoAccess");
        }
        bool iscreator = dt.Rows[0]["gm_user_name"].ToString() == username;

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='mitem'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' target=_blank><img src='/upload/photo/{0}-s.jpg' alt='{0}' {2} /></a></div>", row["gm_user_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a class='bold' href='/{0}{1}' target=_blank>{0}</a>{3}<br /><span class='em'>加入时间 {2}</span></div>", row["gm_user_name"], Settings.Ext, Tools.DateString(row["gm_uptime"].ToString()), (i == 0 ? " <span class='em'>(创建者)</span>" : ""));
            sb.Append("<div class='col3'>");
            if (i > 0)
            {
                if (iscreator)
                {
                    if (row["gm_class"].ToString() == "1")
                        sb.Append("<a href='javascript:void(0);' onclick='javascript:toAdmin(this,false);return false;'>撤销管理员</a>");
                    else sb.Append("<a href='javascript:void(0);' onclick='javascript:toAdmin(this,true);return false;'>升级为管理员</a>");
                    sb.Append("<a href='javascript:void(0);' onclick='javascript:toOut(this);return false;'>踢出该群</a>");
                }
                else if (row["gm_class"].ToString() == "0")
                {
                    sb.Append("<a href='javascript:void(0);' onclick='javascript:toOut(this);return false;'>踢出该群</a>");
                }
            }
            sb.Append("</div>");
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblMsgList.Text = sb.ToString();
    }
}
