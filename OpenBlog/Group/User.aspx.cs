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

public partial class Group_User : BasePage
{
    protected string username;
    protected string who, sex;
    protected bool self;

    protected void Page_Load(object sender, EventArgs e)
    {
        //link: /group/zhuyi.htm ==> /group/user.aspx?q=zhuyi
        who = "";
        if (Request.QueryString["q"] != null)
        {
            who = Request.QueryString["q"].Replace("'", "").ToLower();
        }
        if (who == "")
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        username = CKUser.Username;
        self = who == username;
        if (!self)
        {
            sex = DB.GetValue("select [sex] from [users] where [_name]='" + who + "'").ToString();
            Page.Title = who + "参与的群组 - cnOpenBlog";
        }

        BindData();
    }

    private void BindData()
    {
        string sql = "exec getUserGroup '{0}';exec getUserTopic '{0}';";
        sql = String.Format(sql, who);

        DataSet ds = DB.GetDataSet(sql);
        if (ds == null || ds.Tables.Count != 7)
        {
            Server.Transfer("~/note.aspx?q=SystemError");
        }
        DataTable dt_group = ds.Tables[0];//[g_id],[g_name],[g_user_name],[g_description],[g_uptime],[g_redu],[gm_class]
        DataTable dt_member = ds.Tables[1];//cnt(成员数),gid
        DataTable dt_topic = ds.Tables[2];//cnt1(帖子数),cnt2(回复数),cnt3(阅读次数),gid

        DataTable dt_hotgroup = ds.Tables[3];

        DataTable dt_topic1 = ds.Tables[4];
        DataTable dt_topic2 = ds.Tables[5];
        DataTable dt_topic3 = ds.Tables[6];

        DataRow[] rows = null;
        DataRow[] rows2 = null;

        StringBuilder sb = new StringBuilder();

        #region created group
        rows = dt_group.Select("g_user_name='" + who + "'", "g_id asc");
        int i = 0;
        foreach (DataRow row in rows)
        {
            string gid = row["g_id"].ToString();
            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : " gitem1");
            sb.AppendFormat("<div class='itemcol1'><a href='/group/{0}{1}'><img src='/upload/group/{0}.jpg' onerror='this.src=\"/upload/group/nophoto.jpg\";' /></a></div>", gid, Settings.Ext);
            sb.AppendFormat("<div class='itemcol2'><a class='bold' href='/group/{0}{1}'>{2}</a><span class='em'>（建于 {3}）</span>", gid, Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()), Tools.DateString(row["g_uptime"].ToString()));
            rows2 = dt_member.Select("gid=" + gid);
            int mcnt = (int)rows2[0]["cnt"];
            sb.AppendFormat("<br />成员：{0}", mcnt);
            rows2 = dt_topic.Select("gid=" + gid);
            if (rows2.Length > 0)
            {
                sb.AppendFormat(" | 帖子：{0} | 回复：{1} | 人气：{2}", rows2[0]["cnt1"], rows2[0]["cnt2"], row["g_redu"]);
            }
            else
            {
                sb.AppendFormat(" | 帖子：0 | 回复：0 | 人气：{0}", row["g_redu"]);
            }
            sb.AppendFormat("<br /><span class='em'>{0}</span></div>", Tools.HtmlEncode(Tools.CutString(row["g_description"].ToString(), 80)));
            sb.Append("<div class='itemcol3'>");
            if (self)
            {
                sb.AppendFormat("<a href='/groups/{0}.aspx'>修改信息</a>", gid);
                sb.AppendFormat("<a href='/groups/{0}/member.aspx'>成员管理</a>", gid);
                sb.AppendFormat("<a href='javascript:void(0);' onclick='javascript:deleteGroup(this,{0});return false;'>解散该群</a>", gid);
            }
            sb.Append("</div>");
            sb.Append("<div class='clear'></div></div>");

