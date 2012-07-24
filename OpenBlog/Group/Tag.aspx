<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Tag.aspx.cs" Inherits="Group_Tag" Title="群组标签 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<style type="text/css">
.left { float:left; width:560px; }
.right { float:right; width:280px; }
.loginhead { border-top:solid 2px #acd5f8; background-color:#D3E7F8; padding:6px 16px; font-weight:bold;}
.loginbody { background-color:#E4F2FD; padding:16px 20px;}
.loginphoto { width:80px; border:solid 1px #ccc; padding:1px; }
.loginline { margin-top:16px;padding-top:12px; line-height:200%; border-top:solid 1px #ffffff; }
a.publish { background:url(/images/layout_add.gif) no-repeat; padding-left:18px; padding-bottom:2px; }
.chkRe { background:url(/images/icon_check.gif) no-repeat; padding:0px 0px 4px 22px; }
.nochk { background-image:url(/images/icon_check_off.gif);}
.bhead { margin:10px 0px; padding:6px 14px; border-top:solid 2px #cccccc; border-bottom:solid 1px #cccccc; }
.bhead b { display:block; float:left; color:#555555; }
.bhead b span { color:#ff0000; }
.ileft { float:left; width:54px; }
.ileft img { border:solid 1px #ccc; padding:1px; width:50px; }
.iright { float:right; width: 480px; }
.pagelist { text-align:center; margin:20px 0px; }
.pagelist a,.pagelist b { padding:1px 5px; }
.pagelist a:hover { color:#ffffff; background-color:#0066cc; }
.bold { font-size:13px; }
a.feed { background:url(/images/feed.png) no-repeat; padding-left:20px; padding-top:2px; }
.hotitem { float:left; width:120px; text-align:center; }
.hotitem img { border:none;}
.hotitem p { padding-bottom:20px; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<h1 style="font-size:24px;">群组标签：<span style="color:#ff0000;"><%=tag %></span></h1>
<div class="bhead">标签 <span style="color:#ff0000;"><%=tag %></span> 的搜索结果。没有需要的？试一下<a href="/search.aspx?q=group:<%=tag %>">全文搜索</a></div>
<asp:Literal ID="lblGroupList" runat="server" EnableViewState="false"></asp:Literal>
<div class="pagelist"><asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal></div>
</div>
<div class="right">
<asp:Literal ID="lblLogin" runat="server" EnableViewState="false"></asp:Literal>
<br />
<div class="bhead"><b><span>HOT</span>人气群组推荐</b><div class="clear"></div></div>
<div style="padding-top:20px;">
<asp:Literal ID="lblHotGroups" runat="server" EnableViewState="false"></asp:Literal>
</div>
<br />
<div class="bhead"><a class="feed" href='/group/tag/<%=tag+"/feed"+DLL.Settings.Ext %>'>订阅此标签</a></div>
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

