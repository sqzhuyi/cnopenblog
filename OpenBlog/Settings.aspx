<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="bSettings" Title="个人主页设置 - cnOpenBlog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/css/settings.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">

<div id="leftdiv" class="left">
    <h2 class="subtitle">个人主页设置</h2>
    <div id="notediv"></div>
    <div id="div_title">
    <br />
    <p>主标题：<input id="txtTitle" type="text" class="put" style="width:300px;" maxlength="30" /></p>
    <br />
    <p>副标题：<input id="txtSubtitle" type="text" class="put" style="width:400px;" maxlength="50" /></p>
    <p style="padding:80px 100px;"><a class="btns" href="javascript:void(0);" onclick="javascript:saveTitle();return false;">保 存</a><a class="btns" href="javascript:void(0);" onclick="javascript:cancel();return false;">取 消</a></p>
    </div>
    <div id="div_column" style="display:none;">
    <div class="chk_box">
    <label for="chk_m_1"><input id="chk_m_1" type="checkbox" checked="checked" disabled="disabled" />作者信息</label>
    <label for="chk_m_2"><input id="chk_m_2" type="checkbox" />文章分类</label>
    <label for="chk_m_3"><input id="chk_m_3" type="checkbox" />参与的群组</label>
    <div class="clear"></div>
    <label for="chk_m_4"><input id="chk_m_4" type="checkbox" />我的好友</label>
    <label for="chk_m_5"><input id="chk_m_5" type="checkbox" onclick="click_fl(this.checked)" />友情链接</label>
    <label for="chk_m_6"><input id="chk_m_6" type="checkbox" />给作者留言</label>
    <div class="clear"></div>
    </div>
    <fieldset id="fl_box" style="display:none;">
    <legend><b>添加/修改友情链接</b></legend>
    <ol>
    <li>标题：<input type="text" class="put" maxlength="50" /> URL：<input type="text" class="put" style="width:280px;" maxlength="100" /></li>
    <li>标题：<input type="text" class="put" maxlength="50" /> URL：<input type="text" class="put" style="width:280px;" maxlength="100" /></li>
    <li>标题：<input type="text" class="put" maxlength="50" /> URL：<input type="text" class="put" style="width:280px;" maxlength="100" /></li>
    <li>标题：<input type="text" class="put" maxlength="50" /> URL：<input type="text" class="put" style="width:280px;" maxlength="100" /></li>
    <li>标题：<input type="text" class="put" maxlength="50" /> URL：<input type="text" class="put" style="width:280px;" maxlength="100" /></li>
    </ol>
    <p style="padding-left:20px;">* 最多只能添加<b>5</b>个友情链接</p>
    </fieldset>
    <p style="padding:60px 200px;"><a class="btns" href="javascript:void(0);" onclick="javascript:saveColumn();return false;">保 存</a><a class="btns" href="javascript:void(0);" onclick="javascript:cancel();return false;">取 消</a></p>
    </div>
    <div id="div_style" style="display:none;">
    <p>选择一个您喜欢的背景</p>
    <div class="bg_box">
    <script type="text/javascript">for(var i=0;i<13;i++)document.write('<a id="bg'+i+'" href="javascript:void(0);" onclick="javascript:setBG(this);return false;"></a>');</script>
    <div class="clear"></div>
    </div>
    <p style="padding:60px 200px;"><a class="btns" href="javascript:void(0);" onclick="javascript:saveBG();return false;">保 存</a><a class="btns" href="javascript:void(0);" onclick="javascript:cancel();return false;">取 消</a></p>
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
    <p id="p8"><a href="/friend.aspx">我的好友</a></p>
    <p id="p6"><a href="/favorite.aspx">我的网摘</a></p>
    <p id="p7"><a href="/groups.aspx">我的群组</a></p>
    <p id="p9"><a class="curr" href="/settings.aspx">主页设置</a></p>
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
    <div id="tablink2" class="tablink2" style="padding-left:14px;">
    <p><a class="curr" href="#title" onclick="javascript:show('title');">修改博客标题</a></p>
    <p><a href="#column" onclick="javascript:show('column');">管理博客栏目</a></p>
    <p><a href="#style" onclick="javascript:show('style');">更改背景</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<input id="hd_bg" type="hidden" />
<div id="prevbox" style="display:none;"><img alt="" src="#" /><p style="padding-left:90px;padding-top:10px;"><a class="btns" href="javascript:void(0);" onclick="javascript:useBG(true);return false;">应 用</a><a class="btns" href="javascript:void(0);" onclick="javascript:useBG(false);return false;">取 消</a></p></div>
<div class="clear"></div>
<script type="text/javascript"><%=script %></script>
<script type="text/javascript" src="/js/settings.js"></script>
</asp:Content>

