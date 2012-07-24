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

public partial class ShortBlog_UserSB : System.Web.UI.Page
{
    protected string who;
    protected string title;
    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        who = "";
        if (Request.QueryString["q"] != null)
        {
            string q = Request.QueryString["q"];
            who = q.Split('/')[0].Replace("'", "");
            if (q.Contains("/"))
            {
                if (int.TryParse(q.Split('/')[1], out pageIndex))
                    pageIndex -= 1;
                else pageIndex = 0;
            }
        }
        if (who == "")
        {
            Response.Redirect("~/shortblog/");
        }
        BindAuthorData();
        BindShortBlog();
        BindHot();
    }

    private void BindAuthorData()
    {
        DBUser dbuser = new DBUser(who);
        if (!dbuser.Exist)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        title = dbuser.Fullname + "的迷你博客";
        Page.Title = title + " - cnOpenBlog";

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

    private void BindShortBlog()
    {
        int kind = 1;//个人

        string sqlc = "select count(*) from [short_blog] where [sb_user_name]='" + who + "'";

        int cnt = (int)DB.GetValue(sqlc);
        if (cnt == 0)
        {
            return;
        }
        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        string sql = String.Format("exec getShortBlog '{0}',{1},{2},{3}", who, kind, pageIndex, pageSize);

        DataTable dt = DB.GetTable(sql);

        StringBuilder sb = new StringBuilder();
        DataRow row;
        if (dt.Rows.Count > 0)
        {
            sb.Append("<div class='arrowtop'></div>");
            row = dt.Rows[0];
            sb.Append("<div class='item big'>");
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(row["sb_content"].ToString()));
            sb.AppendFormat("<span class='em'>{0} <a class='reply' href='{1}' onclick='javascript:replyItem(this,{2});return false;'>", Tools.FormatDate(row["sb_uptime"].ToString()), Strings.JSVoid, row["sb_id"]);
            sb.AppendFormat("回复（<span id='sp_r_{0}'>{1}</span>）</a></span>", row["sb_id"], row["sb_reply_cnt"]);
            sb.Append("</div>");

            if (who == CKUser.Username)
            {
                sb.Append("<div class='sendbox'>");
                sb.Append("<p>发表一句话博客</p>");
                sb.Append("<textarea id='txtMsg' class='put' rows='4' style='width:580px;'></textarea>");
                sb.Append("<p style='text-align:right;'>");
                sb.Append("<a class='btns' href='javascript:void(0);' onclick='javascript:sendMsg();return false;'>发 布</a><span id='sp_note'></span>");
                sb.Append("<span id='sp_num'>0</span>/240 字</p>");
                sb.Append("</div>");
            }
        }
        for (int i = 1; i < dt.Rows.Count;i++ )
        {
            row = dt.Rows[i];
            sb.Append("<div class='item'>");
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(row["sb_content"].ToString()));
            sb.AppendFormat("<span class='em'>{0} <a class='reply' href='{1}' onclick='javascript:replyItem(this,{2});return false;'>", Tools.FormatDate(row["sb_uptime"].ToString()), Strings.JSVoid, row["sb_id"]);
            sb.AppendFormat("回复（<span id='sp_r_{0}'>{1}</span>）</a></span>", row["sb_id"], row["sb_reply_cnt"]);
            sb.Append("</div>");
        }
        lblDataList.Text = sb.ToString();
        if (pageCount > 1)
        {
            string url = "/shortblog/" + who + "/{0}" + Settings.Ext;
            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
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
