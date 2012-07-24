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

public partial class Group_Member : BasePage
{
    protected int groupID;
    protected string barstr;

    protected void Page_Load(object sender, EventArgs e)
    {
        // :/group/10000/member.ashx ==> /group/member.aspx?q=10000
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
        string sql = "select [g_id],[g_name] from [group] where [g_id]={0};";
        sql += "select [gm_user_name],[gm_uptime] from [group_member] where [gm_g_id]={0} order by [gm_user_name];";
        sql = String.Format(sql, groupID);

        DataSet ds = DB.GetDataSet(sql);
        if (ds == null || ds.Tables.Count != 2)
        {
            Server.Transfer("~/note.aspx?q=SystemError");
        }
        DataTable dtg = ds.Tables[0];
        DataTable dt = ds.Tables[1];

        barstr = String.Format("<a href='/group/{0}{1}'>{2}</a>", groupID, Settings.Ext, Tools.HtmlEncode(dtg.Rows[0]["g_name"].ToString()));

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='item'><a href='/{0}{1}' target=_blank><img src='/upload/photo/{0}-s.jpg' onerror='this.src=\"/upload/photo/nophoto-s.jpg\";' /></a><br /><a href='/{0}{1}' target=_blank>{0}</a></div>", row["gm_user_name"], Settings.Ext);
            if (++i % 8 == 0) sb.Append("<div class='clear'></div>");
        }
        sb.Append("<div class='clear'></div>");

        lblMember.Text = sb.ToString();
    }
}
