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

public partial class Group_List : BasePage
{
    protected int catID;

    protected string keywords;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        //link: /group/12/2.ashx ==> /group/list.aspx?q=12/2 : catid/page

        PageInit();

        BindData();
    }

    private void PageInit()
    {
        catID = 0;
        string q = "";
        if (Request.QueryString["q"] != null)
        {
            q = Request.QueryString["q"];
            int.TryParse(q.Split('/')[0], out catID);

            if (q.Split('/').Length > 1)
            {
                if (int.TryParse(q.Split('/')[1], out pageIndex))
                    pageIndex -= 1;
            }
        }
        if (catID < 11) Response.Redirect("~/group/?all");
    }

    private void BindData()
    {
        string sqlc = "select count(*) from [group] where [g_cat_id]=" + catID.ToString();
        int cnt = (int)DB.GetValue(sqlc);

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        string sql = "exec getGroupList {0},{1},{2}";
        sql = String.Format(sql, catID, pageIndex, pageSize);

        DataSet ds = DB.GetDataSet(sql);

        DataTable dt_group = ds.Tables[0];
        DataTable dt_memberCount = ds.Tables[1];
        DataTable dt_topicCount = ds.Tables[2];
        DataTable dt_newGroup = ds.Tables[3];
        DataTable dt_topGroup = ds.Tables[4];
        DataTable dt_newTopic = ds.Tables[5];
        DataTable dt_newMember = ds.Tables[6];

        StringBuilder sb = new StringBuilder();
        DataRow[] rows = null;
        int i = 0;
        foreach (DataRow row in dt_group.Rows)
        {
            sb.AppendFormat("<div class='gitem{0}'>", i % 2 == 1 ? " gitem2" : "");
            sb.AppendFormat("<div class='itemcol1'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a></div>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<div class='itemcol2'><a class='bold' href='/group/{0}{1}'>{2}</a>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            rows = dt_memberCount.Select("g_id=" + row["g_id"]);
            sb.AppendFormat("<br />成员:{0}", rows[0]["cnt"]);
            rows = dt_topicCount.Select("g_id=" + row["g_id"]);
            if (rows.Length > 0)
            {
                sb.AppendFormat(" &nbsp; 帖子:{0} &nbsp; 回复:{1} &nbsp; 人气:{2}", rows[0]["cnt1"], rows[0]["cnt2"], row["g_redu"]);
            }
            else
            {
                sb.AppendFormat(" &nbsp; 帖子:0 &nbsp; 回复:0 &nbsp; 人气:{0}", row["g_redu"]);
            }
            sb.AppendFormat("<br /><span style='color:#888888;'>{0}</span></div>", Tools.CutString(Tools.HtmlEncode(row["g_description"].ToString()), 70));
            sb.AppendFormat("<div class='itemcol3'><a href='/group/joingroup.aspx?q={0}' target=_blank>加入该群</a></div>", row["g_id"]);
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblGroups.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = String.Format("/group/{0}/{1}{2}", catID, "{0}", Settings.Ext);

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }

        sb = new StringBuilder();
        i = 0;
        foreach (DataRow row in dt_newGroup.Rows)
        {
            sb.AppendFormat("<b>&#183;</b><a href='/group/{0}{1}'>{2}</a>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
        }
        lblNewestgroups.Text = sb.ToString();

        sb = new StringBuilder();
        i = 1;
        foreach (DataRow row in dt_topGroup.Rows)
        {
            sb.AppendFormat("<div lang='{1}' class='top20item{0}'>", i % 2 == 0 ? " top20item2" : "", row["g_id"]);
            sb.AppendFormat("<p class='number'>{0}</p>", i.ToString().PadLeft(2, '0'));
            sb.AppendFormat("<p class='name'><a href='/group/{0}{1}'>{2}</a></p>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            sb.AppendFormat("<p class='redu'>{0}</p>", row["g_redu"]);
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        sb.Append("<div class='clear'></div>");//必须 js用

        lblTop20.Text = sb.ToString();

        sb = new StringBuilder();
        i = 0;
        foreach (DataRow row in dt_newTopic.Rows)
        {
            sb.AppendFormat("<div class='ht_item{0}'>", (i % 2 == 1 ? " ht_item2" : ""));
            sb.AppendFormat("<p class='ht_title'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<p class='ht_author'><a href='/{0}{1}'>{0}</a></p>", row["t_user_name"], Settings.Ext);
            sb.AppendFormat("<p class='ht_time'>{0}</p>", Tools.DateString(row["t_uptime"].ToString()));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblHuati.Text = sb.ToString();

        sb = new StringBuilder();
        i = 0;
        foreach (DataRow row in dt_newMember.Rows)
        {
            sb.AppendFormat("<div class='jiaru'><a href='/{0}{1}'>{0}</a><br /><a href='/group/{2}{1}'>{3}</a></div>", row["gm_user_name"], Settings.Ext, row["g_id"], Tools.HtmlEncode(row["g_name"].ToString()));
        }
        lblJoin.Text = sb.ToString();

        ds.Dispose();
    }
}
