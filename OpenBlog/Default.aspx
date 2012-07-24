<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<meta name="keywords" content="博客,blog,群组,小组,圈子,group" />
<meta name="verify-v1" content="dp+0k9Gpt/3aJRflbFdOLKRNdPkF2LpPFA2ZP9eE3iQ=" />
<link type="text/css" rel="Stylesheet" href="/css/default.css" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">
<div class="left">
<div class="catbox">
<div class="clear"></div>
<asp:Literal ID="lblCategory" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
</div>
<br />
<div class="bhead">
<b style="width:290px;"><span>NEW</span>最新文章</b><b><span>HOT</span>热门文章</b><div class="clear"></div>
</div>
<div>
<div class="newblogbox">
<asp:Literal ID="lblNewBlogs" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="hotblogbox">
<asp:Literal ID="lblHotBlogs" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="clear"></div>
</div>
<div class="hotgroupbox radius">
<div class="hotgrouphead"><span style="color:#ff0000;">HOT GROUP</span>人气群组推荐</div>
<asp:Literal ID="lblGroups" runat="server" EnableViewState="false"></asp:Literal>
<div class="clear"></div>
</div>
<div class="bhead">
<b style="width:290px;"><span></span>生活/娱乐</b><b><span></span>IT</b><div class="clear"></div>
</div>
<div>
<div class="newblogbox">
<asp:Literal ID="lblBlogs1" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="hotblogbox">
<asp:Literal ID="lblBlogs2" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="clear"></div>
</div>
<div class="bhead">
<b style="width:290px;"><span></span>情感</b><b><span></span>女人</b><div class="clear"></div>
</div>
<div>
<div class="newblogbox">
<asp:Literal ID="lblBlogs3" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="hotblogbox">
<asp:Literal ID="lblBlogs4" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div class="clear"></div>
</div>
</div>
<!--right start-->
<div class="right">
<asp:Literal ID="lblLogin" runat="server" EnableViewState="false"></asp:Literal>
<br />
<div class="bhead"><b><span>HOT TOPIC</span>群组热帖</b><div class="clear"></div></div>
<div style="padding:0px 12px;">
<asp:Literal ID="lblHotTopic" runat="server" EnableViewState="false"></asp:Literal>
</div>
<br />
<div class="bhead"><b><span>HOT TAG</span>热门标签</b><div class="clear"></div></div>
<div class="tag_box">
<asp:Literal ID="lblHotTags" runat="server" EnableViewState="false"></asp:Literal>
</div>
<div style="margin-top:40px; padding-left:14px;">
<input id="txtQuickSearch" type="text" maxlength="50" value="快速搜索" title="输入关键字按回车搜索" onfocus="if(this.value='快速搜索')this.value='';" onblur="if(!this.value)this.value='快速搜索';" onkeydown="if(isEnter(event)){window.location='/search.aspx?q='+escape(this.value);}" />
<img class="imgGo" src="/images/go.png" alt="Go" title="开始搜索" onclick="window.location='/search.aspx?q='+escape(el('txtQuickSearch').value);" />
</div>
</div>
<div class="clear"></div>
<script type="text/javascript">
function checkRemember(e)
{
    e.className = e.className=="chkRe"?"chkRe nochk":"chkRe";
}
function login()
{
    el("txtName").className = "put";
    el("txtPassword").className = "put";
    el("txtName").nextSibling.className = "";
    el("txtPassword").nextSibling.className = "";
    
    if(!el("txtName").value.Trim()){
        el("txtName").className = "put erput";
        el("txtName").nextSibling.className = "ersp";
        el("txtName").focus();
        return;
    }
    if(!el("txtPassword").value.Trim()){
        el("txtPassword").className = "put erput";
        el("txtPassword").nextSibling.className = "ersp";
        el("txtPassword").focus();
        return;
    }
    var url = "/login.aspx?n="+escape(el("txtName").value)+"&p="+escape(el("txtPassword").value);
    if(el("chkRe").className=="chkRe") url += "&r=1";
    url += "&from=null";
    window.location = url;
}
</script>
</asp:Content>
