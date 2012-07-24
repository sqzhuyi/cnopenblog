var topicID;
var _init = function(){
    topicID = el("hd_topic").value;
    
    hideCode();
};

window.onload = _init;

function insertCode()
{
    alert("目前帖子内容不支持插入代码");
}
function hideCode()
{
    var f = window.frames["editor_frm"];
    var e = f.document.getElementById("tabtb");
    e.rows[0].cells[20].style.display = "none";
    e.rows[0].cells[22].style.display = "none";
    e.rows[0].cells[23].style.display = "none";
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
function quoteIt(e, i, n)
{
    var s = e.parentNode.previousSibling.innerHTML;
    s = s.split(/<p class=("|')qianming("|')>/i)[0];
    if(s.length>200) s = s.substring(0, 200)+"...";
    s = "<fieldset><legend>引用 #"+(i==0?"楼主":i+"楼")+" "+n+" 的"+(i==0?"内容":"回复")+"</legend>"+s+"</fieldset>"
    var f = window.frames["editor_frm"].frames["HtmlEditor_frm"];
    f.document.body.innerHTML = s +"<br>";
}

function getBody()
{
    var win = window.frames["editor_frm"].frames["HtmlEditor_frm"];
    var valu = win.document.body.innerHTML;
    
    return valu;
}
var times = 0;
function submitReply(e)
{
    if(el("hidetopic_div")){
        el("sp_note").innerHTML = "* 该贴以被管理员屏蔽，不能回复。";
        return;
    }
    if(times>1){
        el("sp_note").innerHTML = "* 只能连续提交两次";
        return;
    }
    var s = getBody();
    if(!s.Trim()){
        el("sp_note").innerHTML = "* 请输入回复内容";
        return;
    }
    times ++;
    el("sp_note").innerHTML = "";
    if(s.length>2000) s = s.substring(0,2000);
    var url = execURL +"?replytopic=1&topic="+topicID;
    var data = "body="+ URLencode(s);
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            if(re.length<100){
                var arr = re.split(',');
                if(arr[0]){
                    var loc = "<a href='/search.aspx?location:"+arr[0]+"'>"+arr[0]+"</a>";
                    if(arr[0]!=arr[1]) loc += "，<a href='/search.aspx?location:"+arr[1]+"'>"+arr[1]+"</a>";
                    el("sp_loca"+times).innerHTML = loc;
                }
            }
        }
    };
    req.send(data);
    
    var u = el("login_a").innerHTML;
    var text = [];
    text.push("<div class='left'><div style='line-height:22px;'>");
    text.push("<a href='/"+u+ext+"'><img class='photo' src='/upload/photo/"+u+".jpg' onerror='this.src=\"/upload/photo/nophoto.jpg\";' /></a>");
    text.push("<a href='#'>加为好友</a>");
    text.push("<br /><a href='#'>发送私信</a>");
    text.push("<br /><a href=''>个人博客</a>");
    text.push("<div class='clear'></div></div>");
    text.push("<p><a class='userlink' href='/"+u+ext+"'>"+u+"</a></p>");
    text.push("<p>城市：<span id='sp_loca"+times+"'></span></p>");
    text.push("</div><div id='righttmp' class='right'>");
    var f = 1;
    var spans = el("end_div").previousSibling.getElementsByTagName("span");
    var t = spans[0].innerHTML;
    if(t=="楼主") f = 1;
    else f = parseInt(t.substring(2, t.length-1))+1;
    text.push("<div class='righttop'>发表于："+getDate()+"<span class='spright'># "+f+"楼</span></div>");
    text.push("<div class='rightmiddle'>");
    text.push("<p>"+ s +"</p>");
    text.push("</div><div class='rightbottom'>");
    text.push("<a class='quote' href='#reply' onclick='javascript:quoteIt(this,"+f+",\""+u+"\");'>引用</a>");
    text.push(" &nbsp; <a class='reply' href='#reply'>回复</a>");
    text.push(" &nbsp; <a href='#top'>TOP</a></div>");
    text.push("</div><div class='clear'></div>");
    
    var div = document.createElement("div");
    div.className = "item radius";
    div.innerHTML = text.join("");
    el("bodymiddle").insertBefore(div, el("end_div"));
    
    el("sp_reply_cnt").innerHTML = parseInt(el("sp_reply_cnt").innerHTML)+1;
    el("righttmp").style["backgroundColor"] = "#ffcccc";
    setTimeout(function(){
        el("righttmp").style["backgroundColor"] = "#ffffff";
        el("righttmp").id = "";},
    500);
    
    var win = window.frames["editor_frm"].frames["HtmlEditor_frm"];
    win.document.body.innerHTML = "";
}
var arr_data = new Array();
var isover = false;
function overUser(e)
{
    isover = true;
    var n = e.childNodes[0].alt;
    
    el("hui_box_div").style.display = "inline";
    el("hui_box_div").style.top = elementPos(e).y+24+"px";
    el("hui_box_span").innerHTML = loading;

    if(arr_data[n]){
        fillData(n,arr_data[n]);
    }else{
        var url = dataURL +"?getuserdata="+escape(n);
        var req = getAjax();
        req.open("GET", url, true);
        req.onreadystatechange = function(){
            if(req.readyState==4){
                var re = req.responseText;
                arr_data[n] = re;
                fillData(n,re);
            }
        };
        req.send(null);
    }
}
function outUser()
{
    isover = false;
    setTimeout(function(){
        if(!isover){
            el("hui_box_span").innerHTML = "";
            el("hui_box_div").style.display = "none";
        }
    },1000);
}
function fillData(n,data)
{
    var arr = data.split('<col>');
    var s = [];
    if(arr[0]=="null"){
        s.push("找不到该用户的信息，");
        s.push("可能已被管理员删除。");
    }else{
        s.push("<span>"+arr[0]+"，"+arr[1]+(arr[2]?"，"+arr[2]:"")+"</span>");
        if(arr[3]) s.push("<span>行业：<a href='/industry/"+arr[3]+ext+"' target=_blank>"+arr[3]+"</a></span>");
        if(arr[4]) s.push("<span>Email：<a href='mailto:"+arr[4]+"'>"+arr[4]+"</a></span>");
        if(arr[5]) s.push("<span>QQ：<a href='http://wpa.qq.com/msgrd?V=1&Uin="+arr[5]+"&Site=cnopenblog.com&Menu=yes' target=_blank>"+arr[5]+"</a></span>");
        if(arr[6]) s.push("<span>MSN：<a href='mailto:"+arr[6]+"'>"+arr[6]+"</a></span>");
        s.push("<span>访问量："+arr[7]+"</span>");
        if(arr[8]) s.push("<span>简介："+arr[8]+"</span>");
        s.push("<span><a href='http://www.cnopenblog/"+n+ext+"'>http://www.cnopenblog/"+n+ext+"</a></span>");
    }
    
    el("hui_box_span").innerHTML = s.join('<br />');
}

function addFriend(e, name)
{
    if(!el("login_a")){
        window.location = "/login.aspx";
        return;
    }
    var url = execURL +"?addfriend=1&name="+escape(name);
    ajaxGet(url);
    alert("好友已添加。");
}
var inter_len;
function sendMessage(e, name)
{
    shideBody();
    var s = showdiv2();
    s = s.replace("#title#", "给"+name+"发送私信");
    var b = "<p id='msgp'><span style='font-size:12px;'>请输入消息内容：</span><br /><textarea id='txtmsg' class='put' cols="+(isIE?"45":"50")+" rows=4></textarea><br /><span class='em'><span id='sp_len'>0</span>/200个字</span></p>";
    s = s.replace("#body#", b);
    b = "<input type=button value='发送' onclick='sendmsg(this,\""+name+"\")' /> &nbsp; <input type=button value='取消' onclick='cancelShide()' />";
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
function sendmsg(e,name)
{
    if(el("txtmsg").value.Trim()==""){
        el("txtmsg").className = "put erput";
        el("txtmsg").focus();
        return;
    }
    clearInterval(inter_len);
    e.disabled = true;
    var url = execURL +"?sendmsg=1&name="+escape(name);
    var data = "message="+URLencode(el("txtmsg").value.Trim());
    ajaxPost(url, data);
    setTimeout(function(){
        el("msgp").innerHTML = "<span class='success'>私信已发送成功。</span><br /><br />";
    },500);
    setTimeout("cancelShide()", 2000);
}

function deleteIt()
{
    if(!confirm("删除操作不可恢复，是否继续？")) return;
    var url = execURL +"?deletetopic="+topicID;
    ajaxGet(url);
    setTimeout(function(){
        alert('帖子已删除。');
        window.location = '/group/'+el("hd_group").value+ext;
    },1000);
}
function editIt()
{
    window.location = "/group/editpost.aspx?q="+topicID;
}
function hideIt(e)
{
    var url = execURL +"?hidetopic="+topicID;
    ajaxGet(url);
    setTimeout(function(){
        if(e.innerHTML=="屏蔽该贴"){
            alert('帖子已屏蔽。');
            e.innerHTML = "撤销屏蔽";
        }else{
            alert('已撤销对帖子的屏蔽。');
            e.innerHTML = "屏蔽该贴";
        }
    },1000);
}
function topIt(e)
{
    var url = execURL +"?toptopic="+topicID;
    ajaxGet(url);
    setTimeout(function(){
        if(e.innerHTML=="帖子置顶"){
            alert('帖子已置顶。');
            e.innerHTML = "撤销置顶";
        }else{
            alert('已撤销对帖子的置顶。');
            e.innerHTML = "帖子置顶";
        }
    },1000);
}
function hideReply(e, id)
{
    var url = execURL +"?hidereply="+id+"&topic="+topicID;
    ajaxGet(url);
    setTimeout(function(){
        if(e.innerHTML=="屏蔽该回复"){
            alert('该回复已被屏蔽。');
            e.innerHTML = "撤销屏蔽";
        }else{
            alert('已撤销对该回复的屏蔽。');
            e.innerHTML = "屏蔽该回复";
        }
    },1000);
}