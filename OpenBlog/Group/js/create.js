var arr_cat = new Array(new Array('11','电脑/网络'),new Array('12','生活/时尚'),new Array('13','家庭/婚姻'),new Array('14','电子数码'),new Array('15','商业/理财'),new Array('16','教育/学业'),new Array('17','交通/旅游'),new Array('18','社会/文化'),new Array('19','人文学科'),new Array('20','理工学科'),new Array('21','休闲/娱乐'),new Array('22','忧愁/烦恼'));

var _init = function(){
    
    if(!isIE) resetH3();
    bindCat();
    
    el("txtName").onchange = chkName;
    window.frames["photo_frm"].document.body.style.backgroundColor = "#EDF2F6";
};
window.onload = _init;

function bindCat()
{
    var e = el("selCat");
    for(var i in arr_cat){
        e.options[i] = new Option(arr_cat[i][1], arr_cat[i][0]);
    }
}

function resetH3()
{
    var h3s = els("h3");
    for(var i=0;i<h3s.length;i++){
        var s = h3s[i].innerHTML;
        var v = [];
        for(var j=0;j<s.length;j++) v.push(s[j]);
        h3s[i].innerHTML = v.join("<font size=1> </font>");
    }
}

function chkName()
{
    if(!el("txtName").value.Trim()) return;
    var url = dataURL +"?getlikegroup="+ URLencode(el("txtName").value)+"&r="+Math.random();
    var req = getAjax();
    req.open("GET", url, true);
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            if(re!="none") showLikeGroup(re);
        }
    };
    req.send(null);
}
function showLikeGroup(re)
{
    var s = "<div style='background:url(/images/y_start.gif) no-repeat 0px 4px; padding:2px 16px;line-height:150%;'>";
    s += "<span style='color:#888888;'>找到类似群组，建议加入</span>";
    var arr = re.split('<row>');
    for(var i=0;i<arr.length;i++){
        var ar = arr[i].split('<col>');
        s += "<br /><span><a href='/group/"+ar[0]+ext+"'>"+ar[1]+"</a></span>";
    }
    s += "</div>";
    el("txtName").nextSibling.innerHTML = s;
}
var doing = false;
var groupID;
function addGroup()
{
    if(doing) return;
    doing = true;
    var er = el("notediv");
    er.className = "box erbox";
    el("txtName").className = "put";
    if(!el("txtName").value.Trim()){
        el("txtName").className = "put erput";
        er.innerHTML = "请输入要创建的群组名称。";
        el("txtName").focus();
        return false;
    }
    if(el("txtCaptcha").value.toLowerCase()!=el("imgCaptcha").alt.toLowerCase()){
        er.innerHTML = "验证码不正确。";
        el("txtCaptcha").select();
        return false;
    }
    er.className = "";
    er.innerHTML = loading;
    
    var url = execURL +"?creategroup=1";
    var data = "g_name="+ URLencode(el("txtName").value);
    data += "&catid="+ el("selCat").value;
    data += "&jianjie="+ URLencode(el("txtJianjie").value);
    data += "&tags="+ URLencode(el("txtTags").value);
    
    var req = getAjax();
    req.open("POST", url, true);
    req.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
    req.onreadystatechange = function(){
        if(req.readyState==4){
            var re = req.responseText;
            doing = false;
            if(!isNaN(re)){
                groupID = re;
                er.className = "box okbox";
                er.innerHTML = "创建成功，请上传群组照片。";
                el("tb1").style.display = "none";
                el("tb2").style.display = "";
                el("photo_frm").src = "/ajax/uploadfile.aspx?group=1&bg=EDF2F6&id="+ re;
            }else{
                er.className = "box erbox";
                er.innerHTML = "创建失败。";
                window.location.reload();
            }
        }
    };
    req.send(data);
}

function uploadImg(e)
{
    if(doing) return;
    doing = true;
    el("notediv").innerHTML = loading;
    var win = window.frames["photo_frm"];
    win.document.forms[0].submit();
}

function loat()
{
    el("notediv").className = "box okbox";
    el("notediv").innerHTML = "群组照片上传成功，页面将自动跳转。";
    el("gimg").src = "/upload/group/"+groupID+".jpg";
    setTimeout(function(){
        window.location = "/group/"+groupID+ext;
    },500);
}