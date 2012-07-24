using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using DLL;

public partial class _Default : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindCategory();
        BindData();
        
        lblLogin.Text = Print.LoginBox();
        lblHotTags.Text = Data.GetHotTags();
    }

    private void BindCategory()
    {
        Dictionary<int, string> cats = CategoryB.AllCategory;
        StringBuilder sb = new StringBuilder();
        foreach (int key in cats.Keys)
        {
            sb.AppendFormat("<a href='/{0}{1}'>{2}</a>", key, Settings.Ext, cats[key]);
        }
        lblCategory.Text = sb.ToString();
    }

    private void BindData()
    {
        int max = new Random().Next(15, 30);
        int maxb = new Random().Next(10, 20);

        string sql = "select top 10 [_id],[user_name],[title],[filepath] from [blog] order by [_id] desc;";
        sql += "select top 10 * from (select top " + maxb.ToString() + " [_id],[user_name],[title],[filepath] from [blog] order by [read_cnt] desc) b2 order by [_id];";
        sql += "exec [dbo].[getHotGroup] 12," + max.ToString() + ",0;";
        sql += "select top 15 [t_id],[t_title] from [topic] order by [t_reply_cnt] desc;";
        sql += "exec [dbo].[getDefaultPageBlog];";

        DataSet ds = DB.GetDataSet(sql);
        DataTable dt_new = ds.Tables[0];
        DataTable dt_hot = ds.Tables[1];
        DataTable dt_group = ds.Tables[2];
        DataTable dt_topic = ds.Tables[3];
        DataTable dt_1 = ds.Tables[4];
        DataTable dt_2 = ds.Tables[5];
        DataTable dt_3 = ds.Tables[6];
        DataTable dt_4 = ds.Tables[7];

        StringBuilder sb = new StringBuilder();
        
        foreach (DataRow row in dt_new.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblNewBlogs.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_hot.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblHotBlogs.Text = sb.ToString();

        sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt_group.Rows)
        {
            sb.AppendFormat("<div class='gitem'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<p><a href='/group/{0}{1}'>{2}</a></p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            if (++i % 6 == 0) sb.Append("<div class='clear'></div>");
        }
        lblGroups.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_topic.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='/group/topic/{0}{1}'>{2}</a></p>", row["t_id"], Settings.Ext, Tools.HtmlEncode(row["t_title"].ToString()));
        }
        lblHotTopic.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_1.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblBlogs1.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_2.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblBlogs2.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_3.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblBlogs3.Text = sb.ToString();

        sb = new StringBuilder();
        foreach (DataRow row in dt_4.Rows)
        {
            sb.AppendFormat("<p class='nitem'><a href='{0}'>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString()));
            sb.AppendFormat(" - <a class='hui' href='{0}{1}'>{0}</a></p>", row["user_name"], Settings.Ext);
        }
        lblBlogs4.Text = sb.ToString();

        ds.Dispose();
    }

}
