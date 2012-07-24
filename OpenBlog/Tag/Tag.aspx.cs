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
using SearchEngine.Store;

public partial class Tag_Tag : BasePage
{
    protected string tag;

    private int pageIndex = 0, pageSize = 20, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        tag = "";
        if (Request.QueryString["q"] != null)
        {
            string[] qs = Request.QueryString["q"].Split('/');
            tag = TagTools.Filter(qs[0]).Replace(",", "");
            if (qs.Length > 1)
            {
                if (int.TryParse(qs[1], out pageIndex)) pageIndex -= 1;
                else pageIndex = 0;
            }
        }
        if (tag == "")
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        BindBlogs();

        lblLogin.Text = Print.LoginBox();

        lblHotTags.Text = Data.GetHotTags();

        Page.Title = "Tag:" + tag + " - cnOpenBlog";

    }

    private void BindBlogs()
    {
        Documents docs = SearchEngine.Simple.SearchTag(tag);

        int cnt = docs.Count;

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        StringBuilder sb = new StringBuilder();
        int ind;
        for (int i = 0; i < pageSize; i++ )
        {
            ind = pageIndex * pageSize + i;
            if (ind == cnt) break;

            Document doc = docs[ind];

            sb.Append("<div style='margin-top:16px;margin-left:14px;'>");
            sb.AppendFormat("<div class='ileft'><a href='/{0}{1}' title='{0}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a></div>", doc.Author, Settings.Ext, Strings.UserSmallImageError);
            sb.AppendFormat("<div class='iright'><a class='bold' href='{0}' target=_blank>{1}</a>", doc.Path, Tools.HtmlEncode(doc.Title));
            sb.AppendFormat("<p>{0}</p>", Tools.HtmlEncode(doc.Body));
            sb.AppendFormat("<span class='em'>标签：{0}</span></div>", TagTools.ToLinks(doc.Tag));
            sb.Append("<div class='clear'></div></div>");
        }
        lblBlogList.Text = sb.ToString();

        if (pageCount > 1)
        {
            string url = String.Format("/tag/{0}/{1}{2}", tag, "{0}", Settings.Ext);
            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }
}
