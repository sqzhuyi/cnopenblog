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

public partial class Favorite : UserPage
{
    protected string username;

    protected int catID;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            username = CKUser.Username;

            BindCategory();
            BindFavorite();

            Page.Title = "我的网摘 - cnOpenBlog";
        }
    }

    private void BindCategory()
    {
        catID = 0;
        if (Request.QueryString["cat"] != null)
        {
            if (!int.TryParse(Request.QueryString["cat"], out catID))
                catID = 0;
        }
        if (Request.QueryString["p"] != null)
        {
            if (int.TryParse(Request.QueryString["p"], out pageIndex))
                pageIndex -= 1;
            else pageIndex = 0;
        }
        string sql = "select [_id],[cat_name] from [favorite_category] where [user_name]='" + username + "' order by [_id]";
        DataTable dt = DB.GetTable(sql);
        bool find = false;
        foreach (DataRow row in dt.Rows)
        {
            ListItem item = new ListItem(row["cat_name"].ToString(), row["_id"].ToString());
            if (catID == (int)row["_id"])
            {
                item.Selected = true;
                find = true;
            }
            selCat.Items.Add(item);
            if (row["cat_name"].ToString() != "默认分类")
                selCat2.Items.Add(item);
        }
        if (!find) catID = 0;
    }

    private void BindFavorite()
    {
        string where = "[favorite].[user_name]='" + username + "'";
        string sqlc = "select count(*) from [favorite] where " + where;
        int cnt = (int)DB.GetValue(sqlc);

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        where += " and [blog_id]=[blog].[_id]";
        string fieldlist = "[favorite].[_id] as [f_id],[blog_id],[favorite].[uptime],[title],[filepath]";

        DataTable dt = Data.GetPagingData("[favorite],[blog]", fieldlist, "[f_id]", "[favorite].[_id]", pageIndex, pageSize, where, true);

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<p class='item'><a href='{0}'>{1}</a><span>{2}</span>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()), Tools.DateString(row["uptime"].ToString()));
            sb.AppendFormat("<a class='delete' href='#' onclick='javascript:delItem(this, {0});return false;' title='删除此条网摘'>删除</a></p>", row["f_id"]);
        }
        lblBlogList.Text = sb.ToString();
        if (pageCount > 1)
        {
            string url = "favorite.aspx?cat=" + catID.ToString() + "&p={0}";
            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
}
