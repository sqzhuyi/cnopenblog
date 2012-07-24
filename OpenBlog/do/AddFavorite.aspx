<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFavorite.aspx.cs" Inherits="do_AddFavorite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>添加新的网摘</title>
    <link type="text/css" rel="stylesheet" href="/css/other.css" />
    <script type="text/javascript" src="/js/public.js"></script>
    <script type="text/javascript" src="/js/addfavorite.js"></script>
    <style type="text/css">
    .allbox { margin:20px 30px; padding:20px; padding-top:10px; background-color:#ffffff; border:solid 1px #fff; -moz-border-radius: 5px;-webkit-border-radius:5px; }
    #sp_note { padding-left: 20px; }
    a.myfavorite { float:right; margin-top:-20px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="add_div" class="allbox">
        <p>文章标题：<br /><input id="txtTitle" type="text" class="put" style="width:360px;" readonly="readonly" /></p>
        <p>文章链接：<br /><input id="txtUrl" type="text" class="put" style="width:360px;" readonly="readonly" /></p>
        <br />
        <p>网摘类别：<select id="selCat" runat="server"></select> &nbsp; <a class="add_blue" href="#" onclick="javascript:addCat();return false;">新建分类</a></p>
        <p id="p_addcat" style="display:none;"><input id="txtnewCat" type="text" class="put" style="width:160px;" maxlength="30" /></p>
        <br /><br />
        <p><a class="btns" href="#" onclick="javascript:addit();return false;">添 加</a><span id="sp_note"></span>
        <a class="myfavorite" href="#" onclick="javascript:opener.location='/favorite.aspx';return false;">查看我的收藏夹(父窗口)</a></p>
        </div>
        <div id="log_div" class="allbox" style="display:none;padding:20px 50px 30px 50px;">
        <h3>请先登录</h3>
        <div id="note_div2" style="margin:10px 0px;"></div>
        <p>用户名：<span style="padding-left:10px"></span><input id="txtUsername" type="text" class="put" style="width:200px;" maxlength="20" /></p>
        <p>密&nbsp;&nbsp;&nbsp;&nbsp;码：<span style="padding-left:10px"></span><input id="txtPwd" type="password" class="put" style="width:200px;" maxlength="20" /></p>
        <p style="padding-left:58px;"><label><input id="chkRemember" type="checkbox" checked="checked" /> 记住我</label><span style="padding-left:20px;"><a href="#" onclick="javascript:changediv(3);return false;">忘记密码?</a></span></p>
        <br />
        <p style="padding-left:60px;"><a class="btns" href="#" onclick="javascript:login();return false;">登 录</a></p>
        </div>
        <div id="pwd_div" class="allbox" style="display:none;padding:20px 50px 30px 50px;">
        <h3>找回密码</h3>
        <br />
        <p>用户名：<span style="padding-left:10px"></span><input id="txtName" type="text" class="put" style="width:200px;" maxlength="20" /></p>
        <p>邮&nbsp;&nbsp;&nbsp;&nbsp;箱：<span style="padding-left:10px"></span><input id="txtEmail" type="text" class="put" style="width:200px;" maxlength="50" /></p>
        <br /><br />
        <p style="padding-left:60px;"><a class="btns" href="#" onclick="javascript:setpwd();return false;">确 定</a><span style="padding-left:20px;"><a href="#" onclick="javascript:changediv(2);return false;">返回</a></span></p>
        </div>
    </form>
    <script type="text/javascript">document.write(decodeURI('%3Cscript%20type=%22text/javascript%22%20src=%22/js/all.js%22%3E%3C/script%3E'));</script>
    <script type="text/javascript"><%=script %></script>
</body>
</html>
