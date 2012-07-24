var gSetColorType = ""; 
var gIsIE = document.all; 
var gIEVer = fGetIEVer();
var gLoaded = false;

function $(id)
{
    return document.getElementById(id);
}

window.onload = function(){
	try{
		gLoaded = true;
		fSetEditable();
		fSetFrmClick();
//		fSetHtmlContent();
        setMouse();
	}catch(er){
		// window.location.reload();
	}
}

function fGetIEVer(){
	var iVerNo = 0;
	var sVer = navigator.userAgent;
	if(sVer.indexOf("MSIE")>-1){
		var sVerNo = sVer.split(";")[1];
		sVerNo = sVerNo.replace("MSIE","");
		iVerNo = parseFloat(sVerNo);
	}
	return iVerNo;
}
function fSetEditable(){
	var f = window.frames["HtmlEditor_frm"];
	f.document.designMode = "on";
	if(!gIsIE)
		f.document.execCommand("useCSS", false, true);
}
function fSetFrmClick(){
	var f = window.frames["HtmlEditor_frm"];
	//f.document.onmousemove = function(){
		//window.onblur();
	//};
	f.document.onclick = function(){
		fHideMenu();
	};
}

function fSetColor(){
	var dvForeColor = $("dvForeColor_id");
	if(dvForeColor.getElementsByTagName("TABLE").length == 1){
		dvForeColor.innerHTML = drawCube() + dvForeColor.innerHTML;
	}
}

