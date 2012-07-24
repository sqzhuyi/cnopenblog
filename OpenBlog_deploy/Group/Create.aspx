<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Group_Create, App_Web_lizp4a2f" title="创建一个群组 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<script type="text/javascript" src="/group/js/create.js"></script>
<style type="text/css">
h3 { font-family:宋体; }
th { font-size:13px; width:80px; font-weight:normal; text-align:right; vertical-align:top; color:#444444; }
.capt_box { width:210px; border:solid 1px #999; background-color:#ffffff; -moz-border-radius: 3px;-webkit-border-radius:3px;}
#txtCaptcha { padding:4px; font-size:16px; font-weight:bold; border:solid 1px #ffffff; width:80px; }
#capt_n { float:left; position:relative; left:240px; top:-30px;}
.left { float:left; width:420px; }
.right { float:right; width:460px; background-color:#EDF2F6; -moz-border-radius: 8px;-webkit-border-radius:8px;}
.title { font-size:28px; font-family:华文新魏; color:#267BB5; margin-top:0px; }
.subtitle { background-position:0px 0px; background-repeat:no-repeat; margin-left:10px; padding:8px 40px; color:#394C08; }
.subtitle1 { background-image:url(/images/number_1.gif); }
.subtitle2 { background-image:url(/images/number_2.gif); }
.guanyu { background:url(/images/gbig.gif) no-repeat 0px 0px; padding:5px 40px; font-size:16px; color:#555555; }
#gimg { border:solid 1px #999999; padding:2px; background-color:#ffffff; }
a.btnb { float:left; display:block; width:155px; background:url(/images/green_button.gif) no-repeat; padding:9px 0px; text-align:center; font-size:14px; font-weight:bold; color:#ffffff; text-decoration:none;}
a.btnb:hover { text-decoration:underline;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<div class="left">
<h1 class="title">创建一个群组</h1>
<h3 class="subtitle subtitle1">填写群组基本信息</h3>
<h3 class="subtitle subtitle2">上传群组标志照片</h3>
<br />
<h3 class="guanyu">关于群组</h3>
<div style="padding-left:20px;">
<p><a href="/about.html" target="_blank">cnOpenBlog</a>群组为<span style="color:#ff0000;">公开</span>群组，任何注册用户可随意加入任何群组。</p>
<p>群组可以设定不超过5个的标签，用来描述群组的性质。标签作为关键词可以被用户搜索到。多个标签之间用逗号(,)间隔开。</p>
<p>群组的所有信息在创建后都可以随时更改。</p>
</div>
<br />
<p style="text-align:right;"><a href="/group/">返回群组首页</a></p>
</div>
<div class="right">
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/ul.gif" alt="" style="float:left;" /><img src="/images/box/ur.gif" alt="" style="float:right;" />');
if(isIE) document.write("<br />");
</script>
<h3 style="padding-left:20px; color:#267BB5;">群组基本信息</h3>
<div id="notediv" style="margin:10px 20px;"></div>
<table id="tb1" cellspacing="8" style="margin:0px 10px;">
<tr><th>群组名称：</th><td><input id="txtName" type="text" class="put" style="width:200px;" maxlength="50" /><span></span></td></tr>
<tr><th>群组类别：</th><td><select id="selCat" class="put"></select><span></span></td></tr>
<tr><th>群组简介：</th><td><textarea id="txtJianjie" rows="4" class="put" style="width:300px;"></textarea><span></span></td></tr>
<tr><th>群组标签：</th><td><input id="txtTags" type="text" class="put" style="width:300px;" maxlength="50" /><span></span></td></tr>
<tr><th style="vertical-align:top; padding-top:6px;">验证码：</th><td><div class="capt_box"><table cellpadding="0"><tr><td style="border-right:solid 1px #999;"><input id="txtCaptcha" type="text" maxlength="6" /></td>
<td><img id="imgCaptcha" runat="server" src="/do/captcha.aspx" /></td></tr></table></div><div id="capt_n"><span></span>&nbsp;</div></td></tr>
<tr><th></th><td style="padding-top:10px;"><a class="btnb" href="javascript:void(0);" onclick="javascript:addGroup();return false;">创 建 群 组</a></td></tr>
</table>
<div id="tb2" style="text-align:center; display:none;">
<p><img id="gimg" src="/upload/group/nophoto.jpg" /></p>
<br />
<table align="center" cellpadding="0" cellspacing="0"><tr><td><iframe id="photo_frm" name="photo_frm" src="about:blank" frameborder="0" scrolling="no" style="width:256px; height:30px;"></iframe></td>
<td style="vertical-align:top;"><input type="button" value="上 传" onclick="uploadImg(this);" /></td></tr></table>
<p>图片标准大小为 120 x 120 像素</p>
</div>
<br />
<script type="text/javascript">
if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;" /><img src="/images/box/br.gif" alt="" style="float:right;" />');
</script>
</div>
<div class="clear"></div>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>

