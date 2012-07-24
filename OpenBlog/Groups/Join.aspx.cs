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

public partial class Groups_Join : UserPage
{
    protected string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        BindGroups();
    }

    private void BindGroups()
    {
        string sql = "select [g_id],[g_name],[g_user_name],[g_uptime],[gm_class] from [group],[group_member]";
        sql += " where [g_id]=[gm_g_id] and [g_user_name]<>'{0}' and [gm_user_name]='{0}'";

        sql = String.Format(sql, username);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        int i = 0;
        string shenfen;
        foreach (DataRow row in dt.Rows)
        {
            shenfen = "组员";
            if (row["gm_class"].ToString() == "1") shenfen = "管理员";

            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : "");
            sb.AppendFormat("<div class='col1'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' onerror='this.src=\"/upload/group/nophoto-s.jpg\";' /></a></div>", row["g_id"], Settings.Ext);
            sb.AppendFormat("<div class='col2'><a class='bold' href='/group/{0}{1}'>{2}</a>（{3}）<p>创建于：{4}</p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()), shenfen, Tools.DateString(row["g_uptime"].ToString()));
            sb.Append("<div class='col3'>");
            if (row["gm_class"].ToString() == "1")
            {
                sb.AppendFormat("<a href='/groups/{0}.aspx'>修改资料</a><a href='/groups/{0}/member.aspx'>成员管理</a>", row["g_id"]);
            }
            sb.AppendFormat("<a href='javascript:void(0);' onclick='javascript:outGroup(this,{0});return false;'>退出该群</a>", row["g_id"]);
            sb.Append("</div>");
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        if (i == 0)
        {
            sb.Append("<p>您还没有加入任何群组，系统给您推荐以下群组，可以选择加入</p>");
            sb.AppendFormat("<div style='margin-top:20px;'>{0}<div class='clear'></div></div>", Print.HotGroups(10, 5));
            sb.Append("<p><span style='color:#ff0000;font-weight:bold;'>or</span> <a class='addg bold' href='/group/create.aspx'>创建一个自己的群组</a></p>");
        }
        lblMsgList.Text = sb.ToString();
    }
}
