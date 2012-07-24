function addMsg(e)
{
    if(document.cookie.indexOf("addedmsg")!=-1){
        alert("你已经提交过留言，不能频繁提交。");
        return;
    }
    if(!el("txtMsg").value.Trim()) return;
    var author = el("author_img").alt;
    var msg = el("txtMsg").value.Trim();
    if(msg.length>200) msg = msg.substring(0,200);
    var url = execURL +"?addmsg=1&_to="+ escape(author);
    var data = "msg="+ URLencode(msg);
    
    ajaxPost(url, data);
    
    e.disabled = true;
    setTimeout(function(){
        alert("留言已经成功提交");
        el("txtMsg").value = "";
    },1000);
}
function addFavorite(id)
{
    window.open("/do/addfavorite.aspx?blog="+id,"_blank","width=480px,height=320px");
}
function addFriend(e,name)
{
    if(!el("login_a")){
        window.location = "/login.aspx";
        return;
    }
    var url = execURL +"?addfriend=1&name="+escape(name);
    ajaxGet(url);
    e.style.display = "none";
    alert("好友已添加。");
}
function resetHeight()
{
    var h = el("leftdiv").offsetHeight;
    if(h>el("rightdiv").offsetHeight)
        el("rightdiv").style.height = h+"px";
}
window.onload = function(){

    resetHeight();
};
setTimeout(function(){if(el("login_a"))showimpop();},1000);
function showimpop(){var req=getAjax();req.open("GET","/ajax/other.ashx?showmsg=1&r="+Math.random(),true);req.onreadystatechange=function(){if(req.readyState==4){var re=req.responseText;eval(re);notice();}};req.send(null);setTimeout("showimpop()",30000);}
function notice(){var d=el("smsp");if(!d.innerHTML)return;var tit=document.title;var count=0;var inter=setInterval(function(){if(count%2==0){document.title="您有新的消息 - cnOpenBlog";d.style.display="none";}else{document.title=tit;d.style.display="";}if(count++>6)clearInterval(inter);},500);}

