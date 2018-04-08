$(function () {
    //代理商筛选
    $(".txtDisName").keyup(function () {
        $(".jxs-box .name").show();
        var hid = eval('(' + $(".divDisList").text() + ')');
        var strtext = $(this).val();
        $(".jxs-box .name").html("");
        if ($.trim(strtext) != "") {
            $(hid).each(function (index, obj) {
                if ($(".jxs-box .name li").length < 5) {
                    if (obj.DisName.indexOf(strtext) != -1) {
                        $(".jxs-box .name").append("<li><a href=\"javascript:;\" tip=\"" + obj.ID + "\">" + obj.DisName + "</a></li>");
                    }
                }
            })
        } else {
            $(".jxs-box .name").hide();
        }
    })
    //失去焦点，延迟加载
    var t = '';
    $(document).on("blur", ".project2", function () {
        clearTimeout(t);
        t = setTimeout("send_data()", 200);
    });
    var tt = '';
    $(document).on("blur", ".txtDisName", function () {
        clearTimeout(tt);
        tt = setTimeout("send_data2()", 200);
    });
    //代理商筛选
    $(".txtDisName").focus(function () {
        $(".jxs-box .search-opt").show();
        $(".jxs-box .name").show();
    })
    //代理商选中
    $(document).on("click", ".jxs-box .name li", function () {
        $(".add2").trigger("click");
        Clear(); //清除缓存
        $("#hrOrderInv").val("");
        $("#hidLookUp").val("");
        $("#hidContext").val("");
        $("#hidBank").val("");
        $("#hidAccount").val("");
        $("#hidRegNo").val("");
        disId = $.trim($(this).find("a").attr("tip"));
        var disname = $.trim($(this).find("a").text());
        disBind(disId, disname);
    })
    //选中筛选商品
    $(document).on("click", ".search-opt .list li", function () {
        var goodsInfoId = $(this).attr("tip"); //goodsInfoid
        var disId = $("#hidDisID").val();
        var compId = $("#hidCompId").val();
        var bol = false;
        var inindex = $(this).parent().parent().parent().parent().parent().index();
        $(".tabLine table tbody tr").each(function (indexss, objs) {
            if ($(".tabLine table tbody tr").eq([indexss]).attr("tip") != undefined) {
                if (goodsInfoId == $(".tabLine table tbody tr").eq([indexss]).attr("tip")) {
                    bol = true;
                    return false;
                } else {
                    bol = false;
                }
            }
        })
        if (bol) { layerCommon.msg("商品已存在", IconOption.错误); } else {
            GoodsInfoList(goodsInfoId, disId, compId, inindex, "");
        }
    })

    //增加选择商品行
    $(document).on("click", ".minus2", function () {
        //$(".tabLine table tbody tr:last").show();
        var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        $(".tabLine table tbody").append(html);
    })
    //修改单价
    $(document).on("blur", ".boxs", function () {
        var id = $(this).parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).attr("tip2"); //index
        onchengSum(id, index, 0);
    })
    //商品数量减
    $(document).on("click", ".divnum .minus", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        var valstr = $(".txtGoodsNum" + index).val();
        // if (valstr >= 1) {
        onchengSum(id, index, -1);
        // }

    })
    //商品数量框
    $(document).on("change", ".divnum .txtGoodsNum", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, 0);
    })
    //商品数量增
    $(document).on("click", ".divnum .add", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        var valstr = $(".txtGoodsNum" + index).val();
        // if (valstr >= 1) {
        onchengSum(id, index, 1);
        //  }
    })
    //配送方式
    $(".carry .menu a").click(function () {
        var type = $.trim($("#lblPsType").text());
        if (type == "送货") {
            $("#hidPsType").val("自提");
            $("#lblPsType2").text("送货");
            $("#lblPsType").text("自提");
        } else {
            $("#hidPsType").val("送货");
            $("#lblPsType2").text("自提");
            $("#lblPsType").text("送货");
        }
    })
    //开票信息
    $(".invoice .edit-i").click(function () {
        if (disId == undefined) {
            layerCommon.msg("请填写代理商", IconOption.错误);
            return false;
        }
        var disid = disId;
        var id = $("#hidVal").val();
        var lookup = $("#hidLookUp").val();
        var context = $("#hidContext").val();
        var bank = $("#hidBank").val();
        var account = $("#hidAccount").val();
        var regno = $("#hidRegNo").val();
        var index = layerCommon.openWindow("开票信息", "../../Distributor/newOrder/InvoiceInfo.aspx?DisId=" + disId + "&val=" + id + "&Rise=" + lookup + "&Context=" + context + "&Bank=" + bank + "&Account=" + account + "&RegNo=" + regno, "570px", "440px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })

    //提交订单
    $(".btn-box .btn-area").click(function () {
        var num = $(".tabLine tbody tr").length;
        var adder = $.trim($(".site .site-if").text());
        var bol = false;
        for (var i = 0; i < num; i++) {
            var tip = $(".tabLine tbody tr").eq(i).attr("tip");
            if (tip != undefined && tip != "") {
                bol = true;
                break;
            }
        }
        if (!bol) {
            layerCommon.msg("请先选择商品", IconOption.错误);
            return false;
        }
        if (adder == "") {
            layerCommon.msg("请填写收货地址", IconOption.错误);
            return false;
        }
        setTimeout(200);
        var total = $.trim($("#lblTotalAmount").text()).replace(/,/gm, '');
        var cx = $.trim($("#lblCux").text()).replace(/,/gm, '');
        var ps = $.trim($("#lblPostFee").text()).replace(/,/gm, '');
        var zprice = $.trim($(".price-sum .price").text().substring(1)).replace(/,/gm, '');
        var rebate = $.trim($(".txtRebate").text()).replace(/,/gm, '');
        var price1 = parseFloat(total) + parseFloat(ps) - parseFloat(cx);
        var price2 = parseFloat(total) + parseFloat(ps) - parseFloat(cx) - parseFloat(zprice);
        if (price1 < rebate) {
            layerCommon.msg("返利金额不能大于（商品总额+运费）-促销优惠", IconOption.错误);
            return false;
        }
        if (price2 < rebate) {
            layerCommon.msg("返利金额不能大于（商品总额+运费+应付金额）-促销优惠", IconOption.错误);
            return false;
        }
        $("#btnAdd").trigger("click");
    })
    //返回
    $(".gray-btn").click(function () {
        location.href = "../Order/OrderCreateList.aspx";
    })
    //返回
    $(".gray-btn2").click(function () {
        location.href = "../OrderList.aspx";
    })
    //更多商品3
    $(document).on("click", ".sPic", function () {
        var indexs = $(this).parent().parent().index();
        var goodsInfoId = $(this).parent().parent().attr("tip");
        var goodsInfoIdList = "";
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        if (goodsInfoId != undefined) {
            gengduo(indexs, goodsInfoId, goodsInfoIdList);
        } else {
            gengduo(indexs, "", "");
        }
    })
    //更多商品2
    $(document).on("click", ".tabLine .opt-i", function () {
        var indexs = $(this).parent().parent().parent().index();
        var goodsInfoIdList = "";
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList);
    })
    //更多商品1
    $(document).on("click", ".tabLine .search-opt .opt", function () {
        var indexs = $(this).parent().parent().parent().parent().index();
        var goodsInfoIdList = "";
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList);
    })
    //更多代理商
    $(document).on("click", ".jxs-box  .search-opt .opt,.jxs-box .opt-i", function () {
        gengduoDis();
    })
    //商品添加备注
    $(document).on("click", ".alink a", function () {
        var compId = $("#hidCompId").val();
        var goodsInfoId = $.trim($(this).parent().parent().parent().attr("tip")); //goodsinfoId
        var indexs = $.trim($(this).parent().parent().parent().attr("trindex"));
        var remark = $.trim($(this).parent().find(".cur").text());
        var index = layerCommon.openWindow("添加备注", "../../Distributor/newOrder/remarkview.aspx?disId=" + disId + "&CompId=" + compId + "&type=2&KeyID=" + goodsInfoId + "&index=" + indexs + "&remark=" + remark, "770px", "450px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })

    //收货地址
    $(".site .edit-i").click(function () {
        var disId = $("#hidDisID").val();
        var compId = $("#hidCompId").val();
        var index = layerCommon.openWindow("收货地址", "../../Distributor/newOrder/addrinfo.aspx?DisID=" + disId + "&CompId=" + compId, "750px", "590px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })
    //删除当前商品行
    $(document).on("click", ".add2", function () {
        if (disId == undefined || disId == "") {
            disId = $("#hidDisID").val();
        }
        if ($(".tabLine table tbody tr").length > 1) {
            //            if ($(this).attr("tip") == "alast") {
            //                $(this).parent().parent().parent().hide();
            //            } else {
            $(this).parent().parent().parent().remove();
            //   $(".tabLine table tbody tr:last").show();
            // }
        } else {
            $(this).parent().parent().parent().remove();
            var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
            $(".tabLine table tbody").append(html);
        }
        if (disId == "") {
            return;
        }
        var goodsInfoId = $(this).parent().parent().parent().attr("tip");
        editGoods(disId, goodsInfoId);
    })

    //运费
    $(".postfee").click(function () {
        var DisID = $("#hidDisID").val();
        var tatol = $("#lblPostFee").text();
        var keyId = $("#hidKeyId").val();
        var url = 'amountof.aspx?type=2&DisID=' + DisID + '&KeyID=' + keyId + '&t=' + tatol;
        var index = layerCommon.openWindow("修改运费", url, '500px', '350px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })
    //查看返利
    $(document).on("click", ".seebate", function () {
        var KeyID = $("#hidKeyId").val();
        var DisID = $("#hidDisID").val();

        //转向网页的地址; 
        var url = '../../Distributor/newOrder/DisReBate.aspx?DisID=' + DisID + '&type=0&KeyID=' + KeyID;
        var index = layerCommon.openWindow("查看返利", url, '575px', '415px'); //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    });
})
//过滤非法字符
function stripscript(strHtlm) {
    strHtlm = strHtlm + "";
    //var pattern = new RegExp("exec|insert|delete|drop|truncate|update|declare|frame|or|style|expression|and|select|create|script|img|alert|href|1=1|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62))","g")
    //s.replace(pattern,"");
    //var pattern = new RegExp("[%--`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]")        //格式 RegExp("[在中间定义特殊过滤字符]")
    var pattern = /(insert|delete|truncate|update|declare|frame|style|expression|select|create|script|alert|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62)) /ig;
    return strHtlm.replace(pattern, "");
}
function editGoods(disId, goodsInfoId) {
    var compId = stripscript($("#hidCompId").val());
    var hidId = stripscript($("#hidGoodsInfoId").val());
    var batePrice = stripscript($(".txtRebate").val());
    //  var goodsInfoId = $(this).parent().parent().parent().attr("tip");
    if (hidId == "") {
        $("#hidGoodsInfoId").val(goodsInfoId)
    } else {
        $("#hidGoodsInfoId").val(hidId + "," + goodsInfoId);
    }
    $.ajax({
        type: "post",
        url: "../../Handler/orderHandle.ashx",
        data: { ck: Math.random(), ActionType: "DelGoods", goodsInfoId: goodsInfoId, disId: disId, compId: compId },
        dataType: "text",
        success: function (data) {
            var result = eval('(' + data + ')');
            var ds = result["ds"];
            if (ds == "True") {
                //商品小计计算
                var psf = $.trim($("#lblPostFee").text());
                $("#lblTotalAmount").text(result["SumTotal"]);
                if (result["proPrice"].toString() != "") {
                    $("#lblCux").text(result["proPrice"] == 0 ? "0.00" : formatMoney(result["proPrice"], 2));
                    //商品总价
                    if (formatMoney(parseFloat(result["SumTotal"].toString().replace(/,/gm, '')) - parseFloat(result["proPrice"]), 2) == "0") {
                        $(".price-sum .price").text("￥ 0.00");
                    } else {
                        $(".price-sum .price").text("￥ " + formatMoney(parseFloat(result["SumTotal"].toString().replace(/,/gm, '')) - parseFloat(result["proPrice"]) - parseFloat(batePrice.toString().replace(/,/gm, '')) + parseFloat(psf), 2));
                    }
                    if (parseFloat($.trim($("#lblFanl").text().toString().replace(/,/gm, ''))) > parseFloat(result["SumTotal"].toString().replace(/,/gm, '')) - parseFloat(result["proPrice"].toString().replace(/,/gm, '')) + parseFloat(psf)) {
                        $("#lblFanl").text(formatMoney(parseFloat(result["SumTotal"].toString().replace(/,/gm, '')) - parseFloat(result["proPrice"].toString().replace(/,/gm, '')) + parseFloat(psf), 2));
                        $("#txtRebate").val(formatMoney(parseFloat(result["SumTotal"].toString().replace(/,/gm, '')) - parseFloat(result["proPrice"].toString().replace(/,/gm, '')) + parseFloat(psf), 2));
                    }
                }
                else {
                    $("#lblCux").text("0.00");
                    //商品总价
                    $(".price-sum .price").text("￥ " + formatMoney(result["SumTotal"] - parseFloat(batePrice.replace(/,/gm, '')) + parseFloat(psf), 2));
                }

            } else {
                $(".lblTotal_" + index).text("0.00");
            }
        }
    })
}
function send_data() {
    $(".tabLine .search-opt").css("display", "none");
}
function send_data2() {
    $(".jxs-box .search-opt").css("display", "none");
}
//dis绑定
function disBind(disId, disname) {
    disId = stripscript(disId);
    disname = stripscript(disname)
    $(".txtDisName").val(disname);
    $(".jxs-box .name").hide();
    $(".jxs-box .search-opt").hide();
    var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
    $(".tabLine table tbody").html(html);
    $("#hidDisID").val(disId);
    var compId = $("#hidCompId").val();
    var keyId = $("#hidKeyId").val();
    $.ajax({
        type: "post",
        data: { ck: Math.random(), action: "dislist", compId: compId, disId: disId },
        dataType: "text",
        success: function (data) {
            $(".divGoodsName").text(data);
        }, complete: function () {
            var type = $("#hidType").val();
            if (type == "2") {
                addre($.trim(disId), 0);
            } else {
                addre($.trim(disId), keyId);
            }
        }
    })
}
//更多商品
function gengduo(indexs, goodsInfoId, goodsInfoIdList) {
    if ((disId == undefined || disId == "") && $("#hidDisID").val() == "") {
        layerCommon.msg("请填写代理商", IconOption.错误);
        return false;
    } else {
        disId = $("#hidDisID").val();
    }
    var compId = $("#hidCompId").val();
    var index = layerCommon.openWindow("选择商品", "../../Distributor/newOrder/selectgoods.aspx?disId=" + disId + "&CompId=" + compId + "&index=" + indexs + "&goodsInfoId=" + goodsInfoId + "&goodsInfoIdList=" + goodsInfoIdList, "985px", "630px");  //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}
//更多代理商
function gengduoDis() {
    var compId = $("#hidCompId").val();
    var index = layerCommon.openWindow("选择代理商", "optDis.aspx?CompId=" + compId, "755px", "630px");  //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}
//收货地址
function addre(disId, keyId) {
    $.ajax({
        type: "post",
        url: "../../Handler/orderHandle.ashx",
        data: { ck: Math.random(), ActionType: "getDisAdd", disId: disId, keyId: keyId },
        dataType: "text",
        success: function (data) {
            if (data != "") {
                $("#hidAddName").val($.trim(data.split('@@')[0]));
                $("#hidAddPhone").val($.trim(data.split('@@')[1]));
                $("#hidAdder").val($.trim(data.split('@@')[2]));
                $("#hrAdder").val($.trim(data.split('@@')[3]));
                if ($.trim(data.split('@@')[0]) != "") {
                    var str = "收货人：" +$.trim(data.split('@@')[0]) + "，联系电话：" + $.trim(data.split('@@')[1]) + "，收货地址：" + $.trim(data.split('@@')[2]);
                    $(".site .site-if").text(str)
                };
                $(".edit-ok .txt").html("可用返利￥ <label id=\"lblRebate\">" + formatMoney(parseFloat(data.split('@@')[4].replace(/,/gm, '')).toFixed(2), 2) + "</label><i class=\"sus-i  seebate\"></i>"); //返利金额
            }
        }
    })
}
//缓存数据
function GoodsInfoList(goodsInfolist, disId, compId, inindex, goodsInfoId) {
    if (goodsInfoId != "") {
        editGoods(disId, goodsInfoId);
    }
    // $(".tabLine table tbody tr:last").hide();
    $(".project2").val("");
    $(".search-opt .list").html("");
    $(".search-opt").hide();
    var index = $(".tabLine table tbody tr").length; //  - 1;
    var goodsInfoId = "";
    if (index == 0) {
        goodsInfoId = goodsInfolist;
    } else {
        var bol = false;
        for (var i = 0; i < goodsInfolist.split(',').length; i++) {
            $(".tabLine table tbody tr").each(function (indexss, objs) {
                if ($(".tabLine table tbody tr").eq([indexss]).attr("tip") != undefined) {
                    if (goodsInfolist.split(',')[i] == $(".tabLine table tbody tr").eq([indexss]).attr("tip")) {
                        bol = true;
                        return false;
                    } else {
                        bol = false;
                    }
                }
            })
            if (!bol) {
                goodsInfoId += goodsInfolist.split(',')[i] + ",";
            }
        }
        if (goodsInfoId != "") {
            goodsInfoId = goodsInfoId.substring(0, goodsInfoId.length - 1);
        }
    }
    if (goodsInfoId != "") {
        $.ajax({
            type: "post",
            url: "../../Handler/orderHandle.ashx",
            data: { ck: Math.random(), ActionType: "GoodsInfoList", goodsInfoId: goodsInfoId, disId: disId, compId: compId },
            dataType: "text",
            success: function (data) {
                if (data != "") {
                    GoodsList(goodsInfoId, compId, inindex)
                }
            }
        })
    }
}
//截取字符串
//商品名称，属性值，是否需要截取
function GetGoodsName(goodsName, valueInfo, type) {
    goodsName= stripscript(goodsName)
    var str = "";
    var str2 = "";
    if (valueInfo != "") {
        str2 = valueInfo.toString().substring(0, valueInfo.length - 1).toString().replace('；', ',');
    } else {
        str2 = valueInfo;
    }
    str = goodsName + "&nbsp;" + str2;
    if (type == "1") {
        if (str.length > 30) {
            str = str.substring(0, 30) + "...";
        }
    }
    return str;
};
//备注
function remarkinfo(type, remark, goodsInfoId, index) {
    remark = stripscript(remark)
    type = stripscript(type)
    goodsInfoId = stripscript(goodsInfoId)
    index = stripscript(index)
    if (type == 2) {
        if ($.trim(remark) == "") {
            $(".aremark" + index).text("添加");
        } else {
            $(".aremark" + index).text("编辑");
        }
        $(".aremark" + index).nextAll().remove();
        if ($.trim(remark) != "") {
            if (remark.length > 6) {
                var remark2 = remark.substring(0, 6) + "...";
                $(".aremark" + index).after("<div class=\"divremark" + index + "\">" + remark2 + "</div><div class=\"cur\">" + remark + "</div>");
            } else {
                $(".aremark" + index).after("<div class=\"divremark" + index + "\">" + remark + "</div><div class=\"cur\">" + remark + "</div>");
            }
        }
        onchengSum(goodsInfoId, index, 0);
        //GoodsInfoList(goodsInfoId);
    }
}
//关闭弹出窗口
function CloseGoods() {
    layerCommon.layerClose("hid_Alert");
}

//清楚缓存
function Clear() {
    $.ajax({
        type: "post",
        url: "../../Handler/orderHandle.ashx",
        data: { ck: Math.random(), ActionType: "Clear" },
        dataType: "text",
        success: function (data) {
            if (data != "") {

            }
        }
    })
}
//返利
function bate() {
    var yunf =stripscript( $.trim($("#hidPostFree").val())); //运费
    var batePrice =stripscript( $(".txtRebate").val()); //使用的返利
    if (isNaN(batePrice)) {
        batePrice = "0.00";
    }
    var Rebate = $("#lblRebate").text(); //返利总额
    var Total = $("#lblTotalAmount").text();  //订单总价
    var cux = $("#lblCux").text(); //促销
    if (parseFloat(batePrice) == parseFloat(0) || batePrice.toString() == "") {
        batePrice = 0;
        $(".txtRebate").val("0.00");
    }
    // alert(batePrice+","+Rebate)
    var re = Rebate.replace(/,/gm, '');
    Total = Total.replace(/,/gm, '');
    Total = parseFloat(Total) + parseFloat($.trim(yunf)) - parseFloat($.trim(cux));
    // Total = parseFloat(Total) - parseFloat(cux);
    if (parseFloat(batePrice) > parseFloat(re)) {
        var cyprice = parseFloat(Total) - parseFloat($.trim(cux));
        if (parseFloat(re) > parseFloat(cyprice)) {
            $("#txtRebate").val($.trim(parseFloat(cyprice).toFixed(2)));
            $("#lblFanl").text(" " + $.trim(parseFloat(cyprice).toFixed(2)));
            $(".price-sum  .price").text("￥ 0.00");
        } else {
            if (parseFloat(cyprice) > parseFloat(Rebate)) {
                $("#txtRebate").val($.trim(parseFloat(Rebate).toFixed(2)));
                $("#lblFanl").text(" " + $.trim(parseFloat(Rebate).toFixed(2)));
            } else {
                $("#txtRebate").val($.trim(parseFloat(cyprice).toFixed(2)));
                $("#lblFanl").text(" " + $.trim(parseFloat(cyprice).toFixed(2)));
            }

            var Total_bate = parseFloat(Total) - parseFloat(Rebate);
            if (Total_bate <= 0) {
                Total_bate = "0.00";
            }
            $(".price-sum .price").text("￥ " + formatMoney(Total_bate, 2));
        }
    } else {
        if (parseFloat(batePrice) >= parseFloat(Total)) {
            $("#txtRebate").val($.trim(parseFloat(Total).toFixed(2)));
            $("#lblFanl").text(" " + $.trim(parseFloat(Total).toFixed(2)));
            $(".price-sum .price").text("￥ 0.00");
        } else {
            var Total_bate = parseFloat(Total) - parseFloat(batePrice);
            if (Total_bate <= 0) {
                Total_bate = "0.00";
            }
            $(".price-sum .price").text("￥ " + formatMoney(Total_bate, 2));
            $("#lblFanl").text(" " + $.trim(parseFloat(batePrice).toFixed(2)));
            $("#txtRebate").val($.trim(parseFloat(batePrice).toFixed(2)));
        }
    }
}
//回调收货地址
function addr_info(id, principal, phone, Address) {
    principal = stripscript(principal)
    phone = stripscript(phone)
    Address = stripscript(Address)
    $("#hidAddName").val(principal);
    $("#hidAddPhone").val(phone);
    $("#hidAdder").val(Address);
    $("#hrAdder").text(id);
    $("#hrAdder").val(id);
    var html = "收货人：" + principal + "，联系电话：" + phone + "，收货地址：" + Address;
    $(".site .site-if").text(html);
}
//回调开票信息
function invinfo(DisAccID, val, LookUp, Context, Bank, Account, RegNo) {
    val = stripscript(val)
    LookUp = stripscript(LookUp)
    Context = stripscript(Context)
    Bank = stripscript(Bank)
    Account = stripscript(Account)
    RegNo = stripscript(RegNo)
    $("#hrOrderInv").text(DisAccID);
    $("#hidVal").val(val);
    $("#hidLookUp").val(LookUp);
    $("#hidContext").val(Context);
    $("#hidBank").val(Bank);
    $("#hidAccount").val(Account);
    $("#hidRegNo").val(RegNo);
    if (val == 0) {
        $(".invoice .in-if").text("不开票");
    } else if (val == 1) {
        $(".invoice .in-if").text("发票抬头：" + LookUp + "，发票内容：" + Context);
    } else if (val == 2) {
        $(".invoice .in-if").text("发票抬头：" + LookUp + "，发票内容：" + Context + "，开户银行：" + Bank + "，开户账户：" + Account + "，纳税人登记号：" + RegNo + "");
    }
}
//运费
function amount_info(type, tatol, num, num2) {
    type = stripscript(type)
    tatol = stripscript(tatol)
    num = stripscript(num)
    num2 = stripscript(num2)
    var postfee = $.trim($("#lblPostFee").text());
    $("#lblPostFee").text(parseFloat($.trim(tatol)).toFixed(2));
    $("#hidPostFree").val(parseFloat($.trim(tatol)).toFixed(2));
    var price = $(".price-sum .price").text();
    price = price.substring(price.indexOf("￥") + 1).replace(/,/gm, '');
    if (postfee == "0" || postfee == "0.00") {
        $(".price-sum .price").text("￥ " + formatMoney(parseFloat(parseFloat(tatol) + parseFloat($.trim(price))).toFixed(2), 2));
    } else {
        $(".price-sum .price").text("￥ " + formatMoney(parseFloat(parseFloat(tatol) + parseFloat($.trim(price)) - parseFloat(postfee)).toFixed(2), 2));
    }
}

function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
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