
function compareNine(value) {
      return value > 9 ? value : '0' + value;
 }
/**       
* 时间戳转换日期       
* @param <int> unixTime  待时间戳(秒)       
* @param <int> isFull  返回完整时间(1:Y-m-d 2: Y-m-d H:i:s 3:m/d 4:H:i)       
* @param <int> timeZone  时区 
* @param szj      
*/
function UnixToDate(value) {
    ///.../gi是用来标记正则开始和结束；\是转义符；()标注了正则匹配分组1，$1 
    var now = eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    if (isNaN(now)) {
        return "";
    }
    //直接借助datapattern.js扩展 return now.pattern('yyyy-MM-dd hh:mm:ss');
    //或者使用下面方式计算
    var year = now.getYear() + 1900;//或者 now.getFullYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    return year + "-" + compareNine(month) + "-" + compareNine(date);
}

//	伪下拉框
function beginSelect(elem) {
    if (elem.className == "btn") {
        elem.className = "btn btnhover"
        elem.onmouseup = function () {
            this.className = "btn"
        }
    }
    var ul = elem.parentNode.parentNode;
    var li = ul.getElementsByTagName("li");
    var selectArea = li[li.length - 1];
    if (selectArea.style.display == "block") {
        selectArea.style.display = "none";
    }
    else {
        selectArea.style.display = "block";
        mouseoverBg(selectArea);
    }
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
function mouseoverBg(elem1) {
    var input = elem1.parentNode.getElementsByTagName("input")[0];
    var p = elem1.getElementsByTagName("p");
    var pLength = p.length;
    for (var i = 0; i < pLength; i++) {
        p[i].onmouseover = showBg;
        p[i].onmouseout = showBg;
        p[i].onclick = postText;
    }
    function showBg() {
        this.className == "hover" ? this.className = " " : this.className = "hover";
    }
    function postText() {
        var selected = this.innerHTML;
        input.setAttribute("value", selected);
        elem1.style.display = "none";
    }
}
//禁用Enter
document.onkeydown = function (event) {
    var target, code, tag;
    if (!event) {
        event = window.event; //针对ie浏览器  
        target = event.srcElement;
        code = event.keyCode;
        if (code == 13) {
            tag = target.tagName;
            if (tag == "TEXTAREA") { return true; }
            else { return false; }
        }
    }
    else {
        target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
        code = event.keyCode;
        if (code == 13) {
            tag = target.tagName;
            if (tag == "INPUT") { return false; }
            else { return true; }
        }
    }
}
/************************* 订单详细 start **************************************/
$(function () {

    var ostate = $("#hidOstate").val();
    if (~ ~ostate > 2) {
        $(".order").find("a.edit-i").css("display", "none");
        //$(".add").css("display", "none");
        $(".list").find("a.attrdel").css("display", "none");

        var usertype = $("#hidUserType").val();
        if (usertype == 3 || usertype == 4) {
            $(".payamount").css("display", "none");
            $(".postfee").css("display", "none"); //inline-block

            //除作废订单不能改发票
            if (~ ~ostate != 6) {
                if (~ ~($("#hidisBill").val()) == 0)
                    $(".addbill").css("display", "inline-block");
            }
        }
    } else {
        var usertype = $("#hidUserType").val();
        if (usertype == 3 || usertype == 4) {
            var paystate = $("#hidpaystate").val();
            if (~ ~paystate == 0 && ~ ~ostate < 2) {
                $(".payamount").css("display", "inline-block");
                $(".postfee").css("display", "inline-block");
            } else {
                $(".payamount").css("display", "none");
                $(".postfee").css("display", "none");
            }
        }
    }

    $(".goods-flow").find("li").attr("class", "");

    if (~ ~ostate == 5 || ~ ~ostate == 6 || ~ ~ostate == 7) {
        $(".goods-flow").find("li[tip=\"5\"]").prevAll().attr("class", "cur");
        $(".goods-flow").find("li[tip=\"5\"]").attr("class", "cur");
        $(".del").css("display", "none");
        $(".addfj").css("display", "none");
        if (~ ~ostate == 6) {
            $(".ordercancel").css("display", "inline-block");
        }
    }
    else if (~ ~ostate == -1 || ~ ~ostate == 0 || ~ ~ostate == 1) {
        $(".goods-flow").find("li[tip=\"1\"]").prevAll().attr("class", "cur");
        $(".goods-flow").find("li[tip=\"1\"]").attr("class", "cur");
        $(".del").css("display", "inline-block");
    }
    else if (~ ~ostate == 3) {
        $(".goods-flow").find("li[tip=\"5\"]").prevAll().attr("class", "cur");
        $(".goods-flow").find("li[tip=\"5\"]").attr("class", "cur");
        $(".addfj").css("display", "none");
    }
    else {
        var isoutstate = $.trim($("#hidIsOutstate").val());
        if (parseInt(isoutstate) == 1 || parseInt(isoutstate) == 2) {
            $(".goods-flow").find("li[tip=\"2\"]").prevAll().attr("class", "cur");
            $(".goods-flow").find("li[tip=\"2\"]").attr("class", "cur");
        } else {
            $(".goods-flow").find("li[tip=\"" + ostate + "\"]").prevAll().attr("class", "cur");
            $(".goods-flow").find("li[tip=\"" + ostate + "\"]").attr("class", "cur");
        }
        if (~ ~ostate == 2) {
            $(".del").css("display", "inline-block");
            $(".addfj").css("display", "inline-block");
        } else {
            $(".del").css("display", "none");
            $(".addfj").css("display", "none");
        }
    }

    $(document).on("click", ".goods-title li", function () {
        var tip = $(this).attr("tip");
        var ostate = $("#hidOstate").val();

        //订单作废
        //        if (~ ~ostate == 6) {
        //            $(".order").attr("style", "display:block;");
        //            $(this).siblings("li").attr("class", "");
        //            $(".goods-title").find("li:first").attr("class", "hover");
        //        } else {
        $(this).attr("class", "hover");
        $(this).siblings("li").attr("class", "");

        if (~ ~tip == 1) {
            //订单
            $(".order").attr("style", "display:block;");
            $(".goodstakn").attr("style", "display:none;");
            $(".payment").attr("style", "display:none;");

        } else if (~ ~tip == 2) {
            //收货
            $(".order").attr("style", "display:none;");
            $(".goodstakn").attr("style", "display:block;");
            $(".payment").attr("style", "display:none;");
        } else if (~ ~tip == 3) {
            //付款
            $(".order").attr("style", "display:none;");
            $(".goodstakn").attr("style", "display:none;");
            $(".payment").attr("style", "display:block;");
        }
        //}
    });

    //查看返利
    $(document).on("click", ".seebate", function () {
        var KeyID = $("#hidOrderID").val();
        var DisID = $("#hidDisID").val();

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/DisReBate.aspx?DisID=' + DisID + '&type=0&KeyID=' + KeyID;
        var index = layerCommon.openWindow("查看返利", url, '575px', '415px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });

    //交货时期、配送方式
    $(document).on("click", ".po_deli", function () {
        var tip = $(this).attr("tip");
        var KeyID = $("#hidOrderID").val();
        var DisID = $("#hidDisID").val();

        var tipval = "";
        var name = "";
        if (~ ~tip == 0) {
            name = "交货日期";
            tipval = $("#lblArriveDate").text();
        }
        else {
            name = "配送方式";
            tipval = $("#lblGiveMode").text();
        }
        //转向网页的地址; 
        var url = '../../Distributor/newOrder/po_deli.aspx?DisID=' + DisID + '&type=' + tip + "&KeyID=" + KeyID + "&tipval=" + tipval;
        var index = layerCommon.openWindow(name, url, '505px', '345px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //订单备注
    $(document).on("click", ".orderRemark", function () {
        var KeyID = $("#hidOrderID").val();
        var DisID = $("#hidDisID").val();

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/remarkview.aspx?DisID=' + DisID + '&type=0&KeyID=' + KeyID;
        var index = layerCommon.openWindow("订单备注", url, '730px', '445px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //商品备注
    $(document).on("click", ".addRemark", function () {
        var KeyID = $(this).attr("tip");
        //var Remark = $(this).siblings("div[class=\"cur\"]").text();
        var DisID = $("#hidDisID").val();

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/remarkview.aspx?DisID=' + DisID + '&type=1&KeyID=' + KeyID;
        var index = layerCommon.openWindow("商品备注", url, '730px', '445px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //收货地址
    $(document).on("click", ".addr", function () {
        var DisID = $("#hidDisID").val();
        var AddrID = $("#hidAddrID").val();

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/addrinfo.aspx?DisID=' + DisID + '&AddrID=' + AddrID;
        var index = layerCommon.openWindow("收货地址", url, '750px', '590px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //开票信息
    $(document).on("click", ".Invoi", function () {

        var disID = stripscript($("#hidDisID").val());
        var val = stripscript($("#hidval").val());
        var id = stripscript($("#hidDisAccID").val());
        var lookup = stripscript($("#lblRise").text());
        var context = stripscript($("#lblContent").text());
        var bank = stripscript($("#lblOBank").text());
        var account = stripscript($("#lblOAccount").text());
        var regno = stripscript($("#lblTRNumber").text());

        //转向网页的地址; 
        var url = "../../Distributor/newOrder/InvoiceInfo.aspx?DisId=" + disID + "&val=" + val + "&Rise=" + lookup + "&Context=" + context + "&Bank=" + bank + "&Account=" + account + "&RegNo=" + regno;

        var index = layerCommon.openWindow("开票信息", url, '575px', '445px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });

    //查看物流信息
    $(document).on("click", ".Logistics", function () {

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/logistview.aspx?KeyID=' + $.trim($(this).attr("tip"));
        var index = layerCommon.openWindow("查看物流信息", url, '600px', '500px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });

    //确认收货
    $(document).on("click", ".btnsign", function () {
        var outID = $.trim($(this).attr("tip"));
        var oID = $.trim($("#hidOrderID").val());
        var th = this;
        var ts = $.trim($(this).attr("dts"));
        var str = "";

        $(this).closest("div.tabLine").find("table tbody tr").each(function (item) {
            var oldid = $(this).attr("ttrd");

            var batchno = $.trim($(this).find("td:eq(-2)").find("div.tc").find("input[type=\"text\"][class*=\"BatchNO\"]").val());

            var validDate = $.trim($(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").val());
            str += oldid + "：" + batchno + "：" + validDate + "；";
        });

        $.ajax({
            type: 'post',
            url: '../../Handler/orderHandle.ashx',
            data: { ck: Math.random(), ActionType: "Sign", oID: oID, outID: outID, ts: ts, str: str },
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    layerCommon.msg("请尽快进行入库操作！", IconOption.笑脸, 3000, function () {
                        window.location.href = 'orderdetail.aspx?top=5&KeyID=' + oID;
                    });
                } else
                    layerCommon.msg(data.Msg, IconOption.错误);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    });

    //再次购买
    $(document).on("click", "#buyagain", function () {
        var oID = $.trim($("#hidOrderID").val());

        var usertype = $("#hidUserType").val();
        if (usertype == 3 || usertype == 4) {
            window.location.href = '../../../Company/newOrder/orderBuy.aspx?type=2&KeyID=' + oID;
            return;
        } else if (usertype == 1 || usertype == 5) {
            window.location.href = 'orderBuy.aspx?type=2&KeyID=' + oID;
            return;
        }

        //        $.ajax({
        //            type: 'post',
        //            url: '../../Handler/orderHandle.ashx',
        //            data: { ck: Math.random(), ActionType: "Buy", oID: oID, DisID: $.trim($("#hidDisID").val()) },
        //            dataType: 'json',
        //            success: function (data) {
        //                if (data.Result) {
        //                    var usertype = $("#hidUserType").val();
        //                    if (usertype == 3 || usertype == 4) {
        //                        window.location.href = '../../../Company/newOrder/orderdetail.aspx?KeyID=' + data.Code;
        //                    } else {
        //                        window.location.href = '../orderdetail.aspx?KeyID=' + data.Code;
        //                    }
        //                } else
        //                    layerCommon.msg(data.Msg, IconOption.错误);
        //            },
        //            error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
        //            }
        //        });
    });

    //订单作废确认
    $(document).on("click", "#ordervoid", function () {
        $(".po-bg").attr("class", "po-bg");
        $("#p-delete").attr("class", "popup p-delete");
        $(".btn-area").attr("class", "btn-area canOrderSave");
    });

    //订单作废确认取消
    $(document).on("click", ".canOrder", function () {
        $(".po-bg").attr("class", "po-bg none");
        $("#p-delete").attr("class", "popup p-delete none");
        $(".btn-area").attr("class", "btn-area");
    });


    //订单作废
    $(document).on("click", ".canOrderSave", function () {
        var oID = $.trim($("#hidOrderID").val());
        var ts = $.trim($("#hidDts").val());

        $.ajax({
            type: 'post',
            url: '../../Handler/orderHandle.ashx',
            data: { ck: Math.random(), ActionType: "Cancel", oID: oID, ts: ts },
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    location = location;
                    //window.location.href = "orderdetail.aspx?KeyID=" + oID;    //刷新页面
                    //再次购买
                    //                    $("#buyagain").css("display", "inline-block");
                    //                    //订单修改
                    //                    $("#modifyorder").css("display", "none");
                    //                    //支付
                    //                    $("#btn_pay").css("display", "none");

                    //                    //订单作废
                    //                    $("#ordervoid").css("display", "none");
                    //                    $("#lblOstate").text("已作废");
                    //                    $(".ordercancel").css("display", "inline-block");

                    //                    $("#hidOstate").val(6);

                    //                    $(".goods-flow").find("li").attr("class", "");
                    //                    $(".goods-flow").find("li[tip=\"5\"]").prevAll().attr("class", "cur");
                    //                    $(".goods-flow").find("li[tip=\"5\"]").attr("class", "cur");

                    //                    $(".goods-title").find("li").attr("class", "");
                    //                    $(".goods-title").find("li:first").attr("class", "hover");
                    //                    $(".order").find("a.edit-i").css("display", "none");
                    //                    $(".addRemark").remove();

                    //                    var usertype = $("#hidUserType").val();
                    //                    if (usertype == 3 || usertype == 4) {
                    //                        //订单审核
                    //                        $("#orderaudit").css("display", "none");
                    //                        $(".payamount").css("display", "none");
                    //                        $(".postfee").css("display", "none");
                    //                    }
                    //                    layerCommon.msg("订单作废成功", IconOption.笑脸);
                } else
                    layerCommon.msg(data.Msg, IconOption.错误);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
        $(".po-bg").attr("class", "po-bg none");
        $("#p-delete").attr("class", "popup p-delete none");
        $(".btn-area").attr("class", "btn-area");
    });

    //打印订单
    $(document).on("click", "#orderprint", function () {
        var oID = $.trim($("#hidOrderID").val());
        var DisID = $.trim($("#hidDisID").val());

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/printorder.aspx?KeyID=' + oID + '&DisID=' + DisID;
        //var index = layerCommon.openWindow("打印订单", url, '1000px', '600px'); //记录弹出对象
        //$("#hid_Alert").val(index); //记录弹出对象

        var name = '订单打印';                     //网页名称，可为空; 
        var iWidth = 850;                          //弹出窗口的宽度; 
        var iHeight = 600;                    //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
        //获得窗口的水平位置 
        var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
        window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft);
    });

    //发货打印
    $(document).on("click", ".print", function () {
        var oID = $.trim($(this).attr("tip"));
        var DisID = $.trim($("#hidDisID").val());

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/printout.aspx?KeyID=' + oID + '&DisID=' + DisID;
        //var index = layerCommon.openWindow("打印订单", url, '1000px', '600px'); //记录弹出对象
        //$("#hid_Alert").val(index); //记录弹出对象

        var name = '发货单打印';                     //网页名称，可为空; 
        var iWidth = 850;                          //弹出窗口的宽度; 
        var iHeight = 600;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
        //获得窗口的水平位置 
        var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
        window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft);
        //+',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no'
    });

    //查看合约
    $(document).on("click", ".contract", function () {
        var oID = $.trim($("#hidOrderID").val());
        var index = layerCommon.openWindow('交易合约', '../../Distributor/Contract.aspx?KeyID=' + oID, '605px', '545px');  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });

    /******** 厂商 start *********/

    //订单审核确认
    //    $(document).on("click", "#orderaudit", function () {
    //        $(".po-bg").attr("class", "po-bg");
    //        $("#divAdiut").attr("class", "popup p-delete");
    //    });

    //订单审核确认取消
    //    $(document).on("click", ".canNoAudit", function () {
    //        $(".po-bg").attr("class", "po-bg none");
    //        $("#divAdiut").attr("class", "popup p-delete none");
    //    });

    //订单审核
    $(document).on("click", "#orderaudit", function () {
        var oID = $("#hidOrderID").val();
        // $("#divAdiut").attr("class", "popup p-delete none");
        var ts = $.trim($("#hidDts").val());

        $.ajax({
            type: 'post',
            url: '../../Handler/orderHandle.ashx',
            data: { ck: Math.random(), ActionType: "orderAudit", oID: oID, ts: ts },
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    //                    $("#orderaudit").attr("style", "display:none;");
                    //                    $("#modifyorder").attr("style", "display:none;");
                    //                    $(".goods-flow").find("li").attr("class", "");
                    //                    $(".goods-flow").find("li[tip=\"" + data.Code + "\"]").prevAll().attr("class", "cur");
                    //                    $(".goods-flow").find("li[tip=\"" + data.Code + "\"]").attr("class", "cur");
                    //                    $("#lblOstate").text("待发货");
                    //                    layerCommon.msg("审核成功", IconOption.笑脸);

                    //window.location.href = "orderdetail.aspx?KeyID=" + oID;    //刷新页面
                    location = location;
                } else
                    layerCommon.msg(data.Msg, IconOption.错误);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    });

    //退货申请
    $(document).on("click", "#orderReturn", function () {
        var oID = $("#hidOrderID").val();
        /// <reference path="../../ReturnOrderAdd.aspx?KeyID="+oID />
        var index = layerCommon.openWindow('申请退货', '../ReturnOrderAdd.aspx?KeyID=' + oID, '480px', '230px');  //记录弹出对象
        $("#hid_Alert").val(index);
    });

    //修改运费
    $(document).on("click", ".postfee", function () {
        var DisID = $("#hidDisID").val();
        var oID = $("#hidOrderID").val();
        var tatol = $("#lblPostFee").text();

        //转向网页的地址; 
        var url = '../../../Company/newOrder/amountof.aspx?type=1&DisID=' + DisID + '&KeyID=' + oID + '&t=' + tatol;
        var index = layerCommon.openWindow("修改运费", url, '505px', '355px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //应付总额
    $(document).on("click", ".payamount", function () {
        var DisID = $("#hidDisID").val();
        var oID = $("#hidOrderID").val();
        var tatol = $("#lblAuditAmount").text();

        //转向网页的地址; 
        var url = '../../../Company/newOrder/amountof.aspx?type=0&DisID=' + DisID + '&KeyID=' + oID + '&t=' + tatol;
        var index = layerCommon.openWindow("应付总额", url, '505px', '355px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //发票信息
    $(document).on("click", ".addbill", function () {
        var DisID = stripscript($("#hidDisID").val());
        var oID = stripscript($("#hidOrderID").val());
        var BillNo = stripscript($("#lblBillNo").text());
        var IsBill = $("#lblIsBill").attr("tip");

        //转向网页的地址; 
        var url = '../../../Company/newOrder/billinfo.aspx?DisID=' + DisID + '&KeyID=' + oID + '&BillNo=' + BillNo + '&IsBill=' + IsBill;
        var index = layerCommon.openWindow("发票信息", url, '505px', '355px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });

    //增加发货数量
    $(document).on("click", ".add", function () {
        var Digits = $("#hidDigits").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var Nonum = $(this).siblings("input[type=\"hidden\"][class=\"Notshipnum\"]").val();
        var num = $(this).siblings("input[type=\"text\"][class*=\"txtGoodsNum\"]").val();
        num = parseFloat(num) + parseFloat(1);

        if (parseFloat(num) <= parseFloat(Nonum))
            $(this).siblings("input[type=\"text\"][class*=\"txtGoodsNum\"]").val(parseFloat(num).toFixed(sDigits));
    });

    //减少发货数量
    $(document).on("click", ".minus", function () {
        var Digits = $("#hidDigits").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;

        var Nonum = $(this).siblings("input[type=\"hidden\"][class=\"Notshipnum\"]").val();
        var num = $(this).siblings("input[type=\"text\"][class*=\"txtGoodsNum\"]").val();
        num = parseFloat(num) - parseFloat(1);

        if (parseInt(num) <= parseInt(0))
            num = 0;

        if (parseFloat(num) >= 0)
            $(this).siblings("input[type=\"text\"][class*=\"txtGoodsNum\"]").val(parseFloat(num).toFixed(sDigits));
    });

    $(document).on("change", ".ddrBatchNO", function () {
        var BatchNO = $(this).val();
        var validDate = $(this).find("option[value='" + BatchNO + "']").attr("tip");
        $(this).siblings("input[type=\"hidden\"][class*=\"BatchNO\"]").val(BatchNO);
        $(this).closest("tr").find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").val(validDate);
    });

    //发货
    $(document).on("click", ".outOg", function () {
        $(".po-bg2").removeClass("none"); //等待跳转的层
        $(".p-delete2").removeClass("none"); //等待跳转的层

        var oID = $("#hidOrderID").val();
        var str = "";

        $(".tabLine table.noOut tbody tr").each(function (item) {
            var oldid = $(this).attr("tld");
            var oldnum = $(this).find("td:eq(-3)").find("div.sl").find("input[type=\"text\"][class*=\"txtGoodsNum\"]").val();
            var batchno = $.trim($(this).find("td:eq(-2)").find("div.tc").find("input[type=\"hidden\"][class*=\"BatchNO\"]").val());

            var validDate = $.trim($(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").val());
            if (parseFloat(oldnum) != parseFloat(0))//发货数量为0的商品不发货
                str += oldid + "：" + oldnum + "：" + batchno + "：" + validDate + "；";
        });
        var ts = stripscript($.trim($("#hidDts").val()));
        var Logistics = stripscript($("#hidLogistics").val());
        var LogisticsNo = stripscript($("#hidLogisticsNo").val());
        var CarUser = stripscript($("#hidCarUser").val());
        var CarNo = stripscript($("#hidCarNo").val());
        var Car = stripscript($("#hidCar").val());
        var date = stripscript($("#txtDate").val());

        if (str != "") {
            $.ajax({
                type: 'post',
                url: '../../Handler/orderHandle.ashx',
                data: { ck: Math.random(), ActionType: "outOrder", oID: oID, str: str, ComPName: Logistics, LogisticsNo: LogisticsNo, CarUser: CarUser, CarNo: CarNo, Car: Car, date: date, ts: ts },
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        //修改商品备注
                        $("div.order a.addRemark").remove();

                        //清空物流信息
                        $(".in-if").text("无物流信息");
                        $("#hidLogistics").val("");
                        $("#hidLogisticsNo").val("");
                        $("#hidCarUser").val("");
                        $("#hidCarNo").val("");
                        $("#hidCar").val("");
                        //清空交货日期
                        $("#txtDate").val("");
                        $(".sendde").text("");

                        $(".order").find("a.edit-i").css("display", "none");
                        $(".list").find("a.attrdel").css("display", "none");
                        var usertype = $("#hidUserType").val();
                        if (usertype == 3 || usertype == 4) {
                            $(".payamount").css("display", "none");
                            $(".postfee").css("display", "none");
                        }
                        $("#hidDts").val(data.ts)

                        if (data.Rvlue != "") {
                            $("#outGoods").prepend(data.Rvlue);
                        }
                        //当前发货时间
                        $(".sendde").text(data.Rdate);

                        if (data.Code == "") {
                            //没有未发货的商品
                            $(".deliver").attr("style", "display:none;");
                            $("div.tabLine table[class=\"noOut\"] tbody tr").remove();

                            //修改订单状态
                            $(".goods-flow").find("li").attr("class", "");
                            $(".goods-flow").find("li[tip=\"4\"]").prevAll().attr("class", "cur");
                            $(".goods-flow").find("li[tip=\"4\"]").attr("class", "cur");

                        } else {
                            var json = data.Code; //eval('(' + data.Code + ')');

                            if (json.length > 0) {
                                //修改订单状态
                                $(".goods-flow").find("li").attr("class", "");
                                $(".goods-flow").find("li[tip=\"2\"]").prevAll().attr("class", "cur");
                                $(".goods-flow").find("li[tip=\"2\"]").attr("class", "cur");

                                json = eval('(' + data.Code + ')');
                                var Stock = eval('(' + data.Stock + ')');
                                outNoGoods(json, Stock);
                            } else {
                                //没有未发货的商品
                                $(".deliver").attr("style", "display:none;");
                                $("div.tabLine table[class=\"noOut\"] tbody tr").remove();

                                //修改订单状态
                                $(".goods-flow").find("li").attr("class", "");
                                $(".goods-flow").find("li[tip=\"4\"]").prevAll().attr("class", "cur");
                                $(".goods-flow").find("li[tip=\"4\"]").attr("class", "cur");
                            }
                        }
                        if (data.Msg.toString() == "0")
                            $("#lblOstate").text("待收货");
                        else {
                            $("#lblOstate").text("已完成");
                            $(".deliver").attr("style", "display:none;");
                            //当前发货时间
                            $(".fulfil").text(data.Rdate);

                            //修改订单状态
                            $(".goods-flow").find("li").attr("class", "");
                            $(".goods-flow").find("li[tip=\"5\"]").prevAll().attr("class", "cur");
                            $(".goods-flow").find("li[tip=\"5\"]").attr("class", "cur");
                        }

                        layerCommon.msg("发货成功", IconOption.笑脸);
                    } else
                        layerCommon.msg(data.Msg, IconOption.错误);
                }, complete: function () {
                    $(".po-bg2").addClass("none");
                    $(".p-delete2").addClass("none");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });
        } else
            layerCommon.msg("商品发货数量有误！", IconOption.错误);
    });

    //修改发货物流信息
    $(document).on("click", ".upLogistics", function () {
        var tip = $(this).attr("tip");
        var DisID = $("#hidDisID").val();
        var CompID = $("#hidCompID").val();

        //        var Logistics = $("#hidLogistics").val();
        //        var LogisticsNo = $("#hidLogisticsNo").val();
        //        var CarUser = $("#hidCarUser").val();
        //        var CarNo = $("#hidCarNo").val();
        //        var Car = $("#hidCar").val();
        //        //物流公司：物流单号：司机名称：司机手机：车号
        //        var str = Logistics + ":" + LogisticsNo + ":" + CarUser + ":" + CarUser + ":" + CarNo + ":" + Car;

        //转向网页的地址; 
        var url = '../../../Company/newOrder/logistadd.aspx?KeyID=' + tip + '&DisID=' + DisID + '&CompID=' + CompID;
        var index = layerCommon.openWindow("物流信息", url, '580px', '435px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象

    });

    //修改发货
    $(document).on("click", ".btnupout", function () {
        $(this).text("保存");
        $(this).attr("class", "bule btnsave");
        var tip = $(this).attr("tip");
        var str = "";

        $(".tabLine table[tip=\"tab_" + tip + "\"] tbody tr").each(function (item) {
            var ooutID = $(this).attr("ttrd"); //发货单明细ID
            $(this).find("td:eq(-3)").find("div.sl").attr("style", "display:inline-block;");
            $(this).find("td:eq(-3)").find("div.tc").attr("style", "display:none;");

            $(this).find("td:eq(-2)").find("div.tc").find("input[type=\"text\"][class*=\"BatchNO\"]").attr("style", "display:inline-block;");
            $(this).find("td:eq(-2)").find("div.tc").find("label").attr("style", "display:none;");

            $(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").attr("style", "display:inline-block;");
            $(this).find("td:last").find("div.tc").find("label").attr("style", "display:none;");
        });

    });

    //修改保存发货信息
    $(document).on("click", ".btnsave", function () {
        //发货单ID
        var th = this;
        var tip = $(this).attr("tip");
        //订单ID
        var oID = $("#hidOrderID").val();
        var ts = $(this).attr("dts");

        var str = "";
        $(".tabLine table[tip=\"tab_" + tip + "\"] tbody tr").each(function (item) {
            var ooutID = $(this).attr("ttrd"); //发货单明细ID
            var oldnum = $(this).find("td:eq(-3)").find("div.sl").find("input[type=\"text\"][class*=\"txtGoodsNum\"]").val();

            var batchno = $(this).find("td:eq(-2)").find("div.tc").find("input[type=\"text\"][class*=\"BatchNO\"]").val();

            var validDate = $(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").val();
            if (parseFloat(oldnum) != parseFloat(0))
                str += ooutID + "：" + oldnum + "：" + batchno + "：" + validDate + "；";
        });

        if (str != "") {
            $.ajax({
                type: 'post',
                url: '../../Handler/orderHandle.ashx',
                data: { ck: Math.random(), ActionType: "upOut", outid: tip, oID: oID, str: str, ts: ts },
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        $(th).text("修改");
                        $(th).attr("class", "bule btnupout");

                        $(".tabLine table[tip=\"tab_" + tip + "\"] tbody tr").each(function (item) {
                            var ooutID = $(this).attr("ttrd"); //发货单明细ID
                            $(this).find("td:eq(-3)").find("div.sl").attr("style", "display:none;");
                            $(this).find("td:eq(-3)").find("div.tc").attr("style", "");
                            var oldnum = $(this).find("td:eq(-3)").find("div.sl").find("input[type=\"text\"][class*=\"txtGoodsNum\"]").val();
                            $(this).find("td:eq(-3)").find("div.tc").text(oldnum);

                            //批次号
                            $(this).find("td:eq(-2)").find("div.tc").find("input[type=\"text\"][class*=\"BatchNO\"]").attr("style", "display:none;");
                            $(this).find("td:eq(-2)").find("div.tc").find("label").attr("style", "");

                            var batchno = $(this).find("td:eq(-2)").find("div.tc").find("input[type=\"text\"][class*=\"BatchNO\"]").val();
                            $(this).find("td:eq(-2)").find("div.tc").find("label").text(batchno);

                            //有效期
                            $(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").attr("style", "display:none;");
                            $(this).find("td:last").find("div.tc").find("label").attr("style", "");
                            var validDate = $(this).find("td:last").find("div.tc").find("input[type=\"text\"][class*=\"validDate\"]").val();
                            $(this).find("td:last").find("div.tc").find("label").text(validDate);

                        });
                        $(".sendde").text("");

                        $(th).attr("dts", data.ts);
                        $(th).siblings("a[class*='btnorderoutdel']").attr("dts", data.ts);
                        $("#hidDts").val(data.ts)

                        if (data.Code == "") {
                            //没有未发货的商品

                            //当前发货时间
                            $(".sendde").text(data.Rdate);

                            $("#lblOstate").text("待收货");
                            $(".deliver").attr("style", "display:none;");
                            $("div.tabLine table[class=\"noOut\"] tbody tr").remove();

                            //修改订单状态
                            $(".goods-flow").find("li").attr("class", "");
                            $(".goods-flow").find("li[tip=\"4\"]").prevAll().attr("class", "cur");
                            $(".goods-flow").find("li[tip=\"4\"]").attr("class", "cur");
                        } else {
                            var json = data.Code; //eval('(' + data.Code + ')');

                            if (json.length > 0) {
                                $("#lblOstate").text("待发货");

                                //修改订单状态
                                $(".goods-flow").find("li").attr("class", "");
                                $(".goods-flow").find("li[tip=\"2\"]").prevAll().attr("class", "cur");
                                $(".goods-flow").find("li[tip=\"2\"]").attr("class", "cur");

                                json = eval('(' + data.Code + ')');
                                outNoGoods(json, "");
                            } else {
                                //当前发货时间
                                $(".sendde").text(data.Rdate);

                                //没有未发货的商品
                                $("#lblOstate").text("待收货");
                                $(".deliver").attr("style", "display:none;");
                                $("div.tabLine table[class=\"noOut\"] tbody tr").remove();

                                //修改订单状态
                                $(".goods-flow").find("li").attr("class", "");
                                $(".goods-flow").find("li[tip=\"4\"]").prevAll().attr("class", "cur");
                                $(".goods-flow").find("li[tip=\"4\"]").attr("class", "cur");
                            }
                        }
                        layerCommon.msg("修改发货单成功", IconOption.笑脸);
                        //window.location.href = window.location.href;
                    } else
                        layerCommon.msg(data.Msg, IconOption.错误);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });
        } else
            layerCommon.msg("发货的商品数量不能为0！", IconOption.错误);
    });

    //发货单作废确认
    $(document).on("click", ".btnorderoutdel", function () {
        $(".po-bg").attr("class", "po-bg");
        $("#p-delete").attr("class", "popup p-delete");
        $(".btn-area").attr("class", "btn-area canOrderOutSave");
        $(".btn-area").attr("tip", $(this).attr("tip"));
        $(".btn-area").attr("dts", $(this).attr("dts"));
    });

    //发货单作废
    $(document).on("click", ".canOrderOutSave", function () {
        //发货单ID
        var outid = $(this).attr("tip");
        //订单ID
        var oID = $("#hidOrderID").val();
        var ts = $(this).attr("dts");

        $.ajax({
            type: 'post',
            url: '../../Handler/orderHandle.ashx',
            data: { ck: Math.random(), ActionType: "CancelOut", outid: outid, oID: oID, ts: ts },
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    if (data.Rvlue == "1") {

                        //修改订单状态
                        $(".goods-flow").find("li").attr("class", "");
                        $(".goods-flow").find("li[tip=\"2\"]").prevAll().attr("class", "cur");
                        $(".goods-flow").find("li[tip=\"2\"]").attr("class", "cur");

                        $("div.order div.tabLine tbody tr").each(function (item) {
                            var trd = $(this).attr("trd");
                            var val = $(this).find("div.subRemark").text();
                            var ar = "";
                            if (val != "")
                                ar = "<a href=\"javascript:;\" tip=\"" + trd + "\" class=\"addRemark\">编辑</a>";
                            else
                                ar = "<a href=\"javascript:;\" tip=\"" + trd + "\" class=\"addRemark\">添加</a>";
                            // 订单状态返回上一步
                            $("#lblOstate").text("待发货");
                            $(this).find("div.subRemark").before(ar);
                        });
                    }

                    //当前发货时间
                    $(".sendde").text(data.Rdate);
                    $("#hidDts").val(data.ts)

                    if (data.Code == "") {
                        //没有未发货的商品
                        $(".deliver").attr("style", "display:none;");
                        $("div.tabLine table[class=\"noOut\"] tbody tr").remove();
                    } else {
                        var json = data.Code; //eval('(' + data.Code + ')');

                        if (json.length > 0) {
                            json = eval('(' + data.Code + ')');

                            var Stock = eval('(' + data.Stock + ')');
                            outNoGoods(json, Stock);
                        } else {
                            //没有未发货的商品
                            $(".deliver").attr("style", "display:none;");
                            $("div.tabLine table[class=\"noOut\"] tbody tr").remove();
                        }
                    }
                    var th = $(".btnorderoutdel[tip=\"" + outid + "\"]");
                    $(th).siblings().remove();
                    $(th).remove();

                    $("a.upLogistics[tip=\"" + outid + "\"]").remove();
                    $("table[tip=\"tab_" + outid + "\"]").parents("div.tabLine").prepend("<div class=\"cancel\"></div>");

                    $("#lblOstate").text("待收货");
                    layerCommon.msg("作废发货单成功", IconOption.笑脸);
                } else
                    layerCommon.msg(data.Msg, IconOption.错误);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });

        $(".po-bg").attr("class", "po-bg none");
        $("#p-delete").attr("class", "popup p-delete none");
        $(".btn-area").attr("class", "btn-area");
        $(".btn-area").removeAttr("tip");
        $(".btn-area").removeAttr("th");
    });

    //物流信息
    $(document).on("click", ".addlogistics", function () {
        var DisID = stripscript($("#hidDisID").val());
        var CompID = stripscript($("#hidCompID").val());

        var Logistics = stripscript($("#hidLogistics").val());
        var LogisticsNo = stripscript($("#hidLogisticsNo").val());
        var CarUser = stripscript($("#hidCarUser").val());
        var CarNo = stripscript($("#hidCarNo").val());
        var Car = stripscript($("#hidCar").val());
        //物流公司：物流单号：司机名称：司机手机：车号
        var str = Logistics + ":" + LogisticsNo + ":" + CarUser + ":" + CarUser + ":" + CarNo + ":" + Car;

        //转向网页的地址; 
        var url = '../../../Company/newOrder/logistadd.aspx?DisID=' + DisID + '&CompID=' + CompID + '&str=' + str;
        var index = layerCommon.openWindow("物流信息", url, '580px', '435px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
    });
    /******** 厂商 end *********/

});

//交货时期、配送方式回调方法
function pedeli_info(type, str) {
   str= stripscript(str)
    //    if (~ ~type == 0)
    //        $("#lblArriveDate").text(str);
    //    else
    //        $("#lblGiveMode").text(str);

    CloseGoods()
    var oID = $("#hidOrderID").val();
    //    var usertype = $("#hidUserType").val();
    //    if (usertype == 3 || usertype == 4) {
    //        window.location.href = "/Company/newOrder/orderdetail.aspx?KeyID=" + oID;    //刷新页面
    //    } else {
    //        window.location.href = "../newOrder/orderdetail.aspx?KeyID=" + oID;    //刷新页面
    //    }
    location = location;
}

//订单备注、商品备注回调方法
function remarkinfo(type, KeyID, remark, goodsInfoId, index) {
    type= stripscript(type)
    remark= stripscript(remark)
    goodsInfoId= stripscript(goodsInfoId)
    index= stripscript(index)
    KeyID= stripscript(KeyID)
    //订单备注
    //$("#iRemark").text(remark);
    var oID = $("#hidOrderID").val();
    $.ajax({
        type: "Post",
        url: "../../Handler/orderHandle.ashx?ActionType=remarkview",
        data: { ck: Math.random(), KeyID: KeyID, type: type, remark: remark },
        dataType: "json",
        success: function (data) {
            if (data.Result) {
                if (~ ~type == 0) {
                    //window.location.href = "orderdetail.aspx?KeyID=" + oID;    //刷新页面
                    location = location;
                } else {
                    //商品备注
                    window.parent.CloseGoods();
                    var tr = $(".tabLine table tbody tr[trd=\"" + goodsInfoId + "\"]");
                    var addtype = "";
                    if (remark == "") {
                        addtype = "添加";
                        if (!$("div[class=\"cur\"]").is($(tr).find("td:last")))
                            $(tr).find("td:last").find("div[class=\"cur\"]").remove();
                    }
                    else {
                        addtype = "编辑";
                        if (!$("div[class=\"cur\"]").is($(tr).find("td:last"))) {
                            $(tr).find("td:last").find("div[class=\"cur\"]").remove();
                            $(tr).find("td:last").find("div[class=\"subRemark\"]").before("<div class=\"cur\">" + remark + "</div>");
                        } else {
                            $(tr).find("td:last").find("div[class=\"cur\"]").text(remark);
                        }
                    }

                    $(tr).find("td:last").find("div[class=\"tc alink\"]").find("a").text(addtype);
                    var info = remark;
                    if (remark.length > 6)
                        info = remark.substr(0, 6) + "...";
                    $(tr).find("td:last").find("div[class=\"subRemark\"]").text(info);
                }
            } else {
                layerCommon.msg(data.Code, IconOption.错误);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            var a;
        }
    });
}

//收货地址回调方法
function addr_info(AddrID, Principal, Phone, Address) {
    AddrID= stripscript(AddrID)
    Principal= stripscript(Principal)
    Phone= stripscript(Phone)
    Address= stripscript(Address)
    $.ajax({
        type: 'post',
        url: '../../Handler/orderHandle.ashx',
        data: { ck: Math.random(), ActionType: "orderUpaddr", ID: $.trim($("#hidOrderID").val()), AddrID: AddrID, Principal: Principal, Phone: Phone, Address: Address },
        dataType: 'json',
        success: function (data) {
            if (data.Result) {
                var oID = $("#hidOrderID").val();
                location = location;
            } else
                layerCommon.msg(data.Msg, IconOption.错误);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
        }
    });
}

//开票信息回调方法
function invinfo(DisAccID, val, LookUp, Context, Bank, Account, RegNo) {
    DisAccID= stripscript(DisAccID)
    val= stripscript(val)
    LookUp= stripscript(LookUp)
    Bank= stripscript(Bank)
    Context= stripscript(Context)
    RegNo= stripscript(RegNo)
    $.ajax({
        type: 'post',
        url: '../../Handler/orderHandle.ashx',
        data: { ck: Math.random(), ActionType: "orderUpInvoi", DisID: $.trim($("#hidDisID").val()), ID: $.trim($("#hidOrderID").val()), DisAccID: DisAccID, Rise: LookUp, Content: Context, OBank: Bank, OAccount: Account, TRNumber: RegNo, val: val },
        dataType: 'json',
        success: function (data) {
            if (!data.Result)
                layerCommon.msg(data.Msg, IconOption.错误);
            else {
                //                $("#hidval").val(val);
                //                $("#hidDisAccID").val(DisAccID);
                //                $("#lblRise").text(LookUp);
                //                $("#lblContent").text(Context);
                //                $("#lblOBank").text(Bank);
                //                $("#lblOAccount").text(Account);
                //                $("#lblTRNumber").text(RegNo);
                //                var str = "";
                //                str += "<input type=\"hidden\" id=\"hidDisAccID\" value=\"" + DisAccID + "\" />";
                //                str += " 发票抬头：<label id=\"lblRise\">" + LookUp + "</label>";
                //                str += "，发票内容：<label id=\"lblContent\" >" + Context + "</label>";
                //                if (~ ~val == 0) {
                //                    //不开发票
                //                    $(".iInvoice").html("不开发票");
                //                } else if (~ ~val == 1) {
                //                    //普通发票
                //                    $(".iInvoice").html(str);
                //                } else {
                //                    //增值税发票
                //                    str += "，开户银行：<label id=\"lblOBank\" >" + Bank + "</label>";
                //                    str += "，开户账户：<label id=\"lblOAccount\" >" + Account + "</label>";
                //                    str += "，纳税人登记号：<label id=\"lblTRNumber\" >" + RegNo + "</label>";
                //                    $(".iInvoice").html(str);
                //                }
                var oID = $("#hidOrderID").val();
                location = location;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
        }
    });
}

//退货申请回调
function ResurnOrder() {
    CloseGoods();
    var oID = $("#hidOrderID").val();
    //    window.location.href = "newOrder/orderdetail.aspx?KeyID=" + oID;    //刷新页面
    location = location;
}

/******** 厂商回调 start *********/

//修改运费、应付总额
function amount_info(type, tatol, AuditAmount, code) {
    //type：修改类型，tatol：运费 ，AuditAmount：应付总额
    //    if (~ ~type == 1) {//1、修改运费
    //        //原有运费
    //        var PostFee = $("#lblPostFee").text();
    //        //原有应付金额
    //        var Amount = $("#lblAuditAmount").text();

    //        PostFee = PostFee.replace(/,/gm, '');
    //        Amount = Amount.replace(/,/gm, '');

    //        //减去之前的运费
    //        var t = parseFloat(Amount) - parseFloat(PostFee);

    //        var d = parseFloat(t) + parseFloat(tatol);
    //        if (parseFloat(d) <= 0)
    //            d = 0;
    //        //加现有运费
    //        $("#lblAuditAmount").text(formatMoney((parseFloat(d)), 2));
    //        $("#lblPostFee").text(formatMoney(parseFloat(tatol), 2));
    //    } else if (~ ~type == 0) {
    //        //0、修改应付总额
    //        //        var json = eval('(' + code + ')');
    //        //        if (json.length > 0) {
    //        //            $.each(json, function (index, item) {
    //        //                var trd = $("div.tabLine table tbody").find("tr[trd=\"" + item["ID"] + "\"]");
    //        //                $(trd).find("td:eq(3)").find("div[class=\"tc\"]").text("￥" + parseFloat(item["AuditAmount"]).toFixed(2));
    //        //                $(trd).find("td:eq(5)").find("div[class=\"tc\"]").text("￥" + parseFloat(item["sumAmount"]).toFixed(2));
    //        //            });
    //        //        }
    //        $("#lblAuditAmount").text(formatMoney(parseFloat(tatol), 2));

    //        layerCommon.msg("修改应付总额成功", IconOption.笑脸);
    //    }
    var oID = $("#hidOrderID").val();
    
    location = location;
}

//发票信息
function billinfo(BillNo, isBill) {
    BillNo= stripscript(BillNo)
    isBill= stripscript(isBill)
    //    $("#lblBillNo").text(BillNo);
    //    if (~ ~isBill == 0) {
    //        $("#lblIsBill").text("否");
    //    }
    //    else {
    //        $("#lblIsBill").text("是");
    //        $("#hidisBill").val("1");
    //        $(".addbill").css("display", "none");
    //    }
    var oID = $("#hidOrderID").val();
    
    location = location;
}

//发货数量修改
function outOrderNum(th, type) {
    var Digits = $("#hidDigits").val();
    var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;

    var num = $(th).val();
    var Nonum = $(th).siblings("input[type=\"hidden\"][class=\"Notshipnum\"]").val();

    if (parseInt(type) == parseInt(1)) {
        if (parseInt(Nonum) == parseInt(0)) {
            Nonum = $(th).siblings("input[type=\"hidden\"][class=\"outnum\"]").val();
        }
    }
    if (Nonum == null || typeof (Nonum) == "undefined")
        Nonum = 0;

    if (num == null || typeof (num) == "undefined")
        num = Nonum;

    if (parseInt(Nonum) <= parseInt(0))
        Nonum = 0;

    if (parseFloat(num) > parseFloat(Nonum)) {
        $(th).val(parseFloat(Nonum).toFixed(sDigits));
        return false;
    }
    else {
        $(th).val(parseFloat(num).toFixed(sDigits));
        return false;
    }
}

//物流信息回调方法
function logista_info(keyID, Logistics, LogisticsNo, CarUser, CarNo, Car) {
    Logistics= stripscript(Logistics)
    LogisticsNo= stripscript(LogisticsNo)
    CarNo= stripscript(CarNo)
    Car= stripscript(Car)


    if (keyID == "") {

        var str = "";
        if (Logistics != "")
            str += str == "" ? "物流公司：" + Logistics : ",物流公司：" + Logistics;
        if (LogisticsNo != "")
            str += str == "" ? "物流单号：" + LogisticsNo : ",物流单号：" + LogisticsNo;
        if (CarUser != "")
            str += str == "" ? "司机姓名：" + CarUser : ",司机姓名：" + CarUser;
        if (CarNo != "")
            str += str == "" ? "司机手机：" + CarNo : ",司机手机：" + CarNo;
        if (Car != "")
            str += str == "" ? "车牌号：" + Car : ",车牌号：" + Car;
       
        $(".in-if").html(str+"&nbsp;");

        $("#hidLogistics").val(Logistics);
        $("#hidLogisticsNo").val(LogisticsNo);
        $("#hidCarUser").val(CarUser);
        $("#hidCarNo").val(CarNo);
        $("#hidCar").val(Car);
    } else {
        var str = "";
        if (Logistics != "" || LogisticsNo != "")
            str += "物流公司：" + Logistics + "，物流单号：" + LogisticsNo;
        else
            str += "姓名：" + CarUser + " " + CarNo + "，车牌号：" + Car;

        var tab = $(".tabLine b[tipl=\"" + keyID + "\"]");
        var keyID1 = $(tab).attr("tiplog");
        $(tab).html(str + "<a href=\"javascript:;\" tip=\"" + keyID1 + "\" class=\"bule upLogistics\">修改物流</a>");

    }
}

//未发货的商品
function outNoGoods(json, Stock) {
    $(".deliver").attr("style", "");
    $("div.tabLine table[class=\"noOut\"] tbody tr").remove();

    var Digits = $("#hidDigits").val();
    var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
    var str = "";

    //图片路径
    var picpath = $("#hidPicpath").val();

    $.each(json, function (index, item) {
        str += "<tr tld=\"" + item["ID"] + "\">";
        str += "<td><div class=\"sPic\"><span><a href=\"javascript:;\"><img src=\"" + picpath + item["Pic"] + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + item["GoodsCode"] + ordersale(item["ProID"], item["Protype"], item["Unit"]) + "</a><a href=\"javascript:;\" class=\"name\">" + sub(item["GoodsName"], 20, "...") + "<i>" + item["GoodsName"] + "</i></a></div></td>";

        var goodsname = item["GoodsInfos"];
        var name = "";
        if (goodsname != null || typeof (goodsname) != "undefined")
            name = goodsname.replace(/:/g, "：").replace(/；/g, "，").substr(0, item["GoodsInfos"].length - 1);

        str += "<td><div class=\"tc\">" + name + "</div></td>";
        str += "<td><div class=\"tc\">" + item["Unit"] + "</div></td>";
        str += "<td><div class=\"tc\">" + (parseFloat(item["GoodsNum"]) + parseFloat(item["ProNum"])).toFixed(sDigits) + "</div></td>";
        str += "<td><div class=\"tc\">" + parseFloat(item["OutNum"]).toFixed(sDigits) + "</div></td>";

        var num = 0;
        var GoodsNum = parseFloat(item["GoodsNum"]) + parseFloat(item["ProNum"]);
        var OutNum = parseFloat(item["OutNum"]);
        num = GoodsNum - OutNum;

        str += "<td style=\" width:15%;\"><div class=\"sl\"><input type=\"hidden\" id=\"Notshipnum\" class=\"Notshipnum\" value=\"" + num + "\" /><a href=\"javascript:;\" class=\"minus nominus\">-</a><input type=\"text\" class=\"box txtGoodsNum\" onchange=\"outOrderNum(this,0);\" onkeyup='KeyInt2(this)' value=\"" + parseFloat(num).toFixed(sDigits) + "\" /><a href=\"javascript:;\" class=\"add noadd\">+</a></div></td>";

        if (Stock == "") {
            str += "<td><div class=\"tc\"><input type=\"text\" class=\"box BatchNO\" value=\"" + item["BatchNO"] + "\"/></div></td>";
            str += "<td><div class=\"tc\"><input type=\"text\" class=\"Wdate validDate\" readonly=\"readonly\" value=\"" + item["validDate"] + "\" /></div></td>";
        } else {
            str += "<td><div class=\"tc\"><input type=\"hidden\" class=\"box BatchNO\" value=\"" + Stock[0]["BatchNO"] + "\" /><select class=\"box ddrBatchNO\" style=\"width:120px;\">";

            $.each(Stock, function (sindex, sitem) {
                if (item["GoodsinfoID"].toString() == sitem["GoodsInfo"].toString())
                    str += "<option tip=" + UnixToDate(sitem["validDate"], 9) + " value='" + sitem["BatchNO"] + "'>" + sitem["BatchNO"] + "</option>";
            });

            str += "</select></div></td>";
            str += "<td><div class=\"tc\">";
            str += "<input type=\"text\" style=\"width:80px;\" class=\"Wdate validDate\" value=\"" + UnixToDate(Stock[0]["validDate"], 9) + "\" readonly=\"readonly\" />";
            str += "</div></td>";
        }
        str += "</tr>";
    });

    $("div.tabLine table[class=\"noOut\"] tbody").append(str);
}

/******** 厂商回调 end *********/

//关闭弹出层方法
function CloseGoods() {
    layerCommon.layerClose("hid_Alert");
}
/************************* 订单详细 end **************************************/



/*********************付款相关js*start**********************/

//收起按钮
$(document).on("click", "#a_pay", function () {

    $(".offLine").hide();

});


//线下支付按钮
$(document).on("click", "#btn_pay_xx", function () {

    $(".offLine").show();

});


//确认支付按钮
$(document).on("click", "#pay_sub", function () {
    var txtArriveDate = stripscript($("#txtArriveDate").val());
    if (txtArriveDate == null || txtArriveDate=="")
    {
        layerCommon.msg("付款日期不能为空", IconOption.错误);
        return false
    }
    var hidden_pay =stripscript( $("#hidden_pay").val());

    //数据收集
    var KeyID = stripscript($("#hidOrderID").val());
    var DisID = stripscript($("#hidDisID").val());

    //厂商是1，代理商0
    var hid_type = stripscript($("#hid_type").val());

    var paymoney = stripscript($("#paymoney").val());
    var bankname = stripscript($("#bankname").val());
    var bank = stripscript($("#bank").val());
    var bankcode = stripscript($("#bankcode").val());
    var remark = stripscript($("#remark").val());
    //附件信息
    var attach = stripscript($("#HDFileNames").val());
    //加密KeyID
    var desKeyID = stripscript($("#desKeyID").val());


    if (parseFloat(paymoney) > parseFloat(hidden_pay)) {
        layerCommon.msg("本次支付金额，大于订单未付款金额！", IconOption.错误);
        return;
    }


    $.ajax({
        type: 'post',
        url: '../../Handler/orderHandle.ashx',
        data: { ck: Math.random(), ActionType: "Payed", KeyID: KeyID, DisID: DisID, paymoney: paymoney, bankname: bankname, bank: bank, bankcode: bankcode, txtArriveDate: txtArriveDate, remark: remark, attach: attach, hid_type: hid_type },
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            //alert('远程调用开始...'); 
            
            $("#pay_sub").hide();
        },
        success: function (data) {
            
            if (data.Result) {
                layerCommon.msg("付款成功", IconOption.笑脸);
                //window.location.reload();
                window.location.href = "orderdetail.aspx?Top=4&KeyID=" + desKeyID;    //刷新页面

            } else
                layerCommon.msg(data.Msg, IconOption.错误);
        },
        complete: function (XMLHttpRequest, textStatus) {
            // alert('远程调用成功，状态文本值：'+textStatus); 
            
            $("#pay_sub").show();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
        }
    });
});

//在线支付
function payDB(isdb, Id) {
    window.open('../Pay/Pay.aspx?isDBPay=' + isdb + '&KeyID=' + Id);
}

//金额只能输入正数和小数
function KeyIntPrice(val) {

    val.value = val.value.replace(/[^\d.]/g, ''); //^(-)?[1-9][0-9]*$
}


//转到代理商支付详细页
function Go_disPayInfo(paymentId, pretype, orderid) {
    var height = document.documentElement.clientHeight;
    var layerOffsetY = (height - 550) / 2; //计算宽度
    var index = layerCommon.openWindow('付款详情', 'PayItemInfo.aspx?Paymnetid=' + paymentId + '&PreType=' + pretype + '&orderid=' + orderid, '900px', '450px');
    $("#hid_Alert").val(index);
}

//转到厂商收款详细页

function Go_compPayInfo(paymentId, pretype) {
    var height = document.documentElement.clientHeight;
    var layerOffsetY = (height - 550) / 2; //计算宽度
    var index = layerCommon.openWindow('收款详情', '../../../Company/Report/CompOrderInfo.aspx?KeyID=' + paymentId + '&PreType=' + pretype, '900px', '450px');
    $("#hid_Alert").val(index);
}


//厂商线下支付作废/支付确认
function TovoidPay(paymentid, smg) {
    layerCommon.confirm(smg==1?"是否确认支付":"确定要作废吗", function () { conOK(paymentid,smg) });
}

//执行线下支付作废的操作
function conOK(paymentid,smg) {
    //加密KeyID
    var desKeyID = $("#desKeyID").val();
    $.ajax({
        type: 'post',
        url: '../../Handler/orderHandle.ashx',
        data: { ck: Math.random(), ActionType: "PayTovoid", paymentid: paymentid,smg:smg },
        dataType: 'json',
        success: function (data) {
            if (data.Result) {
                layerCommon.msg("操作成功", IconOption.笑脸);
                window.location.href = "orderdetail.aspx?Top=4&KeyID=" + desKeyID;    //刷新页面

            } else
                layerCommon.msg(data.Msg, IconOption.错误);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
        }
    });

}

//页签首次加载
function TopShow() {
    $(".order").attr("style", "display:none;");
    $(".goodstakn").attr("style", "display:none;");
    $(".payment").attr("style", "display:block;");

    $(".goods-title").find("li").attr("class", "");
    $(".goods-title").find("li:last").attr("class", "hover");
}

function TopSignShow() {
    $(".order").attr("style", "display:none;");
    $(".goodstakn").attr("style", "display:block;");
    $(".payment").attr("style", "display:none;");

    $(".goods-title").find("li").attr("class", "");
    $(".goods-title").find("li[tip=\"2\"]").attr("class", "hover");
}
/********************付款相关js*end***********************/