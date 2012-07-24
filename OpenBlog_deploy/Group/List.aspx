<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Group_List, App_Web_lizp4a2f" title="群组 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<meta name="keywords" content="<%=keywords %>" />
<link type="text/css" rel="Stylesheet" href="/group/css/default.css" />
<link type="text/css" rel="Stylesheet" href="/group/css/list.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="newestbox">
<div class="marqtitle">最新创建群组：</div><div id="sp_marq"><asp:Literal ID="lblNewestgroups" runat="server" EnableViewState="false"></asp:Literal></div>
<div class="clear"></div>
</div>
<div id="leftdiv" class="left">
<p class="topbar"><a href="/">首页</a> &gt;&gt; <a href="/group/?all">群组</a> &gt;&gt; <%=DLL.Category.GetNameById(catID) %></p>
<asp:Literal ID="lblGroups" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist">
<asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="clear"></div>
<br />
<div class="huatitop"><a href="javascript:void(0);" onclick="javascript:showhuati(this,1,<%=catID %>);return false;">最新话题</a><a href="javascript:void(0);" onclick="javascript:showhuati(this,2,<%=catID %>);return false;">本周热门话题</a><div class="clear"></div></div>
<div id="huati_div" class="huatibody">
<asp:Literal ID="lblHuati" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="huatibottom"></div>
<!--left end-->
</div>
<div class="right">
<p style="text-align:right;"><a href="/group/create.aspx"><img src="/images/g_add.gif" border="0" alt="Create a group" /></a></p>
<br />
<div class="top20title">
<b style="color:#ffffff; font-size:18px;">TOP 20</b><a class="gtoptab curr" href="javascript:void(0);" onclick="javascript:showtop(this,1);return false;">MONTH</a><a class="gtoptab" href="javascript:void(0);" onclick="javascript:showtop(this,2);return false;">TOTAL</a>
<div class="clear"></div>
</div>
<div id="top20box" class="top20body"><asp:Literal ID="lblTop20" runat="server" EnableViewState="false"></asp:Literal></div>
<div class="top20bottom"></div>
<br />
<div class="top20title" style="padding-bottom:6px;"><b style="color:#ffffff; font-size:16px;">TA 刚刚加入</b></div>
<div class="top20body" style="padding:10px 16px;">
<div id="join_div"><asp:Literal ID="lblJoin" runat="server" EnableViewState="false"></asp:Literal></div>
</div>
<div class="top20bottom"></div>
</div>

<div class="clear"></div>
<script type="text/javascript" src="/group/js/default.js"></script>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>

