<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="ShortBlog_Default" Title="一句话博客 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/shortblog/css/user.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<div class="tixbox">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="padding:10px 14px; font-size:16px;">
一句话博客，记录生活点滴。<a href="/login.aspx?redirect=shortblog">登录</a>后马上分享你的酸甜苦辣。
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear" style="margin-bottom:20px;"></div>
<asp:Literal ID="lblDataList" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist"><asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal></div>
</div>
<div class="right">
<asp:Literal ID="lblLogin" runat="server" EnableViewState="false"></asp:Literal>
<br />
<div class="rightbox">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="padding:10px 20px;">
<p><b>今天最受欢迎的迷你博客</b></p>
<asp:Literal ID="lblHotSB" runat="server" EnableViewState="false"></asp:Literal>
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>

</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
<script type="text/javascript" src="/shortblog/js/default.js"></script>
</asp:Content>

