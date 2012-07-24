function checkRemember(e)
{
    e.className = e.className=="chkRe"?"chkRe nochk":"chkRe";
}
function login()
{
    el("txtName").className = "put";
    el("txtPassword").className = "put";
    el("txtName").nextSibling.className = "";
    el("txtPassword").nextSibling.className = "";
    
    if(!el("txtName").value.Trim()){
        el("txtName").className = "put erput";
        el("txtName").nextSibling.className = "ersp";
        el("txtName").focus();
        return;
    }
    if(!el("txtPassword").value.Trim()){
        el("txtPassword").className = "put erput";
        el("txtPassword").nextSibling.className = "ersp";
        el("txtPassword").focus();
        return;
    }
    var url = "/login.aspx?n="+escape(el("txtName").value)+"&p="+URLencode(el("txtPassword").value);
    if(el("chkRe").className=="chkRe") url += "&r=1";
    url += "&from=null";
    window.location = url;
}