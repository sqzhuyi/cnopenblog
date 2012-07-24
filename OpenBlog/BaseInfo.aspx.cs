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

public partial class BaseInfo : UserPage
{
    protected string username;
    protected string script;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = CKUser.Username;

        BindData();

        Page.Title = "我的个人资料 - cnOpenBlog";
    }

    private void BindData()
    {
        DBUser u = new DBUser(username);

        StringBuilder sb = new StringBuilder();

        sb.Append("<table id='box-1'><tr><td style='vertical-align:top;'>");
        sb.AppendFormat("<img id='uph' class='photo' alt='{0}' src='/upload/photo/{0}.jpg' onerror='this.src=\"/upload/photo/nophoto.jpg\";' />", username);
        sb.Append("<p style='text-align:center;'><a class='upphoto' href='#photo' onclick=\"javascript:show('photo');\">更改头像</a></p>");
        sb.AppendFormat("<p>姓名：{0}<br />性别：{1}<br />年龄：{2}岁（<i>{3}</i>）</p>", u.Fullname, u.Sex, DateTime.Now.Year - u.Birthday.Year, u.ShowBirthday ? "公开" : "未公开");
        sb.Append("</td><td style='vertical-align:top; padding-left:20px;'>");
        sb.Append("<p><a class='edit' href='#edit' onclick=\"javascript:show('edit');\">编辑我的资料</a></p>");
        sb.AppendFormat("<p>行业：{0}</p>", u.Hangye);
        sb.AppendFormat("<p>QQ：{0}（<i>{1}</i>）</p>", u.QQ,u.ShowQQ?"公开":"未公开");
        sb.AppendFormat("<p>MSN：{0}（<i>{1}</i>）</p>", u.MSN, u.ShowMSN ? "公开" : "未公开");
        sb.AppendFormat("<p>E-mail：<a href='mailto:{0}'>{0}</a>（<i>{1}</i>）</p>", u.Email, u.ShowEmail ? "公开" : "未公开");
        sb.AppendFormat("<p>个人网站：<a href='{0}'>{0}</a></p>", u.Url);
        sb.AppendFormat("<p>所在城市：{0}，{1}</p>", u.City, u.State);
        sb.AppendFormat("<p>博客标题：{0}</p>",Tools.HtmlEncode(u.BlogTitle));
        sb.AppendFormat("<p>博客副标题：{0}</p>", Tools.HtmlEncode(u.BlogSubtitle));
        sb.AppendFormat("<p>个人简介：{0}</p>", Tools.HtmlEncode(u.Jianjie));
        sb.AppendFormat("<p>兴趣爱好：{0}</p>", Tools.HtmlEncode(u.Xingqu));
        sb.AppendFormat("<p>个性签名：{0}</p>", Tools.HtmlEncode(u.Qianming));
        sb.Append("</td></tr></table>");

        lblBox1.Text = sb.ToString();

        sb = new StringBuilder();
        sb.Append("setValue = function(){");
        sb.AppendFormat("el('txtFullname').value='{0}';", u.Fullname);
        sb.AppendFormat("el('txtEmail').value='{0}';", u.Email);
        if (u.Sex == "男") sb.Append("el('radsex1').checked=true;");
        else sb.Append("el('radsex2').checked=true;");
        sb.AppendFormat("el('selYear').value='{0}';", u.Birthday.Year);
        sb.AppendFormat("el('selMonth').value='{0}';", u.Birthday.Month);
        sb.AppendFormat("el('selHangye').value='{0}';", u.Hangye);
        sb.AppendFormat("el('txtUrl').value='{0}';", u.Url);
        sb.AppendFormat("el('txtQQ').value='{0}';", u.QQ);
        sb.AppendFormat("el('txtMSN').value='{0}';", u.MSN);
        sb.AppendFormat("el('txtJianjie').value='{0}';", u.Jianjie.Replace("'","\\'"));
        sb.AppendFormat("el('txtXingqu').value='{0}';", u.Xingqu.Replace("'", "\\'"));
        sb.AppendFormat("el('txtQianming').value='{0}';", u.Qianming.Replace("'", "\\'"));
        sb.AppendFormat("el('txtBlogtitle').value='{0}';", u.BlogTitle.Replace("'", "\\'"));
        sb.AppendFormat("el('txtBlogsubtitle').value='{0}';", u.BlogSubtitle.Replace("'", "\\'"));
        if (u.ShowBirthday) sb.Append("el('chkBirthday').checked=true;");
        if (u.ShowEmail) sb.Append("el('chkEmail').checked=true;");
        if (u.ShowQQ) sb.Append("el('chkQQ').checked=true;");
        if (u.ShowMSN) sb.Append("el('chkMSN').checked=true;");
        sb.AppendFormat("_init_state('{0}');_init_city('{0}','{1}');", u.State, u.City);
        sb.Append("};");

        script = sb.ToString();
    }
}
