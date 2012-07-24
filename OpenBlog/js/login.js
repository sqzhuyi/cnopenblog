var _init = function(){
    if((window.location+'').indexOf("er=1")!=-1){
        el("notediv").className = "box erbox";
        el("notediv").innerHTML = "用户名/密码不正确";
    }
    el("txtUsername").focus();
};
window.onload = _init;

var doing = false;
function login()
{
    if(doing) return;
    
    el("notediv").className = "box erbox";
    el("txtUsername").className = "put";
    el("txtPwd").className = "put";
    
    if(!el("txtUsername").value.Trim()){
        el("notediv").innerHTML = "请输入用户名";
        el("txtUsername").className = "put erput";
        return;
    }
    if(!el("txtPwd").value.Trim()){
        el("notediv").innerHTML = "请输入密码";
        el("txtPwd").className = "put erput";
        return;
    }
    el("notediv").className = "";
    el("notediv").innerHTML = loading;
    doing = true;
    var url = execURL +"?login=1&uname="+ escape(el("txtUsername").value) +"&pwd="+ URLencode(el("txtPwd").value);
    if(el("chkimg").src.indexOf("_off.gif")==-1) url += "&remember=1";
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4 || req.readyState=="complete"){
            var re = req.responseText;
            if(re=="false"){
                doing = false;
                el("notediv").className = "box erbox";
                el("notediv").innerHTML = "用户名/密码不正确";
            }else{
                var to = el("hd_redirect").value;
                if(!to||to=="me.aspx") to = el("txtUsername").value+ext;
                window.location = to;
            }
        }
    };
    req.send(null);
}

function checkRemember()
{
    var e = el("chkimg");
    if(e.src.indexOf("_off.gif")!=-1){
        e.src = "/images/icon_check.gif";
    }else{
        e.src = "/images/icon_check_off.gif";
    }
}