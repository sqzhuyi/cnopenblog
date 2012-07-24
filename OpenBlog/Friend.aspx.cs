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

public partial class Friend : UserPage
{
    protected string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        BindFriend();
    }

    private void BindFriend()
    {
        string fields = "[f_id],[_name],[fullname],[hangye],[state],[city],[jianjie]";
        string where = "[f_friend_name]=[_name] and [f_user_name]='" + username + "'";
        string sql = "select {0} from [friends],[users] where {1}";
        sql = String.Format(sql, fields, where);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : "");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}'><img src='/upload/photo/{0}-s.jpg' {2} alt='{0}' /></a></div>", row["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a href='/{0}{1}'>{0}</a> ({2})", row["_name"], Settings.Ext, row["fullname"]);
            if (row["state"].ToString() != "")
            {
                if (row["state"].ToString().Substring(0, 2) == row["city"].ToString().Substring(0, 2))
                {
                    sb.AppendFormat(" <a href='/city/{0}{1}'>{0}</a>", row["city"], Settings.Ext);
                }
                else
                {
                    sb.AppendFormat(" <a href='/city/{0}{1}'>{0}</a>，<a href='/city/{2}{1}'>{2}</a>", row["state"], Settings.Ext, row["city"]);
                }
            }
            if (row["hangye"].ToString() != "")
            {
                sb.AppendFormat(" <span class='hui'>行业：</span><a href='/industry/{0}{1}'>{0}</a>", row["hangye"], Settings.Ext);
            }
            sb.AppendFormat("<p><span class='hui'>简介：</span>{0}</p></div>", Tools.HtmlEncode(Tools.CutString(row["jianjie"].ToString(), 60)));
            sb.AppendFormat("<div class='col3'><a href='{0}' onclick='javascript:sendMsg(this,\"{1}\");return false;'>发送消息</a><a href='{0}' onclick='javascript:deleteFriend(this,\"{1}\");return false;'>删除好友</a></div>", Strings.JSVoid, row["_name"]);
            sb.Append("<div class='clear'></div></div>");

            i++;
        }
        if (i == 0)
        {
            sb.Append("<p>您还没有添加任何好友。</p>");

            DBUser u = new DBUser(username);
            sb.Append("<ul>");
            if (u.City != "") sb.AppendFormat("<li><a href='/city/{0}{1}'>查看<b>{0}</b>的所有用户</a></li>", u.City, Settings.Ext);
            if (u.Hangye != "") sb.AppendFormat("<li><a href='/industry/{0}{1}'>查看<b>{0}</b>行业的所有用户</a></li>", u.Hangye, Settings.Ext);
            sb.Append("</ul>");
        }
        lblFriend.Text = sb.ToString();
    }
}
