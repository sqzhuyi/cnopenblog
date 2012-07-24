<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="TopicList.aspx.cs" Inherits="Group_TopicList" Title="群组帖子列表 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<meta name="keywords" content="<%=keywords %>" />
<link type="text/css" rel="Stylesheet" href="/group/css/group.css" />
<script type="text/javascript" src="/group/js/group.js"></script>
<style type="text/css">
.bodymiddle { padding-top:10px; }
.left { float:left; width:660px; }
.right { float:right; width:240px; }
.pagelist { text-align:center; margin:20px 0px; }
.pagelist a,.pagelist b { padding:1px 5px; }
.pagelist a:hover { color:#ffffff; background-color:#0066cc; }
a.newtopic { float:right; *margin-top:-20px; }
a.newtopic,x:-moz-any-link, x:default { margin-top:-20px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all=1">群组</a> &gt;&gt; <%=barstr %> &gt;&gt; 群组帖子列表</p>
<a class="newtopic" href="/group/newpost.aspx?q=<%=groupID %>" title="发表新帖">发起话题</a>
<br />
<asp:Literal ID="lblTopic" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist">
<asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
</div>
<!--left end-->
</div>
<div class="right">
<asp:Literal ID="lblRight" runat="server" EnableViewState="false"></asp:Literal>
<br />
<div class="radius" style="background-color:#EDF2F6;">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="padding:10px 20px;">
<asp:Literal ID="lblLinks" runat="server" EnableViewState="false"></asp:Literal>
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
</script>
</div>
<div style="margin-top:40px; text-align:center;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索帖子" title="输入话题关键字按回车搜索" onfocus="if(this.value='快速搜索帖子')this.value='';" onblur="if(!this.value)this.value='快速搜索帖子';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q=topic:'+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q=topic:'+escape(el('txtQuickSearch').value);" />
</div>
<!--right end-->
</div>
<div class="clear"></div>
</asp:Content>

