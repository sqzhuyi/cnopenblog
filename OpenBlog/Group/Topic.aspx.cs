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
using System.Data.SqlClient;
using System.Text;
using DLL;

public partial class Group_Topic : BasePage
{
    protected int topicID;
    protected string title;

    protected int groupID;

    protected void Page_Load(object sender, EventArgs e)
    {
        topicID = 0;
        if (Request.QueryString["q"] != null)
        {
            int.TryParse(Request.QueryString["q"], out topicID);
        }
        if (topicID < 10000)
        {
            Server.Transfer("~/note.aspx?q=NoTopic");
        }
        BindData();
    }

    private void BindData()
    {
        string sql = "exec getTopic " + topicID.ToString();
        DataSet ds = DB.GetDataSet(sql);

        DataTable dt_topic = ds.Tables[0];
        DataTable dt_restore = ds.Tables[1];
        DataTable dt_admin = ds.Tables[2];

        if (dt_topic.Rows.Count == 0)
        {
            Server.Transfer("~/note.aspx?q=NoTopic");
        }
        DataRow row = null;
        row = dt_topic.Rows[0];

        groupID = (int)row["g_id"];
        title = row["t_title"].ToString();

        Page.Title = title + " - cnOpenBlog";

        StringBuilder sb = new StringBuilder();

        sb.Append("<p><a name='top'></a>");
        sb.Append("<a href='/'>首页</a> &gt;&gt; <a href='/group/?all'>群组</a> &gt;&gt; ");
        sb.AppendFormat("<a href='/group/{0}{1}'>{2}</a> &gt;&gt; ", groupID, Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
        sb.AppendFormat("<span>{0}</span>", Tools.HtmlEncode(title));
        sb.Append("</p>");

        sb.AppendFormat("<h2>{0}</h2>", Tools.HtmlEncode(title));

        bool isAdmin = CKUser.IsLogin && dt_admin.Select("gm_user_name='" + CKUser.Username + "'").Length > 0;

        if (isAdmin)
        {
            sb.Append("<p id='sp_admin' style='text-align:right;'>");
            sb.AppendFormat("<a class='delete' href='{0}void(0);' onclick='{0}deleteIt();{1}'>删除该贴</a> &nbsp; <a class='edit' href='{0}void(0);' onclick='{0}editIt();{1}'>编辑内容</a>", "javascript:", "return false;");
            if (row["t_hide"].ToString() == "0")
                sb.AppendFormat(" &nbsp; <a class='hide' href='{0}void(0);' onclick='{0}hideIt(this);{1}'>屏蔽该贴</a>", "javascript:", "return false;");
            else
                sb.AppendFormat(" &nbsp; <a class='hide' href='{0}void(0);' onclick='{0}hideIt(this);{1}'>撤销屏蔽</a>", "javascript:", "return false;");
            if (row["t_istop"].ToString() == "0")
                sb.AppendFormat(" &nbsp; <a class='top' href='{0}void(0);' onclick='{0}topIt(this);{1}'>帖子置顶</a>", "javascript:", "return false;");
            else
                sb.AppendFormat(" &nbsp; <a class='top' href='{0}void(0);' onclick='{0}topIt(this);{1}'>撤销置顶</a>", "javascript:", "return false;");
            sb.Append("</p>");
        }
        else if (CKUser.Username == row["t_user_name"].ToString())
        {
            sb.Append("<p id='sp_admin' style='text-align:right;'>");
            sb.AppendFormat("<a class='delete' href='{0}void(0);' onclick='{0}deleteIt();{1}'>删除该贴</a> &nbsp; <a class='edit' href='{0}void(0);' onclick='{0}editIt();{1}'>编辑内容</a>", "javascript:", "return false;");
            sb.Append("</p>");
        }

        if (row["t_hide"].ToString() == "1")
        {
            sb.AppendFormat("<div id='hidetopic_div' class='hidetopic'>{0}</div>", Resources.Note.HideTopic);
            lblData.Text = sb.ToString();
            ds.Dispose();

            return;
        }

        sb.Append("<div class='item radius'><div class='left'><div style='line-height:22px;'>");
        sb.AppendFormat("<a href='/{0}{1}' onmouseover='javascript:overUser(this);' onmouseout='javascript:outUser();' target=_blank><img class='photo' alt='{0}' src='/upload/photo/{0}.jpg' onerror='this.src=\"/upload/photo/nophoto.jpg\";' /></a>", row["t_user_name"], Settings.Ext);
        sb.AppendFormat("<a href='javascript:void(0);' onclick='javascript:addFriend(this,\"{0}\");return false;'>加为好友</a>", row["t_user_name"]);
        sb.AppendFormat("<br /><a href='javascript:void(0);' onclick='javascript:sendMessage(this,\"{0}\");return false;'>发送私信</a>", row["t_user_name"]);
        sb.AppendFormat("<br /><a href='/{0}{1}' target=_blank>个人博客</a>", row["t_user_name"], Settings.Ext);
        sb.Append("<div class='clear'></div></div>");
        sb.AppendFormat("<p><a class='userlink' href='/{0}{1}' target=_blank>{0}</a></p>", row["t_user_name"], Settings.Ext);
        if (row["state"].ToString() != "")
        {
            sb.AppendFormat("<p>城市：<a href='/city/{0}{1}' target=_blank>{0}</a>", row["state"], Settings.Ext);
            if (row["city"].ToString() != row["state"].ToString())
                sb.AppendFormat("，<a href='/city/{0}{1}' target=_blank>{0}</a>", row["city"], Settings.Ext);
            sb.Append("</p>");
        }
        sb.Append("</div><div class='right'>");
        sb.AppendFormat("<div class='righttop' style='background-color:#CFD6E1;'>发表于：{0}<span class='spright'>楼主</span></div>", Tools.DateString(row["t_uptime"].ToString()));
        string qianming = "";
        if (row["qianming"].ToString() != "")
        {
            qianming = "<span class='line'>————————————————</span><br />" + Tools.HtmlEncode(row["qianming"].ToString());
        }
        sb.AppendFormat("<div class='rightmiddle'>{0}<p class='qianming'>{1}</p></div>", row["t_content"], qianming);
        sb.Append("<div class='rightbottom' style='background-color:#CFD6E1;'>");
        sb.AppendFormat("<span class='spleft'>回复次数：<span id='sp_reply_cnt'>{0}</span></span>", row["t_reply_cnt"]);
        sb.AppendFormat("<a class='quote' href='#reply' onclick='javascript:quoteIt(this,0,\"{0}\");'>引用</a>", row["t_user_name"]);
        sb.Append(" &nbsp; <a class='reply' href='#reply'>回复</a> &nbsp; <a href='#top'>TOP</a></div>");
        sb.Append("</div><div class='clear'></div></div>");

        int indx = 1;
        foreach (DataRow ro in dt_restore.Rows)
        {
            sb.Append("<div class='item radius'><div class='left'><div style='line-height:22px;'>");
            sb.AppendFormat("<a href='/{0}{1}' onmouseover='javascript:overUser(this);' onmouseout='javascript:outUser();' target=_blank><img class='photo' alt='{0}' src='/upload/photo/{0}.jpg' onerror='this.src=\"/upload/photo/nophoto.jpg\";' /></a>", ro["r_user_name"], Settings.Ext);
            sb.AppendFormat("<a href='javascript:void(0);' onclick='javascript:addFriend(this,\"{0}\");return false;'>加为好友</a>", ro["r_user_name"]);
            sb.AppendFormat("<br /><a href='javascript:void(0);' onclick='javascript:sendMessage(this,\"{0}\");return false;'>发送私信</a>", ro["r_user_name"]);
            sb.AppendFormat("<br /><a href='/{0}{1}' target=_blank>个人博客</a>", ro["r_user_name"], Settings.Ext);
            sb.Append("<div class='clear'></div></div>");
            sb.AppendFormat("<p><a class='userlink' href='/{0}{1}' target=_blank>{0}</a></p>", ro["r_user_name"], Settings.Ext);
            if (ro["state"].ToString() != "")
            {
                sb.AppendFormat("<p>城市：<a href='/search.aspx?location:{0}' target=_blank>{0}</a>", ro["state"]);
                if (ro["city"].ToString() != ro["state"].ToString())
                    sb.AppendFormat("，<a href='/search.aspx?location:{0}' target=_blank>{0}</a>", ro["city"]);
                sb.Append("</p>");
            }
            sb.Append("</div><div class='right'>");
            sb.AppendFormat("<div class='righttop'>回复于：{0}<span class='spright'># {1}楼</span></div>", Tools.DateString(ro["r_uptime"].ToString()), indx);
            qianming = "";
            if (ro["qianming"].ToString() != "")
            {
                qianming = "<span class='line'>————————————————</span><br />" + Tools.HtmlEncode(ro["qianming"].ToString());
            }
            if (ro["r_hide"].ToString() == "0")
                sb.AppendFormat("<div class='rightmiddle'>{0}<p class='qianming'>{1}</p></div>", ro["r_content"], qianming);
            else sb.AppendFormat("<div class='rightmiddle'><div class='hidereply'>{0}</div><p class='qianming'>{1}</p></div>", Resources.Note.HideReply, qianming);

            sb.Append("<div class='rightbottom'>");
            if (isAdmin)
            {
                if (ro["r_hide"].ToString() == "0")
                    sb.AppendFormat("<a class='hide' href='javascript:void(0);' onclick='javascript:hideReply(this,{0});return false;'>屏蔽该回复</a>", ro["r_id"]);
                else
                    sb.AppendFormat("<a class='hide' href='javascript:void(0);' onclick='javascript:hideReply(this,{0});return false;'>取消屏蔽</a>", ro["r_id"]);
                sb.Append(" &nbsp; ");
            }
            sb.AppendFormat("<a class='quote' href='#reply' onclick='javascript:quoteIt(this,{1},\"{0}\");'>引用</a>", ro["r_user_name"], indx);
            sb.Append(" &nbsp; <a class='reply' href='#reply'>回复</a> &nbsp; <a href='#top'>TOP</a></div>");
            sb.Append("</div><div class='clear'></div></div>");

            indx++;
        }

        lblData.Text = sb.ToString();

        ds.Dispose();
    }
}
