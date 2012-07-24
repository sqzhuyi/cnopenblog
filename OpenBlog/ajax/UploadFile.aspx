<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="ajax_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>上传图片</title>
    <style type="text/css">
    body {margin:0px;padding:0px; font-family:宋体; background-color:Transparent;}
    </style>
</head>
<body>
    <form id="form1" method="post" action="UploadFile.aspx" enctype="multipart/form-data" onsubmit="if(!checkit())return false;">
    <input id="file1" name="file1" type="file" size="24" />
    <input type="hidden" name="up" value="1" />
    <input type="hidden" name="kind" value="0" />
    <input type="hidden" name="data" value="0" />
    </form>
    <script type="text/javascript">
    <%=script %>
    var u = window.location+'';
    if(u.indexOf("size=")!=-1){
        var size = parseInt(u.substr(u.indexOf("size=")+5).split('&')[0]);
        if(document.all) size += 1;
        document.getElementById("file1").size = size;
    }
    if(u.indexOf("bg=")!=-1){
        var bg = u.split('bg=')[1].split('&')[0];
        document.body.style.backgroundColor = "#"+bg;
    }
    if(u.indexOf("img=1")!=-1){
        document.forms[0].kind.value = "img";
    }
    else if(u.indexOf("group=1")!=-1){
        document.forms[0].kind.value = "group";
        document.forms[0].data.value = u.split('id=')[1].split('&')[0];
    }
    function checkit()
    {
        var exts = ".jpg.jpeg.jpe.tiff.tif.bmp.gif.png.";
        var fn = document.getElementById("file1").value;
        if(!fn){
            alert("请选择您要上传的图片。");
            return false;
        }
        var ext = fn.substr(fn.lastIndexOf("."))+".";
        if(exts.indexOf(ext)==-1){
            alert("图片格式不正确。");
            return false;
        }
        return true;
    }
    </script>
</body>
</html>
