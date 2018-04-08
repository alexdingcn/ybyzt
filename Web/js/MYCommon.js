
/*校验文本框，防止SQL注入*/
function validatesql(val)
{
	re= /select|update|delete|truncate|join|union|exec|insert|drop|count/i;
	return re.test(val);
}
/* 获取路径 */
function getRootPath() {  
    var pathName = window.location.pathname.substring(1);  
    var webName = pathName == '' ? '' : pathName.substring(0, pathName.indexOf('/'));  
    return window.location.protocol + '//' + window.location.host + '/' + webName + '/';  
}

function priceKeyup(val) {
    val.value = val.value.replace(/[^\d]/g, '');
}

function priceKeyup2(val) {
    val.value = val.value.replace(/[^\d]/g, '');
}

//整数验证
function priceBlur(val) {
    //val.value = val.value.replace(/[^\d]/g, ''), val.value != "" && (val.value = parseInt(val.value));
}
//整数、小数验证
function priceBlur2(val) {
    val.value = val.value.replace(/[^\d.]/g, ''), val.value != "" && (val.value = parseFloat(val.value));
}

//金额、费率、比例验证
function moneyKeyup(val) {
    var $amountInput = $(this);
    val = window.event || val;
    if (val.keyCode == 37 | val.keyCode == 39) {
        return;
    }
    $amountInput.val($amountInput.val().replace(/[^\d.]/g,"")).
    replace(/^\./g,"").replace(/\,{2,}/g,".").
    replace(".","$#$".replace(/\./g,"").replace)

}
function moneyKeyblur(val) {
    var $amountInput = $(this);
    //最后一位是小数点的话，移除
    val.val(($amountInput.val().replace(/\.$/g, "")));
}

//金额验证
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

//手机电话验证
function IsMobile(value) {
    var isMobile = /^0?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[0-9])[0-9]{8}$/;
    return isMobile.test($.trim(value));
}

//银行卡卡号验证
function IsBankNo(value) {
    var isMobile = /^\d+$/;
    return isMobile.test($.trim(value));
}
//邮箱验证
function IsEmail(value) {
    var isEM = /^([a-zA-Z0-9_-]{1,})+@([a-zA-Z0-9_-]{1,})+\.(com|cn|net)$/i;
    return isEM.test($.trim(value));
}
//验证身份证
function isIDCard(sId) {
    var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var iSum = 0;
    var info = "";
    if (!/^\d{17}(\d|x)$/i.test(sId)) return false; //return "你输入的身份证长度或格式错误";
    sId = sId.replace(/x$/i, "a");
    if (aCity[parseInt(sId.substr(0, 2))] == null) return false; //return "你的身份证地区非法";
    sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
    var d = new Date(sBirthday.replace(/-/g, "/"));
    if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) return false;  //return "身份证上的出生日期非法";
    for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
    if (iSum % 11 != 1) return false;// return "你输入的身份证号非法";
    //aCity[parseInt(sId.substr(0,2))]+","+sBirthday+","+(sId.substr(16,1)%2?"男":"女");//此次还可以判断出输入的身份证号的人性别
    return true;
}
//手机验证码验证
function IsCode(value) {
    var isMobile = /^\d{6}$/;
    return isMobile.test($.trim(value));
}

