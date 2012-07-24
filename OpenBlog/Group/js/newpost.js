var _init = function(){
    hideCode();
    if(el("hd_topic")) setV();
};

window.onload = _init;

function setV()
{
    el("txtSubject").value = el("hd_title").value;
    setTimeout(function(){
        var f = window.frames["editor_frm"].frames["HtmlEditor_frm"];
        f.document.body.innerHTML = el("hd_content").value;
    },1000);
}

function insertCode()
{
    alert("目前帖子内容不支持插入代码");
}
function hideCode()
{
    var f = window.frames["editor_frm"];
    f.document.getElementById("tabtb").rows[0].cells[20].style.display = "none";
}
function insertImage()
{
    shideBody();
    
    var sbody = "<p style='padding-left:20px;font-size:12px;'>请选择您要上传的图片</p>";
    sbody += "<p style='padding-left:20px;'><iframe id='uploadimg_frm' name='uploadimg_frm' src='/ajax/uploadfile.aspx?img=1&size=40' frameborder='0' scrolling='no' style='width:360px; height:30px;'></iframe></p>";
    sbody += "<p style='padding-left:20px;font-size:12px;'>或输入图片的URL</p>";
    sbody += "<p style='padding-left:20px;'><input id='txtImgurl' type='text' style='width:350px;' value='http://' /></p>";
    sbody += "<p>&nbsp;</p>";
    var sbottom = "<input type='button' value='确定' onclick='saveImage(this)' /> <input type='button' value='取消' onclick='cancelShide()' />";
    var s = showdiv2();
    s = s.replace("#title#", "插入图片");
    s = s.replace("#body#", sbody);
    s = s.replace("#bottom#", sbottom);
    el("divh").innerHTML = s;
}
function saveImage(e)
{
    var f = window.frames[1];
    if(f.document.getElementById("file1").value){
        f.document.forms[0].submit();
    }else{
        var src = el("txtImgurl").value;
        if(!src.Replace("http://","")) return;
        loat(src);
    }
}
function loat(src)
{
    var f = window.frames["editor_frm"].frames["HtmlEditor_frm"];
    f.focus();
	f.document.execCommand("InsertImage", false, src);
	f.document.body.innerHTML += " ";
	f.focus();
    
    cancelShide();
}
var doing = false;
function publish(e)
{
    if(doing) return;
    var title = el("txtSubject").value.Trim();
    var body = getBody();
    el("txtSubject").className = "put";
    el("note_div").className = "box erbox";
    el("note_div").style.marginBottom = "10px";
    if(!title){
        el("note_div").innerHTML = "帖子标题不能为空";
        el("txtSubject").className = "put erput";
        el("txtSubject").focus();
        return false;
    }
    if(body.Trim().length<10){
        el("note_div").innerHTML = "帖子内容太少";
        return false;
    }
    doing = true;
    el("note_div").className = "";
    el("note_div").innerHTML = loading +" 正在提交...";
    e.blur();
    if(body.length>4000) body = body.substring(0,4000);
    var url = execURL;
    if(el("hd_topic")) url += "?editpost=1&topic="+ el("hd_topic").value;
    else url += "?newpost=1&group="+ el("hd_group").value;
    var data = "title="+ URLencode(title) +"&body="+ URLencode(body);
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            if(isNaN(re)){
                doing = false;
                el("note_div").className = "box erbox";
                el("note_div").style.marginBottom = "10px";
                el("note_div").innerHTML = "提交失败！";
                window.location.reload();
            }else{
                window.location = "/group/topic/"+re+ext;
            }
        }
    };
    req.send(data);
    
}

function cancel()
{
    if(el("hd_topic")) window.location = "/group/topic/"+el("hd_topic").value+ext;
    else window.history.go(-1);
}

function getBody()
{
    var win = window.frames["editor_frm"];
    var ishtml = win.document.getElementById("switchMode_id").checked;
    var valu = "";
    if(ishtml) valu = win.document.getElementById("sourceEditor_id").value;
    else valu = win.frames["HtmlEditor_frm"].document.body.innerHTML;
    
    return valu;
}