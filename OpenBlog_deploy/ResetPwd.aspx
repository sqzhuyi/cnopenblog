<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="ResetPwd, App_Web_i3mdp3xs" title="找回密码 - cnOpenBlog" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<style type="text/css">
.put { font-size:16px; width:240px; }
.left { float:left; width:540px; }
.right { float:right; width:340px; background-color:#EDF2F6; -moz-border-radius: 8px;-webkit-border-radius:8px;}
a.reg { background:url(/images/r_right.gif) no-repeat 0px -1px; padding-left:18px; font-size:13px;}
.title { font-size:28px; font-family:华文新魏; color:#267BB5; margin-top:0px; }
a.btnb { float:left; display:block; width:155px; background:url(/images/green_button.gif) no-repeat; padding:9px 0px; text-align:center; font-size:14px; font-weight:bold; color:#ffffff; text-decoration:none;}
a.btnb:hover { text-decoration:underline;}
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">
<div class="left">
<h1 class="title">找回密码</h1>
<div style="padding:10px 6px; line-height:22px;">
如果您忘记了自己的账号密码，在右边输入框中输入您注册时使用的Email地址，<br />系统将自动把您的密码发送到该邮箱。
<br /><br /><br />
如果您有任何问题，请发邮件给我们：<a href="mailto:service@cnopenblog.com">service@cnopenblog.com</a>
</div>
</div>
<div class="right">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div id="notediv" style="margin:10px;"></div>
<div style="margin:20px 40px;">
<p>输入您注册账号使用的Email地址：</p>
<p><input id="txtEmail" type="text" class="put" maxlength="50"  /></p>
<br />
<p><a class="btnb" href="javascript:void(0);" onclick="javascript:doSubmit();return false;">找 回 密 码</a></p>
<br /><br />
</div>
<p style="text-align:center;"><img alt="" src="/images/line.gif" style="width:330px; height:2px;" /></p>
<div style="margin:10px 40px;">
<p><b>还没有注册cnOpenBlog?</b></p>
<p><a class="reg" href="/register.aspx">马上注册</a></p>
</div><div class="clear" style="padding-top:20px;"></div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript">
window.onload = function(){
    el("txtEmail").focus();
    el("txtEmail").onkeydown = function(ev){
        if(isEnter(ev)) doSubmit();
    };
};
function doSubmit()
{
}
</script>
</asp:Content>

