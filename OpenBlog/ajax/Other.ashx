<%@ WebHandler Language="C#" Class="Other" %>

using System;
using System.Web;
using DLL;

public class Other : IHttpHandler
{
    HttpContext http;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        http = context;

        if (Par("showmsg") != null)
        {
            ShowMessage();
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    private string Par(string key)
    {
        return http.Request[key];
    }
    private void Print(string str)
    {
        http.Response.Write(str);
    }

    private void ShowMessage()
    {
        if (!CKUser.IsLogin)
        {
            Print("0");
            return;
        }
        string sql = "select count(*) from [inbox] where [_to]='" + CKUser.Username + "' and [readed]=0";
        int cnt = (int)DB.GetValue(sql);
        if (cnt > 0)
        {
            string str = "var s=\"<a class='showmsg' href='/inbox.aspx' title='查看新消息' style='left:\"+(elementPos(el('login_a')).x+el('login_a').offsetWidth)+\"px;'><i>" + cnt.ToString() + "\"+(getVersion()<7?'<br>&nbsp;':'')+\"</i></a>\";";
            str += "el('smsp').innerHTML=s;";
            Print(str);
        }
        else Print("0");
    }

}