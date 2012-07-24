<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Tag_Industry, App_Web_4q5ibh1l" title="行业: 电信 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/tag/css/all.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<h1 style="font-size:24px;">行业标签：<span style="color:#ff0000;"><%=tag %></span></h1>
<div class="bhead">行业 <span style="color:#ff0000;"><%=tag %></span> 的搜索结果。没有需要的？试一下<a href="/search.aspx?q=<%=tag %>">全文搜索</a></div>
<asp:Literal ID="lblDataList" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist"><asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal></div>
</div>
<div class="right">
<asp:Literal ID="lblLogin" runat="server" EnableViewState="false"></asp:Literal>
<br />
<div class="bhead"><b><span>HOT TAG</span>热门标签</b><div class="clear"></div></div>
<div class="tag_box">
<asp:Literal ID="lblHotTags" runat="server" EnableViewState="false"></asp:Literal>
</div>
<br />
<div class="bhead"><a class="feed" href='/industry/<%=tag+"/feed"+DLL.Settings.Ext %>'>订阅此标签</a></div>
<div style="margin-top:40px; padding-left:14px;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入关键字按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q=industry:'+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q=industry:'+escape(el('txtQuickSearch').value);" />
</div>
<p style="padding:50px 0px;"></p>
</div>
<div class="clear"></div>
<script type="text/javascript" src="/tag/js/all.js"></script>
</asp:Content>

