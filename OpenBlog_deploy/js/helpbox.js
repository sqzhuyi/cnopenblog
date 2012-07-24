var box = '';
box += '<table cellpadding="0" cellspacing="0">';
box += '<tr>';
box += '<td align="right" style="width: '+(document.all?21:21)+'px"><img src="/images/helpbox/lt.gif" alt="" /></td>';
box += '<td colspan="2" style="background-image: url(/images/helpbox/t.gif);"></td>';
box += '<td><img src="/images/helpbox/rt.gif" alt="" /></td>';
box += '<td rowspan="2" style="background-image: url(/images/helpbox/rbc.gif); vertical-align:top;"><img src="/images/helpbox/rtb.gif" alt="" /></td>';
box += '</tr>';
box += '<tr>';
box += '<td style="background-image: url(/images/helpbox/l.gif); background-position:right; background-repeat:repeat-y; height:25px; vertical-align:top;"><img src="/images/helpbox/jiantou.gif" alt="" /></td>';
box += '<td style="width:5px;background-color:#ffffff;"></td>';
box += '<td id="bodytbtd" rowspan="2" style="padding-right:6px; background-color:#ffffff;">#body#</td>';
box += '<td style="background-image: url(/images/helpbox/r.gif);"></td>';
box += '</tr>';
box += '<tr>';
box += '<td style="background-image: url(/images/helpbox/l.gif); background-position:right; background-repeat:repeat-y;">&nbsp;</td>';
box += '<td style="background-color:#ffffff;"></td>';
box += '<td style="background-image: url(/images/helpbox/r.gif);"></td>';
box += '<td style="background-image: url(/images/helpbox/rbc.gif);"></td>';
box += '</tr>';
box += '<tr>';
box += '<td align="right"><img src="/images/helpbox/lb.gif" alt="" /></td>';
box += '<td colspan="2" style="background-image: url(/images/helpbox/b.gif);"></td>';
box += '<td><img src="/images/helpbox/rb.gif" alt="" /></td>';
box += '<td style="background-image: url(/images/helpbox/rbc.gif);"></td>';
box += '</tr>';
box += '<tr>';
box += '<td colspan="2" style="background-image: url(/images/helpbox/lbb.gif); background-position:right;background-repeat:no-repeat;"></td>';
box += '<td colspan="2" style="background-image: url(/images/helpbox/bb.gif);"></td>';
box += '<td><img src="/images/helpbox/rbb.gif" alt="" /></td>';
box += '</tr>';
box += '</table>';

function reheight()
{
    var bodytb = document.getElementById("bodytbtd");
    var wh = bodytb.offsetWidth + bodytb.offsetHeight;
    bodytb.style.width = wh/2 +"px";
}
