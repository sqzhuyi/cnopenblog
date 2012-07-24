<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<style type="text/css">
.put { font-size:16px; width:160px; }
.fname { font-size:14px; width:80px; font-weight:bold; text-align:right; }
#chkimg { float:left;}
.left { float:left; width:540px; }
.right { float:right; width:340px; background-color:#EDF2F6; -moz-border-radius: 8px;-webkit-border-radius:8px;}
a.reg { background:url(/images/r_right.gif) no-repeat 0px -1px; padding-left:18px; font-size:13px; }
.title { font-size:28px; font-family:华文新魏; color:#267BB5; margin-top:0px; }
.item { background-repeat:no-repeat; background-position:10px 6px; padding-left:80px; padding-bottom:30px; }
.item p { font-size:13px;font-weight:bold;}
.item1 { background-image:url(/images/webdoc_file.gif);}
.item2 { background-image:url(/images/interface.png);}
.item3 { background-image:url(/images/profile.png);}
.item4 { background-image:url(/images/discussion.png);}
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">
<div class="left">
<h1 class="title">登录到<span style="font-family:Arial;"><span style="font-size:14px;">cn</span>OpenBlog</span></h1>
<br />
<div class="item item1"><p>发表自己的文章</p>cnOpenBlog采用强大的Lucene搜索技术，您的文章讲更容易被找到。</div>
<div class="item item2"><p>清新的用户界面</p>简洁大方的用户界面设计，一扫您以往上网的枯燥乏味。</div>
<div class="item item3"><p>强大的后台信息管理</p>使用cnOpenBlog的后台管理功能，将使您的信息操作更加容易、更加方便。</div>
<div class="item item4"><p>方便快捷的讨论</p>cnOpenBlog提供免费群组功能，任何注册用户都可以随意创建/参加一个群组，进行讨论发帖。</div>
</div>
<div class="right">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div id="notediv" style="margin:10px;"></div>
<table cellspacing="12">
<tr><td class="fname">用户名:</td><td><input id="txtUsername" type="text" class="put" maxlength="20"  /></td></tr>
<tr><td class="fname">密 码:</td><td><input id="txtPwd" type="password" class="put" maxlength="20" onkeydown="if(isEnter(event))login();" /><span></span></td></tr>
<tr><td class="fname"></td><td style="vertical-align:top;"><img id="chkimg" src="/images/icon_check.gif" alt="" onclick="checkRemember()" /><label onclick="checkRemember()" style="padding-left:6px;">下次自动登录</label></td></tr>
<tr><td></td><td style="padding-top:10px;"><a class="btns" href="javascript:void(0);" onclick="javascript:login();return false;">登 录</a><span style="padding-left:20px;"></span><a href="/resetpwd.aspx">忘记密码</a></td></tr>
</table>
<br />
<p style="text-align:center;"><img src="/images/line.gif" style="width:330px; height:2px;" /></p>
<div style="margin:10px 40px;">
<p><b>还没有注册cnOpenBlog?</b></p>
<p><a class="reg" href="/register.aspx">马上注册</a></p>
</div><div class="clear" style="padding-top:20px;"></div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear"></div>
<input id="hd_redirect" type="hidden" value="<%=redirect %>" />
<script type="text/javascript" src="/js/login.js"></script>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>
