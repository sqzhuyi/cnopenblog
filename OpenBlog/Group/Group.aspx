<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="Group_Group" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<meta name="keywords" content="<%=keywords %>" />
<link type="text/css" rel="Stylesheet" href="/group/css/group.css" />
<script type="text/javascript" src="/group/js/group.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<asp:Literal ID="lblLeft" runat="server" EnableViewState="false"></asp:Literal>
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
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索帖子" title="输入话题关键字按回车搜索" onfocus="if(this.value='快速搜索帖子')this.value='';" onblur="if(!this.value)this.value='快速搜索帖子';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q=topic:'+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q=topic:'+escape(el('txtQuickSearch').value);" />
</div>
<!--left end-->
</div>
<div class="right">
<p>
<span class="newt"><span style="font-size:12px; font-weight:bold; color:#ff0000;">NEW</span> 群组最新话题</span>
<asp:Literal ID="lblRightTop" runat="server" EnableViewState="false"></asp:Literal>
</p>
<div class="clear"></div>
<asp:Literal ID="lblTopic" runat="server" EnableViewState="false"></asp:Literal>
<br />
<p>
<span class="newt"><span style="font-size:12px; font-weight:bold; color:#ff0000;">NEW</span> 组员最新文章</span>
</p>
<div class="clear"></div>
<asp:Literal ID="lblBlog" runat="server" EnableViewState="false"></asp:Literal>
<!--right end-->
</div>
<div class="clear"></div>
</asp:Content>

