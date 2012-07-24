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

public partial class ShortBlog_Sblog : System.Web.UI.Page
{
    protected int sb_id;

    protected string who;
    protected string title;

    protected void Page_Load(object sender, EventArgs e)
    {
        sb_id = 0;
        if (Request.QueryString["q"] != null)
        {
            int.TryParse(Request.QueryString["q"], out sb_id);
        }
        if (sb_id < 1)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }

        BindData();
        BindAuthorData();
        BindHot();
    }

    private void BindData()
    {
        string sql = "select * from [short_blog] where [sb_id]={0};";
        sql += "select sbr.*,[_name],[fullname],[city],[hangye] from [short_blog_reply] sbr,[users] where [sbr_user_name]=[_name] and [sbr_sb_id]={0} order by [sbr_id] desc;";
        sql = String.Format(sql, sb_id);

        DataSet ds = DB.GetDataSet(sql);
        DataTable dt_sb = ds.Tables[0];
        DataTable dt_sbr = ds.Tables[1];

        if (dt_sb.Rows.Count == 0)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        DataRow row = dt_sb.Rows[0];

        who = row["sb_user_name"].ToString();

        string con = row["sb_content"].ToString();
        if (con.Length > 50)
        {
            Page.Title = con.Substring(0, 50) + "... - 迷你博客 - cnOpenBlog";
        }
        else Page.Title = con + " - 迷你博客 - cnOpenBlog";

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<p style='font-size:20px;'><a href='/shortblog/{0}{1}' style='font-size:20px;'>@{0}</a> {2}", who, Settings.Ext, Tools.HtmlEncode(con));
        sb.AppendFormat("<span style='color:#666666;font-size:12px;padding-left:20px;'>{0}</span></p>", Tools.FormatDate(row["sb_uptime"].ToString()));
        lblSBlog.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow ro in dt_sbr.Rows)
        {
            sb.Append("<div class='item'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' title='查看{0}的博客'><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", ro["_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a href='/{0}{1}' title='查看{2}的迷你博客'>{2}</a>", ro["_name"], Settings.Ext, ro["fullname"]);
            if (ro["city"].ToString() != "") sb.AppendFormat("（<a href='/city/{0}{1}'>{0}</a>）", ro["city"], Settings.Ext);
            if (ro["hangye"].ToString() != "") sb.AppendFormat(" 行业：<a href='/industry/{0}{1}'>{0}</a>", ro["hangye"], Settings.Ext);
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(ro["sbr_content"].ToString()));
            sb.AppendFormat("<span class='em'>{0}</span></div>", Tools.FormatDate(ro["sbr_uptime"].ToString()));
            sb.Append("<div class='clear'></div></div>");
        }
        lblReply.Text = sb.ToString();

    }
    private void BindAuthorData()
    {
        DBUser dbuser = new DBUser(who);
        if (!dbuser.Exist)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        title = dbuser.Fullname + "的迷你博客";

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("<p style='text-align:center;'><a href='/{0}{1}'><img id='author_img' src='/upload/photo/{0}.jpg' {2} alt='{0}' class='photo' /></a></p>", dbuser.Username, Settings.Ext, Strings.UserBigImageError);
        string age = "";
        if (dbuser.ShowBirthday) age = "，" + Convert.ToString(DateTime.Now.Year - dbuser.Birthday.Year);
        string editstr = "";
        if (CKUser.Username == dbuser.Username)
        {
            editstr = " &nbsp; <a class='edit' href='/baseinfo.aspx'>[编辑资料]</a>";
        }
        else
        {
            editstr = String.Format(" &nbsp; <a class='addf' href='{0}' onclick='javascript:addFriend(this,\"{1}\");return false;'>[加为好友]</a>", Strings.JSVoid, dbuser.Username);
        }
        sb.AppendFormat("<p><a href='/{0}{5}'>{1}</a>，{2}{3}{4}</p>", dbuser.Username, dbuser.Fullname, dbuser.Sex, age, editstr, Settings.Ext);
        sb.AppendFormat("<p>访问次数：{0}</p>", dbuser.ViewCount);
        if (dbuser.State != "")
        {
            sb.Append("<p>城市：");
            if (dbuser.City.Length == 2 || dbuser.State.Substring(0, 2) == dbuser.City.Substring(0, 2))
            {
                sb.AppendFormat("<a href='/city/{0}{1}'>{0}</a>", dbuser.City, Settings.Ext);
            }
            else
            {
                sb.AppendFormat("<a href='/city/{2}{1}'>{2}</a>，<a href='/city/{0}{1}'>{0}</a>", dbuser.City, Settings.Ext, dbuser.State);
            }
            sb.Append("</p>");
        }
        if (dbuser.Hangye != "") sb.AppendFormat("<p>行业：<a href='/industry/{0}{1}'>{0}</a></p>", dbuser.Hangye, Settings.Ext);
        if (dbuser.ShowEmail) sb.AppendFormat("<p>Email：<a href='mailto:{0}'>{0}</a></p>", dbuser.Email);
        if (dbuser.ShowQQ && dbuser.QQ != "") sb.AppendFormat("<p>QQ：<a href='http://wpa.qq.com/msgrd?V=1&Uin={0}&Site=cnopenblog.com&Menu=yes'>{0}</a></p>", dbuser.QQ);
        if (dbuser.ShowMSN && dbuser.MSN != "") sb.AppendFormat("<p>MSN：<a href='mailto:{0}'>{0}</a></p>", dbuser.MSN);
        if (dbuser.Url != "")
        {
            sb.AppendFormat("<p>主页：<a href='{0}'>", dbuser.Url);
            if (dbuser.Url.Length > 24) sb.AppendFormat("{0}<br />{1}", dbuser.Url.Substring(0, 22), dbuser.Url.Substring(22));
            else sb.Append(dbuser.Url);
            sb.Append("</a></p>");
        }
        sb.AppendFormat("<br /><p>个人简介：</p><p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(dbuser.Jianjie));
        if (dbuser.Xingqu != "") sb.AppendFormat("<br /><p>兴趣爱好：</p><p style='text-indent:2em;'>{0}</p>", Tools.HtmlEncode(dbuser.Xingqu));

        lblAuthorData.Text = sb.ToString();
    }

    private void BindHot()
    {
        string sql = "select top 8 [sb_id],[sb_user_name],[sb_content],[sb_uptime] from [short_blog]";
        sql += " where datediff(day,[sb_uptime],getdate())=0 order by [sb_reply_cnt] desc";
        DataTable dt = DB.GetTable(sql);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<div class='item'>");
            sb.AppendFormat("<div class='col1'><a href='/{0}{1}' title='{0}'><img alt='{0}' src='/upload/photo/{0}-s.jpg' {2} /></a></div>", row["sb_user_name"], Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='col2'><a href='/shortblog/sblog.aspx?q={0}'>{1}</a></div>", row["sb_id"], Tools.HtmlEncode(row["sb_content"].ToString()));
            sb.Append("<div class='clear'></div></div>");
        }
        lblHotSB.Text = sb.ToString();
    }
}
