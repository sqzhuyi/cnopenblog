<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="UserSB.aspx.cs" Inherits="ShortBlog_UserSB" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/shortblog/css/sblog.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div id="leftdiv" class="left">
<h1 class="title"><%=title %></h1>
<asp:Literal ID="lblDataList" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist"><asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal></div>
<!--left end-->
</div>
<div id="rightdiv" class="right">
<div class="rightsubbox">
<asp:Literal ID="lblAuthorData" runat="server" EnableViewState="false"></asp:Literal>
</div><div class="rightsubbox_bot"></div>
<p><b>今天最受欢迎的迷你博客</b></p>
<div class="rightsubbox">
<asp:Literal ID="lblHotSB" runat="server" EnableViewState="false"></asp:Literal>
</div><div class="rightsubbox_bot"></div>
<div style="margin:30px 0px;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入关键字按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q='+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q='+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript" src="/shortblog/js/usersb.js"></script>
</asp:Content>

