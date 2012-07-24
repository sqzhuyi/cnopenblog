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
using DLL;
public partial class Inbox : UserPage
{
    protected string username;

    private int pageIndex = 0, pageSize = 12, pageCount = 1;
    private int pageNumber = 9;//显示页数

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            username = CKUser.Username;

            int id = 0;
            string filter = "";
            if (Request.QueryString["q"] != null)
            {
                string q = Request.QueryString["q"];
                //q=filter/pageindex/id
                filter = q.Split('/')[0].ToLower();
                if (q.Contains("/"))
                {
                    if (int.TryParse(q.Split('/')[1], out pageIndex))
                        pageIndex -= 1;
                    else pageIndex = 0;
                }
                if (q.Split('/').Length > 2)
                {
                    int.TryParse(q.Split('/')[1], out id);
                }
            }
            if (id > 0)
            {
                BindOneMessage(id);
            }
            else
            {
                BindMessage(filter);
            }
            PrintFilter(filter);

            Page.Title = "管理留言 - cnOpenBlog";
        }
    }
    
    private void BindMessage(string filter)
    {
        string where = "[_to]='" + username + "'";
        switch (filter)
        {
            case "blogger":
                where += " and ([_from]<>'system' and [_from] not like '%.%')";
                break;
            case "system":
                where += " and [_from]='system'";
                break;
            case "anonym":
                where += " and [_from] like '%.%'";
                break;
            default:
                filter = "all";
                break;
        }
        DataTable dt = Data.GetPagingData("[inbox]", "*", "[_id]", "[_id]", pageIndex, pageSize, where, true);
        if (dt.Rows.Count == 0) return;

        string sql = "update [inbox] set [readed]=1 where {1} and [readed]=0;";
        sql += "select count([_id]) cnt,[_fid] from [inbox] where [_fid]<>0 and ([_from]='{0}' or [_to]='{0}') group by [_fid];";
        sql += "select count(*) from [inbox] where {1};";
        sql = String.Format(sql, username, where);
        DataSet ds = DB.GetDataSet(sql);

        DataTable dt2 = ds.Tables[0];

        int cnt = (int)ds.Tables[1].Rows[0][0];

        pageCount = cnt / pageSize;
        if (cnt % pageSize > 0) pageCount++;
        pageIndex = Math.Max(0, pageIndex);
        pageIndex = Math.Min(pageCount - 1, pageIndex);

        StringBuilder sb = new StringBuilder();

        string _from;
        DataRow[] rows;
        foreach (DataRow row in dt.Rows)
        {
            _from = row["_from"].ToString();
            if (_from == "system")
            {
                sb.Append("<div class='msgbox sys'>");
                sb.AppendFormat("<a class='delmsg' href='{1}' onclick='javascript:deletemsg(this,{0});return false;' title='删除此条消息'></a>", row["_id"], Strings.JSVoid);
                sb.Append(MessageFormat((int)row["_id"], row["_body"].ToString()));
                sb.AppendFormat("<span class='em'>{0}</span>", FormatDate(row["uptime"].ToString()));
                sb.Append("</div>");
                continue;
            }
            sb.Append("<div class='msgbox'><div class='msgleft'>");
            
            if (_from.Contains("."))
            {
                sb.AppendFormat("<img src='/upload/photo/nophoto-s.jpg' />");
            }
            else
            {
                sb.AppendFormat("<a href='/{0}{1}'><img src='/upload/photo/{0}-s.jpg' {2} /></a>", _from, Settings.Ext, Strings.UserSmallImageError);
            }
            sb.AppendFormat("</div><div class='msgright'><p class='msgtit{0}'>", (row["readed"].ToString() == "0" ? " bold" : ""));
            if (_from.Contains("."))
            {
                sb.Append(Tools.IPString(_from));
            }
            else
            {
                sb.AppendFormat("<a href='/{0}{1}'>{0}</a>", _from, Settings.Ext);
            }
            sb.AppendFormat(" 于 {0} ", FormatDate(row["uptime"].ToString()));
            sb.AppendFormat("<a href='/inbox/{0}/{1}/{2}.aspx'>", filter, (pageIndex + 1), (row["_fid"].ToString() == "0" ? row["_id"] : row["_fid"]));
            if (row["_fid"].ToString() == "0") rows = dt2.Select("_fid=" + row["_id"]);
            else rows = dt2.Select("_fid=" + row["_fid"]);
            if (rows.Length > 0)
            {
                sb.AppendFormat("（{0}）", (int)rows[0]["cnt"] + 1);
            }
            sb.Append("</a>");
            if (!_from.Contains("."))
            {
                sb.AppendFormat("<a class='replymsg' href='{1}' onclick='javascript:replymsg(this,{0});return false;'>回复</a>", row["_id"], Strings.JSVoid);
            }
            sb.AppendFormat("<a class='delmsg' href='{1}' onclick='javascript:deletemsg(this,{0});return false;' title='删除此条留言'></a></p>", row["_id"], Strings.JSVoid);
            sb.AppendFormat("<p>{0}</p>", MessageFormat(0, row["_body"].ToString()));
            sb.Append("</div><div class='clear'></div></div>");
        }
        if (pageCount > 1)
        {
            string url = String.Format("/inbox/{0}/{1}.aspx", filter, "{0}");

            lblPageList.Text = Tools.GetPager(pageIndex, pageCount, pageNumber, url);
        }

        lblMsgList.Text = sb.ToString();
    }

    private void BindOneMessage(int id)
    {
        string sql = "select [_id],[_fid],[_from],[_body],[uptime] from [inbox] where ([_id]={0} or [_fid]={0}) order by [_id] asc";
        sql = String.Format(sql, id);
        DataTable dt = DB.GetTable(sql);

        if (dt.Rows.Count == 0) Response.Redirect("~/inbox.aspx");

        StringBuilder sb = new StringBuilder();
        sb.Append("<h4>留言对话记录<a href='/inbox.aspx' onclick='javascript:history.go(-1);return false;' style='font-size:12px;margin-left:20px;'>- 返回收件箱</a></h4>");
        sb.Append("<div class='msgdetailsbox'>");
        int i = 0;
        string bg;
        foreach (DataRow row in dt.Rows)
        {
            bg = i++ % 2 == 1 ? "background-color:#f5f5f5;" : "";
            sb.AppendFormat("<p style='{3}'><a href='/{0}{1}'>{0}</a> 于 {2}</p>", row["_from"], Settings.Ext, FormatDate(row["uptime"].ToString()), bg);
            sb.AppendFormat("<p style='padding-bottom:14px;{1}'>{0}</p>", MessageFormat((int)row["_id"], row["_body"].ToString()), bg);
        }
        sb.Append("</div>");
        sb.Append("<div class='msgdetailsbox' style='padding:4px 6px;'>");
        sb.Append("<textarea rows='3' style='width:99%;border:solid 1px #ffffff;'></textarea>");
        sb.Append("</div>");
        sb.AppendFormat("<p style='clear:both;margin-top:10px;'><a class='btns' href='#' onclick='javascript:sendreply(this,{0});return false;'>回 复</a></p>", id);

        lblMsgList.Text = sb.ToString();
    }

    private void PrintFilter(string filter)
    {
        string[] links = { "<p><a href='/inbox.aspx'>查看全部留言</a></p>", "<p><a href='/inbox/blogger.aspx'>只看用户留言</a></p>", "<p><a href='/inbox/system.aspx'>只看系统通知</a></p>", "<p><a href='/inbox/anonym.aspx'>只看匿名用户留言</a></p>" };
        StringBuilder sb = new StringBuilder();
        switch (filter)
        {
            case "blogger":
                sb.Append(links[0]);
                sb.Append("<p><a class='curr' href='/inbox/blogger.aspx'>只看用户留言</a></p>");
                sb.Append(links[2]);
                sb.Append(links[3]);
                break;
            case "system":
                sb.Append(links[0]);
                sb.Append(links[1]);
                sb.Append("<p><a class='curr' href='/inbox/system.aspx'>只看系统通知</a></p>");
                sb.Append(links[3]);
                break;
            case "anonym":
                sb.Append(links[0]);
                sb.Append(links[1]);
                sb.Append(links[2]);
                sb.Append("<p><a class='curr' href='/inbox/anonym.aspx'>只看匿名用户留言</a></p>");
                break;
            default:
                sb.Append("<p><a class='curr' href='/inbox.aspx'>查看全部留言</a></p>");
                sb.Append(links[1]);
                sb.Append(links[2]);
                sb.Append(links[3]);
                break;
        }
        lblFilter.Text = sb.ToString();
    }

    private string FormatDate(string date)
    {
        DateTime dt = DateTime.Now;
        DateTime.TryParse(date, out dt);
        if (dt.Year == DateTime.Now.Year)
        {
            return dt.ToString("M-d hh:mm");
        }
        else
        {
            return dt.ToString("yyyy-M-d hh:mm");
        }
    }

    private string MessageFormat(int msgid, string message)
    {
        string str = Tools.HtmlEncode(message);
        str = Regex.Replace(str, @"(http://\S+)","<a href='$1' target=_blank>$1</a>", RegexOptions.IgnoreCase);
        str = Regex.Replace(str, @" (www\.\S+)", " <a href='http://$1' target=_blank>$1</a>", RegexOptions.IgnoreCase);
        str = Regex.Replace(str, Settings.UserFormat, String.Format("<a href='/$1{0}' target=_blank>@$1</a>", Settings.Ext));
        if (msgid > 0)
        {
            str = Regex.Replace(str, Settings.GroupFormat, String.Format("<a href='/group/$1{0}' target=_blank>$2</a>", Settings.Ext));
            str = Regex.Replace(str, Settings.ApplyFormat, String.Format("<span class='sp_apply'><a href='{0}' onclick='javascript:applyOk(this,{1},$1,\"$2\");return false;'>批准</a> | <a href='{0}' onclick='javascript:applyNo(this,{1},$1,\"$2\");return false;'>拒绝</a></span>", Strings.JSVoid, msgid));
        }
        return str;
    }
}
