//弹出式树形  
//url：路径,areaX：区域长,areaY：区域宽,offX：对左定位,offY：对右定位
function ChooseTreeDialog(url, areaX, areaY, offX, offY) {
    var index = $.layer({
        title: false,
        type: 2,
        fix: false,
        treeclose:true,
        border: [1, 1, '#d7d7d7', true],
        shade: [0], //不显示遮罩
        iframe: { src: url },
        area: [areaX, areaY],
        offset: [offX, offY]
    });
    return index;
}

function MoneyYZ(val)//val=this
{
    var id = $(val).attr("id");
    var el = $("#" + id + "").get(0);
    var pos = 0;
    if ('selectionStart' in el) {
        pos = el.selectionStart;
    } else if ('selection' in document) {
        el.focus();
        var Sel = document.selection.createRange();
        var SelLength = document.selection.createRange().text.length;
        Sel.moveStart('character', -el.value.length);
        pos = Sel.text.length - SelLength;
    }
    var str = new RegExp("[1234567890.]")
    var d = new RegExp("[.]")
    var s = $("#" + id + "").val();
    var rs = "";
    for (var i = 0; i < s.length; i++) {
        if (str.test(s.substr(i, 1))) {
            if (d.test(s.substr(i, 1))) {
                if (rs.indexOf('.') < 0 && rs.length > 0) {
                    rs = rs + s.substr(i, 1);
                }
            }
            else {
                var index = rs.indexOf('.');
                if (index > 0) {
                    var strs = rs.substring(index, rs.length)
                    if (strs.length < 3) {
                        rs = rs + s.substr(i, 1);
                    }
                }
                else {
                    rs = rs + s.substr(i, 1)
                }
            }
        }
    }
    if (s != rs) {
        $("#" + id + "").val(rs);
        if (val.setSelectionRange) {
            val.focus();
            val.setSelectionRange(pos - 1, pos - 1);
        }
        else if (input.createTextRange) {
            var range = val.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos - 1);
            range.moveStart('character', pos - 1);
            range.select();
        }
    }

}



function validateIdCard(idCard) {
    //15位和18位身份证号码的正则表达式
    var regIdCard = /^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/;
    //如果通过该验证，说明身份证格式正确，但准确性还需计算
    if (regIdCard.test(idCard)) {
        if (idCard.length == 18) {
            var idCardWi = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2); //将前17位加权因子保存在数组里
            var idCardY = new Array(1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2); //这是除以11后，可能产生的11位余数、验证码，也保存成数组
            var idCardWiSum = 0; //用来保存前17位各自乖以加权因子后的总和
            for (var i = 0; i < 17; i++) {
                idCardWiSum += idCard.substring(i, i + 1) * idCardWi[i];
            }
            var idCardMod = idCardWiSum % 11; //计算出校验码所在数组的位置
            var idCardLast = idCard.substring(17); //得到最后一位身份证号码
            //如果等于2，则说明校验码是10，身份证号码最后一位应该是X
            if (idCardMod == 2) {
                if (idCardLast == "X" || idCardLast == "x") {
                    //alert("恭喜通过验证啦！");
                    return true;
                } else {
                    //alert("身份证号码错误！");
                    return false;
                }
            } else {
                //用计算出的验证码与最后一位身份证号码匹配，如果一致，说明通过，否则是无效的身份证号码
                if (idCardLast == idCardY[idCardMod]) {
                    //alert("恭喜通过验证啦！");
                    return true;
                } else {
                    //alert("身份证号码错误！");
                    return false;
                }
            }
        }
    } else {
        //alert("身份证格式不正确!");
        return false;
    }
}

function KeyPress(event) {
    var keyCode;
    if (window.event) {
        keyCode = window.event.keyCode;
    } else {
    keyCode = event.which;
}
if (keyCode == 32 && window.event) {
    window.event.returnValue = false;
} else if (keyCode == 32) {
event.preventDefault();
}
}

