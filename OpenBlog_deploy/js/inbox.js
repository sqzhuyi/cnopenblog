var _init_ = function(){
    var divs = els("msglist", "div");
    for(var i=0;i<divs.length;i++){
        if(divs[i].className.split(' ')[0]=="msgbox"){
            divs[i].onmouseover = function(){
                this.style.backgroundColor = "#ffffcc";
                var ar = els(this, "a");
                for(var a=0;a<ar.length;a++){
                    if(ar[a].className=="delmsg"){
                        ar[a].style.visibility = "visible";
                    }
                }
            };
            divs[i].onmouseout = function(){
                this.style.backgroundColor = "#ffffff";
                var ar = els(this, "a");
                for(var a=0;a<ar.length;a++){
                    if(ar[a].className=="delmsg"){
                        ar[a].style.visibility = "hidden";
                    }
                }
            };
        }
    }
};
addLoad(_init_);

function deletemsg(e,id)
{
    removeRow(e.parentNode.parentNode.parentNode);
    var url = execURL +"?deletemsg="+ id;
    ajaxGet(url);
}
var reply_div;
function replymsg(e,id)
{
    if(reply_div) reply_div.parentNode.removeChild(reply_div);
    reply_div = document.createElement("div");
    reply_div.id = id;
    var s = [];
    s.push("<textarea rows='3' style='width:80%;' class='put'></textarea>");
    s.push("<p><input type='button' onclick='submitmsg(this);' value='回 复'></input>");
    s.push("<input type='button' onclick='cancelmsg();' value='取 消'></input></p>");
    reply_div.innerHTML = s.join("");
    e.parentNode.parentNode.appendChild(reply_div);
}
function submitmsg(e)
{
    if(!reply_div) return;
    var msg = e.parentNode.previousSibling.value.Trim();
    if(!msg) return;
    msg = msg.Replace("\r", "").Replace("\n", " ");
    var url = execURL +"?replymsg="+ reply_div.id;
    var data = "msg="+ URLencode(msg);
    ajaxPost(url, data);
    var a = els(reply_div.parentNode,"a")[1];
    if(!a.innerHTML) a.innerHTML = "（2）";
    else a.innerHTML = "（"+(parseInt(a.innerHTML.substring(1))+1)+"）";
    reply_div.innerHTML = "<p style='color:green'>回复已提交</p>";
    setTimeout("cancelmsg()", 2000);
}
function cancelmsg()
{
    if(reply_div){
        reply_div.parentNode.removeChild(reply_div);
        reply_div = null;
    }
}
function sendreply(e,id)
{
    var put = e.parentNode.previousSibling.firstChild;
    var msg = put.value.Trim();
    if(!msg) return;
    put.value = "";
    msg = msg.Replace("\r", "").Replace("\n", " ");
    var url = execURL +"?replymsg="+ id;
    var data = "msg="+ URLencode(msg);
    ajaxPost(url, data);
    
    var s = [];
    s.push("<p><a href='/"+el("head_img").alt+".ashx'>"+el("head_img").alt+"</a> 于 "+ getDatestr() +"</p>");
    s.push("<p style='padding-left:10px;'>"+msg.Replace("<","&lt;").Replace(">","&gt;")+"</p>");
    s.push("<p> </p>");
    
    var box = e.parentNode.previousSibling.previousSibling;
    box.innerHTML += s.join("");
}

function applyOk(e, msgid, gid, uname)
{
    var url = execURL +"?applyok="+msgid+"&gid="+gid+"&uname="+escape(uname);
    ajaxGet(url);
    setTimeout(function(){e.parentNode.style.color="green";e.parentNode.innerHTML="已批准";},500);
}

function applyNo(e, msgid, gid, uname)
{
    var url = execURL +"?applyno="+msgid+"&gid="+gid+"&uname="+escape(uname);
    ajaxGet(url);
    setTimeout(function(){e.parentNode.style.color="red";e.parentNode.innerHTML="已拒绝";},500);
}