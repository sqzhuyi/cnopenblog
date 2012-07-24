<%@ page language="C#" masterpagefile="~/Admin/MasterPage.master" autoeventwireup="true" inherits="Admin_Default, App_Web_1bxv0wc1" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holdHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holdBody" Runat="Server">
<h3>在线用户数：<%=DLL.UserSetting.Online %></h3>
</asp:Content>