var ua = navigator.userAgent.toLowerCase(); //浏览器信息
var Naviinfo = {
    ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
    op: /opera/.test(ua),     //匹配Opera浏览器    
    sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
    ch: /chrome/.test(ua),     //匹配Chrome浏览器    
    ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
};

//数据检测
function CheckNull() {
    //区域名称
    if (!checkIsNullOrEmpty("#txt_product_class")) {
        return false;
    }
    return true;
}

function KeyInt(val, defaultValue) {

    if (val.value == "0")
        val.value = (defaultValue == undefined ? "" : defaultValue);
    else
        val.value = val.value.replace(/[^\d]/g, '');
}

//取消
function Cancel() {
    window.parent.CloseDialog("2");
}

//选择区域
function ChoseProductClass(url, x, y) {
    if ($("#hid_Alert").val() != null && $("#hid_Alert").val() != "") {
        CloseDialog();
    }
    //获取滚动条高度
    var yy = $(document).scrollTop();
    y = parseInt(y) - parseInt(yy);

    //获取光标位置
    x = parseInt(x) + "px";
    y = parseInt(y + 30) + "px";

    var index = ChooseTreeDialog(url, '200px', '200px', y, x);
    $("#hid_Alert").val(index);
}
function ChoseProductClassRe(url, x, y, id) {
    if ($("#" + id + "").val() != null && $("#" + id + "").val() != "") {
        CloseDialogRe(id);
    }
    //获取滚动条高度
    var yy = $(document).scrollTop();
    y = parseInt(y) - parseInt(yy);

    //获取光标位置
    x = parseInt(x) + "px";
    y = parseInt(y + 30) + "px";

    var index = ChooseTreeDialog(url, '200px', '200px', y, x);
    $("#" + id + "").val(index);
}

//关闭选择区域
function CloseProductClass(txtId, hidId, id, name) {
    $("#" + txtId).focus(); //解决 IE11 弹出层后文本框不能输入
    CloseDialog();
    $("#" + txtId).val(name); //区域名称
    $("#" + hidId).val(id); //区域id

}
//关闭选择区域
function CloseProductClass2(txtId, hidId, id, name, compId, lblattr, divheight) {
    $("#" + txtId).focus(); //解决 IE11 弹出层后文本框不能输入
    CloseDialog();
    $("#" + txtId).val(name); //区域名称
    $("#" + hidId).val(id); //区域id
    $.ajax({
        type: "post",
        url: "../../Handler/GoodsAttr.ashx",
        data: { ck: Math.random(), action: "attr", id: id, compId: compId },
        dataType: "text",
        success: function (data) {
            if (data != "") {
                if ($.trim(data.split("@")[0]) == "") {
                    $("#" + lblattr).text("该商品分类没有描述信息"); //属性
                } else {
                    $("#" + lblattr).text(data.split("@")[0]); //属性
                }
                $("." + divheight).html(data.split("@")[1]); //属性值
            }
        }, error: function () {

        }
    })

}
//关闭弹出的窗口
function CloseDialog() {
    var showedDialog = $("#hid_Alert").val();
    closeDialog(showedDialog);
    $("#hid_Alert").val("");
}

function CloseDialogRe(id) {
    var showedDialog = $("#" + id + "").val();
    if (showDialog != "") {
        closeDialog(showedDialog);
        $("#" + id + "").val("");
    }
}

//关闭弹出框的iframe
//i：当前弹出的index
function closeDialog(i) {
    layer.close(i);
    //    var inputTexts = window.$('input[type=text]:visible');
    //    inputTexts.each(function (j, item) {
    //        if (!($(item).attr("onfocus") != undefined && $(item).attr("onfocus").indexOf("WdatePicker") >= 0))
    //        { item.focus(); }
    //    });


}

function AnnexDel(obj,type, id, AnexName) {
    $.ajax({
        type: "post",
        url: "../../Controller/AnnexDelProGrame.ashx",
        data: { Dtype: type, Id: id, AnnexDelName: AnexName },
        dataType: "text",
        timeout:4000,
        success: function (data) {
            var jData = eval('(' + data + ')');
            if (jData.result) {
                $(obj).parents("div:eq(0)").remove();
            } else {
                if (jData.code) {
                    alert(jData.code);
                }
            }
        }, error: function (msg, status, e) {
            var s = msg;
        }
    })
}

