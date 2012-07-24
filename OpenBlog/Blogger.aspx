<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Blogger.aspx.cs" Inherits="Blogger" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=titlestr %> - cnOpenBlog</title>
<meta http-equiv="content-type" content="text/html; charset=utf-8" />
<link rel="shortcut icon" href="favicon.gif" />
<meta name="keywords" content="<%=keywords %>" />
<link type="text/css" rel="Stylesheet" href="/css/blogger.css" />
<script type="text/javascript" src="/js/public.js"></script>
<style type="text/css">
<%=stylestr %>
</style>
</head>
<body>
<form id="form1" runat="server">
<div class="header">
<a href="http://www.cnopenblog.com/"><img src="/images/logo-b.gif" border="0" style="float:left;" /></a>
<asp:Literal ID="lblTabs" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
</div>
<div id="bodytop" class="bodytop"><asp:Literal ID="lblTitle" runat="server" EnableViewState="false"></asp:Literal></div>
<div id="bodymiddle" class="bodymiddle">
<div id="leftdiv" class="left">
<a class="write" href="/write.aspx">发表文章</a>
<div class="clear"></div>
<div style="padding-left:10px;">
<asp:Literal ID="lblBlogList" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="pagelist">
<asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
</div>
</div>
<div id="rightdiv" class="right">
<p class="ptit">作者信息</p>
<div id="authorinfo_box" class="rbox">
<asp:Literal ID="lblAuthorInfo" runat="server" EnableViewState="false"></asp:Literal>
</div>
<asp:Literal ID="lblRightData" runat="server" EnableViewState="false"></asp:Literal>

<div style="margin:20px 0px; text-align:center;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入关键字按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q='+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q='+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
</div>
<div id="bodybottom" class="bodybottom"></div>
</form>
<div class="footer">
<p style="text-align:right; padding-right:40px;"><a href="/about.html">关于cnOpenBlog</a> &nbsp; <a href="/contact.html">联系我们</a> &nbsp; &copy;2009 <a href="/">cnOpenBlog.com</a> &nbsp;版权所有</p>
</div><div id="smsp"></div>
<script type="text/javascript" src="/js/blogger.js"></script>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript">
</script>
</body>
</html>