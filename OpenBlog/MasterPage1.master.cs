using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DLL;

public partial class MasterPage1 : System.Web.UI.MasterPage
{
    protected string sentence;

    protected void Page_Load(object sender, EventArgs e)
    {
        string str = "";
        if (CKUser.Username != "")
        {
            str = "<div class='topright'><table cellpadding=0 cellspacing=0 class='log22'><tr><td class='log22left'></td>";
            str += "<td><a id='login_a' href='/{0}{1}' title='个人主页'>{0}</a></td><td class='dot'></td><td><a href='/baseinfo.aspx' title='个人管理'>管 理</a></td><td class='dot'></td><td><a href='/login.aspx?logout=1' title='注销账户'>注 销</a></td>";
            str += "<td class='log22right'></td></tr></table>";
            str = String.Format(str, CKUser.Username,"{0}");
        }
        else
        {
            str = "<div class='topright'><a class='reg' href='/register.aspx'>快速注册</a><a href='/login.aspx' title='登录到cnOpenBlog'><b>登 录</b></a>";
        }
        str += "<a href='/shortblog/' title='一句话博客'><b>迷你博客</b></a><a href='/group/' title='博友群组'><b>群 组</b></a><a href='/100{0}' title='查看最新的文章'><b>最新文章</b></a><a href='/' title='cnOpenBlog首页'><b>首 页</b></a></div>";
        str = String.Format(str, Settings.Ext);

        string p = Request.Path.ToLower();
        if (p.Contains("/group")) str = str.Replace("href='/group/'", "class='curr' href='/group/'");
        else if (p.Contains("/login.aspx")) str = str.Replace("href='/login.aspx'", "class='curr' href='/login.aspx'");
        else if (p.Contains("/register.aspx")) str = str.Replace("href='/register.aspx'", "class='curr' href='/register.aspx'");
        else if (p.Contains("/list.aspx")) str = str.Replace("href='/100" + Settings.Ext + "'", "class='curr' href='/100" + Settings.Ext + "'");
        else if (p.Contains("/shortblog")) str = str.Replace("href='/shortblog/'", "class='curr' href='/shortblog/'");
        else if (p.Contains("/default.aspx")) str = str.Replace("href='/'", "class='curr' href='/'");

        lblTabs.Text = str;

        sentence = DB.GetValue("select top 1 [_content] from [sentence] order by newid()").ToString();
    }
}
