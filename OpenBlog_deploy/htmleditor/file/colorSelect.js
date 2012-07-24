var s = "";
var hex = new Array("FF","CC","99","66","33","00");

function drawCell(red, green, blue) {
	var color = '#' + red + green + blue;
	s += '<td title="'+color.substr(1)+'" style="cursor:default;background-color:'+color+';';
	if(document.all) s += 'padding:5px;font-size:4px;">&nbsp;';
	else s += 'padding:6px;">';
	s += '</td>';
}
function drawRow(red, blue) {
	s += '<tr>';
	for (var i = 0; i < 6; ++i) {
		drawCell(red, hex[i], blue);
	}
	s += '</tr>';
}
function drawTable(blue) {
	s += '<table cellpadding=0 cellspacing=0 border=0 onmouseover="setcolor=true;" onmouseout="setcolor=false;">';
	for (var i = 0; i < 6; ++i) {
		drawRow(hex[i], blue);
	}
	s += '</table>';
}
function drawCube() {
	s += '<table cellpadding=0 cellspacing=0 style="border:1px #999 solid;" onmousemove="viewColor(event)"><tr>';
	for (var i = 0; i < 3; ++i) {
		s += '<td bgcolor="#FFFFFF">';
		drawTable(hex[i]);
		s += '</td>';
	}
	s += '</tr><tr>';
	for (var i = 3; i < 6; ++i) {
		s += '<td bgcolor="#FFFFFF">';
		drawTable(hex[i]);
		s += '</td>';
	}
	s += '</tr></table>';
	return s;
}

