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

public partial class Group_Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.Url.Query.ToLower().Contains("all") && CKUser.IsLogin)
        {
            Response.Redirect(String.Format("~/group/{0}{1}", CKUser.Username, Settings.Ext));
        }
        BindData();
    }

    private void BindData()
    {
        string sql = "exec getGroupDefault";
        DataSet ds = DB.GetDataSet(sql);

        DataTable dt_category = ds.Tables[0];
        DataTable dt_group = ds.Tables[1];
        DataTable dt_groupCount = ds.Tables[2];
        DataTable dt_newest = ds.Tables[3];
        DataTable dt_top = ds.Tables[4];
        DataTable dt_topic = ds.Tables[5];
        DataTable dt_join = ds.Tables[6];

        StringBuilder sb = new StringBuilder();
        DataRow[] rows = null;
        int i = 0;
        bool IE = Request.UserAgent.Contains("MSIE");

        foreach (DataRow row in dt_category.Rows)
        {
            sb.Append("<div class='groupbox'>");
            rows = dt_groupCount.Select("g_cat_id=" + row["_id"]);
            sb.AppendFormat("<p><a class='gtitle' href='/group/{0}{1}'>{2}</a><a class='gmember' href='/group/{0}{1}'>{3}</a></p>", row["_id"], Settings.Ext, row["_name"], rows[0]["cnt"]);
            rows = dt_group.Select("g_cat_id=" + row["_id"]);
            foreach (DataRow ro in rows)
            {
                sb.AppendFormat("<p><img class='gphoto' src='/upload/group/{0}-s.jpg' {3} /><a class='bold' href='/group/{0}{1}'>{2}</a>", ro["g_id"], Settings.Ext, Tools.HtmlEncode(Tools.CutString(ro["g_name"].ToString(),10)), Strings.GroupSmallImageError);
                sb.AppendFormat("<br /><span class='jieshao' title='{0}'>{1}</span></p>", Tools.CutString(ro["g_description"].ToString(), 30).Replace("'", "&apos;"), Tools.HtmlEncode(Tools.CutString(ro["g_description"].ToString(), IE ? 11 : 13)));
            }
            sb.Append("</div>");
            if (++i % 3 == 0) sb.Append("<div class='clear'></div><div style='padding-top:10px;'></div>");
        }
        lblGroup.Text = sb.ToString();

        sb = new StringBuilder();
        
        foreach (DataRow row in dt_newest.Rows)
        {
            sb.AppendFormat("<b>&#183;</b><a href='/group/{0}{1}'>{2}</a> ", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
        }
        lblNewestgroups.Text = sb.ToString();

        sb = new StringBuilder();
        i = 1;
        foreach (DataRow row in dt_top.Rows)
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
        foreach (DataRow row in dt_topic.Rows)
        {
            sb.AppendFormat("<div class='ht_item{0}'>", (i % 2 == 1 ? " ht_item2" : ""));
            sb.AppendFormat("<p class='ht_title'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"],Settings.Ext,Tools.HtmlEncode(row["t_title"].ToString()));
            sb.AppendFormat("<p class='ht_author'><a href='/{0}{1}'>{0}</a></p>", row["t_user_name"],Settings.Ext);
            sb.AppendFormat("<p class='ht_time'>{0}</p>", Tools.DateString(row["t_uptime"].ToString()));
            sb.Append("<div class='clear'></div></div>");
            i++;
        }
        lblHuati.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_join.Rows)
        {
            sb.AppendFormat("<div class='jiaru'><a href='/{0}{1}'>{0}</a><br /><a href='/group/{2}{1}'>{3}</a></div>", row["gm_user_name"], Settings.Ext, row["g_id"], Tools.HtmlEncode(row["g_name"].ToString()));
        }
        lblJoin.Text = sb.ToString();

        ds.Dispose();
    }

}
