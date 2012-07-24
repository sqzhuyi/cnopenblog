<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="BlogList.aspx.cs" Inherits="Group_BlogList" Title="群组成员文章 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="Stylesheet" href="/group/css/group.css" />
<script type="text/javascript" src="/group/js/group.js"></script>
<style type="text/css">
.bodymiddle { padding-top:10px; }
.left { float:left; width:660px; }
.right { float:right; width:240px; }
.boxhead { margin-top:16px; text-align:right; }
.pagelist { text-align:center; margin:20px 0px; }
.pagelist a,.pagelist b { padding:1px 5px; }
.pagelist a:hover { color:#ffffff; background-color:#0066cc; }
a.write { float:right; width:80px; background:url(/images/bg/btn_bg_r.gif) no-repeat; text-align:center; padding:6px 0px; margin-top:-16px; color:#ffffff; }
a.write:hover { color:#F3A217;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all=1">群组</a> &gt;&gt; <%=barstr %> &gt;&gt; 成员文章列表</p>
<a class="write" href="/write.aspx">发表文章</a>
<div class="boxhead"><%=orderstr %></div>
<asp:Literal ID="lblBlog" runat="server" EnableViewState="false"></asp:Literal>
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
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索文章" title="输入话题关键字按回车搜索" onfocus="if(this.value='快速搜索文章')this.value='';" onblur="if(!this.value)this.value='快速搜索文章';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q='+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q='+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
</asp:Content>

