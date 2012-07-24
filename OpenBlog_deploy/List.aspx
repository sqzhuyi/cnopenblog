<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="List, App_Web_i3mdp3xs" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<meta name="keywords" content="<%=keywords %>" />
<link type="text/css" rel="Stylesheet" href="/css/list.css" />
<style type="text/css">
.left { float:left; width:180px; padding-right:3px; border-left:solid 1px #EEF8FC; background:url(/images/bg/vline_bg_right.gif) repeat-y right top #EEF8FC; }
.right { float:right; width:720px; text-align:left; }
.subtitle { padding:6px 12px;}
.subtitle a { font-weight:bold; color:#555555; }
.subbox { background-color:#F7FBFD; padding:4px 10px; }
#cat_box a { float:left; display:block; width:80px; height:22px; text-align:center; padding-top:6px; border-bottom:solid 1px #eee; color:#0f419d; }
#cat_box a:hover { background-color:#eee; border-bottom-color:#ffffff; text-decoration:underline; }
#cat_box a.curr { color:#333333; font-weight:bold; border-bottom-color:#333333; }
.subbox .gitem { border-bottom:solid 1px #eee; padding:4px 0px; }
a.write { float:right; width:80px; background:url(/images/bg/btn_bg_r.gif) no-repeat; text-align:center; padding:6px 0px; margin-top:-16px; color:#ffffff; }
a.write:hover { color:#F3A217;}
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">

<div id="leftdiv" class="left">
<p class="subtitle"><a href="/100<%=DLL.Settings.Ext %>">全部分类</a></p>
<div id="cat_box" class="subbox">
<div class="clear"></div>
<asp:Literal ID="lblCat" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
</div>
<p class="subtitle"><a href="/group/<%=gcatID.ToString()+DLL.Settings.Ext %>">推荐群组</a></p>
<div class="subbox">
<asp:Literal ID="lblGroup" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div style="margin-top:40px; text-align:center;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入关键字按回车搜索" style="width:110px;" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q='+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q='+escape(el('txtQuickSearch').value);" />
</div>
<br /><br />
</div>
<div id="rightdiv" class="right">
<div style="padding-bottom:10px;">
<asp:Literal ID="lblSortBar" runat="server" EnableViewState="false"></asp:Literal>
<a class="write" href="/write.aspx">发表文章</a>
<div class="clear"></div>
</div>
<div class="bloglist">
<asp:Literal ID="lblBlogList" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="pagelist">
<asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
</div>
</div>
<div class="clear"></div>
<script type="text/javascript">
window.onload = function(){
    var h = el("rightdiv").offsetHeight;
    if(h>el("leftdiv").offsetHeight)
        el("leftdiv").style.height = h+"px";
};
</script>

</asp:Content>