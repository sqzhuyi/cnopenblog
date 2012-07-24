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

public partial class Group_EditPost : UserPage
{
    protected int topicID;
    protected string topicTitle, topicContent;

    protected int groupID;
    protected string groupName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["q"] != null)
            {//接受的是topicID要改正
                int.TryParse(Request.QueryString["q"], out topicID);
            }
            if (topicID < 10000)
            {
                Server.Transfer("~/note.aspx?q=NoTopic");
            }

            GetData();
        }
    }

    private void GetData()
    {
        string sql = "select [g_id],[g_name],[t_user_name],[t_title],[t_content] from [topic],[group] where [t_g_id]=[g_id] and [t_id]=" + topicID.ToString();
        DataTable dt = DB.GetTable(sql);
        if (dt.Rows.Count == 0)
        {
            Server.Transfer("~/note.aspx?q=NoTopic");
        }
        DataRow row = dt.Rows[0];

        groupID = (int)row["g_id"];
        groupName = row["g_name"].ToString();

        if (row["t_user_name"].ToString() != CKUser.Username)
        {
            sql = "select count(*) from [group_member] where [gm_class]=1 and [gm_g_id]=" + groupID.ToString();
            int cnt = (int)DB.GetValue(sql);
            if (cnt == 0)
            {
                Server.Transfer("~/note.aspx?q=NoAccess");
            }
        }
        topicTitle = row["t_title"].ToString().Replace("\"", "\\\"");
        topicContent = row["t_content"].ToString();
    }
}
