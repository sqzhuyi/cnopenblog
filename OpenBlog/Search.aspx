<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" Title="搜索 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/css/search.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<div class="searchbox">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
</script>
<div class="searchbox2">
<asp:Literal ID="lblStab" runat="server" EnableViewState="false"></asp:Literal>

<input id="btn1" class="btn" type="button" value="搜 索" onclick="doSearch()" />
<input id="btn2" class="btn" type="button" value="站外搜索" onclick="goSearch()" />
<p style="float:left;"><input id="chkOnlyTitle" name="chkOnlyTitle" type="checkbox" /><label for="chkOnlyTitle"> 只搜索标题</label></p>
<p style="text-align:right; padding-right:14px;"><a href="/advanced_search.aspx">高级搜索</a> &nbsp; <a href="/help/search.html">帮助</a></p>
</div>
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
</script>
</div>
<div style="margin-top:10px;">
<% PrintResult(); %>
</div>
<div class="pagelist">
<asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
</div>
</div>
<div class="right">
<div class="bhead"><span>HOT TAG</span>热门标签</div>
<div class="tag_box">
<asp:Literal ID="lblHotTags" runat="server" EnableViewState="false"></asp:Literal>
</div>
<p style="padding:150px 0px;"></p>
</div>
<div class="clear"></div>

<script type="text/javascript">
window.onload = function(){
    document.forms[0].onsubmit = function(){return false;};
    if("群组,评论".indexOf(els("stab","b")[0].innerHTML)!=-1){
        el("chkOnlyTitle").parentNode.style.display = "none";
    }else if(getQuery(window.location,"onlytitle")){
        el("chkOnlyTitle").checked = true;
    }
    el("txtKey").onfocus = function(){this.parentNode.style.borderColor="#fa9b1f";};
    el("txtKey").onblur = function(){this.parentNode.style.borderColor="#A1C1D7";};
    el("txtKey").onkeydown = function(ev){ if(isEnter(ev))doSearch();}
    el("txtKey").focus();
};
function doSearch(){
    var q = "?q="+ el("txtKey").previousSibling.innerHTML+escape(el("txtKey").value);
    if(el("chkOnlyTitle").checked) q += "&onlytitle=1";
    window.location = q;
}
function goSearch(){
    var q = "?q="+ el("txtKey").previousSibling.innerHTML+escape(el("txtKey").value);
    q += "&web=1";
    window.location = q;
}
</script>
</asp:Content>

