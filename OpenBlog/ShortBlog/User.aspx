<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="ShortBlog_User" Title="一句话博客 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/shortblog/css/user.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<div class="sendbox">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div style="padding:10px 14px;">
<p>一句话，记录生活点滴。(和别人分享你的想法、心情、状态。。。)</p>
<textarea id="txtMsg" class="put" rows="4" style="width:580px;"></textarea>
<p style="text-align:right;">
<a class="btns" href="javascript:void(0);" onclick="javascript:sendMsg();return false;">发 布</a><span id="sp_note"></span>
<span id="sp_num">0</span>/240 字</p>
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear" style="padding-top:20px;"></div>
<div id="bardiv" class="bar"><b></b><a href="javascript:void(0);" onclick="javascript:showblog(this,1);return false;"><b>个 人</b></a><b></b><a class="curr" href="javascript:void(0);" onclick="javascript:showblog(this,2);return false;"><b>好 友</b></a><b></b><a href="javascript:void(0);" onclick="javascript:showblog(this,3);return false;"><b>全 部</b></a><b style="width:380px;"></b></div>
<div class="clear" style="margin-bottom:20px;"></div>
<div id="bloglist_div">
</div>
<!--left end-->
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
<script type="text/javascript" src="/shortblog/js/user.js"></script>
</asp:Content>

