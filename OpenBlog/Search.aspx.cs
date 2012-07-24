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
using System.Text.RegularExpressions;
using System.IO;
using DLL;
using SearchEngine.Store;

public partial class Search : BasePage
{
    protected string q = "";
    protected string k = "blog";
    protected string key = "";

    private int pageIndex = 0, pageSize = 10, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["q"] != null)
        {
            q = Request.QueryString["q"].Trim();
        }
        if (q.Split(':')[0] == "city")
        {
            Response.Redirect("~/city/" + q.Substring(5) + Settings.Ext);
        }
        if (q.Split(':')[0] == "industry")
        {
            Response.Redirect("~/industry/" + q.Substring(9) + Settings.Ext);
        }
        PageInit();
        PrintSearchTab();
        lblHotTags.Text = Data.GetHotTags();
    }

    private void PageInit()
    {
        if (Request.QueryString["page"] != null)
        {
            if (int.TryParse(Request.QueryString["page"], out pageIndex))
                pageIndex -= 1;
            else pageIndex = 0;
        }
    }

    private void PrintSearchTab()
    {
        string str = "";

        if (q.Contains(":"))
        {
            k = q.Split(':')[0].ToLower();
        }
        switch (k)
        {
            case "group":
                key = q.Substring(6);
                str = "<p id='stab' class='stab'><a href='/search.aspx'>博客</a> &nbsp; <b>群组</b> &nbsp; <a href='/search.aspx?q=topic:'>帖子</a> &nbsp; <a href='/search.aspx?q=comment:'>评论</a></p>";
                str += "<span class='sbox'><span style='font-size:14px;color:#777777;'>group:</span><input id='txtKey' type='text' maxlength='50' style='width:329px' value='" + key.Replace("'","\\'") + "' /></span>";
                break;
            case "topic":
                key = q.Substring(6);
                str = "<p id='stab' class='stab'><a href='/search.aspx'>博客</a> &nbsp; <a href='/search.aspx?q=group:'>群组</a> &nbsp; <b>帖子</b> &nbsp; <a href='/search.aspx?q=comment:'>评论</a></p>";
                str += "<span class='sbox'><span style='font-size:14px;color:#777777;'>topic:</span><input id='txtKey' type='text' maxlength='50' style='width:336px' value='" + key.Replace("'", "\\'") + "' /></span>";
                break;
            case "comment":
                key = q.Substring(8);
                str = "<p id='stab' class='stab'><a href='/search.aspx'>博客</a> &nbsp; <a href='/search.aspx?q=group:'>群组</a> &nbsp; <a href='/search.aspx?q=topic:'>帖子</a> &nbsp; <b>评论</b></p>";
                str += "<span class='sbox'><span style='font-size:14px;color:#777777;'>comment:</span><input id='txtKey' type='text' maxlength='50' style='width:309px' value='" + key.Replace("'", "\\'") + "' /></span>";
                break;
            default:
                key = q;
                str = "<p id='stab' class='stab'><b>博客</b> &nbsp; <a href='/search.aspx?q=group:'>群组</a> &nbsp; <a href='/search.aspx?q=topic:'>帖子</a> &nbsp; <a href='/search.aspx?q=comment:'>评论</a></p>";
                str += "<span class='sbox'><span></span><input id='txtKey' type='text' maxlength='50' style='width:370px' value='" + key.Replace("'", "\\'") + "' /></span>";
                break;
        }
        lblStab.Text = str;
    }

    protected void PrintResult()
    {
        if (key == "") return;

        if (Request.QueryString["web"] != null)
        {
            UseGoogle();
            return;
        }
        switch (k)
        {
            case "group":
                SearchGroup();
                break;
            case "topic":
                SearchTopic();
                break;
            case "comment":
                SearchComment();
                break;
            default:
                SearchBlog();
                break;
        }
    }
    #region search code
    private void SearchBlog()
    {
        Documents h = SearchEngine.Simple.Search(key, Request["onlytitle"] != null);

        int cnt = h != null ? h.Count : 0;

        if (cnt == 0)
        {
            Response.Write("<p class='resulttitle'>没有找到 <b>" + key + "</b> 的相关内容</p>");
            return;
        }
        Response.Write("<p class='resulttitle'>找到有关 <b>" + key + "</b> 的 <b>" + cnt + "</b> 个结果</p>");

        for (int i = 0; i < cnt && i < 10; i++)
        {
            Document doc = h[i];
            Response.Write("<div class='item'>");
            Response.Write("<div class='itemleft'>");
            Response.Write(String.Format("<a href='/{0}{1}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a>", doc.Author, Settings.Ext, Strings.UserSmallImageError));
            Response.Write("</div><div class='itemright'>");
            Response.Write(String.Format("<a class='bold' href='{0}' target=_blank>{1}</a>", doc.Path, SetRed(doc.Title)));
            Response.Write("<p>" + SetRed(doc.Body) + "</p>");
            Response.Write("<span class='em'>标签: " + TagTools.ToLinks(doc.Tag) + "</span>");
            Response.Write("</div>");
            Response.Write("<div class='clear'></div></div>");
        }
    }

    private void SearchGroup()
    {
        string fields = "[g_id],[g_name],[g_description],[g_tags]";
        string where = "[g_name] like '%" + key.Replace("'", "''") + "%'";
        string sql = "select top {0} {1} from [group] where {2}";
        sql = String.Format(sql, pageSize, fields, where);

        DataTable dt = DB.GetTable(sql);

        int cnt = dt != null ? dt.Rows.Count : 0;

        if (cnt == 0)
        {
            Response.Write("<p class='resulttitle'>没有找到 <b>" + key + "</b> 的相关内容</p>");
            return;
        }
        Response.Write("<p class='resulttitle'>找到有关 <b>" + key + "</b> 的 <b>" + cnt + "</b> 个结果</p>");

        foreach (DataRow row in dt.Rows)
        {
            Response.Write("<div class='item'>");
            Response.Write("<div class='itemleft'>");
            Response.Write(String.Format("<a href='/{0}{1}' target=_blank><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError));
            Response.Write("</div><div class='itemright'>");
            Response.Write(String.Format("<a class='bold' href='/{0}{1}' target=_blank>{2}</a>", row["g_id"], Settings.Ext, SetRed(row["g_name"].ToString())));
            Response.Write("<p>" + Tools.HtmlEncode(row["g_description"].ToString()) + "</p>");
            Response.Write("<span class='em'>标签: " + TagTools.ToLinks(row["g_tags"].ToString(), true) + "</span>");
            Response.Write("</div>");
            Response.Write("<div class='clear'></div></div>");
        }
    }

    private void SearchTopic()
    {
        string fields = "[t_id],[t_user_name],[t_title],[t_content],[t_last_edit_time]";
        string where = "([t_title] like '%" + key.Replace("'", "''") + "%'";
        if (Request["onlytitle"] == null) where += " or [t_content] like '%" + key.Replace("'", "''") + "%'";
        where += ")";

        string sql = "select top {0} {1} from [topic] where {2}";
        sql = String.Format(sql, pageSize, fields, where);

        DataTable dt = DB.GetTable(sql);

        int cnt = dt != null ? dt.Rows.Count : 0;

        if (cnt == 0)
        {
            Response.Write("<p class='resulttitle'>没有找到 <b>" + key + "</b> 的相关内容</p>");
            return;
        }
        Response.Write("<p class='resulttitle'>找到有关 <b>" + key + "</b> 的 <b>" + cnt + "</b> 个结果</p>");

        foreach (DataRow row in dt.Rows)
        {
            Response.Write("<div class='item'>");
            Response.Write("<div class='itemleft'>");
            Response.Write(String.Format("<a href='/{0}{1}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a>", row["t_user_name"], Settings.Ext, Strings.UserSmallImageError));
            Response.Write("</div><div class='itemright'>");
            Response.Write(String.Format("<a class='bold' href='/{0}{1}' target=_blank>{2}</a>", row["t_id"], Settings.Ext, SetRed(row["t_title"].ToString())));
            Response.Write("<p>" + CutStringByKey(row["t_content"].ToString()) + "</p>");
            Response.Write("<span class='em'>最后修改时间: " + Tools.DateString(row["t_last_edit_time"].ToString()) + "</span>");
            Response.Write("</div>");
            Response.Write("<div class='clear'></div></div>");
        }
    }

    private void SearchComment()
    {
        string fields = "[blog_comment].[_id],[blog_id],[blog].[title],[filepath],[blog_comment].[user_name],[fullname],[url],[content],[blog_comment].[uptime]";
        string where = "[blog].[_id]=[blog_id] and [content] like '%" + key.Replace("'", "''") + "%'";
        string sql = "select top {0} {1} from [blog_comment],[blog] where {2}";
        sql = String.Format(sql, pageSize, fields, where);

        DataTable dt = DB.GetTable(sql);

        int cnt = dt != null ? dt.Rows.Count : 0;

        if (cnt == 0)
        {
            Response.Write("<p class='resulttitle'>没有找到 <b>" + key + "</b> 的相关内容</p>");
            return;
        }
        Response.Write("<p class='resulttitle'>找到有关 <b>" + key + "</b> 的 <b>" + cnt + "</b> 个结果</p>");

        foreach (DataRow row in dt.Rows)
        {
            Response.Write("<div class='item'>");
            Response.Write("<div class='itemleft'>");
            if (row["user_name"].ToString() != "")
                Response.Write(String.Format("<a href='/{0}{1}' target=_blank><img src='/upload/photo/{0}-s.jpg' {2} /></a>", row["user_name"], Settings.Ext, Strings.UserSmallImageError));
            else
                Response.Write(String.Format("<a href='{0}' target=_blank><img src='/upload/photo/nophoto-s.jpg' /></a>", row["url"]));
            Response.Write("</div><div class='itemright'>");
            Response.Write(String.Format("<a class='bold' href='{0}' target=_blank>{1}</a>", row["filepath"], Tools.HtmlEncode(row["title"].ToString())));
            Response.Write("<p>" + CutStringByKey(row["content"].ToString()) + "</p>");
            Response.Write("<span class='em'>评论时间: " + Tools.DateString(row["uptime"].ToString()) + "</span>");
            Response.Write("</div>");
            Response.Write("<div class='clear'></div></div>");
        }
    }

    private void UseGoogle()
    {
        string _key = "JRuYnw9QFHKM5XYgSMigEbOleKWNr3Qk";
        com.google.api.GoogleSearchService server = new com.google.api.GoogleSearchService();
        com.google.api.GoogleSearchResult result = null;
        try
        {
            result = server.doGoogleSearch(_key, key, pageIndex * pageSize, pageSize, false, "", true, "", "", "");
        }
        catch
        {
            Response.Redirect("http://www.google.cn/search?q=" + Tools.UrlEncode(key));
        }
        int cnt = result.estimatedTotalResultsCount;

        if (cnt == 0)
        {
            Response.Write("<p class='resulttitle'>没有找到 <b>" + key + "</b> 的相关内容</p>");
            return;
        }
        Response.Write("<p class='resulttitle'>找到有关 <b>" + key + "</b> 的 <b>" + cnt + "</b> 个结果</p>");

        foreach (com.google.api.ResultElement element in result.resultElements)
        {
            Response.Write("<div class='item ggitem'>");
            Response.Write(String.Format("<a class='bold' href='{0}' target=_blank>{1}</a>", element.URL, element.title));
            Response.Write("<p>" + element.snippet.Replace("<b>...</b>", "...") + "</p>");
            Response.Write("<span style='color:green'>" + element.URL + "</span>");
            Response.Write("</div>");
        }
        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        if (pageCount > 1)
        {
            string url = "/search.aspx?q=" + q + "&web=1&page={0}";

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }
    }

    #endregion

    #region help function
    private string SetRed(string raw)
    {
        string str = Tools.HtmlEncode(raw);
        string reg = Regex.Replace(key, @"[\W]+", " ");
        reg = Regex.Replace(reg, @"[\s]+", "|").Trim('|');
        reg = "(" + reg + ")";

        str = Regex.Replace(str, reg, "<span style='color:#ff0000'>$1</span>", RegexOptions.IgnoreCase);

        return str;
    }

    private string CutStringByKey(string raw)
    {
        string str = Regex.Replace(raw, @"<.+?>", " ");
        str = Regex.Replace(str, @"\s+", " ");
        str = Tools.HtmlDecode(str);

        string key2 = Regex.Replace(key, @"[\W]+", " ");
        key2 = key2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
        int start = str.IndexOf(key2);
        int len = 126;

        if (start == -1)
        {
            str = Tools.CutString(str, len);
            return SetRed(str);
        }
        if (str.Length < len + 10) return SetRed(str);

        if (str.Length > start + len) str = str.Substring(0, start + len);

        System.Globalization.UnicodeCategory uCat;
        for (; start > 0; start--)
        {
            uCat = Char.GetUnicodeCategory(str[start]);
            if (uCat == System.Globalization.UnicodeCategory.OtherPunctuation)
            {
                start += 1;
                break;
            }
        }
        if (str.Length > start + len) str = str.Substring(start, len) + "...";
        else if (str.Length > len) str = str.Substring(str.Length - len);

        return SetRed(str);
    }
    #endregion

    #region 关键字正则
    protected Regex PrepareRegex(string query)
    {
        Regex r = null;

        query = Regex.Replace(query, @"[\s]{2,}", " ");

        StringBuilder sb = new StringBuilder();

        sb.Append(EscapeRegexChars(query).Replace("\\*", ".*").Replace("\\?", "."));

        if (query.StartsWith("\"") && query.EndsWith("\""))
        {
            sb.Remove(sb.Length - 1, 1);
            sb.Remove(0, 1);
            sb.Insert(0, @"\b(");
            sb.Append(@")\b");
            sb.Replace(" ", @")\b[\s\r\n]+\b(");

            r = new Regex(sb.ToString(), RegexOptions.IgnoreCase);
        }
        else
        {
            sb.Insert(0, @"(\b(");
            sb.Append(@")\b)");
            sb.Replace(" ", @")\b)|(\b(");

            r = new Regex(sb.ToString(), RegexOptions.IgnoreCase);
        }
        return r;
    }

    protected string EscapeRegexChars(string input)
    {
        StringBuilder sb = new StringBuilder(input);

        sb.Replace(@"\", @"\\");
        sb.Replace("~", @"\~");
        sb.Replace("!", @"\!");
        sb.Replace("\"", @"&quot;");
        sb.Replace("(", @"\(");
        sb.Replace(")", @"\)");
        sb.Replace("[", @"\[");
        sb.Replace("]", @"\]");
        sb.Replace("{", @"\{");
        sb.Replace("}", @"\}");
        sb.Replace("/", @"\/");
        sb.Replace("^", @"\^");
        sb.Replace("$", @"\$");
        sb.Replace("?", @"\?");
        sb.Replace("+", @"\+");
        sb.Replace("*", @"\*");
        sb.Replace("#", @"\#");
        sb.Replace(".", @"\.");
        sb.Replace(":", @"\:");
        sb.Replace("<", @"\<");
        sb.Replace(">", @"\>");
        sb.Replace("=", @"\=");

        return sb.ToString();
    }
    #endregion
}
