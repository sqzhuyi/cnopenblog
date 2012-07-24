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

public partial class Group_Group : BasePage
{
    protected int groupID;

    protected string keywords;

    protected string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            groupID = 0;
            if (Request.QueryString["q"] != null)
            {
                int.TryParse(Request.QueryString["q"], out groupID);
            }
            if (groupID <= 0) Response.Redirect("~/group/");

            username = CKUser.Username;

            BindData();

            BindTopic();

            BindBlog();
        }
    }

    private void BindData()
    {
        Group group = new Group(groupID);

        if (!group.Exist)
        {
            Server.Transfer("~/note.aspx?q=NoGroup");
        }

        string sql = "select * from [group_member] where [gm_g_id]=" + groupID.ToString();
        DataTable dtMember = DB.GetTable(sql);
        DataRow[] rows = null;

        Page.Title = "群组：" + group.Name + " - cnOpenBlog";
        keywords = group.Name + ",群组," + group.Tags;

        StringBuilder sb = new StringBuilder();

        #region data
        //basic
        sb.Append("<div class='leftboxtop'></div><div class='leftbox'>");
        sb.AppendFormat("<p style='text-align:center;'><img class='photo' src='/upload/group/{0}.jpg' onerror='this.src=\"/upload/group/nophoto.jpg\";' /></p>", group.ID);
        sb.AppendFormat("<p class='ginfo'><a class='bold' href='/group/{0}{1}'>{2}</a> <span title='人气指数 {3}' style='cursor:default'>({3})</span>", group.ID, Settings.Ext, Tools.HtmlEncode(group.Name), group.Redu);
        if (username == group.Creator) sb.AppendFormat(" &nbsp; <a class='edit' href='/groups/{0}.aspx' title='修改群组资料'></a>", groupID);
        sb.AppendFormat("<br /><a href='/{0}{1}'>{0}</a><span style='padding:0px 4px; color:#999999;'>创建于</span>{2}</p>", group.Creator, Settings.Ext, group.Uptime.ToString(Settings.DateFormat));
        sb.Append("<p class='line'></p>");
        sb.AppendFormat("<p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(group.Description));
        sb.Append("</div><div class='leftboxbottom'></div>");
        if (group.Gonggao != "")
        {
            sb.Append("<br />");
            sb.Append("<div class='leftboxtop'></div><div class='leftbox'>");
            sb.Append("<span class='boxtitle'>组员公告</span><p class='line'></p>");
            sb.AppendFormat("<p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(group.Gonggao));
            sb.Append("</div><div class='leftboxbottom'></div>");
        }
        //admin
        sb.Append("<br />");
        sb.Append("<div class='leftboxtop'></div><div class='leftbox'>");
        sb.Append("<span class='boxtitle'>群组管理员</span><p class='line'></p>");
        sb.Append("<div id='admin_div'>");
        rows = dtMember.Select("gm_class=1", "gm_id asc");
        int i = 0;
        foreach (DataRow row in rows)
        {
            i++;
            sb.AppendFormat("<span class='photo_box{0}'>", i % 3 == 2 ? " cbox" : "");
            sb.AppendFormat("<a href='/{0}{1}' title='{0}'><img src='/upload/photo/{0}-s.jpg' alt='' onerror='this.src=\"/upload/photo/nophoto-s.jpg\";' /></a>", row["gm_user_name"], Settings.Ext);
            sb.AppendFormat("<br /><a href='/{0}{1}'>{0}</a></span>", row["gm_user_name"], Settings.Ext);
            if (i % 3 == 0) sb.Append("<div class='clear'></div>");
        }
        sb.Append("<div class='clear'></div>");
        sb.Append("</div>");
        sb.Append("</div><div class='leftboxbottom'></div>");
        //member
        sb.Append("<br />");
        sb.Append("<div class='leftboxtop'></div><div class='leftbox'>");
        sb.Append("<span class='boxtitle'>新加入组员</span><p class='line'></p>");
        sb.Append("<div id='member_div'>");
        rows = dtMember.Select("gm_class=0", "gm_id desc");
        i = 0;
        foreach (DataRow row in rows)
        {
            i++;
            sb.AppendFormat("<span class='photo_box{0}'>", i % 3 == 2 ? " cbox" : "");
            sb.AppendFormat("<a href='/{0}{1}' title='{0}'><img src='/upload/photo/{0}-s.jpg' alt='' onerror='this.src=\"/upload/photo/nophoto-s.jpg\";' /></a>", row["gm_user_name"], Settings.Ext);
            sb.AppendFormat("<br /><a href='/{0}{1}'>{0}</a></span>", row["gm_user_name"], Settings.Ext);
            if (i % 3 == 0) sb.Append("<div class='clear'></div>");
        }
        sb.Append("<div class='clear'></div>");
        sb.AppendFormat("<p style='text-align:right;'><a href='/group/{0}/member{1}'>查看所有成员</a></p>", groupID, Settings.Ext);
        sb.Append("</div>");
        sb.Append("</div><div class='leftboxbottom'></div>");

        #endregion

        lblLeft.Text = sb.ToString();

        sb = new StringBuilder();

        #region links

        sb.Append("<ul>");
        sb.AppendFormat("<li><a href='/newpost.aspx?q={0}' title='发表新帖'>发起新话题</a></li>", groupID);

        rows = null;
        if (username != "")
        {
            rows = dtMember.Select("gm_user_name='" + username + "'");
            if (rows.Length > 0)
            {
                if (rows[0]["gm_class"].ToString() == "1")
                {
                    sb.AppendFormat("<li><a href='/groups/{0}/member.aspx'>组员管理</a></li>", groupID);
                    if (username != group.Creator) sb.AppendFormat("<li><a href='/group/outgroup.aspx?q={0}'>退出该群</a></li>", groupID);
                }
                else
                {
                    sb.AppendFormat("<li><a href='/group/apply.aspx?q={0}'>申请当群主</a></li>", groupID);
                    sb.AppendFormat("<li><a href='/group/outgroup.aspx?q={0}'>退出该群</a></li>", groupID);
                }
            }
            else sb.AppendFormat("<li><a href='/group/joingroup.aspx?q={0}'>加入该群</a></li>", groupID);
        }
        else sb.AppendFormat("<li><a href='/group/joingroup.aspx?q={0}'>加入该群</a></li>", groupID);
        sb.Append("</ul>");
        sb.Append("<div class='line2'></div>");
        sb.Append("<ul>");
        sb.AppendFormat("<li><a href='/group/topiclist/{0}{1}'>更多群组话题</a></li>", groupID, Settings.Ext);
        sb.AppendFormat("<li><a href='/group/{0}/feed{1}'>订阅群组讨论 RSS</a></li>", groupID, Settings.Ext);
        sb.AppendFormat("<li><a href='/group/{0}/blog/feed{1}'>订阅组员文章 RSS</a></li>", groupID, Settings.Ext);
        sb.Append("</ul>");
        sb.Append("<div class='line2'></div>");
        if (rows != null && rows.Length > 0)
        {
            sb.Append("<ul>");
            sb.Append("<li><a href='/groups/post.aspx'>我发起的话题</a></li>");
            sb.Append("<li><a href='/groups/reply.aspx'>我回复的话题</a></li>");
            sb.Append("</ul>");
            sb.Append("<div class='line2'></div>");
        }
        sb.Append("<ul>");
        sb.Append("<li><a href='/group/'>群组首页</a></li>");
        sb.Append("</ul>");

        #endregion

        lblLinks.Text = sb.ToString();

        sb = new StringBuilder();

        sb.AppendFormat("<span style='padding-left:10px;'><a class='newtopic' href='/group/newpost.aspx?q={0}' title='发表新帖'>发起话题</a></span>", groupID);
        if (rows == null || rows.Length == 0)
            sb.AppendFormat("<a class='joing' href='/group/joingroup.aspx?q={0}'>加入该群</a>", groupID);

        lblRightTop.Text = sb.ToString();
    }

    private void BindTopic()
    {
        string fields = "[t_id],[t_user_name],[t_title],[t_read_cnt],[t_reply_cnt],[t_uptime],[t_last_edit_time],[t_last_reply],[t_last_reply_time]";
        string sql = "select top 12 {0} from [topic] where [t_g_id]={1} and [t_istop]=1 order by [t_last_reply_time] desc;";
        sql += "select top 12 {0} from [topic] where [t_g_id]={1} and [t_istop]=0 order by [t_last_reply_time] desc";
        sql = String.Format(sql, fields, groupID);
        
        DataSet ds = DB.GetDataSet(sql);
        DataTable dt1 = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];

        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt1.Rows)
        {
            if (i % 2 == 0)
            {
                sb.Append("<div class='topicitemtop'></div><div class='topicitem'>");
            }
            else
            {
                sb.Append("<div class='topicitem2'>");
            }
            sb.Append("<div class='topicleft'>");

            sb.AppendFormat("<p class='topictitle istop'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<p class='font11'>回复 {0} &nbsp; 浏览 {1}", row["t_reply_cnt"], row["t_read_cnt"]);
            sb.AppendFormat("<span class='spreply'>最后回复 <a href='/{0}{1}'>{0}</a> 于 {2}</span></p>", row["t_last_reply"], Settings.Ext, Tools.DateString(row["t_last_reply_time"].ToString(), true));
            sb.Append("</div><div class='topicright'>");
            sb.AppendFormat("<a class='pha' href='/{0}{1}'><img src='/upload/photo/{0}-s.jpg' alt='' {2} /></a>", row["t_user_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<p class='rightfabiao'><a href='/{0}{1}'>{0}</a> 发表<br />于 {2}</p>", row["t_user_name"], Settings.Ext, Tools.DateString(row["t_uptime"].ToString(), true));
            sb.Append("</div><div class='clear'></div></div>");
            if (i % 2 == 0) sb.Append("<div class='topicitembottom'></div>");
            i++;
        }
        foreach (DataRow row in dt2.Rows)
        {
            if (i == 12) break;

            sb.AppendFormat("<div class='topicitem{0}'>", i % 2 == 1 ? "2" : "");
            sb.Append("<div class='topicleft'>");
            sb.AppendFormat("<p class='topictitle'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<p class='font11'>回复 {0} &nbsp; 浏览 {1}", row["t_reply_cnt"], row["t_read_cnt"]);
            sb.AppendFormat("<span class='spreply'>最后回复 <a href='/{0}{1}'>{0}</a> 于 {2}</span></p>", row["t_last_reply"], Settings.Ext, Tools.DateString(row["t_last_reply_time"].ToString(), true));
            sb.Append("</div><div class='topicright'>");
            sb.AppendFormat("<a class='pha' href='/{0}{1}'><img src='/upload/photo/{0}-s.jpg' alt='' {2} /></a>", row["t_user_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<p class='rightfabiao'><a href='/{0}{1}'>{0}</a> 发表<br />于 {2}</p>", row["t_user_name"], Settings.Ext, Tools.DateString(row["t_uptime"].ToString(), true));
            sb.AppendFormat("</div><div class='clear{0}'></div></div>", i % 2 == 0 ? " topicitembottom" : "");
            i++;
        }
        sb.AppendFormat("<p style='text-align:right;'><a href='/group/topiclist/{0}{1}' title='浏览更多群组话题'><img src='/images/more.gif' border='0' alt='MORE' /></a></p>", groupID, Settings.Ext);

        lblTopic.Text = sb.ToString();
    }

    private void BindBlog()
    {
        string where = "[user_name]=[gm_user_name] and [gm_g_id]=" + groupID.ToString();
        string fields = "[_id],[user_name],[title],[zhaiyao],[tags],[filepath],[read_cnt],[uptime]";

        DataTable dt = Data.GetPagingData("[blog],[group_member]", fields, "[_id]", "[_id]", 0, 6, where, true);
        if (dt == null) return;
 
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='blogitem'>");
            sb.AppendFormat("<a class='blogtitle' href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat("<a href='/{0}{1}'>{0}</a> 发表于 {2}", row["user_name"], Settings.Ext, Tools.DateString(row["uptime"].ToString()));
            sb.Append("</div><div class='topicitembottom'></div>");
            sb.AppendFormat("<p style='text-indent:2em;padding-left:10px;'>摘要：{0} - <a href='{1}'>阅读全文</a></p>", Tools.HtmlEncode(row["zhaiyao"].ToString()), row["filepath"]);
            sb.AppendFormat("<p style='text-align:right; padding-right:10px;'>阅读({0}) &nbsp; 标签：{1}</p>",row["read_cnt"], TagTools.ToLinks(row["tags"].ToString()));
        }
        sb.AppendFormat("<p style='text-align:right;'><a href='/group/{0}/blog{1}' title='浏览更多组员文章'><img src='/images/more.gif' border='0' alt='MORE' /></a></p>", groupID, Settings.Ext);

        lblBlog.Text = sb.ToString();
    }
}
