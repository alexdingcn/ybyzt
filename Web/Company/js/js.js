
////禁用F12
//document.onkeydown = function (e) {
//    var ev = window.event || e;
//    var code = ev.keyCode || ev.which;
//    var code = ev.keyCode || ev.which || ev.charCode;
//    if (code == 123) {
//        if (ev.preventDefault) {
//            ev.preventDefault();
//        } else {
//            ev.keyCode = 505;
//            ev.returnValue = false;
//        }
//    }
//}
(function ($) {
    $_def = { ID: "", type: "" }
    //Enter键表单自动提交  
    document.onkeydown = function (event) {
        if ($_def.ID == "")
            return;
        var target, code, tag;
        if (!event) {
            event = window.event; //针对ie浏览器  
            target = event.srcElement;
            code = event.keyCode;
            if (code == 13) {
                //tag = target.tagName;
                //if (tag == "TEXTAREA") { return true; }
                //else { return false; }
                if ($_def.type == "") {
                    $("#" + $_def.ID).trigger("click");
                    return false;
                }
            } else if (code == 123) {
                if (event.preventDefault) {
                    event.preventDefault();
                } else {
                    event.keyCode = 505;
                    event.returnValue = false;
                }
            }
        }
        else {
            target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
            code = event.keyCode;
            if (code == 13) {
                //tag = target.tagName;
                //if (tag == "INPUT") { return false; }
                //else { return true;}
                if ($_def.type == "") {
                    $("#" + $_def.ID).trigger("click");
                    return false;
                }
            } else if (code == 123) {
                if (event.preventDefault) {
                    event.preventDefault();
                } else {
                    event.keyCode = 505;
                    event.returnValue = false;
                }
            }
        }
    }
})(jQuery);

//价格格式
function formatMoney(s, type) {
    if (/[^0-9\.]/.test(s))
        return "0.00";
    if (s == null || s == "")
        return "0.00";
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

//数据行全选反选
function SelectAllGoods(tempControl) {
    var theBox = tempControl;
    var xState = theBox.checked;
    var elem = theBox.form.elements;
    for (i = 0; i < elem.length; i++) {
        if (elem[i].type == "checkbox" && elem[i].id != theBox.id && elem[i].name != "chbPro") {
            if (elem[i].checked != xState)
                elem[i].click();
        }
    }
}

//数据行全选反选
function SelectAll(tempControl) {
    var theBox = tempControl;
    var xState = theBox.checked;
    var elem = theBox.form.elements;
    for (i = 0; i < elem.length; i++) {
        if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
            if (elem[i].checked != xState)
                elem[i].click();
        }
    }
}

