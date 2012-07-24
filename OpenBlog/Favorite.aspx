<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Favorite.aspx.cs" Inherits="Favorite" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="holderHead" runat="server" EnableViewState="false">
<style type="text/css">
#newcat_div,#delcat_div { background-color:#f0f0f0; padding:4px; }
</style>
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="holderBody" runat="server" EnableViewState="false">

<div id="leftdiv" class="left">
    <h2 class="subtitle">我的网摘</h2>
    <div style="margin-bottom:12px;">网摘类别：<select id="selCat" runat="server" enableviewstate="false" onchange="chgcat(this.value)"><option value="0">所有分类</option></select> &nbsp; <a class="add_blue" href="#" onclick="javascript:el('newcat_div').style.display='';return false;">新建分类</a> &nbsp; <a class="del" href="#" onclick="javascript:if(el('selCat').length<3){alert('\r\n没有可删除的分类');}else{el('delcat_div').style.display='';}return false;">删除分类</a></div>
    <div id="newcat_div" style="display:none;">分类名称：<input id="txtNewcat" type="text" style="width:120px;" maxlength="20" /> <input type="button" value="添 加" onclick="addcat(this)" /> <input type="button" value="取 消" onclick="el('newcat_div').style.display='none';" /></div>
    <div id="delcat_div" style="display:none;">选择要删除的类别：<select id="selCat2" runat="server" enableviewstate="false"></select> <input type="button" value="删 除" onclick="delcat(this)" /> <input type="button" value="取 消" onclick="el('delcat_div').style.display='none';" /></div>
    <div id="bloglist" class="bloglist" style="background-color:Transparent; margin-top:4px;">
    <asp:Literal ID="lblBlogList" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <div id="pagelist" class="pagelist">
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
    <p id="p8"><a href="/friend.aspx">我的好友</a></p>
    <p id="p6"><a class="curr" href="/favorite.aspx">我的网摘</a></p>
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
    <asp:Literal ID="lblFilter" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <script type="text/javascript">
    if(isIE) document.write('<img src="/images/box/bl.gif" alt="" style="float:left;margin-top:-4px;" /><img src="/images/box/br.gif" alt="" style="float:right;margin-top:-4px;" />');
    </script>
    </div>
</div>
<div class="clear"></div>

<script type="text/javascript">
function addcat(e)
{
    var c = el("txtNewcat").value.Trim();
    if(!c) return;
    e.value = "……";
    e.disabled = true;
    var req = getAjax();
    req.open("GET", execURL+"?addcat=1&newcat="+escape(c), true);
    req.onreadystatechange = function(){
        if(req.readyState==4 || req.readyState=="complete"){
            var re = req.responseText;
            el("selCat").options[el("selCat").length] = new Option(c, re);
            e.value = "添 加";
            e.disabled = false;
            el('newcat_div').style.display = 'none';
        }
    };
    req.send(null);
}
function delcat(e)
{
    var id = el("selCat2").value;
    ajaxGet(execURL+"?delcat="+id+"&firstid="+el("selCat").options[1].value);
    
    el("selCat").removeChild(el("selCat").options[el("selCat2").selectedIndex+2]);
    el("selCat2").removeChild(el("selCat2").options[el("selCat2").selectedIndex]);
    el('delcat_div').style.display='none';
}
function chgcat(v)
{
    if(v=="0") window.location = "favorite.aspx";
    else window.location = "favorite.aspx?cat="+ v;
}
function delItem(e, id)
{
    removeRow(e.parentNode);
    ajaxGet(execURL+"?delfavorite="+id);
}
</script>

</asp:Content>