//金额验证
function IsNum(value) {
    var isMobile = /^\d{1,9}$/;
    return isMobile.test($.trim(value));
}
//邮编验证
function IsZip(value) {
    var isMobile = /^[1-9]\d{5}$/;
    return isMobile.test($.trim(value));
}
//Monny验证
function IsMoney(value) {
    var money = /^[0-9]*(\.{1}[0-9]{1,})?$/;
    return money.test($.trim(value));
}
var MYCommon = {
    ShowScoend: 4000,
    IsWait: true,
    dataType: "json",
    Posion: 2,
    //ajax异步请求   ActionUrl 请求地址，Data  请求数据 ，callback 回调函数，Isasync 请求同步或异步,errorMsg 错误回调函数 或者显示错误信息 ，AjaxStart ajax执行之前调用的方法
    AjaxRequest: function (ActionUrl, Data, callBack, Isasync, errorMsg, AjaxStart,secend) {
        Isasync = Isasync == undefined ? true : Isasync;
        $.ajax({
            url: ActionUrl,
            type: 'Post',
            data: Data,
            dataType: MYCommon.dataType,
            async:Isasync,
            timeout:8000,
            beforeSend: function () {
              typeof AjaxStart == "function" && AjaxStart();
            },
            success: function (result) {
                typeof callBack == "function" 
                && (MYCommon.IsWait && (setTimeout(function () { callBack(result) }, 250),!0) || callBack(result));
            },
            error: function (result) {
                typeof errorMsg == "function" && errorMsg(),!0 || layerCommon.msg(errorMsg, IconOption.哭脸);
            }
        });
    },
    //验证是否为空   并弹出指定提示
    Empty: function (CheckControl, NullMsg, type) {
        return ($.trim(CheckControl.val()) == "" || CheckControl.length==0) && LayerTip(CheckControl, NullMsg, type);
    },
    //div内容验证是否为空   并弹出指定提示
    TextEmpty: function (CheckControl, NullMsg, type) {
        return $.trim(CheckControl.text()) == "" && LayerTip(CheckControl, NullMsg, type);
    },
    //验证是否为空 并返回布尔值
    IsnullOrEmpty:function(val){
    	return !val || $.trim(val)=="";
    },
    EmptyHd: function (Control,showControl, NullMsg, type) {
        return ($.trim(Control.val()) == "" || Control.length==0) && LayerTip(showControl, NullMsg, type);
    },
    //验证输入的位数是否在指定的位数之间
    Length: function (CheckControl, Start, End, ErrorMsg, type) {
        return !($.trim(CheckControl.val()).length >= Start && $.trim(CheckControl.val()).length <= End) && LayerTip(CheckControl, ErrorMsg, type);
    },
    //验证是否通过
    IsCheck: function (CheckControl, Ischeck, ErrorMsg, type) {
    	CheckControl=CheckControl.offset().left==0?CheckControl.next():CheckControl;
        return !Ischeck && LayerTip(CheckControl, ErrorMsg, type);
    },
    //验证下拉选项是否未选中值
    NoSelect: function (CheckControl, DfValue, ErrorMsg) {
        return CheckControl.val() == DfValue && LayerTip(CheckControl, ErrorMsg);
    },
    //验证数值是否在指定范围以内
    Between: function (CheckControl, Start, End, ErrorMsg, type) {
        return !(parseFloat($.trim(CheckControl.val())) >= Start && parseFloat($.trim(CheckControl.val())) <= End) && LayerTip(CheckControl, ErrorMsg, type);
    },
    //验证数值是否大于0    modify by genggh   2016-12-26
    Betstart: function (CheckControl, Start,ErrorMsg, type) {
        return $.trim(CheckControl.val()) <= Start && LayerTip(CheckControl, ErrorMsg, type);
    },
    //验证起始值是否大于终止值
    CpareNumEnd: function (StartControl, EndControl, ErrorMsg) {
        return (parseFloat($.trim(StartControl.val())) > parseFloat($.trim(EndControl.val()))) && LayerTip(StartControl, ErrorMsg);
    },
    //验证起始日期是否大于终止日期
    CpareDateEnd: function (StartControl, EndControl, ErrorMsg) {
        return (new Date($.trim(StartControl.val()).replace(/-/g, "/")) > new Date($.trim(EndControl.val()).replace(/-/g, "/"))) && LayerTip(EndControl, ErrorMsg);
    },
    //金额输入框 键盘按下时调用的方法
    PriceKeyUp: function (Demo) {
        setTimeout(function () { Demo.value = Demo.value.replace(/[^\d.]/g, '') }, 200);
    },
   //金额输入框 失去焦点时调用的方法
    PriceKeyBlur: function (Demo) {
        Demo.value = Demo.value.replace(/[^\d.]/g, ''), Demo.value != "" && (Demo.value = parseFloat(Demo.value).toFixed(2));
    },
  //数字输入框 失去焦点时调用的方法
    NumKeyUp: function (Demo) {
        Demo.value = Demo.value.replace(/[^\d]/g, '');
    },
  //数字输入框 失去焦点时调用的方法
    NumKeyBlur: function (Demo) {
        Demo.value = Demo.value.replace(/[^\d]/g, '');
    },
    ShowJonKeys: function (Json, Keys) {
        var keys = Keys.split(","), ShowName = "";
        for (var key in keys) {
            ShowName += Json[keys[key]] + ",";
        }
        return ShowName != "" ? ShowName.substr(0, ShowName.length - 1) : "";
    },
    Textlenght: function (str) {
        var s = str;
        if (s.length>12) {
            return s.substr(0,12)+"..."
        }
        return s;
    },
    //发送短信验证码
    Sendcode:function(sendcontrol,sendtype,phone,uname){
    	return  $(sendcontrol).data("IsSend") == true || function(){
			 MYCommon.AjaxRequest(getRootPath()+"/commons/sendmsg", {sendtype:sendtype,phone:phone,uname:uname}, function(result){
				 if(result.success){
					 EndTime=60,
					 sendcode();
      			 }else{
      				 layerCommon.msg(result.msg, IconOption.哭脸);
      				$(sendcontrol).data("IsSend", false).removeClass("btnDisable").text("获取验证码");
      			 }
			 }, function(){
				 layerCommon.msg("验证码发送失败", IconOption.哭脸);
				 $(sendcontrol).data("IsSend", false).removeClass("btnDisable").text("获取验证码");
			 },function(){
				 $(sendcontrol).data("IsSend",true).addClass("btnDisable").text("正在发送中..");
			 })
		 }(),!0;
    	var EndTime = 0; 
    	function sendcode(){
    		if (EndTime > 0) {
                $(sendcontrol).text("" + EndTime + "s后重新获取");
                EndTime--;
                setTimeout(function () { sendcode() }, 1000);
            } else {
                $(sendcontrol).data("IsSend", false).removeClass("btnDisable").text("获取验证码");
            }
    	}
    }
}