function InputFocus(Id) {
    Id.select();
    if (Id.value == parseInt(0))
        Id.value = '';
}
function priceBlur(own) {
    if (!isNaN(own.value)) {
        own.value = own.value == "" ? "0" : own.value;
    } else {
        own.value = "0.00";
    }
    own.value = parseFloat(own.value).toFixed(2);
}
//只能输入数字验证
function KeyInt(val) {
    val.value = val.value.replace(/[^\d]/g, '');
}
//价格验证
function KeyInt2(val) {
    //val.value = val.value.replace(/[^\d.]/g, '');
    //val.value = val.value.replace(/^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$/, '');
    var r = /^[0-9]\d*?\.?\d*?$/;
    if (!val.value.match(r)) {
        val.value = '';
    }
}
//库存验证
function KeyInt3(val) {
    var r = /^[1-9]\d*?\.?\d*?$/;
    if (!val.value.match(r)) {
        val.value = '';
    }
//    var r = "^\\d+$";
//    if (!val.value.match(r)) {
//        val.value = '';
//    }
}
//验证手机号码和电话号码
function Phone(val) {
    var isMobile = /^0?1[0-9]{10}$/;

    var Phone = val.value;

    if (Phone.toString() != "") {

        if (isMobile.test(Phone)) {
            return true;
        } else {
            errMsg("提示", "请输入正确的联系方式", "");
            val.value = "";
            return false;
        }
    }
}
//附件
$(function () {
    //删除附件
    $(document).on("click", ".attach .del", function () {
        $(this).parent().remove();
        if ($(".attach .list li").length >= 5) {
            $(".attach .add").hide();
        } else {
            $(".attach .add").show();
        }
        var file = $(this).attr("tip");
        var orderid = $(this).attr("orderid");
        var filelist = $("#hrOrderFj").text();
        $.ajax({
            type: "post",
            url: "../../Handler/HandleImg.ashx",
            data: { ck: Math.random(), action: "delfile", files: file, orderid: orderid },
            dataType: "text",
            success: function (data) {
                if (data == "cg") {
                    var str = "";
                    for (var i = 0; i < filelist.split("@@").length; i++) {
                        if ($.trim(filelist.split("@@")[i]) != "") {
                            if (file != filelist.split("@@")[i]) {
                                str += filelist.split("@@")[i] + "@@";
                            }
                        }
                    }
                    $("#hrOrderFj").val(str);
                } else {
                    layerCommon.msg("附件删除失败", IconOption.错误);
                    return false;
                }
            }
        })
    })
})
//上传uploadAvatar
function uploadAvatar(ele, dizhi, orderid) {
    var ua = navigator.userAgent.toLowerCase(); //浏览器信息
    var info = {
        ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
        op: /opera/.test(ua),     //匹配Opera浏览器    
        sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
        ch: /chrome/.test(ua),     //匹配Chrome浏览器    
        ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
    };
    if (!info.ie) {
        if (ele.files[0].size > 20 * 1024 * 1024) {
            layerCommon.msg("只能上传20M以下的附件", IconOption.错误);
            return false;
        }
    }
    $.ajaxFileUpload(
        {
            type: "post",
            url: "../../Handler/HandleImg.ashx?orderid=" + orderid,            //需要链接到服务器地址
            secureuri: false,
            fileElementId: "AddBanner",                        //文件选择框的id属性
            dataType: "text",
            //服务器返回的格式，可以是json
            success: function (msgs, status)            //相当于try语句块的用法
            {
                if (msgs == "0") {
                    layerCommon.msg("附件上传失败", IconOption.错误);
                    return false;
                } else if (msgs == "1") {
                    layerCommon.msg("只能上传20M以下的附件", IconOption.错误);
                    return false;
                } else if (msgs == "2") {
                    //上传的文件支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP
                    layerCommon.msg("请确认您的文件上传格式", IconOption.错误);
                    return false;
                } else {
                    var msg = msgs.split("@#$")[0];
                    var msg2 = msgs.split("@#$")[1];
                    var url = dizhi + "OrderFJ/" + msg;
                    var name = msg.substring(0, msg.indexOf("^^")) + msg.substring(msg.lastIndexOf("."));
                    var html = "<li><a href=\"" + url + "\" class=\"name\" target=\"_blank\">" + name + "（大小：" + (msg2 / 1024).toFixed(2) + "KB）</a><a href=\"javascript:;\" tip=\"" + msg + "\" orderid=\"" + orderid + "\"  class=\"bule del\">删除</a><a href=\"" + url + "\" target=\"_blank\" class=\"bule\">下载</a></li>";
                    $(".attach .list").append(html);
                    var fj = $("#hrOrderFj").val();
                    if (fj == "") {
                        $("#hrOrderFj").val(msg + "@@");
                    } else {
                        $("#hrOrderFj").val(fj + msg + "@@");
                    }
                    if ($(".attach .list li").length >= 5) {
                        $(".attach .add").hide();
                    } else {
                        $(".attach .add").show();
                    }
                }
            }, error: function (msg, status, e)            //相当于java中catch语句块的用法
            {
                layerCommon.msg(msg + "," + status, IconOption.错误);
                return false;
            }
        })
}

function ordersale(ProID, Protype, Unit) {
    var str = "";

    if (ProID != "") {
        if (Protype != "") {
            var type = Protype.split(",");
            str += sale(ProID, type[0], type[1], type[2], type[3], Unit);
        }
    }
    return str;
}

