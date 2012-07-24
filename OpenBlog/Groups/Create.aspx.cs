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

public partial class Groups_Create : UserPage
{
    protected string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        BindGroups();
    }

    private void BindGroups()
    {
        string sql = "select [g_id],[g_name],[g_user_name],[g_uptime] from [group]";
        sql += " where [g_user_name]='{0}'";

        sql = String.Format(sql, username);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : "");
            sb.AppendFormat("<div class='col1'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' onerror='this.src=\"/upload/group/nophoto-s.jpg\";' /></a></div>", row["g_id"], Settings.Ext);
            sb.AppendFormat("<div class='col2'><a class='bold' href='/group/{0}{1}'>{2}</a><p>创建于：{3}</p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()), Tools.DateString(row["g_uptime"].ToString()));
            sb.Append("<div class='col3'>");
            sb.AppendFormat("<a href='/groups/{0}.aspx'>修改资料</a><a href='/groups/{0}/member.aspx'>成员管理</a>", row["g_id"]);
            sb.AppendFormat("<a href='javascript:void(0);' onclick='javascript:deleteGroup(this,{0});return false;'>解散该群</a>", row["g_id"]);
            sb.Append("</div>");
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        if (i == 0)
        {
            sb.Append("<p>您还没有创建任何群组，<a class='addg bold' href='/group/create.aspx'>马上创建</a></p>");
        }
        lblMsgList.Text = sb.ToString();
    }
}
