var arr_hy = new Array("未指定","安保执法","博物馆或图书馆","出版业","电信","法律","房地产","非营利","工程业","广告","海运业","互联网","化工","环保业","会计","建筑设计","建筑施工","交通设备","交通运输","教育","军事","科技","科学","旅游业","农业","人力资源","商业服务","生物技术","时尚业","市场营销","通信或媒体业","投资银行业","学生","艺术","银行业","运动或娱乐","政府机关","制造业","咨询","宗教");
var setValue = null;
var _init = function(){
    loadDate();
    loadHangye();

    if(setValue){
        setValue();
        el("sp_num1").innerHTML = el("txtJianjie").value.length;
        el("sp_num2").innerHTML = el("txtXingqu").value.length;
        el("sp_num3").innerHTML = el("txtQianming").value.length;
    }
    
    el("txtJianjie").onfocus = setSize1;
    el("txtXingqu").onfocus = setSize2;
    el("txtQianming").onfocus = setSize3;
    
    var u = window.location+'';
    if(u.indexOf("#")!=-1) show(u.split('#')[1]);
};
addLoad(_init);

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
function loadHangye()
{
    var sel = el("selHangye");
    for(var i in arr_hy){
        sel.options[i] = new Option(arr_hy[i], arr_hy[i]);
    }
}

function show(what)
{
    if(!what){
        var u = window.location+'';
        if(u.indexOf("#")!=-1) what = u.split('#')[1];
    }
    for(var i=-1;i<3;i++){
        el("box"+i).style.display = "none";
        el("p0"+(i+2)).firstChild.className = "";
    }
    switch(what){
        case "edit":
            el("box0").style.display = "";
            el("p02").firstChild.className = "curr";
            if(!isIE) el("txtJianjie").rows=el("txtXingqu").rows=3;
            break;
        case "photo":
            el("box1").style.display = "";
            el("p03").firstChild.className = "curr";
            break;
        case "password":
            el("box2").style.display = "";
            el("p04").firstChild.className = "curr";
            break;
        default:
            el("box-1").style.display = "";
            el("p01").firstChild.className = "curr";
            break;
    }
    reshowFooter();
    document.body.focus();
}

function setSize1()
{
    setInterval(function(){
        var s = el("txtJianjie").value;
        var l = s.length;
        if(l>240){ el("txtJianjie").value = s.substr(0,240); l = 240;}
        el("sp_num1").innerHTML = l;
    },200);
}
function setSize2()
{
    setInterval(function(){
        var s = el("txtXingqu").value;
        var l = s.length;
        if(l>240){ el("txtXingqu").value = s.substr(0,240); l = 240;}
        el("sp_num2").innerHTML = l;
    },200);
}
function setSize3()
{
    setInterval(function(){
        var s = el("txtQianming").value;
        var l = s.length;
        if(l>120){ el("txtQianming").value = s.substr(0,120); l = 120;}
        el("sp_num3").innerHTML = l;
    },200);
}

var doing = false;
function uploadfile(e)
{
    if(doing) return;
    doing = true;
    el("note").innerHTML = loading;
    var win = window.frames["photo_frm"];
    win.document.forms[0].submit();
}
/*cut:true(可以剪切)*/
function loat(cut)
{
    doing = false;
    el("note").innerHTML = "";
    if(cut){
        el("box1_2").style.display = "";
        el("head_frm").src = "/cropper/headphoto.aspx?"+Math.random();
    }else{
        loadOk();
    }
}
function cutImg()
{
    if(doing) return;
    doing = true;
    el("note").innerHTML = loading;
    
    var win = window.frames["head_frm"];
    var puts = win.document.forms[0].elements;
    var url = "/ajax/uploadfile.aspx";
    for(var i=0;i<puts.length;i++){
        url += "&"+puts[i].name+"="+puts[i].value;
    }
    url = url.replace("aspx&", "aspx?");
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        doing = false;
        loadOk();
    };
    req.send(null);
}
function loadOk()
{
    el("note").className = "box okbox";
    el("note").innerHTML = "头像已改变";
    clearNote();
    el("box1_2").style.display = "none";
    setTimeout("goview()",3000);
}

