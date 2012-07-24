<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="Group_User" Title="我参与的群组 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="Stylesheet" href="/group/css/user.css" />
<script type="text/javascript" src="/group/js/user.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<p class="topbar"><a href="/">首页</a> &gt;&gt; <a href="/group/?all">群组</a> &gt;&gt; <%=self?"我":who %>参与的群组</p>
<h1 class="boxhead"><%=self?"我":who %>创建的群组</h1>
<asp:Literal ID="lblCreate" runat="server" EnableViewState="false"></asp:Literal>
<br /><br />
<h1 class="boxhead"><%=self?"我":who %>加入的群组</h1>
<asp:Literal ID="lblJoin" runat="server" EnableViewState="false"></asp:Literal>
<br />
<h1 class="boxhead2"><%=self?"我":who %>参于的群组最新话题</h1>
<asp:Literal ID="lblNewTopic" runat="server" EnableViewState="false"></asp:Literal>
<br />
<h1 class="boxhead2">所有群组最新话题</h1>
<asp:Literal ID="lblAllNewTopic" runat="server" EnableViewState="false"></asp:Literal>
<br />
<h1 class="boxhead2">今日热门话题</h1>
<asp:Literal ID="lblHotTopic" runat="server" EnableViewState="false"></asp:Literal>
<br />
</div>
<div class="right">
<p style="text-align:right;"><a href="/group/create.aspx"><img src="/images/g_add.gif" border="0" alt="Create a group" /></a></p>
<h1><span style="color:#ff0000;">HOT</span>人气群组推荐</h1>
<div>
<asp:Literal ID="lblHotGroup" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="clear"></div>
<p style="text-align:right;"><a href="/group/?all">查看所有群组</a></p>
<h1><span style="color:#ff0000;">STA</span> 统计信息</h1>
<div style="padding-left:20px;">
<asp:Literal ID="lblStatistics" runat="server" EnableViewState="false"></asp:Literal>
</div>
<p style="text-align:right;">注：10分钟更新一次</p>
<div style="margin-top:40px;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入群组名称按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q=group:'+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q=group:'+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>

