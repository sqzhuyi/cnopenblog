var doing=false;function changediv(k)
{el("add_div").style.display="none";el("log_div").style.display="none";el("pwd_div").style.display="none";switch(k){case 1:el("add_div").style.display="";break;case 2:el("log_div").style.display="";break;case 3:el("pwd_div").style.display="";break;}}
function setpwd()
{changediv(2);}
function login()
{if(doing)return;el("note_div2").className="box erbox";el("txtUsername").className="put";el("txtPwd").className="put";if(!el("txtUsername").value.Trim()){el("note_div2").innerHTML="请输入用户名";el("txtUsername").className="put erput";return;}
if(!el("txtPwd").value.Trim()){el("note_div2").innerHTML="请输入密码";el("txtPwd").className="put erput";return;}
el("note_div2").className="";el("note_div2").innerHTML=loading;doing=true;var url=execURL+"?login=1&uname="+escape(el("txtUsername").value)+"&pwd="+escape(el("txtPwd").value);if(el("chkRemember").checked)url+="&remember=1";var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;if(re=="false"){doing=false;el("note_div2").className="box erbox";el("note_div2").innerHTML="用户名/密码不正确";}else{window.location.reload();}}};req.send(null);}
function addCat()
{el('p_addcat').style.display='';el("selCat").selectedIndex=0;el('txtnewCat').focus();el('txtnewCat').onblur=function(){if(this.value.Trim())el("selCat").selectedIndex=0;};}
function addit()
{if(doing)return;doing=true;var id=getQuery(window.location,"blog");var url=execURL+"?addfavorite=1&blogid="+escape(id);url+="&cat="+el("selCat").value;if(!el("txtnewCat").value.Trim())url+="&newcat="+URLencode(el("txtnewCat").value);el("sp_note").innerHTML=loading;var req=getAjax();req.open("GET",url,true);req.onreadystatechange=function(){if(req.readyState==4||req.readyState=="complete"){var re=req.responseText;if(re=="true"){el("sp_note").innerHTML="<font color=green>添加成功</font>";setTimeout("window.opener=null;window.close();",3000);}else{window.alert("出现未知错误");window.opener=null;window.close();}}};req.send(null);}