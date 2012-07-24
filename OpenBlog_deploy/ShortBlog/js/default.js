function checkRemember(e)
{
    e.className = e.className=="chkRe"?"chkRe nochk":"chkRe";
}
function login()
{
    el("txtName").className = "put";
    el("txtPassword").className = "put";
    el("txtName").nextSibling.className = "";
    el("txtPassword").nextSibling.className = "";
    
    if(!el("txtName").value.Trim()){
        el("txtName").className = "put erput";
        el("txtName").nextSibling.className = "ersp";
        el("txtName").focus();
        return;
    }
    if(!el("txtPassword").value.Trim()){
        el("txtPassword").className = "put erput";
        el("txtPassword").nextSibling.className = "ersp";
        el("txtPassword").focus();
        return;
    }
    var url = "/login.aspx?n="+escape(el("txtName").value)+"&p="+escape(el("txtPassword").value);
    if(el("chkRe").className=="chkRe") url += "&r=1";
    url += "&from=null";
    window.location = url;
}
var arr_reply = new Array();
var r_div;
var doing = false;
function replyItem(e, itemid)
{
    if(doing) return;
    cancelReply();
    
    var d = e.parentNode.parentNode.parentNode;
    d.style.backgroundColor = "#dddddd";
    r_div = document.createElement("div");
    r_div.id = "r_div";
    if(!arr_reply[itemid] && el("sp_r_"+itemid).innerHTML=="0"){
        arr_reply[itemid] = "<p id='rp_line'></p>";
    }
    if(arr_reply[itemid]){
        var s = arr_reply[itemid];
        r_div.innerHTML = s;
        d.appendChild(r_div);
        return;
    }
    var url = dataURL +"?getshortblogreply=1&sb_id="+itemid;
    r_div.innerHTML = loading;
    doing = true;
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            arr_reply[itemid] = re;
            r_div.innerHTML = re;
            d.appendChild(r_div);
            doing = false;
        }
    };
    req.send(null);
}
function cancelReply()
{
    if(r_div){
        r_div.parentNode.style.backgroundColor = "#ffffff";
        r_div.parentNode.removeChild(r_div);
        r_div = null;
    }
}

