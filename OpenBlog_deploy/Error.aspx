<%@ page language="C#" autoeventwireup="true" inherits="Error, App_Web_sflcdod5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <h1><% Response.Write(Request.RawUrl); %></h1>
    <h1><% Response.Write(Request.FilePath); %></h1>
    </form>
</body>
</html>
