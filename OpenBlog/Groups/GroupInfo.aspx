<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="GroupInfo.aspx.cs" Inherits="Groups_GroupInfo" Title="修改群组资料 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<script type="text/javascript" src="/groups/js/groupinfo.js"></script>
<style type="text/css">
.phbox { float:right; width:220px; }
.phtitle { font-size:16px; font-weight:bold; color:#666666; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div id="leftdiv" class="left">
    <h2 class="subtitle">修改群组资料</h2>
    <div id="notediv" style="width:500px; margin:10px 0px;"></div>
    <div class="phbox">
    <p class="phtitle">群组照片</p>
    <p><img id="gimg" src="/upload/group/<%=groupID %>.jpg" onerror="this.src='/upload/group/nophoto.jpg';" style="width:120px;border:solid 1px #ccc;" /></p>
    <p><iframe id="photo_frm" name="photo_frm" src="/ajax/uploadfile.aspx?size=12&group=1&id=<%=groupID %>" frameborder="0" scrolling="no" style="width:200px; height:24px;"></iframe></p>
    <p><input type="button" value="上 传" onclick="uploadImg(this);" /></p>
    <p>图片标准大小为 120 x 120 像素</p>
    </div>
    <table cellspacing="8" style="margin:0px 10px;">
    <tr><td>群组名称：</td><td><input id="txtName" type="text" class="put" style="width:200px;" maxlength="50" /><span></span></td></tr>
    <tr><td>群组类别：</td><td><select id="selCat" class="put"></select><span></span></td></tr>
    <tr><td>群组标签：</td><td><input id="txtTags" type="text" class="put" style="width:300px;" maxlength="50" /><span></span></td></tr>
    <tr><td style="vertical-align:top; padding-top:8px;">群组简介：</td><td><textarea id="txtJianjie" rows="4" class="put" style="width:300px;"></textarea><span></span></td></tr>
    <tr><td style="vertical-align:top; padding-top:8px;">群组公告：</td><td><textarea id="txtGonggao" rows="4" class="put" style="width:300px;"></textarea><span></span></td></tr>
    <tr><td></td><td style="padding-top:10px;"><a class="btns" href="javascript:void(0);" onclick="javascript:save();return false;">保 存</a></td></tr>
    </table>
    <br />
    <p style="padding-left:18px;">群组链接：<a href='<%=DLL.Settings.BaseURL+"group/"+groupID.ToString()+DLL.Settings.Ext %>'><%=DLL.Settings.BaseURL+"group/"+groupID.ToString()+DLL.Settings.Ext %></a></p>
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
    <p id="p6"><a href="/favorite.aspx">我的网摘</a></p>
    <p id="p7"><a class="curr" href="/group.aspx">我的群组</a></p>
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
    <p><a class="curr" href="/groups.aspx">所有参与的群组</a></p>
    <p><a href="/groups/create.aspx">我创建的群组</a></p>
    <p><a href="/groups/join.aspx">我参加的群组</a></p>
    <p><a href="/groups/post.aspx">我发起的话题</a></p>
    <p><a href="/groups/reply.aspx">我回复的话题</a></p>
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
    <p><a class="curr" href="/groups/10000.aspx">修改群组资料</a></p>
    <p><a href="/groups/10000/member.aspx">管理群组成员</a></p>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>
<input id="hd_group" type="hidden" value="<%=groupID %>" />
<script type="text/javascript"><%=script %></script>
</asp:Content>

