function OpenMiddleWindow(strUrl, strName) {
     window.open(strUrl, strName, "width=640,height=480,top=" + (window.screen.availHeight - 33 - 480) / 2 + ",left=" + (window.screen.availWidth - 640) / 2 + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no");
}
//弹出正中间大窗体
function OpenLargeWindow(strUrl, strName) {
    return window.open(strUrl, strName, "width=850,height=600,top=" + (window.screen.availHeight - 33 - 600) / 2 + ",left=" + (window.screen.availWidth - 800) / 2 + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no");
}
//弹出满屏窗体
function OpenFullWindow(strUrl, strName) {
    var Wins = window.open(strUrl, strName, "width=" + window.screen.availWidth + ",height=" + (window.screen.availHeight - 33) + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no");
    Wins.moveTo(-3, 0);
    return Wins;
}
//弹出自定义大小窗体
function OpenCustomWindow(strUrl, strName, iWidth, iHeight) {
     window.open(strUrl, strName, "width=" + iWidth + ",height=" + iHeight + ",top=" + (window.screen.availHeight - iHeight - 33) / 2 + ",left=" + (window.screen.availWidth - iWidth) / 2 + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no,center=yes;");
}

function OpenWindowDialog(strUrl, iWidth, iHeight) {
     window.showModalDialog(strUrl, window, "dialogWidth=" + iWidth + "px;dialogHeight=" + iHeight + "px;status=no;resizable=yes;center=yes;location=no;menubar=no;scrollbars=yes;toolbar=no;titlebar=no;");
}