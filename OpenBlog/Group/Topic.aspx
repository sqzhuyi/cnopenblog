<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="Topic.aspx.cs" Inherits="Group_Topic" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="holderHead" Runat="Server">
<meta name="keywords" content="<%=title %>,博客,文章,群组,cnOpenBlog" />
<link type="text/css" rel="stylesheet" href="/group/css/topic.css" />
<link type="text/css" rel="stylesheet" href="/css/shield.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holderBody" Runat="Server">
<asp:Literal ID="lblData" runat="server" EnableViewState="false"></asp:Literal><div id="end_div"></div>
<a name="reply"></a>
<div class="item radius">
<div class="left">
<p>请您对您的言行负责，并遵守中华人民共和国有关法律、法规,尊重网上道德。</p>
</div>
<div class="right">
<iframe name="editor_frm" src="/htmleditor/editor.html" frameborder="0" scrolling="no" style="width:740px;height:320px;"></iframe>
<div style="margin:10px;">
<a class="btna" href="javascript:void(0);" onclick="javascript:submitReply(this);return false;">提交回复</a><span id="sp_note"></span>
</div>
<br />
</div>
<div class="clear"></div>
</div>
<input id="hd_group" type="hidden" value="<%=groupID %>" />
<input id="hd_topic" type="hidden" value="<%=topicID %>" />
<div id="hui_box_div" style="display:none;" onmouseover="isover=true;" onmouseout="outUser(this);">
<div class="hui_box"><div class="hui_box_outer"><div class="hui_box_inner">
<p id="hui_box_span"></p>
</div></div></div>
</div>
<div id="divh"></div>
<script type="text/javascript" src="/js/shield.js"></script>
<script type="text/javascript" src="/group/js/topic.js"></script>
<script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>

</asp:Content>

