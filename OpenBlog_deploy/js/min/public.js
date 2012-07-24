var baseURL="http://www.cnopenblog.com/";var ext=".htm";var loading="<img src='/images/loading.gif' alt='loading...' />";String.prototype.Replace=function(oldStr,newStr){if(oldStr.indexOf("(")!=-1||oldStr.indexOf(")")!=-1)return this.split(oldStr).join(newStr);return this.replace(new RegExp(oldStr,"gmi"),newStr);}
String.prototype.Trim=function(c){if(!c||c==' '){return this.replace(/(^\s*)|(\s*$)/g,"");}else{var s=this;var i,b=0,e=s.length;for(i=0;i<s.length;i++)
if(s.charAt(i)!=c){b=i;break;}
if(i==s.length)return"";for(i=s.length-1;i>b;i--)
if(s.charAt(i)!=c){e=i;break;}
return s.substring(b,e+1);}}
function el(id)
{var e=document.getElementById(id);try{e.id;}
catch(err){e=document.getElementById("ctl00_holderBody_"+id);}
return e;}
function els(e,tag)
{var arr=null;if(tag){if(typeof e=="string")arr=el(e).getElementsByTagName(tag);else arr=e.getElementsByTagName(tag);}else{arr=document.getElementsByTagName(e);}
return arr;}
function nextElement(e){e=e.nextSibling;while(e.nodeType!=1)
e=e.nextSibling;return e;}
function isUrl(va)
{var u1="^http\://[a-z0-9\-\.]+\.[a-z]{2,3}(/\S*)?$";var u2="^(http|https|ftp)\://[a-z0-9\-\.]+\.[a-z]{2,3}(:[a-z0-9]*)?/?([a-z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$";var reg_url=new RegExp(u2);return va.Trim().toLowerCase().match(reg_url);}
function isEmail(va)
{var reg=/^((\w+)|(\w+[.\-\w]*[.\w]+))@([\w\-]+[.])+(.+)$/;return reg.test(va);}
function clearSelection()
{if(document.all)document.selection.empty();else window.getSelection().removeAllRanges();}
function getQuery(url,q)
{q=q.toLowerCase();url=url+'';var re="";url=url.substr(url.indexOf("?")+1);if(!url)return re;var qs=url.split("&");for(var i=0;i<qs.length;i++)
{if(qs[i].split('=')[0].toLowerCase()==q)
{if(qs[i].split('=').length>1)re=qs[i].substr(qs[i].indexOf('=')+1);break;}}
return unescape(re);}
function URLencode(str)
{return escape(str).replace(/\+/g,'%2B').replace(/\"/g,'%22').replace(/\'/g,'%27').replace(/\//g,'%2F');}
function getDate()
{var d=new Date();var s=d.getFullYear()+"-";var t=(d.getMonth()+1);if(t<10)t="0"+t;s+=t+"-";t=d.getDate();if(t<10)t="0"+t;s+=t+" ";t=d.getHours();if(t<10)t="0"+t;s+=t+":";t=d.getMinutes();if(t<10)t="0"+t;s+=t;return s;}
function totop()
{document.documentElement.scrollTop=0;}
function getAjax()
{var oHttpReq=null;if(window.ActiveXObject)
oHttpReq=new ActiveXObject("MSXML2.XMLHTTP");else if(window.createRequest)
oHttpReq=window.createRequest();else
oHttpReq=new XMLHttpRequest();return oHttpReq;}
function ajaxGet(url)
{var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){};req.send(null);}
function ajaxPost(url,data)
{var req=getAjax();req.open("POST",url,true);req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");req.onreadystatechange=function(){};req.send(data);}
function isEnter(eve)
{var ev=eve||window.event;var keycode;if(window.event)keycode=ev.keyCode;else keycode=ev.which;if(keycode==10||keycode==13)return true;else return false;}
function mousePos(eve)
{var ev=eve||window.event;if(window.event)
{return{x:event.clientX+document.documentElement.scrollLeft,y:event.clientY+document.documentElement.scrollTop};}
else
{return{x:ev.pageX,y:ev.pageY};}}
function elementPos(e)
{if(!e)e=el(e.id);var left=0;var top=0;while(e.offsetParent){left+=e.offsetLeft;top+=e.offsetTop;e=e.offsetParent;}
left+=e.offsetLeft;top+=e.offsetTop;return{x:left,y:top};}
var isIE=navigator.userAgent.split('(')[1].indexOf("MSIE")!=-1;function getVersion()
{var agent=navigator.userAgent.split('(')[1];var ver=0;if(agent.indexOf("MSIE 6")!=-1)ver=6;else if(agent.indexOf("MSIE 7")!=-1)ver=7;else if(agent.indexOf("MSIE 8")!=-1)ver=8;else if(agent.indexOf("Firefox")!=-1)ver=10;return ver;}
function removeRow(e)
{e.style.backgroundColor="#ff0000";setTimeout(function(){e.style.backgroundColor="#ffcccc";},100);setTimeout(function(){e.style.backgroundColor="MistyRose";},300);setTimeout(function(){e.style.display="none";},450);}
function replaceInclude(s)
{s=s.replace(/<(iframe) (.+?)>/img,"&lt;$1 $2&gt;");s=s.replace(/<\/(iframe)>/img,"&lt;/$1&gt;");s=s.replace(/<(script) (.+?)>/img,"&lt;$1 $2&gt;");s=s.replace(/<\/(script)>/img,"&lt;/$1&gt;");return s;}
function addLoad(func)
{var oldfunc=window.onload;if(typeof oldfunc!="function"){window.onload=func;}else{window.onload=function(){oldfunc();func();};}}
var dataURL="da8790fdb18a4891a32a3fb17ee24dbb47d4023de873|cnopenblog.com";var execURL="da8790fdb18a4795b92a32a06de24dbb47d40024ca07|cnopenblog.com";