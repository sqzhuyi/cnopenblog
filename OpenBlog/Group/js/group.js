var _init = function(){

    setLeftOver();
};
window.onload = _init;

function setLeftOver()
{
    var spans = els("admin_div", "span");
    for(var i=0;i<spans.length;i++){
        spans[i].onmouseover = function(){
            this.className += " photo_box2";
        };
        spans[i].onmouseout = function(){
            this.className = this.className.replace(" photo_box2","");
        };
    }
    spans = els("member_div", "span");
    for(var i=0;i<spans.length;i++){
        spans[i].onmouseover = function(){
            this.className += " photo_box2";
        };
        spans[i].onmouseout = function(){
            this.className = this.className.replace(" photo_box2","");
        };
    }
}
