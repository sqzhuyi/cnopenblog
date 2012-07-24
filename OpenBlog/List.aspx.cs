using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using DLL;

public partial class List : BasePage
{
    protected string title, keywords;

    protected int catID, gcatID;
    protected string sort;

    private int pageIndex = 0, pageSize = 10, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _Init();

            BindBlog();

            BindGroup();

            Page.Title = title;
        }
    }

    private void _Init()
    {
        string q = Request.QueryString["q"] != null ? Request.QueryString["q"] : "";
        if (q == "")
        {
            Response.Redirect("~/100" + Settings.Ext);
        }
        string cat = q.Split('/')[0];

        sort = q.Contains("/") ? q.Split('/')[1] : "date";

        if (q.Split('/').Length > 2)
        {
            if (int.TryParse(q.Split('/')[2], out pageIndex))
                pageIndex -= 1;
        }

        catID = 0;

        int.TryParse(cat, out catID);

        if (catID == 0)
        {
            Response.Redirect("~/100" + Settings.Ext);
        }

        BindCat();

        string sortbar = "";
        switch (sort)
        {
            case "read":
                sort = "read";
                sortbar = "<a class='ex' href='/{0}/date{1}'>按更新时间</a> | <span>按阅读次数</span> | <a class='ex' href='/{0}/comment{1}'>按评论次数</a>";
                break;
            case "comment":
                sort = "comment";
                sortbar = "<a class='ex' href='/{0}/date{1}'>按更新时间</a> | <a class='ex' href='/{0}/read{1}'>按阅读次数</a> | <span>按评论次数</span>";
                break;
            default:
                sort = "date";
                sortbar = "<span>按更新时间</span> | <a class='ex' href='/{0}/read{1}'>按阅读次数</a> | <a class='ex' href='/{0}/comment{1}'>按评论次数</a>";
                break;
        }
        sortbar += " &nbsp; <a class='rssa' href='/{0}/" + sort + "/feed{1}' target='_blank'>&nbsp;</a>";
        lblSortBar.Text = String.Format(sortbar, catID, Settings.Ext);
    }

    private void BindCat()
    {
        StringBuilder sb = new StringBuilder();
        Dictionary<int, string> cats = CategoryB.AllCategory;
        foreach (int key in cats.Keys)
        {
            if (key == catID) sb.AppendFormat("<a class='curr' href='/{0}{1}'>{2}</a>", key, Settings.Ext, cats[key]);
            else sb.AppendFormat("<a href='/{0}{1}'>{2}</a>", key, Settings.Ext, cats[key]);
        }
        lblCat.Text = sb.ToString();

        title = "最新文章列表 - cnOpenBlog";
        keywords = "最新文章,文章列表,cnOpenBlog";
        if (catID > 100)
        {
            title = cats[catID] + " " + title;
            keywords = cats[catID] + "," + keywords;
        }
    }

    private void BindBlog()
    {
        string where = "1=1";
        if (catID > 100) where += " and [cat_id]=" + catID.ToString();

        string sqlc = "select count(*) from [blog] where " + where;

        where += " and [blog].[user_name]=[users].[_name]";

        int cnt = (int)DB.GetValue(sqlc);
        if (cnt == 0) return;

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        string fieldlist = "[blog].[_id],[user_name],[fullname],[cat_id],[subcat_id],[title],[zhaiyao],[comment_cnt],[read_cnt],[filepath],[edit_time]";
        string orderfield = "[edit_time]";
        if (sort == "read") orderfield = "[read_cnt]";
        else if (sort == "comment") orderfield = "[comment_cnt]";

        DataTable dt = Data.GetPagingData("[blog],[users]", fieldlist, orderfield, "[blog].[_id]", pageIndex, pageSize, where, true);
        if (dt == null || dt.Rows.Count == 0) return;

        DataRow[] rows = null;

        if (sort == "read") rows = dt.Select("", "read_cnt desc,edit_time desc");
        if (sort == "comment") rows = dt.Select("", "comment_cnt desc,edit_time desc");
        else rows = dt.Select("");

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in rows)
        {
            sb.Append("<div>");
            sb.AppendFormat("<p class='title'><a href='{0}'>{1}</a></p>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat("<p class='zhaiyao'>摘要: {0} - <a href='{1}'>阅读全文</a></p>", Tools.HtmlEncode(row["zhaiyao"].ToString()), row["filepath"]);
            sb.Append("<p class='bot'>");
            if (catID == 10)
                sb.AppendFormat("类别：<a href='/{0}{1}'>{2}</a> &nbsp; ", row["cat_id"], Settings.Ext, CategoryB.GetNameById((int)row["cat_id"]));
            sb.AppendFormat("作者：<a href='/{0}{1}'>{2}</a> ", row["user_name"], Settings.Ext, row["fullname"]);
            sb.AppendFormat("【评论:{0}】【阅读:{1}】 {2}</p>", row["comment_cnt"], row["read_cnt"], Tools.DateString(row["edit_time"].ToString()));
            sb.Append("</div>");
        }
        lblBlogList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = String.Format("/{0}/{1}/{2}{3}", catID, sort, "{0}", Settings.Ext);

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }

    private void BindGroup()
    {
        DataTable dt = Data.GetHotGroup(10, CategoryB.ToCategory(catID).ID);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='gitem'><div style='float:left;width:50px;'><img src='/upload/group/{0}-s.jpg' {1} /></div>", row["g_id"], Strings.GroupSmallImageError);
            sb.AppendFormat("<div style='float:right;width:100px;'><a href='/group/{0}{1}' target=_blank>{2}</a>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            sb.AppendFormat("<br /><span class='em'>人气：{0}</span></div><div class='clear'></div></div>", row["g_redu"]);
        }
        lblGroup.Text = sb.ToString();
    }
}