function ConFirmDelteAnnex(obj, type, id, AnexName) {
    var ImgDemo = obj;
    if (typeof layerCommon != "undefined") {
        layerCommon.confirm('确认删除附件？', function () { AnnexDel(ImgDemo, type, id, AnexName) }, "系统提示");
    }
    else {
        confirm('确认删除附件？', function () { AnnexDel(ImgDemo, type, id, AnexName) }, "系统提示");
    }
}

function errMsg(title, message, con, url) {
//    $.layer({
//        shade: [0.5, '#000', true],
//        area: ['auto', 'auto'],
//        border: [5, 0.3, '#fff', true],
//        title: [title, true],
//        dialog: {
//            msg: message,
//            type: 0
//        },
//        end: function () {
//            if (con != null && con != "") {
//                $("#" + con).addClass("txt_focus");
//                $("#" + con).focus();
//            } else if (url != null && url != "") {
//                window.location.href = url;
//            }
//        }
    //    });
    if (url == undefined || url.toString() == "" || url == null) {
        alert(message);
    }
    else {
        alert(message);
        window.location.href = url;
    }
}

function errMsgMo(title, message, callback) {
    $.layer({
        shade: [0.5, '#000', true],
        area: ['auto', 'auto'],
        border: [5, 0.3, '#fff', true],
        title: [title, true],
        dialog: {
            msg: message,
            type: 0
        },
        end: function () {
            if (callback != undefined) {
                callback();
            }
        }
    });
}

function confirm(msg, conOK, title) {
    layer.confirm(msg, function () {
        $(".xubox_layer .xubox_main .xubox_close").trigger("click");
        conOK();
    }, title
     , function () {
         $(".xubox_layer .xubox_main .xubox_close").trigger("click");
     });
}

function confirms(msg, conOK, title,cancel) {
    layer.confirm(msg, function () {
        $(".xubox_layer .xubox_main .xubox_close").trigger("click");
        conOK();
    }, title
     , function () {
         cancel();
//         $(".xubox_layer .xubox_main .xubox_close").trigger("click");
     });
}

function IsMobile(value) {
    var isMobile = /^0?1[0-9]{10}$/;
    return isMobile.test(value);
}

function IsLegalTel(value) {
    var isMobile = /^[0-9]*$/;
    return isMobile.test(value);
}

function IsPhone(value) {
    var isPhone = /^0?(1[0-9]{10})|(^\d{2,5}-\d{7,8})$/;
    return isPhone.test(value);
}

function IsEmail(value) {
    var isEM = /^([a-zA-Z0-9_-]{1,})+@([a-zA-Z0-9_-]{1,})+\.(com|cn)$/i;
    return isEM.test(value);
}

function IsIdentity(value) {
    var Isd = /^\d{15}$|^\d{17}(\d|X|x)$/;
    return Isd.test(value);
}

function IsPayPwd(value) {
    var IsP = /^(([a-zA-Z]{1,}[0-9]{1,})|([0-9]{1,}[a-zA-Z]{1,}))$/;
    return IsP.test(value);
}

//价格格式
function formatMoney(s, type) {
    if (/[^0-9\.]/.test(s))
        return "0";
    if (s == null || s == "")
        return "0";
    s = s.toString().replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s))
        s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    if (type == 0) {// 不带小数位(默认是有小数位) 
        var a = s.split(".");
        if (a[1] == "00") {
            s = a[0];
        }
    }
    return s;
}


//弹出框 iframe  
//titie:提示框名称，url:页面的路径,height:高，width:宽
function showDialog(title, url, width, height, layerOffsetY) {
    if (layerOffsetY < 0) {
        layerOffsetY = 0;
    }
    var i = $.layer({
        type: 2,
        title: title,
        iframe: { src: url },
        area: [width, height],
        offset: [layerOffsetY, ''],
        move: ['.xubox_title', true],
        success: function (layer) {
            layer.focus();
        }
    });
    return i;
}