var LayerTip = function (Control, Msg, type) {
    type ? layerCommon.msg(Msg, IconOption.哭脸) : layerCommon.tip(Msg, Control, { tips: [MYCommon.Posion, "Red"], time: MYCommon.ShowScoend }); !Control.hasClass("Wdate") && Control.focus();
    return !0;
}
String.prototype.formatDate = function (IsTime) {
    return IsTime ? this.toString().substr(0, 19).replace(/[-]/g, "/").replace(/[T]/g, " ") : this.toString().substr(0, 10).replace(/[-]/g, "/");
}
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

//long 类型的时间 转换成Date 并格式化
Number.prototype.timeformatDate = function (format) {
	var covertdate=new Date(this);
	return covertdate.format(format);
}

//重新js的toFixed方法
Number.prototype.toFixed = function (s) {
    changenum = (parseInt(this * Math.pow(10, s) + 0.5) / Math.pow(10, s)).toString();
    index = changenum.indexOf(".");
    if (index < 0 && s > 0) {
        changenum = changenum + ".";
        for (i = 0; i < s; i++) {
            changenum = changenum + "0";
        }

    } else {
        index = changenum.length - index;
        for (i = 0; i < (s - index) + 1; i++) {
            changenum = changenum + "0";
        }

    }
    return changenum;
}

//重写js除法运算
Number.prototype.div = function (arg){
	function accDiv(arg1,arg2){
	    var t1=0,t2=0,r1,r2;
	    try{t1=arg1.toString().split(".")[1].length}catch(e){}
	    try{t2=arg2.toString().split(".")[1].length}catch(e){}
	    with(Math){
	        r1=Number(arg1.toString().replace(".",""))
	        r2=Number(arg2.toString().replace(".",""))
	        return (r1/r2)*pow(10,t2-t1);
	    }
	}
    return accDiv(this, arg);
    
}

//重写js乘法运算
Number.prototype.mul = function (arg){
	function accMul(arg1,arg2){    
		var m=0,s1=arg1.toString(),  
		s2=arg2.toString();    
		try{  
		m+=s1.split(".")[1].length}catch(e){}    
		try{  
		m+=s2.split(".")[1].length}catch(e){}    
		return Number(s1.replace(".",""))*Number(s2.replace(".",""))/Math.pow(10,m  
		)} 
    return accMul(arg, this);
}

