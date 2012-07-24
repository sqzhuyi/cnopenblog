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

public partial class Blogger : BasePage
{
    protected string blogger = "";
    protected string fullname;
    protected string keywords;
    protected string ta;

    protected string titlestr, subtitlestr;

    protected string stylestr;

    protected int catID, subcatID;

    private int pageIndex = 0, pageSize = 10, pageCount = 1, pageNumber = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InPage();

            PrintTab();

            BindUserInfo();
            BindRightData();

            BindBlog();

            AddViewCount();

            keywords = titlestr + "," + subtitlestr + "," + fullname;

            lblTitle.Text = "<h1 class='pagetitle'>" + Tools.HtmlEncode(titlestr) + "</h1><h5 class='pagesubtitle'>" + Tools.HtmlEncode(subtitlestr) + "</h5>";
        }
    }

    private void InPage()
    {
        if (Request.QueryString["q"] != null)
        {
            string q = Request.QueryString["q"];
            blogger = q.Split('/')[0];

            catID = subcatID = 0;
            if (q.Contains("/"))
            {
                int.TryParse(q.Split('/')[1], out subcatID);
            }
            if (q.Split('/').Length > 2)
            {
                if (int.TryParse(q.Split('/')[2], out pageIndex))
                    pageIndex -= 1;
            }
        }
        if (blogger == "")
        {
            Response.Redirect("~/");
        }
    }

    private void PrintTab()
    {
        string str = "";
        if (CKUser.Username != "")
        {
            str = "<div class='topright'><table cellpadding=0 cellspacing=0 class='log22'><tr><td class='log22left'></td>";
            str += "<td><a id='login_a' href='/{0}{1}' title='个人主页'>{0}</a></td><td class='dot'></td><td><a href='/baseinfo.aspx' title='个人管理'>管 理</a></td><td class='dot'></td><td><a href='/login.aspx?logout=1' title='注销账户'>注 销</a></td>";
            str += "<td class='log22right'></td></tr></table>";
            str = String.Format(str, CKUser.Username, "{0}");
        }
        else
        {
            str = "<div class='topright'><a class='reg' href='/register.aspx'>快速注册</a><a href='/login.aspx' title='登录到cnOpenBlog'><b>登 录</b></a>";
        }
        str += "<a href='/shortblog/' title='一句话博客'><b>迷你博客</b></a><a href='/group/' title='博友群组'><b>群 组</b></a><a href='/100{0}' title='查看最新的文章'><b>最新文章</b></a><a href='/' title='cnOpenBlog首页'><b>首 页</b></a></div>";
        str = String.Format(str, Settings.Ext);

        lblTabs.Text = str;
    }

    private void BindRightData()
    {
        string sql = "exec getBloggerData '" + blogger + "'";
        DataSet ds = DB.GetDataSet(sql);

        DataRow row_st = ds.Tables[0].Rows[0];

        StringBuilder sb = new StringBuilder();

        DataTable dt = null;
        int i = 0;

        if (row_st["us_cat_ok"].ToString() == "1")
        {
            dt = ds.Tables[1];
            sb.AppendFormat("<p class='ptit'>文章分类 &nbsp; <a class='rssa' href='/{0}/feed{1}' title='订阅{2}的最新文章' target='_blank'>&nbsp;</a></p>", blogger, Settings.Ext, fullname);
            sb.Append("<div id='category_box' class='rbox'>");
            sb.Append("<ul>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<li{5}><a href='/{0}/{1}{2}'>{3}（{4}）</a> - <a href='/{0}/{1}/feed{2}' target='_blank'>RSS</a></li>", blogger, row["uc_id"], Settings.Ext, Tools.HtmlEncode(row["uc_name"].ToString()), row["cnt"], (int)row["uc_id"] == subcatID ? " style='font-weight:bold;'" : "");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("\r\n");
        }
        if (row_st["us_group_ok"].ToString() == "1")
        {
            dt = ds.Tables[2];
            sb.Append("<p class='ptit'>参与的群组</p>");
            sb.Append("<div id='group_box' class='rbox'>");
            i = 0;
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<div class='gitem'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a><p><a href='/group/{0}{1}'>{3}</a></p></div>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError, Tools.HtmlEncode(row["g_name"].ToString()));
                if (++i % 2 == 0) sb.Append("<div class='clear'></div>");
            }
            sb.Append("<div class='clear'></div></div>");
            sb.Append("\r\n");
        }
        if (row_st["us_friend_ok"].ToString() == "1")
        {
            dt = ds.Tables[3];
            sb.AppendFormat("<p class='ptit'>{0}的好友</p>", ta);
            sb.Append("<div id='friend_box' class='rbox'>");
            i = 0;
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<div class='flistbox'><a href='/{0}{1}'><img src='/upload/photo/{0}-s.jpg' {2} alt='{0}' /></a>", row["f_friend_name"], Settings.Ext, Strings.UserSmallImageError);
                sb.AppendFormat("<p><a href='/{0}{1}'>{0}</a></p></div>", row["f_friend_name"], Settings.Ext);
                if (++i % 3 == 0) sb.Append("<div class='clear'></div>");
            }
            sb.Append("<div class='clear'></div></div>");
            sb.Append("\r\n");
        }
        if (row_st["us_link_ok"].ToString() == "1")
        {
            dt = ds.Tables[4];
            sb.Append("<p class='ptit'>友情链接</p>");
            sb.Append("<div id='friendlink_box' class='rbox'>");
            sb.Append("<ul>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<li><a href='{0}' target=_blank>{1}</a></li>", row["ul_url"], Tools.HtmlEncode(row["ul_title"].ToString()));
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("\r\n");
        }
        if (row_st["us_msg_ok"].ToString() == "1")
        {
            sb.Append("<div style='padding:10px;'><p class='comment_a'>给作者留言</p>");
            sb.Append("<textarea id='txtMsg' class='put' rows='3' style='width:210px;'></textarea>");
            sb.Append("<p style='font-size:11px;'>( 提醒：非注册用户在留言中加上您的联系方式，作者可以很快回复您。)</p>");
            sb.Append("<p><input type='button' value='提 交' onclick='addMsg(this)' /></p></div>");
        }
        int bg = (int)row_st["us_bg"];
        GetStyle(bg);

        lblRightData.Text = sb.ToString();
    }

    private void BindUserInfo()
    {
        DBUser dbuser = new DBUser(blogger);
        if (dbuser.Username == "") Response.Redirect("~/");

        blogger = dbuser.Username;
        fullname = dbuser.Fullname;

        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("<p style='text-align:center;'><a href='/{0}{1}'><img id='author_img' src='/upload/photo/{0}.jpg' {2} alt='{0}' /></a></p>", dbuser.Username,Settings.Ext,Strings.UserBigImageError);
        string age = "";
        if (dbuser.ShowBirthday) age = "，" + Convert.ToString(DateTime.Now.Year - dbuser.Birthday.Year);
        string editstr = "";
        if (CKUser.Username == dbuser.Username)
        {
            editstr = " &nbsp; <a class='edit' href='/baseinfo.aspx'>[编辑资料]</a>";
            ta = "我";
        }
        else
        {
            editstr = String.Format(" &nbsp; <a class='addf' href='{0}' onclick='javascript:addFriend(this,\"{1}\");return false;'>[加为好友]</a>", Strings.JSVoid, dbuser.Username);
            if (dbuser.Sex == "男") ta = "他";
            else ta = "她";
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
        
        lblAuthorInfo.Text = sb.ToString();

        if (dbuser.BlogTitle != "")
        {
            titlestr = dbuser.BlogTitle;
        }
        else titlestr = blogger + "的博客";

        subtitlestr = dbuser.BlogSubtitle;
    }

    private void GetStyle(int bg)
    {
        int k = bg;
        if (Request.QueryString["style"] != null)
        {
            int.TryParse(Request.QueryString["style"], out k);
        }
        switch (k)
        {
            case 1:
                stylestr = "body { background:url(/images/blogger/bg/bg1.gif) no-repeat left top #95E4E8;} .right { background-color:#DDFFCC;} .rbox { background-color:#eefce7;}";
                break;
            case 2:
                stylestr = "body { background:url(/images/blogger/bg/bg2.gif) no-repeat left top #C4E0EC;} .right { background-color:#DAECF4;} .rbox { background-color:#eaf3f7;}";
                break;
            case 3:
                stylestr = "body { background:url(/images/blogger/bg/bg3.gif) no-repeat left top #EDECE8;} .right { background-color:#E3E2DE;} .rbox { background-color:#f1f0ec;}";
                break;
            case 4:
                stylestr = "body { background:url(/images/blogger/bg/bg4.gif) repeat-x left top #0099B9;} .right { background-color:#95E8EC;} .rbox { background-color:#d0eff1;}";
                break;
            case 5:
                stylestr = "body { background:url(/images/blogger/bg/bg5.gif) no-repeat left top #352726;} .right { background-color:#99CC33;} .rbox { background-color:#bfdc84;}";
                break;
            case 6:
                stylestr = "body { background:url(/images/blogger/bg/bg6.gif) no-repeat left top #709397;} .right { background-color:#A0C5C7;} .rbox { background-color:#c2e2e4;}";
                break;
            case 7:
                stylestr = "body { background:url(/images/blogger/bg/bg7.gif) no-repeat left top #EBEBEB;} .right { background-color:#F3F3F3;} .rbox { background-color:#f9f9f9;}";
                break;
            case 8:
                stylestr = "body { background:url(/images/blogger/bg/bg8.gif) no-repeat left top #8B542B;} .right { background-color:#EADEAA;} .rbox { background-color:#f2ebca;}";
                break;
            case 9:
                stylestr = "body { background:url(/images/blogger/bg/bg9.gif) no-repeat left top #1A1B1F;} .right { background-color:#999999;} .ptit { color:#ffffff;} .rbox { background-color:#dddddd;}";
                break;
            case 10:
                stylestr = "body { background:url(/images/blogger/bg/bg10.gif) repeat left top #642C8D;} .right { background-color:#7AC3EE;} .rbox { background-color:#b4dff8;}";
                break;
            case 11:
                stylestr = "body { background:url(/images/blogger/bg/bg11.gif) repeat left top #FF679A;} .right { background-color:#f9545c;} .ptit { color:#ffffff;} .rbox { background-color:#fb9a9e;}";
                break;
            case 12:
                stylestr = "body { background:url(/images/blogger/bg/bg12.gif) no-repeat left top #BADFCD;} .right { background-color:#FFF7CC;} .rbox { background-color:#fcf8e4;}";
                break;
        }
    }

    private void BindBlog()
    {
        string where = "[user_name]='" + blogger + "'";
        if (subcatID > 0) where += " and [subcat_id]=" + subcatID.ToString();

        string sql = "select count(*) from [blog] where {0};";
        sql += "select top 1 [sb_id],[sb_content],[sb_uptime] from [short_blog] where [sb_user_name]='{1}' order by [sb_id] desc";
        sql = String.Format(sql, where, blogger);
        DataSet ds = DB.GetDataSet(sql);

        StringBuilder sb = new StringBuilder();

        int cnt = (int)ds.Tables[0].Rows[0][0];
        DataTable dt_sb = ds.Tables[1];
        if (dt_sb.Rows.Count > 0)
        {
            sb.Append("<div class='topbox'>");
            sb.AppendFormat("{0}<span class='sm'>{1} &middot; <a href='/shortblog/sblog.aspx?q={2}'>回复</a>", Tools.HtmlEncode(dt_sb.Rows[0]["sb_content"].ToString()), Tools.FormatDate(dt_sb.Rows[0]["sb_uptime"].ToString()), dt_sb.Rows[0]["sb_id"]);
            sb.AppendFormat(" &middot; <a href='/shortblog/{0}{1}'>更多</a></span>", blogger, Settings.Ext);
            sb.Append("</div>");
        }
        if (cnt == 0)
        {
            lblBlogList.Text = sb.ToString();
            return;
        }

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        DataTable dt = Data.Blogs("uptime", pageIndex, pageSize, where, true);

        bool self = CKUser.Username == blogger;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<p class='ptitle'><a href='{0}'>{1}</a></p>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.Append("<p style='float:right;margin-top:-6px;'>");
            if (self) sb.AppendFormat("<a class='edit' href='/write.aspx?blog={0}'>[修改]</a> &nbsp; ", row["_id"]);
            sb.AppendFormat("阅读：{0} &nbsp; <a href='{1}#comment'>评论：{2}</a> &nbsp; <a class='ebtn' href='javascript:void(0);' onclick='javascript:addFavorite({3});'>收藏</a></p>", row["read_cnt"], row["filepath"], row["comment_cnt"], row["_id"]);
            sb.AppendFormat("<p class='ptime'>发表时间：{0}</p>", Tools.DateString(row["uptime"].ToString()));
            sb.AppendFormat("<p class='zhaiyao'>摘要: {0} &nbsp; - <a href='{1}'>阅读全文</a></p>", Tools.HtmlEncode(row["zhaiyao"].ToString()), row["filepath"]);
        }
        lblBlogList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = String.Format("/{0}/{1}/{2}{3}", blogger, subcatID, "{0}", Settings.Ext);

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }

    private void AddViewCount()
    {
        if (Cookie.GetCookie("viewed") != blogger)
        {
            string sql = "update [users] set [view_cnt]=[view_cnt]+1 where [_name]='{0}'";
            sql = String.Format(sql, blogger);
            DB.Execute(sql);

            Cookie.SetCookie("viewed", blogger, DateTime.Now.AddMinutes(1));
        }
    }
}
