<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Register, App_Web_i3mdp3xs" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<script type="text/javascript" src="/js/register.js"></script>
<link type="text/css" rel="stylesheet" href="/css/register.css" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">
<div class="left">
<h1 class="title">创建一个账户</h1>
<div>
<p class="qp"><span>WHY</span>为什么要注册cnOpenBlog？</p>
<ol>
<li>cnOpenBlog是完全免费的</li>
<li>cnOpenBlog为用户提供一个开放的博客空间</li>
<li>cnOpenBlog为用户提供绝对自由的群组功能</li>
<li>cnOpenBlog为用户提供更强的交流平台</li>
</ol>
<p class="qp"><span>WHO</span>谁在使用cnOpenBlog？</p>
<div class="whobox">
<asp:Literal id="lblWho" runat="server" EnableViewState="false"></asp:Literal><div class="clear"></div>
</div>
<br />
<p class="qp qp2">每天有超过100个用户注册cnOpenBlog</p>
</div>
</div>
<div class="right">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="padding:20px 0px 10px 20px; font-family:华文行楷;"><span class="ltsp">开始注册</span><span class="lrsp"><a href="/login.aspx">登 录</a></span></div>
<div id="notediv" style="margin:10px;"></div>
<table cellspacing="8" style="margin:0px 10px;">
<tr><th>用户名：</th><td><input id="txtUsername" type="text" class="put" maxlength="20" title="只能包括字母、数字、下划线" /><span></span></td></tr>
<tr><th>真实姓名：</th><td><input id="txtFullname" type="text" class="put" maxlength="20" title="文章中作者的名字" /><span></span></td></tr>
<tr><th>密 码：</th><td><input id="txtPwd1" type="password" class="put" maxlength="20" title="6-20个任意字符" /><span></span></td></tr>
<tr><th>密码确认：</th><td><input id="txtPwd2" type="password" class="put" /><span></span></td></tr>
<tr><th>E-mail：</th><td><input id="txtEmail" type="text" class="put" /><span></span></td></tr>
<tr><th>性 别：</th><td><label><input id="radsex1" name="radsex" type="radio" checked="checked" /> 男</label> &nbsp; <label><input id="radsex2" name="radsex" type="radio" /> 女</label></td></tr>
<tr><th>出生日期：</th><td><select id="selYear"><option>1980</option></select> - <select id="selMonth"><option>1</option></select></td></tr>
<tr><th style="vertical-align:top; padding-top:6px;">验证码：</th><td><div class="capt_box"><table cellpadding="0"><tr><td style="border-right:solid 1px #999;"><input id="txtCaptcha" type="text" maxlength="6" /></td>
<td><img id="imgCaptcha" runat="server" src="/do/captcha.aspx" /></td></tr></table></div><div id="capt_n"><span></span>&nbsp;</div></td></tr>
<tr><th></th><td style="padding-top:10px;"><a class="btnb" href="javascript:void(0);" onclick="javascript:addUser();return false;">立 即 注 册</a></td></tr>
</table>
<br />
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>