//重写js加法运算
Number.prototype.add = function (arg){
	function accAdd(arg1,arg2){
	    var r1,r2,m;
	    try{r1=arg1.toString().split(".")[1].length}catch(e){r1=0}
	    try{r2=arg2.toString().split(".")[1].length}catch(e){r2=0}
	    m=Math.pow(10,Math.max(r1,r2))
	    return (arg1*m+arg2*m)/m
	}
    return accAdd(arg,this);
}

//重写js减法运算
Number.prototype.sub = function (arg){
	function accsubtract(arg1,arg2){
	    var r1,r2,m;
	    try{r1=arg1.toString().split(".")[1].length}catch(e){r1=0}
	    try{r2=arg2.toString().split(".")[1].length}catch(e){r2=0}
	    m=Math.pow(10,Math.max(r1,r2))
	    return (arg1*m-arg2*m)/m
	}
    return accsubtract(this,arg);
}


//验证是否合法有效的日期
function IsDate(RQ) {
    var date = RQ;
    var result = date.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);

    if (result == null)
        return false;
    var d = new Date(result[1], result[3] - 1, result[4]);
    return (d.getFullYear() == result[1] && (d.getMonth() + 1) == result[3] && d.getDate() == result[4]);
}

$.fn.extend({ defaultValue: function () {
    var c = function () {
        var a = document.createElement("input");
        return "placeholder" in a
    } ();
    c ? !1 : this.each(function (index, control) {
        $(control).data("defaultValued", $(control).attr("placeholder")),
        $(control).val($(control).data("defaultValued")),
        $(control).undelegate("focus").delegate("", "focus", function () {
            $.trim($(this).val()) == $(this).data("defaultValued") && $(this).val("");
        }),
        $(control).undelegate("blur").delegate("", "blur", function () {
        	$.trim($(this).val()) == "" && $(this).val($(this).data("defaultValued"));
            
        })
    });
    return this;
}
});

//判断是否下滑到屏幕底部(弹出层)
//HidCrdex  当前页数ID
//HidMaxPage 最大页数ID
function IsScreenbot(HidCrdex, HidMaxPage) {
    var scrollTop = parseInt($(".Div_flow").scrollTop());
    var scrollHeight = parseInt($(".o-p-list").height());
    var windowHeight = parseInt($(".Div_flow").height());
    if (parseInt($("#" + HidCrdex).val()) < parseInt($("#" + HidMaxPage).val()) && scrollTop + windowHeight + 1 >= scrollHeight) {
        return true;
    }
    return false;
}

//判断是否下滑到屏幕底部
//HidCrdex  当前页数ID
//HidMaxPage 最大页数ID
function IsWindow(HidCrdex, HidMaxPage) {
    var scrollTop = parseInt($(document).scrollTop());
    var scrollHeight = parseInt($("body").height());
    var windowHeight = parseInt($(window)[0].innerHeight);
    return parseInt($("#" + HidCrdex).val()) < parseInt($("#" + HidMaxPage).val()) && scrollTop + windowHeight + 1 >= scrollHeight;
}

function FullListFail() {
    $("#footer").show().children("span:eq(0)").hide().next().html("网点数据加载失败");
    setTimeout(function () {
        $("#footer").hide();
        $(document).scrollTop($("body").height() - $(window)[0].innerHeight - 5);
        MyPagingFunc.BindPagin();
    }, 2500)
}


//全局变量
var num_length = 8, num_saml = 2;  					 //数量长度是8位（加小数位），2位小数点
var price_length = 10, price_saml = 4; 		 //单价长度 是10位（加小数位），4位小数
var nsl_length = 5, nsl_saml = 2;
var number_len = 10;
//税率长度是5位（加小数位），2位长度
//创建时间：2017-03-29
//创建人：耿国行
//内容：数量小数位判断（2位小数，总长度8）
function NumberOfval(demo, event) {
    event = window.event || event;
    var keyCode = event.keyCode || event.charCode;
    if (keyCode == 8 || (keyCode == 46 && event.code == "Delete") || keyCode == 37 || keyCode == 39) {
        return;
    }
    if ((keyCode >= 48 && keyCode <= 57) || keyCode == 46) {
        if (demo.value.length == num_length) {
            event.returnValue = false;
            event.preventDefault();
            return;
        }
        var cursurPosition = findSelection(demo);
        if (keyCode == 46) {
            var vs = String.fromCharCode(keyCode);
            if (demo.value.indexOf(vs) > 0 || demo.value.length == 0 || cursurPosition == 0) {
                event.returnValue = false;
                event.preventDefault();
            }
        } else {
            if (demo.value.indexOf(".") > 0) {
                var vs = String.fromCharCode(keyCode);
                var aNew = demo.value.replace(/([^.]*).*/, "$1");
                aNew = demo.value.replace(aNew + ".", "").length;
                if (aNew >= num_saml) { //控制小数位
                    if (cursurPosition > demo.value.length - 3) {
                        event.returnValue = false;
                        event.preventDefault();
                    }
                }
            }
        }
    }
    else {
        event.returnValue = false;
        event.preventDefault();
    }
}