function jsRemove() {
    $("script[src='../Company/js/jquery-1.11.1.min.js']").remove();
}

String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}
        

$(document).ready(function () {
    $(".tablelist thead input:checkbox").on("click", function () {
        var checked = $(this).is(":checked");
        $(".tablelist input:checkbox").each(function () {
            this.checked = checked;
        });
    })

    if ($.trim($("li #txtPageSize").val()) == "") {
        $("li #txtPageSize").val("12");
    }

    $('.tb tbody tr td').each(function (index, obj) {
        if (index % 2 == 0) {
            $(obj).addClass('odd');
        }
    });


    $(".pagin > .paginList > .message").css("width", "auto");

    $(".liSenior").unbind().on("click", function () {
        $("div.hidden").slideToggle(100);
    })

})

function CheckTitle(obj, check, title) {
    if (check) {
        if ($.trim($(obj).val()) != "") {
            $(obj).siblings("i").attr("class", "okIcon");
        }
        $(obj).parents("div:eq(0)").attr("class", "regBox");
        $(obj).parents("div:eq(0)").siblings("label").addClass("none");
    } else {
        $(obj).siblings("i").attr("class", "ErIcon");
        $(obj).parents("div:eq(0)").siblings("label").attr("class", "text tRed").text(title);
        $(obj).parents("div:eq(0)").attr("class", "regBox bRed");
    }
}

function ReturnClientClickname(strs) {
    for (var i = 0; i < strs.length; i++) {
        if (i > 0) {
            strs[i] = "$" + strs[i];
        }
    }
    var str = strs.join("");
    return str;
}

function Confirms(ID, str) {
    var id = ID;
    confirm(str, function () {
        var fc = ReturnClientClickname(id.split("_"));
        __doPostBack(fc);
    }, "提示");
    return false;
}
function Orderclass(LiId) {

    //$(window.parent.leftFrame.document).find(".menuson").css("display", "none");
    //$(window.parent.leftFrame.document).find(".menuson li.active").removeClass("active");
    ////$(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent().parent("li").find("div").css("display", "none");

    //var ert = $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent()[0].nodeName;
    //if (ert == "DIV") {
    //    $(window.parent.leftFrame.document).find(".menuson li").find(".lista1").find(".al").removeClass("active");

    //    //LiId 一父级
    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent().parent().parent().css("display", "block");
    //    //LiId 二父级
    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent().parent().addClass("active");
    //    //LiId 三级
    //    //window.parent.leftFrame.document.getElementById(LiId).parentNode.style.display = "block";
    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent().css("display", "block");

    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).addClass("al active");
    //    //window.parent.leftFrame.document.getElementById(LiId).className = "al active";

    //} else {
    //    //LiId一父级
    //    //window.parent.leftFrame.document.getElementById(LiId).parentNode.style.display = "block";
    //    //window.parent.leftFrame.document.getElementById(LiId).className = "active";

    //    //LiId一父级
    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).parent().css("display", "block");
    //    $(window.parent.leftFrame.document).find(".menuson").find("#" + LiId).addClass("al active");
    //}
}

//功能菜单
function onlinkOrder(url, LiId) {
    Orderclass(LiId);
    window.location.href = url;
}

//过滤非法字符
function stripscript(strHtlm) {
    strHtlm = strHtlm + "";
    //var pattern = new RegExp("exec|insert|delete|drop|truncate|update|declare|frame|or|style|expression|and|select|create|script|img|alert|href|1=1|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62))","g")
    //s.replace(pattern,"");
    //var pattern = new RegExp("[%--`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]")        //格式 RegExp("[在中间定义特殊过滤字符]")
    var pattern = /(insert|delete|truncate|update|declare|frame|style|expression|select|create|script|alert|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62)) /ig;
    return strHtlm.replace(pattern, "");
}

