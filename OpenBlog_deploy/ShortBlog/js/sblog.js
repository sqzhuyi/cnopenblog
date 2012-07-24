function resetHeight()
{
    var h1 = el("leftdiv").offsetHeight;
    if(h1-el("rightdiv").offsetHeight>0){
        el("rightdiv").style.height = h1+"px";
    }
}

window.onload = function(){
    resetHeight();
    el("txtMsg").onfocus = resetSize;
    el("txtMsg").onblur = function(){clearInterval(inter);};
};
var doing = false;
function sendMsg(e)
{
    if(doing) return;
    if(!el("login_a")){
        alert("登录之后才能回复。");
        return;
    }
    el("txtMsg").className = "put";
    if(!el("txtMsg").value.Trim()){
        el("txtMsg").className = "put erput";
        el("txtMsg").focus();
        return;
    }
    el("sp_note2").innerHTML = loading;
    
    var url = execURL +"?replyshortblog="+el("hd_id").value;
    var data = "_msg="+ URLencode(el("txtMsg").value.Trim());
    
    doing = true;
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            el("sp_note2").innerHTML = "回复已提交。";
            el("txtMsg").value = "";
            setTimeout(function(){window.location.reload();},1000)
        }
    };
    req.send(data);
}
var inter;
function resetSize()
{
    inter = setInterval(function(){
        var e = el("txtMsg");
        if(e.value.length>240) e.value=e.value.substr(0,240);
        el("sp_num").innerHTML = e.value.length;
    },200);
}