//创建时间：2017-03-29
//创建人：耿国行
//内容：单价小数位判断（4位小数，总长度10）
function PriceOfval(demo, event) {
    event = window.event || event;
    var keyCode = event.keyCode || event.charCode;
    if (keyCode == 8 || (keyCode == 46 && event.code == "Delete") || keyCode == 37 || keyCode == 39) {
        return;
    }
    if ((keyCode >= 48 && keyCode <= 57) || keyCode == 46) {
        if (demo.value.length == price_length) {
            event.returnValue = false;
            event.preventDefault();
            return;
        }
        var cursurPosition = findSelection(demo);
        if (keyCode == 46) {
            var vs = String.fromCharCode(keyCode);
            if (demo.value.indexOf(vs) > 0 || demo.value.length == 0 || cursurPosition == 0) {
                event.returnValue = false;
                event.preventDefault();
            }
        } else {
            if (demo.value.indexOf(".") > 0) {
                var vs = String.fromCharCode(keyCode);
                var aNew = demo.value.replace(/([^.]*).*/, "$1");
                aNew = demo.value.replace(aNew + ".", "").length;
                if (aNew >= price_saml) {
                    if (cursurPosition > demo.value.length - 3) {
                        event.returnValue = false;
                        event.preventDefault();
                    }
                }
            }
        }
    }
    else {
        event.returnValue = false;
        event.preventDefault();
    }

}

//创建时间：2017-03-29
//创建人：耿国行
//内容：失去焦点检查，把无效的0去掉。
function keyBlue(Demo) {
    Demo.value = Demo.value.replace(/[^\d.]/g, '')
    if (Demo.value != "")
        Demo.value = parseFloat(Demo.value);

    if (Demo.value.toString().indexOf(".") > -1) {
        var nu = Demo.value.substring(Demo.value.toString().indexOf(".") + 1);
        if (parseFloat(nu) == 0)
            Demo.value = Demo.value.substring(0, Demo.value.toString().indexOf("."));
    }
}

//获取当前文本数字的长度
//创建时间：2017-03-29
function findSelection(demo) {
    var cursurPosition = -1;
    if (typeof (demo.selectionStart) == "number") {//非IE浏览器
        cursurPosition = demo.selectionStart;
    } else {//IE
        var range = document.selection.createRange();
        range.moveStart("character", -1 * demo.value.length);
        cursurPosition = range.text.length;
    }
    return cursurPosition;
}

//只能输入数字
function NumberPress(demo, event) {
    event = window.event || event;
    var keyCode = event.keyCode || event.charCode;
    if (keyCode == 8 || (keyCode == 46 && event.code == "Delete") || keyCode == 37 || keyCode == 39) {
        return;
    }
    if ((keyCode >= 48 && keyCode <= 57) || keyCode == 46) {
        if (demo.value.length >= number_len) {
            event.returnValue = false;
            event.preventDefault();
            return;
        }
    }
    else {
        event.returnValue = false;
        event.preventDefault();
    }
}
//只能输入数字
function Numberblur(demo) {
    demo.value = demo.value.replace(/[^\d]/g, '');
}

