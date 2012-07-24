var inter_len;
function sendMsg(e, n)
{
    shideBody();
    var s = showdiv2();
    s = s.replace("#title#", "给"+n+"发送消息");
    var b = "<p id='msgp'><span style='font-size:12px;'>请输入消息内容：</span><br /><textarea id='txtmsg' class='put' cols="+(isIE?"45":"50")+" rows=4></textarea><br /><span class='em'><span id='sp_len'>0</span>/200个字</span></p>";
    s = s.replace("#body#", b);
    b = "<input type=button value='发送' onclick='sendOk(this,\""+n+"\")' /> &nbsp; <input type=button value='取消' onclick='cancelShide()' />";
    s = s.replace("#bottom#", b);
    el("divh").innerHTML = s;
    el("txtmsg").focus();
    
    inter_len = setInterval(function(){
        if(!el("txtmsg")){
            clearInterval(inter_len);
            return;
        }
        var l = el("txtmsg").value.length;
        if(l>200) el("txtmsg").value = el("txtmsg").value.substr(0,200);
        el("sp_len").innerHTML = l;
    }, 200);
}
function sendOk(e,n)
{
    if(el("txtmsg").value.Trim()==""){
        el("txtmsg").className = "put erput";
        el("txtmsg").focus();
        return;
    }
    clearInterval(inter_len);
    e.disabled = true;
    var url = execURL +"?addmsg=1&_to="+escape(n);
    var data = "msg="+URLencode(el("txtmsg").value.Trim());
    ajaxPost(url, data);
    setTimeout(function(){
        el("msgp").innerHTML = "<span class='success'>消息已发送成功。</span><br /><br />";
    },500);
    setTimeout("cancelShide()", 2000);
}
function deleteFriend(e, n)
{
    if(!confirm("确定要删除好友 "+ n +" 吗？"))return;
    removeRow(e.parentNode.parentNode);
    var url = execURL +"?deletefriend="+escape(n);
    ajaxGet(url);
}