function save()
{
    if(doing) return;
    
    el("txtFullname").className = "put";
    el("txtEmail").className = "put";
    el("txtUrl").className = "put";
    el("txtBlogtitle").className = "put";
    el("note").className = "box erbox";
    
    if(el("txtFullname").value.Trim()==""){
        el("txtFullname").className = "put erput";
        el("note").innerHTML = "真实姓名不能为空";
        return false;
    }
    if(el("txtEmail").value.Trim()==""){
        el("txtEmail").className = "put erput";
        el("note").innerHTML = "E-mail不能为空";
        return false;
    }
    if(!isEmail(el("txtEmail").value)){
        el("txtEmail").className = "put erput";
        el("note").innerHTML = "E-mail格式不正确";
        return false;
    }
    if(el("txtUrl").value.Trim()!="" && !isUrl(el("txtUrl").value)){
        el("txtUrl").className = "put erput";
        el("note").innerHTML = "URL格式不正确";
        return false;
    }
    if(el("txtBlogtitle").value.Trim()==""){
        el("txtBlogtitle").className = "put erput";
        el("note").innerHTML = "博客标题不能为空";
        return false;
    }
    el("note").className = "";
    el("note").innerHTML = "";
    
    var url = execURL +"?baseinfo=1";
    var data = "fullname="+ URLencode(el("txtFullname").value.Trim());
    data += "&email="+ escape(el("txtEmail").value.Trim());
    data += "&showemail="+ (el("chkEmail").checked?"1":"0");
    data += "&sex="+ (el("radsex1").checked?"1":"0");
    data += "&birthday="+ escape(el("selYear").value+"-"+el("selMonth").value);
    data += "&showbirthday="+ (el("chkBirthday").checked?"1":"0");
    data += "&hangye="+ escape(el("selHangye").selectedIndex>0?el("selHangye").value:"");
    data += "&url="+ escape(el("txtUrl").value.Trim());
    data += "&state="+ escape(el("selState").value);
    data += "&city="+ escape(el("selCity").value);
    data += "&qq="+ escape(el("txtQQ").value);
    data += "&showqq="+ (el("chkQQ").checked?"1":"0");
    data += "&msn="+ escape(el("txtMSN").value);
    data += "&showmsn="+ (el("chkMSN").checked?"1":"0");
    data += "&jianjie="+ URLencode(el("txtJianjie").value);
    data += "&xingqu="+ URLencode(el("txtXingqu").value);
    data += "&qianming="+ URLencode(el("txtQianming").value);
    data += "&blogtitle="+ URLencode(el("txtBlogtitle").value);
    data += "&blogsubtitle="+ URLencode(el("txtBlogsubtitle").value);
    
    doing = true;
    el("note").innerHTML = loading;

    ajaxPost(url, data);
    
    setTimeout(function(){
        el("note").className = "box okbox";
        el("note").innerHTML = "资料已经保存";
        doing = false;
    }, 1000);
    setTimeout(function(){window.location='/baseinfo.aspx';},2000);
}

function savePwd()
{
    if(doing) return;
    
    el("txtOldpwd").className = "put";
    el("txtNewpwd").className = "put";
    el("txtNewpwd2").className = "put";
    el("note").className = "";
    
    if(el("txtOldpwd").value.Trim()==""){
        el("txtOldpwd").className = "put erput";
        el("note").className = "box erbox";
        el("note").innerHTML = "请输入旧密码";
        return false;
    }
    if(el("txtNewpwd").value.Trim()==""){
        el("txtNewpwd").className = "put erput";
        el("note").className = "box erbox";
        el("note").innerHTML = "密码不能为空";
        return false;
    }
    if(el("txtNewpwd").value!=el("txtNewpwd2").value){
        el("txtNewpwd2").className = "put erput";
        el("note").className = "box erbox";
        el("note").innerHTML = "两次输入的密码不一致";
        return false;
    }
    if(el("txtNewpwd").value.indexOf(" ")>-1){
        if(!confirm("密码中包含空格，是否继续？")) return false;
    }
    doing = true;
    el("note").innerHTML = loading;
    
    var url = execURL +"?chgpwd=1&oldpwd="+ URLencode(el("txtOldpwd").value);
    url += "&newpwd="+ URLencode(el("txtNewpwd").value);
    
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4 || req.readyState=="complete"){
            var re = req.responseText;
            doing = false;
            if(re=="0"){
                el("txtOldpwd").className = "put erput";
                el("note").className = "box erbox";
                el("note").innerHTML = "旧密码不正确";
            }else{
                el("note").className = "box okbox";
                el("note").innerHTML = "密码已修改";
                clearNote();
            }
        }
    };
    req.send(null);
}

function goview()
{
    show('view');
    el('uph').src = '/upload/photo/'+el('uph').alt+'.jpg?'+Math.random();
    el('head_img').src = '/upload/photo/'+el('uph').alt+'-s.jpg?'+Math.random();
    window.location = "#view";
}

function clearNote(){
    setTimeout(function(){
        el("note").className = "";
        el("note").innerHTML = "";
    }, 3000);
}