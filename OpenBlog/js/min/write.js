var cat,subcat;var _init_write=function(){el("txtZhaiyao").onfocus=setZhaiyao;el("txtZhaiyao").onblur=function(){this.value=replaceInclude(this.value.replace(/\n/g," ").Trim());};el("txtTag").onblur=function(){this.value=this.value.replace(/<|>|"/g,"").Trim();};if(el("hd_con").value){setTimeout(function(){var win=window.frames["editor_frm"];win=win.frames["HtmlEditor_frm"];win.document.body.innerHTML=el("hd_con").value;},1000);}
if(cat){var ops=el("selCat").options;for(var i=0;i<ops.length;i++){if(ops[i].value==cat){ops[i].selected=true;break;}}}
if(subcat){var ops=el("selSubCat").options;for(var i=0;i<ops.length;i++){if(ops[i].value==subcat){ops[i].selected=true;break;}}}};addLoad(_init_write);var codes=new Array("Assembly","BatchFile","C#","CSS","HTML","INIFile","Java","JScript","Lua","MSIL","Pascal","Perl","PHP","PowerShell","Python","SQL","VB.NET","VBScript","XAML","XML");function insertCode()
{shideBody();var ddl="<select id='selCode'>";for(var i=0;i<codes.length;i++){ddl+="<option value='"+codes[i]+"'>"+codes[i]+"</option>";}
ddl+="</select>";var stitle="插入代码";var sbody="<span style='float:right;margin-top:-"+(isIE?50:40)+"px;'>"+ddl+"</span>";sbody+="<textarea id='txtCode' rows=24 style='width:99%;'></textarea>";var sbottom="<input type='button' value='确定' onclick='saveCode(this)' /> <input type='button' value='取消' onclick='cancelCode()' />";var s=showdiv2();s=s.replace("#title#",stitle);s=s.replace("#body#",sbody);s=s.replace("#bottom#",sbottom);el("divh").innerHTML=s;el("txtCode").focus();}
function saveCode(e)
{var _code=el("selCode").value;var _body=el("txtCode").value;if(!_body){cancelShide();return;}
e.disabled=true;var req=getAjax();req.open("POST","/ajax/parsecode.aspx?code="+URLencode(_code),true);req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;re=re.substr(5,re.length-11);re=re.substr(re.indexOf("<span"));re="<div style='padding:6px;background-color:#cccccc;'><b>"+_code+" code</b></div><pre style='width:100%;margin-top:0px;overflow-x:auto;overflow-y:hidden;background-color:#f0f0f0;'><br>"+re+"<br> <br> </pre><br> <br>";var f=window.frames["editor_frm"];if(f.document.getElementById("switchMode_id").checked)
f.document.getElementById("sourceEditor_id").value+=re;else f.frames["HtmlEditor_frm"].document.body.innerHTML+=re;cancelCode();}};req.send("body="+escape(_body));}
function cancelCode()
{cancelShide();}
function insertImage()
{shideBody();var src="/ajax/uploadfile.aspx?img=1&size=40&r="+Math.random();var sbody="<p style='padding-left:20px;font-size:12px;'>请选择您要上传的图片</p>";sbody+="<p style='padding-left:20px;'><iframe id='uploadimg_frm' name='uploadimg_frm' src='"+src+"' frameborder='0' scrolling='no' style='width:360px; height:30px;'></iframe></p>";sbody+="<p style='padding-left:20px;font-size:12px;'>或输入图片的URL</p>";sbody+="<p style='padding-left:20px;'><input id='txtImgurl' type='text' style='width:350px;' value='http://' /></p>";sbody+="<p>&nbsp;</p>";var sbottom="<input type='button' value='确定' onclick='saveImage(this)' /> <input type='button' value='取消' onclick='cancelShide()' />";var s=showdiv2();s=s.replace("#title#","插入图片");s=s.replace("#body#",sbody);s=s.replace("#bottom#",sbottom);el("divh").innerHTML=s;}
function saveImage(e)
{var f=window.frames[1];if(f.document.getElementById("file1").value){f.document.forms[0].submit();}else{var src=el("txtImgurl").value;if(!src.Replace("http://",""))return;loat(src);}}
function loat(src)
{f=window.frames["editor_frm"].frames["HtmlEditor_frm"];f.focus();f.document.execCommand("InsertImage",false,src);f.document.body.innerHTML+=" ";f.focus();cancelShide();}
function setZhaiyao()
{if(el("txtZhaiyao").value.Trim())return;var valu=getBody();valu=valu.replace(/<.+?>/gm," ").replace(/[\r\n]/g,"").Trim();valu=valu.replace(/&\w+;/gm,"");if(valu.length>200)valu=valu.substr(0,200);el("txtZhaiyao").value=valu;}
var doing=false;function publish()
{if(doing)return;if(!checkit())return;doing=true;shideBody();var s="<p>正在保存内容...</p>";el("divh").innerHTML=loadingf().replace("<i></i>",s);setTimeout(function(){s="<p>正在检索文章关键字...</p>";el("divh").innerHTML=loadingf().replace("<i></i>",s);},3000);setTimeout(function(){s="<p>正在生成静态页...</p>";el("divh").innerHTML=loadingf().replace("<i></i>",s);},5000);var url=execURL+"?writeblog=1";var blogid=getQuery(window.location,"blog");if(blogid)url=execURL+"?editblog="+blogid;var data="_title="+escape(el("txtTitle").value);data+="&_cat="+el("selCat").value;data+="&_subcat="+el("selSubCat").value;data+="&_zhaiyao="+URLencode(el("txtZhaiyao").value);data+="&_tag="+URLencode(el("txtTag").value);data+="&_nocomment="+(el("chkNoComment").checked?"1":"0");data+="&_body="+URLencode(getBody());var req=getAjax();req.open("POST",url,true);req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;if(re.length<30)window.location=re;else window.location.reload();}};req.send(data);}
function review()
{var valu=getBody();var nwin=window.open("blankpage.htm","_blank","width=700px,height=500px,left=100px,top=100px,location=no");nwin.document.write("<html><head></head><body style='font-size:12px;'>"+valu+"</body></html>");}
function cancel()
{window.location="/postlist.aspx";}
function checkit()
{var n=el("note_div");n.className="box erbox";var t=el("txtTitle");var z=el("txtZhaiyao");var g=el("txtTag");t.className="put";z.className="put";g.className="put";if(!t.value.Trim()){n.innerHTML="请输入文章标题。";t.className="put erput";t.focus();return false;}
var con=getBody();if(!con.replace(/<[^>]*>/g,"").Trim()){n.innerHTML="请输入文章内容。";return false;}
if(!z.value.Trim()){n.innerHTML="请输入文章摘要。";z.className="put erput";z.focus();return false;}
if(!g.value.Trim()){n.innerHTML="请输入文章标签。";g.className="put erput";g.focus();return false;}
n.className="";n.innerHTML="";return true;}
function getBody()
{var win=window.frames["editor_frm"];var ishtml=win.document.getElementById("switchMode_id").checked;var valu="";if(ishtml)valu=win.document.getElementById("sourceEditor_id").value;else valu=win.frames["HtmlEditor_frm"].document.body.innerHTML;return valu;}
function editsubcat()
{shideBody();var sbody="<iframe id='frmCat' name='frmCat' src='/do/editusercat.aspx' frameborder=0 scrolling='auto' style='width:360px;height:160px;'></iframe>";var sbottom="<input type='button' value='完成' onclick='editsubcat_over()' /> <input type='button' value='取消' onclick='editsubcat_over()' />";var s=showdiv2();s=s.replace("#title#","添加/修改文章分类");s=s.replace("#body#",sbody);s=s.replace("#bottom#",sbottom);el("divh").innerHTML=s;}
function editsubcat_over()
{var s=window.frames["frmCat"].document.getElementById("hd_cat").value;var arr=s.split('$|$');var sel=el("selSubCat");sel.length=0;for(var i=0;i<arr.length;i++){var p=arr[i].indexOf("|");sel.options[i]=new Option(arr[i].substr(p+1).Replace("$#$","$|$"),arr[i].substr(0,p));}
cancelShide();}