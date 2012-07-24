<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="ShortBlog_Sblog, App_Web_4gtjblkg" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/shortblog/css/sblog.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div id="leftdiv" class="left">
<h1 class="title"><%=title %></h1>
<div class="sbconbox">
<asp:Literal ID="lblSBlog" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="replybox">
<p>回复该一句话博客<span style="padding-left:10px;"><%=!DLL.CKUser.IsLogin?"（登录之后才可回复，<a href='/login.aspx'>点此登录</a>。）":"" %></span></p>
<textarea id="txtMsg" class="put" rows="4" style="width:556px;"></textarea>
<p style="text-align:right;">
<a class="btns" href="javascript:void(0);" onclick="javascript:sendMsg(this);return false;">回 复</a><span id="sp_note2"></span>
<span id="sp_num">0</span>/240 字</p>
</div>
<asp:Literal ID="lblReply" runat="server" EnableViewState="false"></asp:Literal>
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
<input id="hd_id" type="hidden" value="<%=sb_id %>" />
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript" src="/shortblog/js/sblog.js"></script>
</asp:Content>

