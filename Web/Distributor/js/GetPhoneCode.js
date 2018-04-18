var i = 0;
function getphonecode(phone1, type1, Module1, userid1, username1) {
    //var isMobile = /^0?1[0-9]{10}$/;
    //if (!isMobile.test(phone1)) {
    //    layerCommon.msg("手机号码格式不正确", IconOption.错误);
    //    return false;
    //}
    if (i == 0) {
        $.ajax({
            url: "../Controller/GetPhoneCode1.ashx",
            async: true,
            data: { phone: phone1, type: type1, Module: Module1, userid: userid1, username: username1 },
            dataType: 'json',
            cache: false,
            timeout: 3500,
            success: function (gim) {
                if (!gim.type) {
                    layerCommon.msg(gim.str, IconOption.错误);
                }
                else {
                    $("#getcode").css("background", "#ccc");
                    i = 120;
                    timeout();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("获取失败，请重试", IconOption.错误);
            }
        });
    }
    else {
        layerCommon.msg("2分钟内请勿重复发送验证码", IconOption.错误);
        return false;
    }
}

var ti;
function timeout() {
    if (i != 0) {
        $("#getcode").text(i.toString()+"s后重新获取");
        ti = setTimeout("timeout()", 1000);
        i--;
    }
    else {
        $("#getcode").text("获取验证码").css("background", "#ff4e02");
        clearTimeout(ti);
    }
}

var ii = 0;
function getCode(phone1, type1, Module1, userid1, username1, CallBack) {
    var isMobile = /^0?1[0-9]{10}$/;
    if (!isMobile.test(phone1)) {
        layerCommon.msg("手机号码格式不正确", IconOption.错误);
        return false;
    }
    if (ii == 0) {
        $.ajax({
            url: "../Controller/GetPhoneCode1.ashx",
            async: true,
            data: { phone: phone1, type: type1, Module: Module1, userid: userid1, username: username1 },
            dataType: 'json',
            cache: false,
            timeout:3500,
            success: function (gim) {
                if (!gim.type) {
                    layerCommon.msg(gim.str, IconOption.错误);
                }
                else {
                    $("#getcode").css({ "background": "#ccc", "cursor": "default" });
                    ii = 120;
                    Out();
                    if (typeof CallBack == "function") {
                        CallBack();
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("短信发送失败，请重试", IconOption.错误);
            }
        });
    }
//    else {
//        layerCommon.msg("2分钟内请勿重复发送验证码", IconOption.错误);
//    }
};

var tii;
function Out() {
    if (ii != 0) {
        $("#getcode").text(ii.toString() + "s后重新获取");
        tii = setTimeout("Out()", 1000);
        ii--;
    }
    else {
        //-webkit-linear-gradient(top, #ffffff, #f1f1f1);
        $("#getcode").text("获取验证码").removeAttr("style");
        clearTimeout(tii);
    }
}