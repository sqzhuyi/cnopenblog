<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Group_Default" Title="群组 - cnOpenBlog" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<meta name="keywords" content="群组,小组,圈子,博客" />
<link type="text/css" rel="Stylesheet" href="/group/css/default.css" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">
<div class="newestbox">
<div class="marqtitle">最新创建群组：</div><div id="sp_marq"><asp:Literal ID="lblNewestgroups" runat="server" EnableViewState="false"></asp:Literal></div>
<div class="clear"></div>
</div>
<div id="leftdiv" class="left">
<asp:Literal ID="lblGroup" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
<br />
<div class="huatitop"><a href="javascript:void(0);" onclick="javascript:showhuati(this,1);return false;">最新话题</a><a href="javascript:void(0);" onclick="javascript:showhuati(this,2);return false;">本周热门话题</a><div class="clear"></div></div>
<div id="huati_div" class="huatibody">
<asp:Literal ID="lblHuati" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="huatibottom"></div>
</div>
<div class="right">
<p style="text-align:right;"><a href="/group/create.aspx"><img src="/images/g_add.gif" border="0" alt="创建一个群组" /></a></p>
<br />
<div class="top20title">
<b style="color:#ffffff; font-size:18px;">TOP 20</b><a class="gtoptab curr" href="javascript:void(0);" onclick="javascript:showtop(this,1);return false;" title="最近一个月创建的群组人气排行">MONTH</a><a class="gtoptab" href="javascript:void(0);" onclick="javascript:showtop(this,2);return false;" title="所有群组人气排行">TOTAL</a>
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
<div style="margin-top:40px;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入群组名称按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q=group:'+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q=group:'+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
<script type="text/javascript" src="/group/js/default.js"></script>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>