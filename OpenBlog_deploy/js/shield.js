var shield;
function shideBody()
{
    shield = document.createElement("DIV");
    shield.id = "shield";
    shield.style.height = document.documentElement.scrollHeight+"px";
    shield.style.filter = "alpha(opacity=0)";
    shield.style.opacity = 0;
    document.body.appendChild(shield);
    
    this.setOpacity = function(obj,opacity){
        if(opacity>=1)opacity = opacity/100;
        try{ obj.style.opacity = opacity; }catch(e){}
        try{
            if(obj.filters.length>0 && obj.filters("alpha")){
                obj.filters("alpha").opacity = opacity*150;
            }else{
                obj.style.filter = "alpha(opacity=\""+(opacity*150)+"\")";
            }
        }catch(e){}
    }
    var c = 0;
    this.doAlpha = function(){
        c += 2;
        if (c > 20){ clearInterval(ad);return 0; }
        setOpacity(shield, c);
    }
    var ad = setInterval("doAlpha()",1);

    el("divh").style.display = "";
    el("divh").style.marginLeft = "-200px";
    el("divh").style.marginTop = -75+document.documentElement.scrollTop + "px";

    setDivh();
}
function cancelShide()
{
    if(el("divh"))
    {
        el("divh").innerHTML = "";
        el("divh").style.display = "none";
    }
    if(shield)
    {
        document.body.removeChild(shield);
        //document.body.onselectstart = function(){return true;}
        //document.body.oncontextmenu = function(){return true;}
    }
}
function setDivh()
{
    if(el("divh"))
    {
        el("divh").onmousedown = divhMouseDown;
        document.onmousemove = divhMouseMove;
        document.onmouseup = divhMouseUp;
    }
}
var divhDown = false;
var divhLeftCha, divhTopCha;
function divhMouseDown(ev)
{
    var ev = ev || window.event;
    var e = ev.srcElement || ev.target;
    if(e.className!="hasbg")return;
    divhDown = true;
    var mousePos = mouseCoords(ev);
    var divPos = getPosition(el("divh"));
    divhLeftCha = mousePos.x-divPos.x;
    divhTopCha = mousePos.y-divPos.y;
}
function divhMouseMove(ev)
{
    if(!divhDown)return;
    var ev = ev || window.event;
    var mousePos = mouseCoords(ev);
    el("divh").style.left = mousePos.x-divhLeftCha+200 +"px";
    el("divh").style.top = mousePos.y-divhTopCha+75 +"px";
}
function divhMouseUp()
{
    divhDown = false;
    //clearSelection();
}

function showdiv()
{
    var text = "<table cellpadding='0' cellspacing='0' class='showbox'>";
    text += "<tr><td id='lt'></td><td class='hasbg'></td><td id='rt'></td></tr>";
    text += "<tr><td class='hasbg'></td><td class='hasbg' style='padding:4px'>";
    text += "<div class='title'>#title#</div>";
    text += "<div class='body'>#body#</div>";
    text += "<div class='bottom'>#bottom#</div>";
    text += "</td><td class='hasbg'></td></tr>";
    text += "<tr><td id='lb'></td><td class='hasbg'></td><td id='rb'></td></tr></table>";
    return text;
}
function showdiv2()
{
    var text = "<table cellpadding='0' cellspacing='0' class='showbox'>";
    text += "<tr><td id='lt'></td><td class='hasbg'></td><td id='rt'></td></tr>";
    text += "<tr><td class='hasbg'></td><td class='hasbg' style='padding:4px'>";
    text += "<div style='padding:8px 12px; background-color:#ffffff;'>";
    text += "<h1 class='h1title'>#title#</h1>#body#</div>";
    text += "<div class='bottom2'>#bottom#</div>";
    text += "</td><td class='hasbg'></td></tr>";
    text += "<tr><td id='lb'></td><td class='hasbg'></td><td id='rb'></td></tr></table>";
    return text;
}
function loadingf()
{
    var text = "<table cellpadding='0' cellspacing='0' class='showbox'>";
    text += "<tr><td id='lt'></td><td class='hasbg'></td><td id='rt'></td></tr>";
    text += "<tr><td class='hasbg'></td><td class='hasbg' style='padding:4px'>";
    text += "<div class='loading'>Loading...<br /><br /><img src='/images/wait.gif' alt='' /><i></i></div>";
    text += "</td><td class='hasbg'></td></tr>";
    text += "<tr><td id='lb'></td><td class='hasbg'></td><td id='rb'></td></tr></table>";
    return text;
}
function noteBox()
{
    var txt = "<table class='notetb' cellpadding=0 cellspacing=0>"
        + "<tr><td class='lt_td'></td><td></td><td class='rt_td'></td></tr>"
        + "<tr><td></td><td>#body#</td><td></td></tr>"
        + "<tr><td class='lb_td'></td><td></td><td class='rb_td'></td></tr></table>";
    return txt;
}