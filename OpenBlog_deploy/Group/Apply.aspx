<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Group_Apply, App_Web_lizp4a2f" title="申请当群主 - cnOpenBlog" %>
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
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all">群组</a> &gt;&gt; <a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><%=DLL.Tools.HtmlEncode(groupName) %></a> &gt;&gt; 申请当管理员</p>
<h3>做群组管理员要具备以下条件：</h3>
<ol>
<li>有充足的时间打理群组事务</li>
<li>能够及时处理组内问题</li>
<li>品行良好，不恶语伤人</li>
</ol>
<div style="text-align:center;">
<p><a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><img src="/upload/group/<%=groupID %>.jpg" onerror="this.src='/upload/group/nophoto.jpg';" style="border:none;" /></a></p>
<p><a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><%=DLL.Tools.HtmlEncode(groupName) %></a></p>
<br />
<p style="padding-left:170px;">
<a class="btns" href="javascript:void(0);" onclick="javascript:applyIt();return false;">申 请</a><a class="btns" href="javascript:void(0);" onclick="javascript:cancel();return false;">返 回</a>
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
function applyIt()
{
    if(doing) return;
    doing = true;
    var url = execURL +"?applygroup=<%=groupID %>";
    ajaxGet(url);
    setTimeout(function(){
        alert('申请已经发送给群组创建者，请等待批准。');
        window.history.back();
    },1000);
}
function cancel()
{
    window.history.back();
}
</script>
</asp:Content>