            i++;
        }
        if (i == 0)
        {
            sb.Append("<p style='padding-left:100px;'>");
            if (self) sb.Append("<a class='addg bold' href='/group/create.aspx'>创建一个群组</a></p>");
            else sb.Append((sex == "男" ? "他" : "她") + "还没有创建任何群组</p>");
        }
        lblCreate.Text = sb.ToString();
        #endregion

        sb = new StringBuilder();

        #region joined group
        rows = dt_group.Select("g_user_name<>'" + who + "'", "g_id asc");
        i = 0;
        foreach (DataRow row in rows)
        {
            string gid = row["g_id"].ToString();
            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : " gitem1");
            sb.AppendFormat("<div class='itemcol1'><a href='/group/{0}{1}'><img src='/upload/group/{0}.jpg' {2} /></a></div>", gid, Settings.Ext, Strings.GroupBigImageError);
            sb.AppendFormat("<div class='itemcol2'><a class='bold' href='/group/{0}{1}'>{2}</a><span class='em'>（建于 {3}）</span>", gid, Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()), Tools.DateString(row["g_uptime"].ToString()));
            rows2 = dt_member.Select("gid=" + gid);
            int mcnt = (int)rows2[0]["cnt"];
            sb.AppendFormat("<br />成员：{0}", mcnt);
            rows2 = dt_topic.Select("gid=" + gid);
            if (rows2.Length > 0)
            {
                sb.AppendFormat(" | 帖子：{0} | 回复：{1} | 人气：{2}", rows2[0]["cnt1"], rows2[0]["cnt2"], (int)rows2[0]["cnt1"] + (int)rows2[0]["cnt2"] + (int)rows2[0]["cnt3"] + mcnt);
            }
            else
            {
                sb.AppendFormat(" | 帖子：0 | 回复：0 | 人气：{0}", mcnt);
            }
            sb.AppendFormat("<br /><span class='em'>{0}</span></div>", Tools.HtmlEncode(Tools.CutString(row["g_description"].ToString(), 80)));
            sb.Append("<div class='itemcol3'>");
            if (self)
            {
                if (row["gm_class"].ToString() == "1")
                {
                    sb.AppendFormat("<a href='/groups/{0}.aspx'>修改信息</a>", gid);
                    sb.AppendFormat("<a href='/groups/{0}/member.aspx'>成员管理</a>", gid);
                }
                sb.AppendFormat("<a href='{0}' onclick='javascript:outGroup(this,{1});return false;'>退出该群</a>", Strings.JSVoid, gid);
            }
            sb.Append("</div>");
            sb.Append("<div class='clear'></div></div>");

            i++;
        }
        if (i == 0)
        {
            sb.AppendFormat("<p style='padding-left:100px;'>{0}还没有加入任何群组，<a class='seeg bold' href='/group/?all'>浏览所有群组</a></p>", self ? "您" : (sex == "男" ? "他" : "她"));
        }
        lblJoin.Text = sb.ToString();
        #endregion

        sb = new StringBuilder();

        i = 0;
        foreach (DataRow row in dt_hotgroup.Rows)
        {
            sb.AppendFormat("<div class='hotgitem'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<p><a href='/group/{0}{1}'>{2}</a></p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            if (++i % 3 == 0) sb.Append("<div class='clear'></div>");
        }
        lblHotGroup.Text = sb.ToString();

        sb = new StringBuilder();

        #region topic
        sb = new StringBuilder();

        sb.Append("<div class='titem titemtop'>");
        sb.Append("<div class='col1'>标题</div><div class='col2'>阅读/回复</div><div class='col3'>发帖人</div><div class='col4'>最后更新</div>");
        sb.Append("<div class='clear'></div></div>");
        i = 0;
        foreach (DataRow row in dt_topic1.Rows)
        {
            sb.AppendFormat("<div class='titem{0}'>", i % 2 == 1 ? " titem2" : "");
            sb.AppendFormat("<div class='col1'><a class='bold' href='/group/topic/{0}{1}'>{2}</a></div>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<div class='col2'>{0} / {1}</div>", row["t_read_cnt"], row["t_reply_cnt"]);
            sb.AppendFormat("<div class='col3'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_user_name"], Settings.Ext, Tools.DateString(row["t_uptime"].ToString(), true));
            sb.AppendFormat("<div class='col4'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_last_reply"], Settings.Ext, Tools.DateString(row["t_last_reply_time"].ToString(), true));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblNewTopic.Text = sb.ToString();

        sb = new StringBuilder();

        sb.Append("<div class='titem titemtop'>");
        sb.Append("<div class='col1'>标题</div><div class='col2'>阅读/回复</div><div class='col3'>发帖人</div><div class='col4'>最后更新</div>");
        sb.Append("<div class='clear'></div></div>");
        i = 0;
        foreach (DataRow row in dt_topic2.Rows)
        {
            sb.AppendFormat("<div class='titem{0}'>", i % 2 == 1 ? " titem2" : "");
            sb.AppendFormat("<div class='col1'><a class='bold' href='/group/topic/{0}{1}'>{2}</a></div>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<div class='col2'>{0} / {1}</div>", row["t_read_cnt"], row["t_reply_cnt"]);
            sb.AppendFormat("<div class='col3'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_user_name"], Settings.Ext, Tools.DateString(row["t_uptime"].ToString(), true));
            sb.AppendFormat("<div class='col4'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_last_reply"], Settings.Ext, Tools.DateString(row["t_last_reply_time"].ToString(), true));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblAllNewTopic.Text = sb.ToString();

        sb = new StringBuilder();

        sb.Append("<div class='titem titemtop'>");
        sb.Append("<div class='col1'>标题</div><div class='col2'>阅读/回复</div><div class='col3'>发帖人</div><div class='col4'>最后更新</div>");
        sb.Append("<div class='clear'></div></div>");
        i = 0;
        foreach (DataRow row in dt_topic3.Rows)
        {
            sb.AppendFormat("<div class='titem{0}'>", i % 2 == 1 ? " titem2" : "");
            sb.AppendFormat("<div class='col1'><a class='bold' href='/group/topic/{0}{1}'>{2}</a></div>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<div class='col2'>{0} / {1}</div>", row["t_read_cnt"], row["t_reply_cnt"]);
            sb.AppendFormat("<div class='col3'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_user_name"], Settings.Ext, Tools.DateString(row["t_uptime"].ToString(), true));
            sb.AppendFormat("<div class='col4'><a href='/{0}{1}'>{0}</a><br />{2}</div>", row["t_last_reply"], Settings.Ext, Tools.DateString(row["t_last_reply_time"].ToString(), true));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblHotTopic.Text = sb.ToString();
        #endregion

        ds.Dispose();

        sb = new StringBuilder();
        
        DataTable dt_sta = null;
        if (Cache["dt_sta"] != null)
        {
            dt_sta = (DataTable)Cache["dt_sta"];
        }
        else
        {
            dt_sta = DB.GetTable("exec getStatistics;");
            Cache.Insert("dt_sta", dt_sta, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        }
        sb.AppendFormat("<p>群组数 - {0}</p>",dt_sta.Rows[0][0]);
        sb.AppendFormat("<p>主题数 - {0}</p>", dt_sta.Rows[1][0]);
        sb.AppendFormat("<p>回复数 - {0}</p>", dt_sta.Rows[2][0]);
        sb.AppendFormat("<p>群组用户数 - {0}</p>", dt_sta.Rows[3][0]);
        sb.AppendFormat("<p>昨日主题数 - {0}</p>", dt_sta.Rows[4][0]);
        sb.AppendFormat("<p>昨日回复数 - {0}</p>", dt_sta.Rows[5][0]);

        lblStatistics.Text = sb.ToString();
    }

}
