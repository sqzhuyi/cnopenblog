using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using DLL;

public partial class Write : UserPage
{
    protected string username;
    protected string script;
    protected string con;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            username = CKUser.Username;

            BindCategory();

            BindTags();
            
            if (Request.QueryString["blog"] != null)
            {
                BindOldData();
            }
            Page.Title = "发表文章 - cnOpenBlog";
        }
    }

    private void BindCategory()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<select id='selCat' class='put'>");
        Dictionary<int, string> cats = CategoryB.AllCategory;
        foreach (int key in cats.Keys)
        {
            sb.AppendFormat("<option value='{0}'>{1}</option>", key, cats[key]);
        }
        sb.Append("</select> ");
        sb.Append("<select id='selSubCat' class='put'>");
        string sql = "select [uc_id],[uc_name] from [user_category] where [uc_user_name]='" + username + "'";
        DataTable dt = DB.GetTable(sql);
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<option value='{0}'>{1}</option>", row["uc_id"], row["uc_name"]);
        }
        sb.Append("</select> ");

        lblCategory.Text = sb.ToString();
    }

    private void BindTags()
    {
        string sql = "select [tags] from [blog] where [user_name]='" + username + "'";
        DataTable dt = DB.GetTable(sql);
        if (dt == null || dt.Rows.Count == 0) return;
        List<string> list = new List<string>();
        foreach (DataRow row in dt.Rows)
        {
            list.AddRange(row[0].ToString().Split(','));
        }
        List<Tag> tags = TagTools.Format(list);

        StringBuilder sb = new StringBuilder();
        foreach (Tag t in tags)
        {
            sb.AppendFormat("<a href='/search.aspx?q=tag:{0}&blogger={1}' style='font-size:{3}px'>{2}</a> ", Tools.UrlEncode(t.Name), username, t.Name, Math.Min(t.Count + 11, 24));
        }
        lblTags.Text = sb.ToString();
    }

    private void BindOldData()
    {
        int blogid = 0;
        if (!int.TryParse(Request.QueryString["blog"], out blogid))
        {
            Response.Redirect("~/write.aspx");
        }
        string sql = "select * from [blog] where [_id]={0} and [user_name]='{1}'";
        sql = String.Format(sql, blogid, username);

        DataTable dt = DB.GetTable(sql);
        if (dt.Rows.Count == 0)
        {
            Response.Redirect("~/write.aspx");
        }
        DataRow row = dt.Rows[0];

        string html = IO.LoadFile(Settings.BasePath + row["filepath"].ToString().TrimStart('/'), Encoding.GetEncoding("GB2312"));
        Match mat = Regex.Match(html, @"<!--body start-->(.+)<!--body end-->", RegexOptions.Singleline);
        
        if (mat.Success) con = mat.Groups[1].Value;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("cat='{0}';subcat='{1}';", row["cat_id"], row["subcat_id"]);
        sb.Append("var _init_write2 = function(){");
        sb.AppendFormat("el('txtTitle').value='{0}';", row["title"].ToString().Replace("'", "\\'"));
        sb.AppendFormat("el('txtZhaiyao').value='{0}';", row["zhaiyao"].ToString().Replace("'","\\'"));
        sb.AppendFormat("el('txtTag').value='{0}';", row["tags"].ToString().Replace("'", "\\'"));
        if (row["no_comment"].ToString() == "1") sb.Append("el('chkNoComment').checked=true;");
        sb.Append("};addLoad(_init_write2);");
        script = sb.ToString();
    }
}