/**       
* 时间戳转换日期       
* @param <int> unixTime  待时间戳(秒)       
* @param <int> isFull  返回完整时间(1:Y-m-d 2: Y-m-d H:i:s 3:m/d 4:H:i)       
* @param <int> timeZone  时区 
* @param szj      
*/
function UnixToDate(unixTime, isFull, timeZone) {
    var now = new Date(); //获取当前时间

    if (unixTime == "" || unixTime == null)
        return "";

    if (typeof (unixTime) != "object") { 
        if (unixTime.indexOf("Date") >= 0) {
            var dte = parseInt(unixTime.replace("/Date(", "").replace(")/", ""), 10);
            if (dte < 0)
                return "";
            now = new Date(dte);
        } else {
            now = new Date(unixTime);
        }
    } else
        now = unixTime;
    
    var year = now.getFullYear();//年
    var month = now.getMonth() + 1;//月
    var date = now.getDate();//日
    var hour = now.getHours() + "";//时
    var minute = now.getMinutes() + "";//分
    var second = now.getSeconds() + "";//秒

    month < 10 && (month = "0" + month);
    date < 10 && (date = "0" + date);
    hour < 10 && (hour = "0" + hour);
    minute < 10 && (minute = "0" + minute);
    second < 10 && (second = "0" + second);
    if (isFull == 2)
        return year + "-" + month + "-" + date + " " + hour + ":" + minute;
    else if (isFull == 3)
        return month + "/" + date;
    else if (isFull == 4)
        return hour + ":" + minute;
    else if (isFull == 6)
        return hour + ":" + minute + ":" + second;
    else if (isFull == 5)
        return year + "-" + month + "-" + date + " " + hour + ":" + minute;
    else
        return year + "-" + month + "-" + date;
}

//字符串转成Time(dateDiff)所需方法 
function stringToTime(string) {
    var f = string.split(' ', 2);
    var d = (f[0] ? f[0] : '').split('-', 3);
    var t = (f[1] ? f[1] : '').split(':', 3);
    return (new Date(parseInt(d[0], 10) || null, (parseInt(d[1], 10) || 1) - 1, parseInt(d[2], 10) || null, parseInt(t[0], 10) || null, parseInt(t[1], 10) || null, parseInt(t[2], 10) || null)).getTime();
}


/*
*
*截取字符串
*截取的字符，截取长度，替换符号
* szj
**/
function sub(val, len, rep) {
    var str = "";
    if (val != null && val.length > parseInt(len)) {
        str += val.substr(0, parseInt(len)) + rep;

    } else if (val == null)
        str += "";
    else
        str += val

    return str;
};



/*
*
*获取页面的高度、宽度
* szj
* 2017年6月8日, PM 12:05:55
**/
function getPageSize() {
    var xScroll, yScroll;
    if (window.innerHeight && window.scrollMaxY) {
        xScroll = window.innerWidth + window.scrollMaxX;
        yScroll = window.innerHeight + window.scrollMaxY;
    } else {
        if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac    
            xScroll = document.body.scrollWidth;
            yScroll = document.body.scrollHeight;
        } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari    
            xScroll = document.body.offsetWidth;
            yScroll = document.body.offsetHeight;
        }
    }
    var windowWidth, windowHeight;
    if (self.innerHeight) { // all except Explorer    
        if (document.documentElement.clientWidth) {
            windowWidth = document.documentElement.clientWidth;
        } else {
            windowWidth = self.innerWidth;
        }
        windowHeight = self.innerHeight;
    } else {
        if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode    
            windowWidth = document.documentElement.clientWidth;
            windowHeight = document.documentElement.clientHeight;
        } else {
            if (document.body) { // other Explorers    
                windowWidth = document.body.clientWidth;
                windowHeight = document.body.clientHeight;
            }
        }
    }
    // for small pages with total height less then height of the viewport    
    if (yScroll < windowHeight) {
        pageHeight = windowHeight;
    } else {
        pageHeight = yScroll;
    }
    // for small pages with total width less then width of the viewport    
    if (xScroll < windowWidth) {
        pageWidth = xScroll;
    } else {
        pageWidth = windowWidth;
    }
    arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight);
  
    return arrayPageSize;
}

/*
*
*获取页面可视区域的宽度
* szj
* 2017年6月8日, PM 12:05:55
**/
function $windowHeight() {
    var psize = getPageSize();
    var $hei = parseFloat(psize[3]) + parseFloat(80);
    return $hei;

}