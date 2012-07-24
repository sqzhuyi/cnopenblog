function resetHeight()
{
    var h1 = el("leftdiv").offsetHeight;
    if(h1-el("rightdiv").offsetHeight>0){
        el("rightdiv").style.height = h1+"px";
    }
}
window.onload = function(){
    resetHeight();
    if(el("txtMsg")){
        el("txtMsg").onfocus = resetSize;
        el("txtMsg").onblur = function(){clearInterval(inter);};
    }
};
var inter;
function resetSize()
{
    inter = setInterval(function(){
        var e = el("txtMsg");
        if(e.value.length>240) e.value=e.value.substr(0,240);
        el("sp_num").innerHTML = e.value.length;
    },200);
}
var doing = false;
function sendMsg(e)
{
    if(doing) return;

    el("txtMsg").className = "put";
    if(!el("txtMsg").value.Trim()){
        el("txtMsg").className = "put erput";
        el("txtMsg").focus();
        return;
    }
    if(document.cookie.indexOf("sendshortblog")!=-1){
        el("sp_note").innerHTML = "* 不要连续发布。";
        return;
    }
    el("sp_note").innerHTML = loading;
    
    var url = execURL +"?sendshortblog=1";
    var data = "_msg="+ URLencode(el("txtMsg").value.Trim());
    
    doing = true;
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            el("sp_note").style.color = "green";
            el("sp_note").innerHTML = "发布成功。";
            el("txtMsg").value = "";
            setTimeout(function(){window.location.reload();},1000)
        }
    };
    req.send(data);
}

var arr_reply = new Array();
var r_div;
var doing = false;
function replyItem(e, itemid)
{
    if(doing) return;
    cancelReply();
    
    var d = e.parentNode.parentNode;
    d.style.backgroundColor = "#dddddd";
    r_div = document.createElement("div");
    r_div.id = "r_div";
    if(arr_reply[itemid]){
        var s = arr_reply[itemid];
        r_div.innerHTML = s;
        d.appendChild(r_div);
        el("txtReply").focus();
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
            re += drawReplyBox(itemid);
            arr_reply[itemid] = re;
            r_div.innerHTML = re;
            d.appendChild(r_div);
            el("txtReply").focus();
            doing = false;
        }
    };
    req.send(null);
}
function drawReplyBox(itemid)
{
    var s = "<p><input id='txtReply' type='text' class='put' style='width:300px;' maxlenght='200' />";
    s += " <input type='button' value='回复' onclick='doReply("+itemid+")' /> <input type='button' value='取消' onclick='cancelReply()' /></p>";
    return s;
}

function doReply(itemid)
{
    if(!el("txtReply").value.Trim()){
        el("txtReply").className = "put erput";
        el("txtReply").focus();
        return;
    }
    var con = el("txtReply").value.Trim();
    var sp = el("sp_r_"+itemid);
    sp.innerHTML = parseInt(sp.innerHTML)+1;
    var un = el("login_a").innerHTML;
    var d = document.createElement("div");
    d.className = "item2";
    var s = "";
    s += "<div class='col1'><a href='/"+un+ext+"'><img src='/upload/photo/"+un+"-s.jpg' onerror='this.src=\"/upload/photo/nophoto-s.jpg\";' /></a></div>";
    s += "<div class='rcol2'><a href='/"+un+ext+"'>"+un+"</a>";
    s += "<p>"+con.Replace("<","&lt;").Replace("&gt;",">")+"</p>";
    s += "<span class='em'>刚刚</span></div>";
    s += "<div class='clear'></div>";
    d.innerHTML = s;
    r_div.insertBefore(d, el("rp_line"));
    el("txtReply").parentNode.innerHTML = "";
    arr_reply[itemid] = r_div.innerHTML;
    
    setTimeout("cancelReply()",2000);
    
    var url = execURL +"?replyshortblog="+itemid;
    var data = "_msg="+ URLencode(con);
    ajaxPost(url, data);
}
function cancelReply()
{
    if(r_div){
        var p_div = r_div.parentNode;
        if(p_div.className=="item big") p_div.style.backgroundColor = "#f0f0f0";
        else p_div.style.backgroundColor = "#ffffff";
        p_div.removeChild(r_div);
        r_div = null;
    }
}

