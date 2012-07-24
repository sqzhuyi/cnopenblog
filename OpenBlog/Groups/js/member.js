var gid;
function toAdmin(e, isadmin)
{
    var un = els(e.parentNode.parentNode,"img")[0].alt;
    if(isadmin){if(!confirm("确定要将组员 "+un+" 升级为管理员吗？")) return;}
    else{ if(!confirm("确定要撤销 "+un+" 的管理员身份吗？")) return;}
    
    var url = execURL +"?setgroupadmin="+(isadmin?1:0)+"&gid="+gid+"&un="+escape(un);
    ajaxGet(url);
    removeRow(e);
}

function toOut(e)
{
    var un = els(e.parentNode.parentNode,"img")[0].alt;
    if(!confirm("确定要将组员 "+un+" 踢出该群吗？")) return;
    
    var url = execURL +"?setoutgroup="+gid+"&un="+escape(un);
    ajaxGet(url);
    removeRow(e.parentNode.parentNode);
}