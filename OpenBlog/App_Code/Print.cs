using System;
using System.Data;
using System.Text;
using DLL;

/// <summary>
/// Print 的摘要说明
/// </summary>
public class Print
{
    public static string LoginBox()
    {
        string str = "";
        if (CKUser.IsLogin)
        {
            str = @"<div class='loginhead'>欢迎回来 {0}</div>
<div class='loginbody'>
<div style='float:left;width:90px;'><a href='/{0}{1}'><img src='/upload/photo/{0}.jpg' class='loginphoto' {3} /></a>
</div><div style='float:right;width:140px;line-height:150%;'>
<a href='/{0}{1}'>我的博客</a><br /><a href='/group/{0}{1}'>我的群组</a><br /><span>博客访问量：{2}</span>
</div><div class='clear'></div>
<p class='loginline'><a class='edit' href='/baseinfo.aspx'>修改个人资料</a>
<br /><a class='publish' href='/write.aspx'>发表文章</a></p></div>";
            DBUser u = new DBUser(CKUser.Username);
            str = String.Format(str, u.Username, Settings.Ext, u.ViewCount, Strings.UserBigImageError);
        }
        else
        {
            str = @"<div class='loginhead'>用 户 登 陆</div>
<div class='loginbody'>
<p>用户名 &nbsp; <input id='txtName' type='text' class='put' style='width:160px;' /><span></span></p>
<p>密<span style='padding-left:12px;'></span>码 &nbsp; <input id='txtPassword' type='password' class='put' style='width:160px;' onkeydown='if(isEnter(event))login();' /><span></span></p>
<p style='padding-left:46px;'><label id='chkRe' class='chkRe' onclick='javascript:checkRemember(this);'>下次自动登录</label></p>
<p style='padding:10px 0px 10px 46px;'><a class='btns' href='javascript:void(0);' onclick='javascript:login();return false;'>登 录</a> &nbsp; <a href='/resetpwd.aspx'>忘记密码?</a></p>
</div>";
        }
        return str;
    }

    public static string HotGroups(int size, int cols)
    {
        DataTable dt = Data.GetHotGroup(size);
        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            sb.AppendFormat("<div class='hotgitem'><a href='/group/{0}{1}'><img src='/upload/group/{0}-s.jpg' {2} /></a>", row["g_id"], Settings.Ext, Strings.GroupSmallImageError);
            sb.AppendFormat("<p><a href='/group/{0}{1}'>{2}</a></p></div>", row["g_id"], Settings.Ext, Tools.HtmlEncode(row["g_name"].ToString()));
            if (++i % cols == 0) sb.Append("<div class='clear'></div>");
        }
        return sb.ToString();
    }
}