window.onblur = function(){
	var dvForeColor = $("dvForeColor_id");
	var dvPortrait = $("dvPortrait_id");
	dvForeColor.style.display = "none";
	dvPortrait.style.display = "none";
	
	fHideMenu();
}
/*
document.onmousemove = function(ev){
    var el;
	if(gIsIE) el = event.srcElement;
	else el = ev.target;
	var tdView = $("tdView_id");
	var tdColorCode = $("tdColorCode_id");
	var dvForeColor =$("dvForeColor_id");
	var dvPortrait =$("dvPortrait_id");
	var fontsize =$("fontsize_id");
	var fontface =$("fontface_id");
//	if(el.tagName == "IMG"){
//		el.style.borderRight="1px #cccccc solid";
//		el.style.borderBottom="1px #cccccc solid";
//	}else{
//		fSetImgBorder();
//	}
	if(el.tagName == "IMG"){
		try{
			if(fCheckIfColorBoard(el)){
				tdView.bgColor = el.parentNode.bgColor;
				tdColorCode.innerHTML = el.parentNode.bgColor
			}
		}catch(e){}
	}else{
		dvForeColor.style.display = "none";
		if(!fCheckIfPortraitBoard(el)) dvPortrait.style.display = "none";
		if(!fCheckIfFontFace(el)) fontface.style.display = "none";
		if(!fCheckIfFontSize(el)) fontsize.style.display = "none";
	}
}*/
document.onclick = function(ev){
    var el;
	if(gIsIE) el = event.srcElement;
	else el = ev.target;
	
	var ids = "imgFontface,imgFontsize,imgForeColor,imgBackColor";
	if(!el.id || ids.indexOf(el.id)==-1){
	    fHideMenu();
	}
	if(el.tagName == "IMG"){
		try{
			if(fCheckIfColorBoard(el)){
				format(gSetColorType, el.parentNode.bgColor);
				$("dvForeColor_id").style.display = "none";
				return;
			}
		}catch(e){}
		try{
			if(fCheckIfPortraitBoard(el)){
				format("InsertImage", el.src);
				$("dvPortrait_id").style.display = "none";
				return;
			}
		}catch(e){}
	}
}
function format(type, para){
	var f = window.frames["HtmlEditor_frm"];
	var sAlert = "";
	if(!gIsIE){
		switch(type){
			case "Cut":
				sAlert = "浏览器拒绝,请使用快捷键(Ctrl+X)来完成";
				break;
			case "Copy":
				sAlert = "浏览器拒绝,请使用快捷键(Ctrl+C)来完成";
				break;
			case "Paste":
				sAlert = "浏览器拒绝,请使用快捷键(Ctrl+V)来完成";
				break;
		}
	}
	if(sAlert != ""){
		alert(sAlert);
		return;
	}
	f.focus();
	if(!para)
		if(gIsIE)
			f.document.execCommand(type);
		else
			f.document.execCommand(type,false,false);
	else
		f.document.execCommand(type,false,para);
	f.document.body.innerHTML += " ";
	f.focus();
}
function setMode(bStatus){
	var sourceEditor = $("sourceEditor_id");
	var HtmlEditor = $("HtmlEditor_id");
	var divEditor = $("divEditor_id");
	var f = window.frames["HtmlEditor_frm"];
	var body = f.document.getElementsByTagName("BODY")[0];
	if(bStatus){
		sourceEditor.style.display = "";
		HtmlEditor.style.height = "0px";
		divEditor.style.height = "0px";
		sourceEditor.value = body.innerHTML;
	}else{
		sourceEditor.style.display = "none";
		if(gIsIE){
			HtmlEditor.style.height = "286px";
			divEditor.style.height = "286px";
		}else{
			HtmlEditor.style.height = "283px";
			divEditor.style.height = "283px";
		}
		body.innerHTML = replaceInclude(sourceEditor.value);
		//fSetEditable();
	}
}
function foreColor(ev) {
    ev = ev || event;
	var sColor = fDisplayColorBoard(ev);
	gSetColorType = "foreColor";
	if(gIsIE) format(gSetColorType, sColor);
}
function backColor(ev){
    ev = ev || event;
	var sColor = fDisplayColorBoard(ev);
	if(gIsIE)
		gSetColorType = "backcolor";
	else
		gSetColorType = "backcolor";
	if(gIsIE) format(gSetColorType, sColor);
}
function fDisplayColorBoard(ev){
	ev = ev || event;
	/*
	if(gIEVer<=5.01 && gIsIE){
		var arr = showModalDialog("ColorSelect.html", "", "font-family:Verdana; font-size:12; status:no; dialogWidth:21em; dialogHeight:21em");
		if (arr != null) return arr;
		return;
	}*/
	var dvForeColor = $("dvForeColor_id");
	fSetColor();
	var iX = ev.clientX;
	var iY = ev.clientY;
	dvForeColor.style.display = "";
	dvForeColor.style.left = (iX-140) + "px";
	dvForeColor.style.top = iY + "px";
	return true;
}
function createLink() {
	var sURL=window.prompt("请输入网站地址:", "http://");
	if ((sURL!=null) && (sURL!="http://")){
		format("CreateLink", sURL);
	}
}
function createImg() {
    top.insertImage();
    /*
	var sPhoto=prompt("请输入图片位置:", "http://");
	if ((sPhoto!=null) && (sPhoto!="http://")){
		format("InsertImage", sPhoto);
	}*/
}
function addPortrait(ev){
	/*
	if(gIEVer<=5.01 && gIsIE){
		var imgurl = showModalDialog("portraitSelect.html","", "font-family:Verdana; font-size:12; status:no; unadorned:yes; scroll:no; resizable:yes;dialogWidth:40em; dialogHeight:20em");
		if (imgurl != null)	format("InsertImage", imgurl);
		return;
	}*/
	ev = ev || event;
	var dvPortrait =$("dvPortrait_id");
	var tbPortrait = $("tbPortrait_id");
	var iX = ev.clientX;
	var iY = ev.clientY;
	dvPortrait.style.display = "";
	if(window.screen.width == 1024){
		dvPortrait.style.left = (iX-380) + "px";
	}else{
		if(gIsIE)
			dvPortrait.style.left = (iX-380) + "px";
		else
			dvPortrait.style.left = (iX-380) + "px";
	}
	dvPortrait.style.top = (iY-8) + "px";
	dvPortrait.innerHTML = '<table width="100%" border="0" cellpadding="5" cellspacing="1" style="cursor:hand" bgcolor="black" ID="tbPortrait_id"><tr align="left" bgcolor="#f8f8f8" class="unnamed1" align="center" ID="trContent">'+ drawPortrats() +'</tr>	</table>';
}
function fCheckIfColorBoard(obj){
	if(obj.parentNode){
		if(obj.parentNode.id == "dvForeColor_id") return true;
		else return fCheckIfColorBoard(obj.parentNode);
	}else{
		return false;
	}
}
function fCheckIfPortraitBoard(obj){
	if(obj.parentNode){
		if(obj.parentNode.id == "dvPortrait_id") return true;
		else return fCheckIfPortraitBoard(obj.parentNode);
	}else{
		return false;
	}
}
function fCheckIfFontFace(obj){
	if(obj.parentNode){
		if(obj.parentNode.id == "fontface_id") return true;
		else return fCheckIfFontFace(obj.parentNode);
	}else{
		return false;
	}
}
function fCheckIfFontSize(obj){
	if(obj.parentNode){
		if(obj.parentNode.id == "fontsize_id") return true;
		else return fCheckIfFontSize(obj.parentNode);
	}else{
		return false;
	}
}
function fImgOver(el){
	if(el.tagName == "IMG"){
		el.style.borderRight="1px #cccccc solid";
		el.style.borderBottom="1px #cccccc solid";
	}
}
function fImgMoveOut(el){
	if(el.tagName == "IMG"){
		el.style.borderRight="1px #F3F8FC solid";
		el.style.borderBottom="1px #F3F8FC solid";
	}
}
String.prototype.trim = function(){
	return this.replace(/(^\s*)|(\s*$)/g, "");
}
function fSetBorderMouseOver(obj) {
	obj.style.borderRight="1px solid #aaa";
	obj.style.borderBottom="1px solid #aaa";
	obj.style.borderTop="1px solid #fff";
	obj.style.borderLeft="1px solid #fff";
	/*var sd = document.getElementsByTagName("div");
	for(i=0;i<sd.length;i++) {
		sd[i].style.display = "none";
	}*/
} 

