<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="BaseInfo.aspx.cs" Inherits="BaseInfo" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
    <script type="text/javascript" src="/js/city.js"></script>
    <script type="text/javascript" src="/js/baseinfo.js"></script>
    <style type="text/css">
    #box0 th,#box2 th { font-size:13px; font-weight:normal; text-align:right; color:#444444; }
    #note { margin-left:30px; margin-bottom:10px; width:460px;}
    a.btns { margin-right:20px;}
    </style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">

<div id="leftdiv" class="left">
    <h2 class="subtitle">个人资料管理</h2>
    <asp:Literal ID="lblBox1" runat="server" EnableViewState="false"></asp:Literal>
    <br />
    <div id="note"></div>
    <table id="box0" cellspacing="8" style="display:none;">
        <tr><th style="width:100px;">真实姓名：</th><td><input id="txtFullname" type="text" class="put" style="width:200px;" maxlength="20" title="文章中作者的名字" /></td></tr>
        <tr><th>E-mail：</th><td><input id="txtEmail" type="text" class="put" style="width:200px;" /> <label><input id="chkEmail" type="checkbox" />公开</label></td></tr>
        <tr><th>性 别：</th><td><label><input id="radsex1" name="radsex" type="radio" checked="checked" /> 男</label> &nbsp; <label><input id="radsex2" name="radsex" type="radio" /> 女</label></td></tr>
        <tr><th>出生日期：</th><td><select id="selYear"><option>1980</option></select> - <select id="selMonth"><option>1</option></select> <label><input id="chkBirthday" type="checkbox" />公开</label></td></tr>
        <tr><th>所在城市：</th><td><select id="selState"></select> <select id="selCity"></select></td></tr>
        <tr><th>行 业：</th><td><select id="selHangye"><option>----</option></select></td></tr>
        <tr><th>个人网站：</th><td><input id="txtUrl" type="text" class="put" style="width:300px;" /></td></tr>
        <tr><th>QQ：</th><td><input id="txtQQ" type="text" class="put" style="width:120px" maxlength="10" /> <label><input id="chkQQ" type="checkbox" />公开</label></td></tr>
        <tr><th>MSN：</th><td><input id="txtMSN" type="text" class="put" style="width:200px;" maxlength="50" /> <label><input id="chkMSN" type="checkbox" />公开</label></td></tr>
        <tr><th>博客标题：</th><td><input id="txtBlogtitle" type="text" class="put" style="width:300px;" maxlength="30" /></td></tr>
        <tr><th>博客副标题：</th><td><input id="txtBlogsubtitle" type="text" class="put" style="width:300px;" maxlength="50" /></td></tr>
        <tr><th style="vertical-align:top; padding-top:10px;">个人简介：</th><td><textarea id="txtJianjie" class="put" rows="4" cols="50" title="输入你的个人简介，方便别人进一步了解你。"></textarea>
        <br /><span id="sp_num1">0</span>/240字</td></tr>
        <tr><th style="vertical-align:top; padding-top:10px;">兴趣爱好：</th><td><textarea id="txtXingqu" class="put" rows="4" cols="50" title="输入你的兴趣爱好，方便别人进一步了解你。"></textarea>
        <br /><span id="sp_num2">0</span>/240字</td></tr>
        <tr><th style="vertical-align:top; padding-top:10px;">个性签名：</th><td><textarea id="txtQianming" class="put" rows="3" cols="50" title="个性签名将出现在群组的帖子中。"></textarea>
        <br /><span id="sp_num3">0</span>/120字<span class="em" style="padding-left:10px;">(个性签名将出现在群组的帖子中)</span></td></tr>
        <tr><th></th><td style="padding-top:10px;"><a class="btns" href="javascript:void(0);" onclick="javascript:save();return false;">保 存</a></td></tr>
    </table>
    <div id="box1" style="display:none;">
    <div style="margin-left:80px;">
    <p>上传真实头像别人会更加了解你。</p>
    <p>你可以在上传头像照片后裁剪适合大小的头像（标准宽度120像素）。</p>
    <table cellpadding="0"><tr><td style="padding-top:6px;">
    <iframe id="photo_frm" name="photo_frm" src="/ajax/uploadfile.aspx" frameborder="0" scrolling="no" style="width:260px; height:30px;"></iframe>
    </td><td><input type="button" onclick="uploadfile(this);" value="上 传" /></td></tr>
    </table>
    </div>
    <div id="box1_2" style="display:none;">
    <iframe id="head_frm" name="head_frm" src="/cropper/headphoto.aspx" frameborder="0" scrolling="no" style="width:0px; height:0px;" onload="reshowFooter()"></iframe>
    <p style="padding:20px 0px 20px 200px;">
    <a class="btns" href="javascript:void(0);" onclick="javascript:cutImg();return false;">确定裁剪</a><a class="btns" href="#view" onclick="javascript:goview();return false;">不裁剪</a>
    </p>
    </div>
    </div>
    <table id="box2" cellspacing="8" style="width:600px;display:none;">
    <tr><th>旧密码：</th><td><input id="txtOldpwd" type="password" class="put" style="width:220px;" maxlength="20" /></td></tr>
    <tr><td colspan="2"></td></tr>
    <tr><th>新密码：</th><td><input id="txtNewpwd" type="password" class="put" style="width:220px;" maxlength="20" /></td></tr>
    <tr><th>密码确认：</th><td><input id="txtNewpwd2" type="password" class="put" style="width:220px;" maxlength="20" /></td></tr>
    <tr><td></td><td style="padding-top:20px;"><a class="btns" href="javascript:void(0);" onclick="javascript:savePwd();return false;">保 存</a></td></tr>
    </table>
</div>
<!--right start-->
<div id="rightdiv" class="right">
    <div class="rightbox">
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
    </script>
    <div class="tablink2">
    <p id="p1"><a class="curr" href="/baseinfo.aspx">我的资料</a></p>
    <p id="p2"><a href="/write.aspx">发表文章</a></p>
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
    <div class="tablink2">
    <p id="p01"><a class="curr" href="#view" onclick="javascript:show('view');">查看资料</a></p>
    <p id="p02"><a href="#edit" onclick="javascript:show('edit');">修改资料</a></p>
    <p id="p03"><a href="#photo" onclick="javascript:show('photo');">更改头像</a></p>
    <p id="p04"><a href="#password" onclick="javascript:show('password');">修改密码</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>
<script type="text/javascript"><%=script %></script>

</asp:Content>