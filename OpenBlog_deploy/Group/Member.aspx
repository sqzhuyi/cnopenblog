<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Group_Member, App_Web_lizp4a2f" title="群组成员 - cnOpenBlog" %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<style type="text/css">
.bodymiddle { padding-top:10px; }
.left { float:left; width:660px; }
.right { float:right; width:240px; }
.boxhead { background-color:#E9E9E9; padding:2px 10px; text-align:right; }
.item { float:left; width:80px; text-align:center; line-height:20px; }
.item img { width:42px; border:solid 1px #f0f0f0; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all=1">群组</a> &gt;&gt; <%=barstr %> &gt;&gt; 成员列表</p>
<br />
<div style="background:url(/images/bg/box_hui.gif) no-repeat 0px 0px; padding-top:5px;"></div>
<div class="boxhead">排序：<a>按用户名</a> | <a>按加入时间</a></div>
<div style="background:url(/images/bg/box_hui.gif) no-repeat 0px -5px; padding-top:5px;"></div>
<br />
<div>
<asp:Literal ID="lblMember" runat="server" EnableViewState="false"></asp:Literal>
</div>
</div>
<div class="right">
</div>
<div class="clear"></div>
</asp:Content>

