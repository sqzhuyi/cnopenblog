window.onload = function(){
    getBlog(2);
    
    el("txtMsg").onfocus = resetSize;
    el("txtMsg").onblur = function(){clearInterval(inter);};
};
function showblog(e,i)
{
    var ar = els("bardiv","a");
    for(var a in ar){
        ar[a].className = "";
    }
    e.className = "curr";
    e.blur();
    getBlog(i);
}
var arr_data = new Array();
var doing = false;
function getBlog(i, p)
{
    if(doing) return;
    
    if(!p) p = 1;
    el("bloglist_div").innerHTML = loading;
    if(arr_data[i+"-"+p]){
        el("bloglist_div").innerHTML = arr_data[i+"-"+p];
        return;
    }
    var url = dataURL +"?getshortblog="+ i +"&p="+ p+"&r="+Math.random();
    doing = true;
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            arr_data[i+"-"+p] = re;
            el("bloglist_div").innerHTML = re;
            doing = false;
        }
    };
    req.send(null);
}

function doPager(k, p)
{
    getBlog(k, p);
}

function sendMsg()
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
    el("txtMsg").value = "";
    doing = true;
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            el("sp_note").style.color = "green";
            el("sp_note").innerHTML = "发布成功。";
            arr_data["1-1"] = null;
            doing = false;
            showblog(els("bardiv","a")[0],1);
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

var arr_reply = new Array();
var r_div;
function replyItem(e, itemid)
{
    if(doing) return;
    cancelReply();
    
    var d = e.parentNode.parentNode.parentNode;
    d.style.backgroundColor = "#dddddd";
    r_div = document.createElement("div");
    r_div.id = "r_div";
    if(!arr_reply[itemid] && el("sp_r_"+itemid).innerHTML=="0"){
         var s = "<p id='rp_line'></p>";
         s += drawReplyBox(itemid);
         arr_reply[itemid] = s;
    }
    if(arr_reply[itemid]){
        var s = arr_reply[itemid];
        r_div.innerHTML = s;
        d.appendChild(r_div);
        if(el("txtReply")) el("txtReply").focus();
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
            if(el("txtReply")) el("txtReply").focus();
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
        r_div.parentNode.style.backgroundColor = "#ffffff";
        r_div.parentNode.removeChild(r_div);
        r_div = null;
    }
}