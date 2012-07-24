function getExpDate(days)
{
    var expDate = new Date();
    if(typeof(days) == "number")
    {
        expDate.setDate(expDate.getDate() + parseInt(days));
    }
    return expDate.toGMTString();
}

function getCookieVal(offset)
{
    var endstr = document.cookie.indexOf(";", offset);
    if(endstr == -1)
    {
        endstr = document.cookie.length;
    }
    var ck = unescape(document.cookie.substring(offset, endstr));
    return ck.replace(/%20/g, ' ').replace(/%2C/gi, ',');
}

function getCookie(name)
{
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;
    while(i < clen)
    {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg)
        {
            return getCookieVal(j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if(i == 0) break;
    }
    return "";
}

function setCookie(name, valu, expires)
{
    if(!expires) expires = getExpDate(30);
    document.cookie = name + "=" + escape(valu) + "; expires=" + expires + ";"
}

function deleteCookie(name)
{
    document.cookie = name + "=" + "; expires=Thu, 01-Jan-70 00:00:01 GMT";
}