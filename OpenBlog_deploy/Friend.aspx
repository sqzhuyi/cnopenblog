<%@ page language="C#" masterpagefile="~/MasterPage2.master" autoeventwireup="true" inherits="Friend, App_Web_i3mdp3xs" title="好友管理 - cnOpenBlog" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<link type="text/css" rel="stylesheet" href="/css/shield.css" />
<style type="text/css">
.hui { color:#888888; }
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">

<div id="leftdiv" class="left">
    <h2 class="subtitle">我的好友</h2>
    <div style="margin-right:40px;">
    <asp:Literal ID="lblFriend" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <div class="pagelist">
    <asp:Literal ID="lblPageList" runat="server" EnableViewState="false"></asp:Literal>
    </div>
</div>
<!--right start-->
<div id="rightdiv" class="right">
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2">
    <p id="p1"><a href="/baseinfo.aspx">我的资料</a></p>
    <p id="p2"><a href="/write.aspx">发表文章</a></p>
    <p id="p3"><a href="/postlist.aspx">管理文章</a></p>
    <p id="p4"><a href="/inbox.aspx">管理留言</a></p>
    <p id="p5"><a href="/feedback.aspx">管理评论</a></p>
    <p id="p8"><a class="curr" href="/friend.aspx">我的好友</a></p>
    <p id="p6"><a href="/favorite.aspx">我的网摘</a></p>
    <p id="p7"><a href="/groups.aspx">我的群组</a></p>
    <p id="p9"><a href="/settings.aspx">主页设置</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
    <br />
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2" style="padding-left:14px;">
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>
<div id="divh"></div>
<script type="text/javascript" src="/js/shield.js"></script>
<script type="text/javascript" src="/js/friend.js"></script>
</asp:Content>

