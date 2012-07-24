function addMsg(e)
{if(document.cookie.indexOf("addedmsg")!=-1){alert("你已经提交过留言，不能频繁提交。");return;}
if(!el("txtMsg").value.Trim())return;var author=el("author_img").alt;var msg=el("txtMsg").value.Trim();if(msg.length>200)msg=msg.substring(0,200);var url=execURL+"?addmsg=1&_to="+escape(author);var data="msg="+URLencode(msg);ajaxPost(url,data);e.disabled=true;setTimeout(function(){alert("留言已经成功提交");el("txtMsg").value="";},1000);}
function addFavorite(id)
{window.open("/do/addfavorite.aspx?blog="+id,"_blank","width=480px,height=320px");}
function addFriend(e,name)
{if(!el("login_a")){window.location="/login.aspx";return;}
var url=execURL+"?addfriend=1&name="+escape(name);ajaxGet(url);e.style.display="none";alert("好友已添加。");}
function resetHeight()
{var h=el("leftdiv").offsetHeight;if(h>el("rightdiv").offsetHeight)
el("rightdiv").style.height=h+"px";}
window.onload=function(){resetHeight();el("bodytop").innerHTML="<h1 class='pagetitle'>"+el("hd_title").value+"</h1><h5 class='pagesubtitle'>"+el("hd_subtitle").value+"</h5>";};