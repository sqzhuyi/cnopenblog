<%@ page language="C#" masterpagefile="~/MasterPage2.master" autoeventwireup="true" inherits="Write, App_Web_i3mdp3xs" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<link type="text/css" rel="stylesheet" href="/css/shield.css" />
<script type="text/javascript" src="/js/shield.js"></script>
<script type="text/javascript" src="/js/write.js"></script>
<style type="text/css">
.btna { float:left; display:block; width:115px; margin-right:10px; background:url(/images/r-btn.gif) no-repeat; padding:9px 0px; text-align:center; font-size:13px; font-weight:bold; color:#2d6f92; text-decoration:none;}
.btna:hover { background-image:url(/images/r-btn-2.gif); color:#000000; text-decoration:none;}
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">

<div id="leftdiv" class="left">
    <h2 class="subtitle">发表文章</h2>
    <div id="note_div" style="margin:10px;"></div>
    <p>文章标题：<br /><input id="txtTitle" type="text" class="put" maxlength="100" style="width:540px;" /></p>
    <p>文章分类：<br /><asp:Literal ID="lblCategory" runat="server" EnableViewState="false"></asp:Literal> <a href="javascript:void(0);" onclick="javascript:editsubcat();return false;">新建/修改分类</a></p>
    <div style="margin-top:10px;">
    <iframe name="editor_frm" src="/htmleditor/editor.html" frameborder="0" scrolling="no" style="width:670px;height:320px;"></iframe>
    </div>
    <p>文章摘要：（一个好的摘要能显著提高文章阅读量）<br /><textarea id="txtZhaiyao" class="put" rows="4" style="width:660px;"></textarea></p>
    <p><span class="en">Tag</span>标签：(多个关键字之间用“,”分隔，Tag长度至少为两个字)<br /><input id="txtTag" type="text" class="put" maxlength="60" style="width:380px;" />
    <span style="padding-left:20px;"><input id="chkNoComment" name="chkNoComment" type="checkbox" /><label for="chkNoComment">禁止评论</label></span></p>
    <div style="margin:20px 20px 0px 0px;">
    <a class="btna" href="javascript:void(0);" onclick="javascript:publish();return false;">发 布</a><a class="btna" href="javascript:void(0);" onclick="javascript:review();return false;">预 览</a><a class="btna" href="javascript:void(0);" onclick="javascript:cancel();return false;">取 消</a>
    </div>
</div>
<div id="rightdiv" class="right">
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2">
    <p id="p1"><a href="/baseinfo.aspx">我的资料</a></p>
    <p id="p2"><a class="curr" href="/write.aspx">发表文章</a></p>
    <p id="p3"><a href="/postlist.aspx">管理文章</a></p>
    <p id="p4"><a href="/inbox.aspx">管理留言</a></p>
    <p id="p5"><a href="/feedback.aspx">管理评论</a></p>
    <p id="p8"><a href="/friend.aspx">我的好友</a></p>
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
    <div class="rightsubbox">
    <p>用过的标签：</p>
    <p><asp:Literal ID="lblTags" runat="server" EnableViewState="false"></asp:Literal></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>
<textarea id="hd_con" cols="100" rows="1" style="visibility:hidden;"><%=con %></textarea>
<div id="divh" style="position:absolute; z-index:101px;"></div>
<script type="text/javascript"><%=script %></script>
    
</asp:Content>
