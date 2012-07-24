<%@ page language="C#" autoeventwireup="true" inherits="do_EditUserCat, App_Web_ikkfl44k" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <style type="text/css">
    * { font-family:Arial, Verdana, 宋体; font-size:12px; }
    body { margin:0px;padding:0px;background-color:#ffffff;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="uc_name" >
                    <ItemStyle Width="240px" />
                </asp:BoundField>
                <asp:CommandField ShowEditButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
            <AlternatingRowStyle BackColor="#f9f9f9" />
        </asp:GridView>
        <p>新建分类 <input id="txtCat" type="text" runat="server" style="width:120px;" maxlength="50" /> <input id="btnAdd" type="button" runat="server" value="添加" onserverclick="btnAdd_ServerClick" /></p>
        <input id="hd_cat" type="hidden" runat="server" />
    </form>
    <script type="text/javascript">
    window.onload = function(){
        var ar = document.getElementById("GridView1").getElementsByTagName("a");
        ar[0].style.display = "none";
        ar[1].style.display = "none";
        for(var i=2;i<ar.length;i++){
            if(ar[i].innerHTML=="删除"){
                ar[i].onclick = function(){return confirm("删除后该类文章将转移到\"未分类\"，继续吗？");};
            }
        }
    };
    </script>
</body>
</html>
