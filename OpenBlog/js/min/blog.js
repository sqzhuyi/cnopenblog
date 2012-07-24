var ext=".htm";var loading="<img src='/images/loading.gif' alt='loading...' />";String.prototype.Replace=function(oldStr,newStr){if(oldStr.indexOf("(")!=-1||oldStr.indexOf(")")!=-1)return this.split(oldStr).join(newStr);return this.replace(new RegExp(oldStr,"gmi"),newStr);}
String.prototype.Trim=function(){return this.replace(/(^\s*)|(\s*$)/g,"");}
function $(id)
{return document.getElementById(id);}
function el(id)
{return $(id);}
function els(e,tag)
{var arr=null;if(tag){if(typeof e=="string")arr=$(e).getElementsByTagName(tag);else arr=e.getElementsByTagName(tag);}else{arr=document.getElementsByTagName(e);}
return arr;}
function URLencode(str)
{return escape(str).replace(/\+/g,'%2B').replace(/\"/g,'%22').replace(/\'/g,'%27').replace(/\//g,'%2F');}
function getDatestr()
{var d=new Date();var s=d.getFullYear()+"-";var t=(d.getMonth()+1);if(t<10)t="0"+t;s+=t+"-";t=d.getDate();if(t<10)t="0"+t;s+=t+" ";t=d.getHours();if(t<10)t="0"+t;s+=t+":";t=d.getMinutes();if(t<10)t="0"+t;s+=t;return s;}
function getAjax()
{var oHttpReq=null;if(window.ActiveXObject)oHttpReq=new ActiveXObject("MSXML2.XMLHTTP");else if(window.createRequest)oHttpReq=window.createRequest();else oHttpReq=new XMLHttpRequest();return oHttpReq;}
function isEnter(eve)
{var ev=eve||window.event;var keycode;if(window.event)keycode=ev.keyCode;else keycode=ev.which;if(keycode==10||keycode==13)return true;else return false;}
function ajaxGet(url)
{var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){};req.send(null);}
function ajaxPost(url,data)
{var req=getAjax();req.open("POST",url,true);req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");req.onreadystatechange=function(){};req.send(data);}
var dataURL="da8790fdb18a4891a32a3fb17ee24dbb47d4023de873|cnopenblog.com";var execURL="da8790fdb18a4795b92a32a06de24dbb47d40024ca07|cnopenblog.com";function resetHeight()
{var h1=$("leftdiv").offsetHeight;if(h1-$("rightdiv").offsetHeight>0){$("rightdiv").style.height=h1+"px";}}
function resetSignbox()
{var url=dataURL+"?resetsignbox=1&r="+Math.random();var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;if(re!="null"){$("toptabs").innerHTML=re;setCommentAuthor();if($("login_a").innerHTML==author)printEditButton();}}};req.send(null);}
function printEditButton()
{var d=$("star_sp").parentNode.parentNode;d=els(d,"div")[1];var node="<a class='edit' href='/write.aspx?blog="+blogid+"'>修改文章</a> &nbsp; ";d.innerHTML=node+d.innerHTML;}
function setCommentAuthor()
{$("txtName").value=$("login_a").innerHTML;$("txtUrl").value=$("login_a").href;$("txtName").disabled=true;$("txtUrl").disabled=true;}
var rating;var rating_titles=new Array("一般般","还行，值得一看","不错，顶一下","非常好，学习。。。","太棒了！");function set_rating_over()
{var as=els("rating_span","i");for(var i=0;i<as.length;i++)as[i].title=rating_titles[i];$("rating_span").onmouseover=rating_over;$("rating_span").onmouseout=rating_out;$("rating_span").onmousedown=rating_down;}
function rating_over(ev)
{var ev=ev||window.event;var e=ev.target||ev.srcElement;var ind=parseInt(e.innerHTML);var as=els("rating_span","i");for(var i=0;i<as.length;i++){as[i].style["backgroundPosition"]="0px 0px";}
for(var j=0;j<ind;j++){as[j].style["backgroundPosition"]="0px -32px";}}
function rating_out()
{var as=els("rating_span","i");for(var i=0;i<as.length;i++){as[i].style["backgroundPosition"]="0px 0px";}
if(!rating)return;for(var j=0;j<rating;j++){as[j].style["backgroundPosition"]="0px -32px";}}
function rating_down(ev)
{var ev=ev||window.event;var e=ev.target||ev.srcElement;var ind=parseInt(e.innerHTML);rating=ind;}
function quote(e)
{var s="";s+="----引用 "+e.parentNode.childNodes[0].innerHTML+" ";s+=els(e.parentNode,"a")[0].innerHTML+" 的评论--------\n";s+=e.parentNode.nextSibling.innerHTML.Replace("<br>","\n");s+="\n------------------------------------------------\n";var v=$("txtComment").value;if(v)$("txtComment").value+="\n"+s;else $("txtComment").value=s;}
function addFavorite()
{window.open("/do/addfavorite.aspx?blog="+escape(blogid),"_blank","width=480px,height=320px");}
function addMsg(e)
{if(document.cookie.indexOf("addedmsg")!=-1){alert("你已经提交过留言，不能频繁提交。");return;}
if(!$("txtMsg").value.Trim())return;var msg=$("txtMsg").value.Trim();if(msg.length>200)msg=msg.substring(0,200);var url=execURL+"?addmsg=1&_to="+escape(author);var data="msg="+URLencode(msg);ajaxPost(url,data);e.disabled=true;setTimeout(function(){alert("留言已经成功提交");$("txtMsg").value="";},1000);}
function addPost(e)
{if(document.cookie.indexOf("addedcomment")!=-1){alert("你已经发表过评论");return;}
var c_name=$("txtName").value.Trim();var c_url=$("txtUrl").value.Trim();var c_content=$("txtComment").value.Trim();$("txtName").className="put";$("txtUrl").className="put";$("txtComment").className="put";if(!c_name){$("txtName").className="put erput";$("sp_note").innerHTML="<font color=red>* 请输入你的姓名</font>";return false;}
if(!c_content){$("txtComment").className="put erput";$("sp_note").innerHTML="<font color=red>* 请输入评论内容</font>";return false;}
if(!rating){$("sp_note").innerHTML="<font color=red>* 请为文章评分</font>";return false;}
e.disabled=true;e.blur();c_content=c_content.Replace("\r","").Replace("\n","{br}");var div=document.createElement("div");div.className="comment";div.style.backgroundColor="#ffffcc";var s=[];var len=0;var cmts=els("comment_box","div");for(var l=0;l<cmts.length;l++){if(cmts[l].className=="comment")len++;}
var d=getDatestr();s.push('<div class="cleft"><a href="'+c_url+'" target=_blank><img src="/upload/photo/'+c_name+'-s.jpg"');s.push(' onerror="this.src=\'/upload/photo/nophoto-s.jpg\';" /></a></div>');s.push('<div class="cright">');s.push('<div class="chead"><span>#'+(len+1)+'楼</span> ');s.push('<a href="'+c_url+'">'+c_name+'</a> ');s.push('于 <span>'+d+'</span> ');s.push('<a href="javascript:void(0);" onclick="javascript:quote(this);">引用</a></div>');s.push('<div class="cbody">'+c_content.Replace("<","&lt;").Replace("&gt;",">").Replace("{br}","<br>")+'</div>');s.push('</div><div class="clear"></div>');div.innerHTML=s.join('');setTimeout(function(){$("comment_box").appendChild(div);$("txtComment").value="";$("sp_note").innerHTML="<font color=green>评论已经提交成功。</font>";$("sp_comment").innerHTML=parseInt($("sp_comment").innerHTML)+1;},1000);setTimeout(function(){$("sp_note").innerHTML="";div.style.backgroundColor="#ffffff";},5000);var url=execURL+"?addcomment=1";var data="blogid="+escape(blogid);data+="&c_name="+escape(c_name);data+="&c_url="+escape(c_url);data+="&rating="+escape(rating);data+="&c_content="+URLencode(c_content);ajaxPost(url,data);}
function getServerData()
{var url=dataURL+"?getblogdata="+escape(blogid);var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;setData(re);resetHeight();}};req.send(null);}
function setData(data)
{var arr=data.split('$#$');var ar=arr[0].split('|');$("sp_read").innerHTML=ar[0];$("sp_comment").innerHTML=ar[1];$("star_sp").style["paddingLeft"]=16*ar[2]+"px";if(ar[3]=="1"){$("sp_note").previousSibling.disabled=true;$("txtComment").value="作者已禁止对该文章的评论";}
var rows,cols;var s;if(arr[1]){rows=arr[1].split('<row>');s=[];for(var i=0;i<rows.length;i++){cols=rows[i].split('<col>');s.push('<div class="comment">');s.push('<div class="cleft"><a href="'+cols[1]+'" target=_blank><img src="/upload/photo/'+cols[0]+'-s.jpg"');s.push(' onerror="this.src=\'/upload/photo/nophoto-s.jpg\';" /></a></div>');s.push('<div class="cright">');s.push('<div class="chead">');s.push('<span>#'+(i+1)+'楼</span> ');s.push('<a href="'+cols[1]+'" target=_blank>'+cols[0]+'</a> ');s.push('于 <span>'+cols[3]+'</span> ');s.push('<a href="javascript:void(0);" onclick="javascript:quote(this);">引用</a></div>');s.push('<div class="cbody">'+cols[2].Replace("{br}","<br />")+'</div>');s.push('</div><div class="clear"></div></div>');}
$('comment_box').innerHTML=s.join('');}
rows=arr[2].split('<row>');s=[];s.push('<ol class="oln">');for(var i=0;i<rows.length;i++){cols=rows[i].split('<col>');s.push('<li><a href="'+cols[1]+'">'+cols[0]+'</a> - ');s.push('<a href="/'+author+'/'+cols[2]+ext+'" class="hui">'+cols[3]+'</a></li>');}
s.push('</ol>');$('newblog_box').innerHTML=s.join('');rows=arr[3].split('<row>');s=[];s.push('<ul class="uln">');for(var i=0;i<rows.length;i++){cols=rows[i].split('<col>');s.push('<li><a href="/'+author+'/'+cols[1]+ext+'">'+cols[2]+'（'+cols[0]+'）</a> - ');s.push('<a href="/'+author+'/'+cols[1]+'/feed'+ext+'" target=_blank>RSS</a></li>');}
s.push('</ul>');$('category_box').innerHTML=s.join('');cols=arr[4].split('<col>');if(cols[0]!='0'){s="下一篇：<a href='"+cols[1]+"'>"+cols[0]+"</a>";$("sp_next1").innerHTML=$("sp_next2").innerHTML=s;if(els("prev_next_blog","a").length>1){var len=(els("prev_next_blog","a")[0].innerHTML+els("prev_next_blog","a")[1].innerHTML).length;if(len>35){$("sp_br").style.display="";$("sp_br2").style.display="";}}}
if(arr[5]){rows=arr[5].split('<row>');s=[];for(var i=0;i<rows.length;i++){cols=rows[i].split('<col>');if(i%2==0)s.push('<div class="clear"></div>');s.push('<div class="groupitem"><a href="/group/'+cols[0]+ext+'" target=_blank>');s.push('<img src="/upload/group/'+cols[0]+'-s.jpg" onerror="this.src=\'/upload/group/nophoto-s.jpg\';" /></a>');s.push('<p><a href="/group/'+cols[0]+ext+'" target=_blank>'+cols[1]+'</a></p></div>');}
s.push('<div class="clear"></div>');$("group_box").innerHTML=s.join('');}
if(els("prev_next_blog","a").length>0)$("sameclassblog_div").style["display"]="";}
var firstc;function showblogs(e)
{var d=$("blogs_div");if(d.style.display==""){d.style.display="none";return;}
d.style.display="";if(d.innerHTML==""){d.innerHTML="<h4 style='padding-left:10px'>"+loading+"</h4>";d.style.left=e.offsetLeft-d.offsetWidth+60+"px";var _url=dataURL+"?getoneclassblogs="+blogid;var req=getAjax();req.open("GET",_url,true);req.onreadystatechange=function(){if(req.readyState==4){var re=req.responseText;d.innerHTML=re;d.style.left=e.offsetLeft-d.offsetWidth+60+"px";if(d.offsetWidth<320)d.style.width="320px";}};req.send(null);}
firstc=true;document.body.onclick=clickBody;}
function clickBody(ev){var evt=ev||window.event;var e=evt.srcElement||evt.target;while(e.nodeName!="DIV"&&e.parentNode)
e=e.parentNode;if(e.id!="blogs_div"){if(!firstc)$("blogs_div").style.display="none";else firstc=false;}}
function _51La()
{var d=document.createElement("div");d.style.visibility="hidden";d.innerHTML="<iframe src='/help/51la.html' style='1px;height:1px;'></iframe>";document.body.appendChild(d);}
var author,blogid;window.onload=function(){if($("author_img"))author=$("author_img").alt;var u=window.location+"";if(u.split('/').length>5)blogid=u.split('/')[5].split('.')[0];else blogid=10008;set_rating_over();resetSignbox();getServerData();_51La();};