function sale(ProID, proGoodsPrice, proDiscount, proTypes, ProType, unit) {
    //return str = '<div class="sale-box"><i class="sale">促销</i><div class="sale-txt"><i class="arrow"></i>满2件，总价打8.00折，参与订单满减活动</div></div>';

    var str = "";
    if (ProID.toString() == "")
        return "";
    else {
        str += '<div class="sale-box"><i class="sale">促销</i><div class="sale-txt"><i class="arrow"></i>'

        if (unit == "" || unit == null || typeof (unit) == "undefined")
            unit = "个";

        if (parseInt(proTypes) == 0) {
            //特价促销
            str += "特价商品";
        } else if (parseInt(proTypes) == 1) {
            //商品促销
            var Digits = $("#hidDigits").val();
            var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
            if (parseInt(ProType) == 3) {
                //商品促销满送
                str += "满" + parseFloat(proDiscount).toFixed(sDigits) + unit + " ，获赠商品（" + parseFloat(proGoodsPrice).toFixed(sDigits) + "）" + unit;
            }
            else if (parseInt(ProType) == 4) {
                //商品促销打折
                str += "在原订货价基础上打" + parseFloat(proDiscount / 10).toFixed(sDigits) + "折";
            }
        }
    }
    str += "</div></div>";

    return str;

};

//截取字符串
//截取的字符，截取长度，替换符号
function sub(val, len, rep) {
    var str = "";
    if (val.length > parseInt(len)) {
        str += val.substr(0, parseInt(len)) + rep;

    } else
        str += val

    return str;
};

/**
* 日期范围工具类
*/
var dateRangeUtil = (function () {
    /***
    * 获得当前时间
    */
    this.getCurrentDate = function () {
        return new Date();
    };

    /***
    * 获得本周起止时间
    */
    this.getCurrentWeek = function () {
        //起止日期数组  
        var startStop = new Array();
        //获取当前时间  
        var currentDate = this.getCurrentDate();
        //返回date是一周中的某一天  
        var week = currentDate.getDay();
        //返回date是一个月中的某一天  
        var month = currentDate.getDate();

        //一天的毫秒数  
        var millisecond = 1000 * 60 * 60 * 24;
        //减去的天数  
        var minusDay = week != 0 ? week - 1 : 6;
        //alert(minusDay);  
        //本周 周一  
        var monday = new Date(currentDate.getTime() - (minusDay * millisecond));
        //本周 周日  
        var sunday = new Date(monday.getTime() + (6 * millisecond));
        //添加本周时间  
        startStop.push(monday); //本周起始时间  
        //添加本周最后一天时间  
        startStop.push(sunday); //本周终止时间  
        //返回  
        return startStop;
    };
    /***
    * 获得本月的起止时间
    */
    this.getCurrentMonth = function () {
        //起止日期数组  
        var startStop = new Array();
        //获取当前时间  
        var currentDate = this.getCurrentDate();
        //获得当前月份0-11  
        var currentMonth = currentDate.getMonth();
        //获得当前年份4位年  
        var currentYear = currentDate.getFullYear();
        //求出本月第一天  
        var firstDay = new Date(currentYear, currentMonth, 2);


        //当为12月的时候年份需要加1  
        //月份需要更新为0 也就是下一年的第一个月  
        if (currentMonth == 11) {
            currentYear++;
            currentMonth = 0; //就为  
        } else {
            //否则只是月份增加,以便求的下一月的第一天  
            currentMonth++;
        }


        //一天的毫秒数  
        var millisecond = 1000 * 60 * 60 * 24;
        //下月的第一天  
        var nextMonthDayOne = new Date(currentYear, currentMonth, 1);
        //求出上月的最后一天  
        var lastDay = new Date(nextMonthDayOne.getTime() - millisecond);

        //添加至数组中返回  
        startStop.push(firstDay);
        startStop.push(lastDay);
        //返回  
        return startStop;
    };
    /***
    * 得到本年的起止日期
    * 
    */
    this.getCurrentYear = function () {
        //起止日期数组  
        var startStop = new Array();
        //获取当前时间  
        var currentDate = this.getCurrentDate();
        //获得当前年份4位年  
        var currentYear = currentDate.getFullYear();

        //本年第一天  
        var currentYearFirstDate = new Date(currentYear, 0, 2);
        //本年最后一天  
        var currentYearLastDate = new Date(currentYear, 11, 31);
        //添加至数组  
        startStop.push(currentYearFirstDate);
        startStop.push(currentYearLastDate);
        //返回  
        return startStop;
    };
    return this;
})();