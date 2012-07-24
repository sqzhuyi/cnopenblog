window.onload = function(){
    setTab();
    reshowFooter();
};
function setTab()
{
    var arr = els("tabdiv", "a");
    for(var i=0; i<arr.length; i++){
        arr[i].onmouseover = function(){
            this.className = "over";
        };
        arr[i].onmouseout = function(){
            if(!this.lang) this.className = "";
        };
    }
}
function reshowFooter()
{
    var allh = document.documentElement.scrollHeight;
    if(el("bodydiv").style.height.indexOf("px")!=-1)
        el("bodydiv").style.height = "auto";
    if(el("bodydiv").offsetHeight<allh-186)
         el("bodydiv").style.height = allh-186+"px";
    el("footerdiv").style.visibility = "visible";
}