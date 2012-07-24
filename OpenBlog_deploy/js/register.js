
window.onload = function(){
    loadDate();
    
    el("txtUsername").onblur = checkUser;
    el("txtFullname").onblur = checkName;
    el("txtPwd1").onblur = checkPwd;
    el("txtPwd2").onblur = checkPwd2;
    el("txtEmail").onblur = checkEmail;
};
function loadDate()
{
    var j = 0;
    for(var i=2009;i>1920;i--){
        el("selYear").options[j++] = new Option(i, i);
    }
    for(var i=1;i<13;i++){
        el("selMonth").options[i-1] = new Option(i, i);
    }
}

var doing = false;

function checkUser()
{
    var e = el("txtUsername");
    var sp = e.nextSibling;
    if(!e.value.Trim()){
        sp.className = "ersp";
        sp.innerHTML = "请输入用户名。";
        return false;
    }
    if(!/^[a-z]{1}[\w]{2,19}$/gi.test(e.value)){
        sp.className = "ersp";
        sp.innerHTML = "用户名不要符合要求。<br /><span style='color:#333333;'>( 用户名只能包括字母、数字和下划线，3-20个字符，并且以字母开头。)</span>";
        return false;
    }
    if(e.value.replace(/(openblog|system|cao|fuck|gan|ri)/i,'').length<e.value.length){
        sp.className = "ersp";
        sp.innerHTML = "用户名含有非法单词。";
        return false;
    }
    sp.className = "";
    sp.innerHTML = loading;
    var req = getAjax();
    req.open("GET",execURL+"?checkuser="+escape(e.value.Trim()),false);
    req.send(null);
    if(req.readyState==4){
        var re = req.responseText;
        if(re=="false"){
            sp.className = "ersp";
            sp.innerHTML = "该用户名已被占用。";
            e.select();
            return false;
        }else{
            sp.className = "oksp";
            sp.innerHTML = "";
            return true;
        }
    }
}
function checkName()
{
    var e = el("txtFullname");
    var sp = e.nextSibling;
    if(!e.value.Trim()){
        sp.className = "ersp";
        sp.innerHTML = "请输入您的真实姓名。";
        return false;
    }else{
        sp.className = "oksp";
        sp.innerHTML = "";
        return true;
    }
}
function checkPwd()
{
    var e = el("txtPwd1");
    var sp = e.nextSibling;
    if(!e.value){
        sp.className = "ersp";
        sp.innerHTML = "请输入密码";
        return false;
    }
    if(!/^[^\s]{1}.+[^\s]{1}$/gi.test(e.value) || e.value.length<6){
        sp.className = "ersp";
        sp.innerHTML = "密码不符合要求。<br /><span style='color:#333333;'>( 密码为6-20个任意字符，且不以空格开头/结尾。)</span>";
        return false;
    }
    sp.className = "oksp";
    sp.innerHTML = "";
    return true;
}
function checkPwd2()
{
    var sp = el("txtPwd2").nextSibling;
    if(!el("txtPwd1").value) return;
    if(el("txtPwd2").value!=el("txtPwd1").value){
        sp.className = "ersp";
        sp.innerHTML = "两次输入密码不一致。";
    }else{
        sp.className = "oksp";
        sp.innerHTML = "";
    }
}
function checkEmail()
{
    var e = el("txtEmail");
    var sp = e.nextSibling;
    if(!e.value.Trim()){
        sp.className = "ersp";
        sp.innerHTML = "请输入E-mail地址。";
        return false;
    }
    if(!isEmail(e.value)){
        sp.className = "ersp";
        sp.innerHTML = "E-mail格式不正确。";
        e.select();
        return false;
    }
    sp.className = "oksp";
    sp.innerHTML = "";
    return true;
}

function checkform()
{
    if(!el("txtUsername").value){
        checkUser();
        return false;
    }else if(el("txtUsername").nextSibling.className=="ersp"){
        el("txtUsername").select();
        return false;
    }
    if(!el("txtFullname").value){
        checkName();
        return false;
    }else if(el("txtFullname").nextSibling.className=="ersp"){
        el("txtFullname").select();
        return false;
    }
    if(!el("txtPwd1").value){
        checkPwd();
        return false;
    }else if(el("txtPwd1").nextSibling.className=="ersp"){
        el("txtPwd1").select();
        return false;
    }
    if(!el("txtPwd2").value){
        checkPwd2();
        return false;
    }else if(el("txtPwd2").nextSibling.className=="ersp"){
        el("txtPwd2").select();
        return false;
    }
    if(!el("txtEmail").value){
        checkEmail();
        return false;
    }else if(el("txtEmail").nextSibling.className=="ersp"){
        el("txtEmail").select();
        return false;
    }
    var sp = el("capt_n").childNodes[0];
    if(el("txtCaptcha").value.toLowerCase()!=el("imgCaptcha").alt.toLowerCase()){
        sp.className = "ersp";
        sp.innerHTML = "验证码不正确。";
        return false;
    }else{
        sp.className = "";
        sp.innerHTML = "";
    }
    return true;
}
function addUser()
{
    if(document.cookie.indexOf("reged")!=-1){
        el("notediv").innerHTML = "不能连续注册";
        el("notediv").className = "box erbox";
        return;
    }
    if(doing) return;
    if(!checkform()) return;
    
    el("notediv").innerHTML = loading;
    doing = true;
    var url = execURL+"?reg=1";
    var data = "name="+ escape(el("txtUsername").value);
    data += "&fullname="+ URLencode(el("txtFullname").value);
    data += "&pwd="+ URLencode(el("txtPwd1").value);
    data += "&email="+ escape(el("txtEmail").value);
    data += "&sex="+ escape(el("radsex1").checked?"1":"0");
    data += "&birthday="+ escape(el("selYear").value+"-"+el("selMonth").value);

    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4||req.readyState=="complete"){
            var re = req.responseText;
            if(re=="false"){
                el("notediv").innerHTML = "不能连续注册";
                el("notediv").className = "box erbox";
            }else{
                el("notediv").innerHTML = "注册成功";
                el("notediv").className = "box okbox";
                window.location = "/baseinfo.aspx";
                //doing = false;
            }
        }
    };
    req.send(data);
}