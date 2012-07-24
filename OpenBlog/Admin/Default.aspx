<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="holdHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="holdBody" Runat="Server">
<h3>在线用户数：<%=DLL.UserSetting.Online %></h3>
</asp:Content>

