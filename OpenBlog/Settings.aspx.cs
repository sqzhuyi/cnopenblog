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

public partial class bSettings : UserPage
{
    protected string username;

    protected string script;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        BindData();
    }

    private void BindData()
    {
        string sql = "select [blog_title],[blog_subtitle] from [users] where [_name]='{0}';";
        sql += "select * from [user_settings] where [us_user_name]='{0}';";
        sql += "select * from [user_link] where [ul_user_name]='{0}';";
        sql = String.Format(sql, username);

        DataSet ds = DB.GetDataSet(sql);

        StringBuilder sb = new StringBuilder();
        sb.Append("var init_0 = function(){");

        DataTable dt = ds.Tables[0];

        sb.AppendFormat("el('txtTitle').value='{0}';", dt.Rows[0]["blog_title"].ToString().Replace("'", "\\'"));
        sb.AppendFormat("el('txtSubtitle').value='{0}';", dt.Rows[0]["blog_subtitle"].ToString().Replace("'", "\\'"));

        dt = ds.Tables[1];
        DataRow row = dt.Rows[0];

        if (row["us_cat_ok"].ToString() == "1") sb.Append("el('chk_m_2').checked=true;");
        if (row["us_group_ok"].ToString() == "1") sb.Append("el('chk_m_3').checked=true;");
        if (row["us_friend_ok"].ToString() == "1") sb.Append("el('chk_m_4').checked=true;");
        if (row["us_link_ok"].ToString() == "1") sb.Append("el('chk_m_5').checked=true;click_fl(true);");
        if (row["us_msg_ok"].ToString() == "1") sb.Append("el('chk_m_6').checked=true;");
        sb.AppendFormat("el('hd_bg').value='{0}';", row["us_bg"]);

        dt = ds.Tables[2];
        sb.Append("var puts=els('fl_box','input');");
        int i = 0;
        foreach (DataRow ro in dt.Rows)
        {
            sb.AppendFormat("puts[{0}].value='{1}';", i, ro["ul_title"].ToString().Replace("'", "\\'"));
            sb.AppendFormat("puts[{0}].value='{1}';", i + 1, ro["ul_url"]);

            i += 2;
        }

        sb.Append("};addLoad(init_0);");

        script = sb.ToString();
    }
}