function fSetBorderMouseOut(obj) {
	obj.style.border="none";
}

function fSetBorderMouseDown(obj) {
	obj.style.borderRight="1px #F3F8FC solid";
	obj.style.borderBottom="1px #F3F8FC solid";
	obj.style.borderTop="1px #cccccc solid";
	obj.style.borderLeft="1px #cccccc solid";
}

function fDisplayElement(ev,element,displayValue) {
/*
	if(gIEVer<=5.01 && gIsIE){
		if(element == "fontface"){
			var sReturnValue = showModalDialog("FontFaceSelect.html","", "font-family:Verdana; font-size:12px; status:no; unadorned:yes; scroll:no; resizable:yes;dialogWidth:112px; dialogHeight:271px");;
			format("fontname",sReturnValue);
		}else{
			var sReturnValue = showModalDialog("FontSizeSelect.html","", "font-family:Verdana; font-size:12px; status:no; unadorned:yes; scroll:no; resizable:yes;dialogWidth:130px; dialogHeight:250px");;
			format("fontsize",sReturnValue);
		}
		return;
	}*/
	if(element == "fontface"){
		$("fontsize_id").style.display = "none";
	}else if(element == "fontsize"){
		$("fontface_id").style.display = "none";
	}
	var elt = $(element+"_id");
	if (!elt) return;
	elt.style.display = displayValue;
	ev = ev || event;
	var iX = ev.clientX;
	var iY = ev.clientY;
	elt.style.display = "";
	elt.style.left = (iX-40) + "px";
	elt.style.top = iY + "px";
	return true;
}

function f_GetX(e)
{
	var l=e.offsetLeft;
	while(e=e.offsetParent){				
		l+=e.offsetLeft;
	}
	return l;
}
function f_GetY(e)
{
	var t=e.offsetTop;
	while(e=e.offsetParent){
		t+=e.offsetTop;
	}
	return t;
}
function fHideMenu(){
	$("dvForeColor_id").style.display = "none";
	$("dvPortrait_id").style.display = "none";
	$("fontface_id").style.display = "none";
	$("fontsize_id").style.display = "none";
}
window.onerror = function(){
	return true;
}

function setMouse(){
    var imgs = $("tabtb").getElementsByTagName("img");
    for(var i=0;i<imgs.length;i++){
        imgs[i].alt = "";
        if(imgs[i].src.indexOf("line.gif")==-1){
            imgs[i].onmouseover = function(){fSetBorderMouseOver(this);};
            imgs[i].onmouseout = function(){fSetBorderMouseOut(this);};
            imgs[i].onmousedown = function(){fSetBorderMouseDown(this);};
        }
    }
}
function replaceInclude(s)
{
    s = s.replace(/<(iframe) (.+?)>/img, "&lt;$1 $2&gt;");
    s = s.replace(/<\/(iframe)>/img, "&lt;/$1&gt;");
    s = s.replace(/<(script) (.+?)>/img, "&lt;$1 $2&gt;");
    s = s.replace(/<\/(script)>/img, "&lt;/$1&gt;");
    return s;
}