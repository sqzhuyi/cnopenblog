function show(wh)
{
    el("div_title").style.display = "none";
    el("div_column").style.display = "none";
    el("div_style").style.display = "none";
    
    var ar = els("tablink2","a");
    for(var i=0;i<ar.length;i++){
        ar[i].className = "";
        if(ar[i].href.split("#")[1]==wh) ar[i].className = "curr";
    }
    
    el("div_"+wh).style.display = "";
}
function click_fl(chked)
{
    el("fl_box").style.display = chked?"":"none";
}
function saveTitle()
{
    if(!el("txtTitle").value.Trim()){
        el("notediv").className = "box erbox";
        el("notediv").innerHTML = "博客主标题不能为空。";
        el("txtTitle").className = "put erput";
        el("txtTitle").focus();
        return;
    }
    el("notediv").className = "";
    el("txtTitle").className = "put";
    el("notediv").innerHTML = loading;
    
    var url = execURL +"?saveblogtitle=1";
    var data = "blogtitle="+ URLencode(el("txtTitle").value);
    data += "&blogsubtitle="+ URLencode(el("txtSubtitle").value);
    ajaxPost(url, data);
    
    over();
}
function saveColumn()
{
    var url = execURL +"?savecolumn=1";
    var data = "c_cat="+ (el("chk_m_2").checked?"1":"0");
    data += "&c_group="+ (el("chk_m_3").checked?"1":"0");
    data += "&c_friend="+ (el("chk_m_4").checked?"1":"0");
    data += "&c_link="+ (el("chk_m_5").checked?"1":"0");
    data += "&c_msg="+ (el("chk_m_6").checked?"1":"0");
    
    var puts = els("fl_box","input");
    var n = 1;
    for(var i=0;i<puts.length;i+=2){
        if(puts[i].value.Trim()&&puts[i+1].value.Trim()){
            data += "&linktitle"+n+"="+ URLencode(puts[i].value);
            data += "&linkurl"+n+"="+ URLencode(puts[i+1].value);
            n++;
        }
    }
    ajaxPost(url, data);
    
    over();
}
function over()
{
    setTimeout(function(){
        el("notediv").className = "box okbox";
        el("notediv").innerHTML = "更改已保存。";
    },1000);
    setTimeout(function(){
        el("notediv").className = "";
        el("notediv").innerHTML = "";
    },5000);
}
function cancel()
{
    window.location = "/"+el("head_img").alt+ext;
}

function setBG(e)
{
    var _bg = e.id.substr(2);
    el("hd_bg").value = _bg;
    el("prevbox").style.display = "";
    el("prevbox").firstChild.src = "/images/blogger/bg/swatch/view"+_bg+"-s.jpg";
}
function useBG(b)
{
    if(!b){
        el("hd_bg").value = bg;
        el("prevbox").style.display = "none";
        return;
    }
    el("bg"+bg).className = "";
    bg = el("hd_bg").value;
    el("bg"+bg).className = "curr";
    el("prevbox").style.display = "none";
}
function saveBG()
{
    var url = execURL +"?savebg="+ el("hd_bg").value;
    ajaxGet(url);
    
    over();
}
var bg = 0;
var _init = function(){
    
    var u = window.location+"";
    if(u.indexOf("#")!=-1){
        var wh = u.split('#')[1];
        if(el("div_"+wh)) show(wh);
    }
    bg = el("hd_bg").value;
    el("bg"+bg).className = "curr";
};
addLoad(_init);