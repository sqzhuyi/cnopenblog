
function deleteGroup(e, id)
{
    if(!confirm("解散群组将删除该群组的所有信息，且不可恢复，是否继续？")) return;
    var url = execURL +"?deletegroup="+id;
    ajaxGet(url);
    removeRow(e.parentNode.parentNode);
}

function outGroup(e, id)
{
    if(!confirm("确定要退出该群组吗？")) return;
    var url = execURL +"?outgroup="+id;
    ajaxGet(url);
    removeRow(e.parentNode.parentNode);
}