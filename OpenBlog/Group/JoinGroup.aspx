<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="JoinGroup.aspx.cs" Inherits="Group_JoinGroup" Title="加入群组 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<style type="text/css">
.left { float:left; width:520px; }
.right { float:right; width:360px; background-color:#EDF2F6; -moz-border-radius: 8px;-webkit-border-radius:8px;}
.hotgitem { float:left; width:130px; text-align:center; }
.hotgitem img { border:none;}
.hotgitem p { padding-bottom:20px; }
a.btns { margin-right:20px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all">群组</a> &gt;&gt; <a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><%=DLL.Tools.HtmlEncode(groupName) %></a> &gt;&gt; 加入群组</p>
<h3>您将要加入以下群组</h3>
<div style="text-align:center;">
<p><a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><img src="/upload/group/<%=groupID %>.jpg" onerror="this.src='/upload/group/nophoto.jpg';" style="border:none;" /></a></p>
<p><a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><%=DLL.Tools.HtmlEncode(groupName) %></a></p>
<br />
<p style="padding-left:170px;">
<a class="btns" href="javascript:void(0);" onclick="javascript:joinIt();return false;">加 入</a><a class="btns" href="javascript:void(0);" onclick="javascript:cancel();return false;">返 回</a>
</p>
</div>
</div>
<div class="right">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="margin:10px 40px;">
<h4>人气群组推荐</h4>
<div class="clear"></div>
<asp:Literal ID="lblHotGroup" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
</div>
<br />
<p style="text-align:center;"><img src="/images/line.gif" style="width:300px; height:2px;" /></p>
<div style="margin:20px 40px;">
<a class="addg bold" href="/group/create.aspx">创建一个群组</a>
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript">
var doing = false;
function joinIt()
{
    if(doing) return;
    doing = true;
    var url = execURL +"?joingroup=<%=groupID %>";
    ajaxGet(url);
    setTimeout(function(){
        alert('您已经成功加入群组 <%=groupName.Replace("'","\\\'") %>。');
        window.location = "/group/<%=groupID %>"+ext;
    },1000);
}
function cancel()
{
    window.location = "/group/<%=groupID %>"+ext;
}
</script>
</asp:Content>

