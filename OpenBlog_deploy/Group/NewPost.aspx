<%@ page language="C#" masterpagefile="~/MasterPage1.master" autoeventwireup="true" inherits="Group_NewPost, App_Web_lizp4a2f" title="发起新话题 - cnOpenBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<link type="text/css" rel="stylesheet" href="/css/shield.css" />
<script type="text/javascript" src="/js/shield.js"></script>
<script type="text/javascript" src="/group/js/newpost.js"></script>
<style type="text/css">
.bodymiddle { padding-top:10px; }
.left { float:left; width:160px; background-color:#f0f0f0; height:460px; }
.right { float:right; width:740px; }
.title { font-size:13px; font-weight:bold; color:#666666;}
.btna { float:left; display:block; width:115px; margin-right:10px; background:url(/images/r-btn.gif) no-repeat; padding:9px 0px; text-align:center; font-size:13px; font-weight:bold; color:#2d6f92; text-decoration:none;}
.btna:hover { background-image:url(/images/r-btn-2.gif); color:#000000; text-decoration:none;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<p><a href="/">首页</a> &gt;&gt; <a href="/group/?all">群组</a> &gt;&gt; <a href="/group/<%=groupID.ToString()+DLL.Settings.Ext %>"><%=DLL.Tools.HtmlEncode(groupName) %></a> &gt;&gt; 发表新帖</p>
<br />
<div id="leftdiv" class="left">
<div style="padding:10px; line-height:150%;">请您对您的言行负责，并遵守中华人民共和国有关法律、法规,尊重网上道德。</div>
</div>
<div id="rightdiv" class="right">
<div id="note_div" style="width:664px;"></div>
<span class="title">帖子标题<span style="font-size:11px; color:#999999; font-weight:normal;">（不超过50个字符）</span></span>
<p><input id="txtSubject" type="text" class="put" maxlength="50" style="width:700px;" /></p>
<p class="title">帖子内容</p>
<div><iframe name="editor_frm" src="/htmleditor/editor.html" frameborder="0" scrolling="no" style="width:710px;height:320px;"></iframe></div>
<div style="margin-top:20px;">
<a class="btna" href="javascript:void(0);" onclick="javascript:publish(this);return false;">发表帖子</a>
<a class="btna" href="javascript:void(0);" onclick="javascript:cancel();return false;">取 消</a>
</div>
</div>
<div class="clear"></div>
<div id="divh"></div>
<input id="hd_group" type="hidden" value="<%=groupID %>" />
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
</asp:Content>

