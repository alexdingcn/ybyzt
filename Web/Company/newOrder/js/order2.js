$(function () {
    var disId = $("#hidDisID").val();
    var isfanl = $("#hidFanl").val(); //返利是否启用
    var keyid = $.trim($("#hidKeyId").val());
    var flprice = $.trim($(".txtRebate").val()); //使用的返利金额
    var type2 = $.trim($("#hidType2").val()); //2再次购买
    if (isfanl == "0") {//不启用返利
        if (keyid == 0) {//新增
            $(".isbate").hide();
        } else {//编辑
            if (type2 == "2") {
                $(".isbate").hide(); //返利界面隐藏
            } else {
                if (parseFloat(flprice) > 0) {//返利金额》0
                    $(".isbate").show(); //返利界面显示
                } else {
                    $(".isbate").hide(); //返利界面隐藏
                }
            }
        }
    } else {
        $(".isbate").show(); //返利界面显示
    }
    //代理商筛选
    $(".txtDisName").keyup(function () {
        $(".jxs-box .name").show(); //列表层显示
        var hid = eval('(' + $(".divDisList").text() + ')'); //代理商json
        var strtext = $(this).val(); //输入的代理商关键字
        $(".jxs-box .name").html(""); //先清空列表
        // if ($.trim(strtext) != "") {
        $(hid).each(function (index, obj) {
            if ($(".jxs-box .name li").length < 5) {//最多显示5条数据
                if (obj.DisName.indexOf(strtext) != -1) {
                    $(".jxs-box .name").append("<li><a href=\"javascript:;\" tip=\"" + obj.ID + "\">" + obj.DisName + "</a></li>");
                }
            }
        })
        //        } else {
        //            $(".jxs-box .name").hide(); //隐藏
        //        }
    })
    //失去焦点，延迟加载
    var t = '';
    $(document).on("blur", ".project2", function () {
        clearTimeout(t);
        t = setTimeout("send_data()", 200); //选中了商品 延迟后关闭
    });
    var tt = '';
    $(document).on("blur", ".txtDisName", function () {
        clearTimeout(tt);
        tt = setTimeout("send_data2()", 200); //选中了代理商 延迟后关闭
    });
    //商品筛选
    $(document).on("keyup", ".project2", function () {
        var disId = $("#hidDisID").val();
        var strtext = $.trim($(this).val()); //输入的商品关键字或者商品编码
        var inindex = $(this).parent().parent().parent().index(); //当前行索引
        showGoods(strtext, inindex, disId); //筛选商品
    })
    //商品显示top5
    $(document).on("focus", ".project2", function () {
        var disId = $("#hidDisID").val();
        var inindex = $(this).parent().parent().parent().index(); //当前行索引
        //setTimeout("showGoods('', " + inindex + ", " + disId + ")", 1000);
        showGoods("", inindex, disId); //筛选商品
    })
    //代理商筛选
    $(".txtDisName").focus(function () {
        $(".jxs-box .search-opt").show(); //代理商列表显示
        $(".jxs-box .name").show(); //代理商列表显示
    })
    //代理商选中
    $(document).on("click", ".jxs-box .name li", function () {
        disId = $.trim($(this).find("a").attr("tip"));
        if (disId == $("#hidDisID").val()) {
            return;
        }
        // $(".add2").trigger("click"); //删除原有的商品数据行
        var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        $(".tabLine table tbody").append(html);
        $("#lblTotalAmount").text("0.00"); //清空商品总额
        $("#lblCux").text("0.00"); //清空促销优惠
        $("#lblPostFee").text("0.00"); //清空运费
        $("#lblYFPrice").text("0.00"); //清空应付总额
        $(".txtRebate").val("0.00"); //清空使用返利
        $("#lblFanl").text("0.00"); //清空返利折扣
        $("#hrOrderInv").val(""); //开票id
        $("#hidLookUp").val(""); //清空抬头
        $("#hidContext").val(""); //清空发票内容
        $("#hidBank").val(""); //清空银行
        $("#hidAccount").val(""); //清空账户
        $("#hidRegNo").val(""); //清空登记号
        $("#OrderNote").val(""); //清空订单备注

        var disname = $.trim($(this).find("a").text());
        disBind(disId, disname);
    })
    //选中筛选商品
    $(document).on("click", ".search-opt .list li", function () {
        var disId = $("#hidDisID").val();
        var goodsInfoId = $(this).attr("tip"); //goodsInfoid
        var compId = $("#hidCompId").val();
        var bol = false;
        var inindex = $(this).parent().parent().parent().parent().parent().index();
        //判断是否存在相同的商品
        $(".tabLine table tbody tr").each(function (indexss, objs) {
            if ($(".tabLine table tbody tr").eq([indexss]).attr("tip") != undefined) {
                //根据商品id与每行商品数据的tip比较
                if (goodsInfoId == $(".tabLine table tbody tr").eq([indexss]).attr("tip")) {
                    bol = true;
                    return false;
                } else {
                    bol = false;
                }
            }
        })
        if (bol) { layerCommon.msg("商品已存在", IconOption.错误); } else {
            GoodsList(goodsInfoId, compId, inindex, disId);
        }
    })
    //增加选择商品行
    $(document).on("click", ".minus2", function () {
        //新增行追加一个空的html
        var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        $(".tabLine table tbody").append(html);
    })
    //删除当前商品行
    $(document).on("click", ".add2", function () {
        var goodsInfoId = $(this).parent().parent().parent().attr("tip"); //当前商品数据行的商品id
        if (goodsInfoId != undefined) {
            var goosinfoList = $("#hidGoodsInfoId").val();
            goosinfoList += goodsInfoId + ",";
            $("#hidGoodsInfoId").val(goosinfoList); //记住删除的商品goodsinfoid 后面用了删除订单明细表的商品数据
        }
        if ($(".tabLine table tbody tr").length > 1) {
            $(this).parent().parent().parent().remove(); //大于1行时直接删除
        } else {
            $(this).parent().parent().parent().remove(); //小于等于1时 先删除 再添加一个空的html
            var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
            $(".tabLine table tbody").append(html);
        }

        var compId = $("#hidCompId").val();
        var trnum = $(".tabLine tbody tr").length;
        var batePrice = $(".txtRebate").val(); //使用的返利金额
        var pricestr = "0"; //计算商品总价
        for (var i = 0; i < trnum; i++) {
            if ($(".tabLine tbody tr").eq(i).attr("tip") != undefined) {
                var z = $(".tabLine tbody tr").eq(i).attr("trindex2");
                pricestr = parseFloat(pricestr) + parseFloat($.trim($(".tabLine tbody tr").eq(i).find(".lblTotal_" + z).html()).replace(/,/gm, '').substring(1));
            }
        }
        //商品总价
        SumTotal = pricestr; // parseFloat(parseFloat(Price) * parseFloat(Num)).toFixed(sDigits);
        // alert(SumTotal + "," + $.trim($("#lblTotalAmount").text()))
        //商品总价
        // var SumPrice = parseFloat(parseFloat($.trim($("#lblTotalAmount").text()).replace(/,/gm, '')) + parseFloat(SumTotal)).toFixed(2);
        // SumPrice = (SumPrice == 0 ? SumTotal : SumPrice);
        var SumPrice = parseFloat($.trim($("#lblTotalAmount").text()) != "0.00" ? SumTotal : $.trim($("#lblTotalAmount").text()).replace(/,/gm, '')).toFixed(2);
        $.ajax({
            type: "post",
            url: "../../Handler/orderHandle.ashx",
            data: { ck: Math.random(), ActionType: "GoodsInfo", disId: disId, compId: compId, SumTotal: SumPrice },
            // async: false,
            success: function (data, status) {
                var result = eval('(' + data + ')');
                var ds = result["ds"];
                if (ds == "True") {
                    var ZJSumTotal = formatMoney(parseFloat(pricestr).toFixed(2), 2); //商品总价
                    $("#lblTotalAmount").text(ZJSumTotal);
                    var psf = $.trim($("#lblPostFee").text().replace(/,/gm, '')); //运费
                    if (result["proPrice"].toString() != "") {
                        //有促销优惠
                        $("#lblCux").text(formatMoney(result["proPrice"].replace(/,/gm, ''), 2));
                        //商品总价
                        $("#lblYFPrice").text(formatMoney(parseFloat(pricestr) - parseFloat(result["proPrice"].replace(/,/gm, '')) - parseFloat(batePrice.replace(/,/gm, '')) + parseFloat(psf), 2));
                    }
                    else {
                        //没促销优惠
                        $("#lblCux").text(parseFloat(0).toFixed(2));
                        //商品总价
                        $("#lblYFPrice").text(formatMoney(parseFloat(pricestr) - parseFloat(batePrice.replace(/,/gm, '')) + parseFloat(psf), 2));
                    }
                }
            },
            //            complete: function () {
            //                $(".txtRebate").trigger("blur"); //再次计算商品价格 因为存在返利金额
            //            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("服务器异常，请稍后再试", IconOption.错误);
            }
        });
    })
    //修改单价
    $(document).on("blur", ".boxs", function () {
        var id = $(this).parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).attr("tip2"); //index
        onchengSum(id, index, 0, disId);
    })
    //商品数量减
    $(document).on("click", ".divnum .minus", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, -1, disId);
    })
    //商品数量框
    $(document).on("change", ".divnum .txtGoodsNum", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, 0, disId);
    })
    //商品数量增
    $(document).on("click", ".divnum .add", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, 1, disId);
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
        if (disId == undefined || disId == "" && $("#hidDisID").val() == "") {
            layerCommon.msg("请选择代理商", IconOption.错误);
            return false;
        } else {
            if (disId == undefined || disId == "") {
                disId = $("#hidDisID").val();
            }
        }
        var id = $("#hidVal").val();
        var lookup = $("#hidLookUp").val(); //抬头
        var context = $("#hidContext").val(); //发票内容
        var bank = $("#hidBank").val(); //银行
        var account = $("#hidAccount").val(); //账户
        var regno = $("#hidRegNo").val(); //登记号
        var index = layerCommon.openWindow("开票信息", "../../Distributor/newOrder/InvoiceInfo.aspx?DisId=" + disId + "&val=" + id + "&Rise=" + lookup + "&Context=" + context + "&Bank=" + bank + "&Account=" + account + "&RegNo=" + regno, "570px", "440px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })
    //返回
    $(".gray-btn").click(function () {
        location.href = "../Order/OrderCreateList.aspx"; //代客下单返回
    })
    //返回
    $(".gray-btn2").click(function () {
        location.href = "../OrderList.aspx"; //代理商返回
    })
    //更多商品3
    $(document).on("click", ".sPic", function () {
        var indexs = $(this).parent().parent().index(); //当前行索引
        var goodsInfoId = $(this).parent().parent().attr("tip"); //当前行商品id
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        if (goodsInfoId != undefined) {
            gengduo(indexs, goodsInfoId, goodsInfoIdList, disId); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
        } else {
            gengduo(indexs, "", "", disId); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
        }
    })
    //更多商品2
    $(document).on("click", ".tabLine .opt-i", function () {
        var indexs = $(this).parent().parent().parent().index(); //当前行索引
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList, disId); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
    })
    //更多商品1
    $(document).on("click", ".tabLine .search-opt .opt", function () {
        var indexs = $(this).parent().parent().parent().parent().index(); //当前行索引
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList, disId); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
    })
    //更多代理商
    $(document).on("click", ".jxs-box  .search-opt .opt,.jxs-box .opt-i", function () {
        gengduoDis(); //更多代理商
    })
    //商品添加备注
    $(document).on("click", ".alink a", function () {
        var compId = $("#hidCompId").val();
        var goodsInfoId = $.trim($(this).parent().parent().parent().attr("tip")); //goodsinfoId
        var indexs = $.trim($(this).parent().parent().parent().attr("trindex2")); //当前行的索引
        var remark = $.trim($(this).parent().find(".cur").text()); //当前行的商品备注
        var index = layerCommon.openWindow("添加备注", "../../Distributor/newOrder/remarkview.aspx?disId=" + disId + "&CompId=" + compId + "&type=2&KeyID=" + goodsInfoId + "&index=" + indexs + "&remark=" + remark, "770px", "450px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })
    //收货地址
    $(".site .edit-i").click(function () {
        if (disId == undefined || disId == "" && $("#hidDisID").val() == "") {
            layerCommon.msg("请选择代理商", IconOption.错误);
            return false;
        } else {
            if (disId == undefined || disId == "") {
                disId = $("#hidDisID").val();
            }
        }
        var compId = $("#hidCompId").val();
        var index = layerCommon.openWindow("收货地址", "../../Distributor/newOrder/addrinfo.aspx?DisID=" + disId + "&CompId=" + compId, "750px", "590px");  //记录弹出对象
        $("#hid_Alert").val(index); //记录弹出对象
    })
    //运费
    $(".postfee").click(function () {
        if (disId == undefined || disId == "" && $("#hidDisID").val() == "") {
            layerCommon.msg("请选择代理商", IconOption.错误);
            return false;
        } else {
            if (disId == undefined || disId == "") {
                disId = $("#hidDisID").val();
            }
        }
        var tatol = $("#lblPostFee").text(); //运费
        var keyId = $("#hidKeyId").val();
        var url = 'amountof.aspx?type=2&DisID=' + disId + '&KeyID=' + keyId + '&t=' + tatol;
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
    //提交订单
    $(document).on("click", ".btn-box .btn-area", function () {
        //$(".btn-box .btn-area").click(function () {

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
        $(".po-bg2").removeClass("none"); //等待跳转的层
        $(".p-delete2").removeClass("none"); //等待跳转的层
        setTimeout("insertGoods()", 200); //延迟操作

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
function insertGoods() {
    var time =stripscript( $.trim($(".Wdate").val())); //交货时间
    var remark =stripscript( $.trim($("#OrderNote").val())); //订单备注
    var atta =stripscript( $.trim($("#hrOrderFj").val())); //附件
    var givemodel = stripscript($.trim($("#hidPsType").val())); //配送方式
    var postfee =stripscript( $.trim($("#lblPostFee").text())); //运费
    var addrid = stripscript($.trim($("#hrAdder").val())); //收货信息id
    var principal = stripscript($.trim($("#hidAddName").val())); //收货人
    var phone = stripscript($.trim($("#hidAddPhone").val())); //收货人手机
    var address = stripscript($.trim($("#hidAdder").val())); //收货地址
    var bateamount =stripscript( $.trim($(".txtRebate").val())); //返利金额
    var disaccid = stripscript($.trim($("#hrOrderInv").val())); //开票id
    var rise = stripscript($.trim($("#hidLookUp").val())); //发票抬头
    var content = stripscript($.trim($("#hidContext").val())); //发票类容
    var obank = stripscript($.trim($("#hidBank").val())); //开户银行
    var oaccount =stripscript( $.trim($("#hidAccount").val())); //开户帐号
    var trnumber = stripscript($.trim($("#hidRegNo").val())); //登记号
    var isobill =stripscript( $.trim($("#hidVal").val())); //是否开发票
    var hidts = stripscript($.trim($("#hidts").val())); //时间戳
    var cux = stripscript($.trim($("#lblCux").text().replace(/,/gm, ''))); //促销
    var keyid = stripscript($.trim($("#hidKeyId").val()));
    var disId = stripscript($.trim($("#hidDisID").val()));
    var compID = stripscript($.trim($("#hidCompId").val()));
    var type2 = stripscript($.trim($("#hidType2").val()));
    var type3 = stripscript($.trim($("#hidType").val())); //是否购物车购买
    var json = "[{\"disid\":\"" + disId + "\",\"arrivedate\":\"" + time + "\",\"remark\":\"" + remark + "\",\"atta\":\"" + atta + "\",\"givemode\":\"" + givemodel + "\",\"postfee\":\"" + postfee + "\",\"addrid\":\"" + addrid + "\",\"principal\":\"" + principal + "\",\"phone\":\"" + phone + "\",\"address\":\"" + address + "\",\"bateamount\":\"" + bateamount + "\",\"disaccid\":\"" + disaccid + "\",\"rise\":\"" + rise + "\",\"content\":\"" + content + "\",\"obank\":\"" + obank + "\",\"oaccount\":\"" + oaccount + "\",\"trnumber\":\"" + trnumber + "\",\"isobill\":\"" + isobill + "\",\"ts\":\"" + hidts + "\",\"cx\":\"" + cux + "\",\"type3\":\"" + type3 + "\",\"orderdetail\":[";
    $(".tabLine tbody tr").each(function (i, obj) {
        var tip = $(".tabLine tbody tr").eq(i).attr("tip");
        if (tip != undefined && tip != "") {
            var price = "";
            if ($(".btn-area").attr("tip") == "1") {
                price = $.trim($(".tabLine tbody tr").eq(i).find("td:eq(3)").find(".boxs").val()); //商品价格
            } else {
                price = $.trim($(".tabLine tbody tr").eq(i).find("td:eq(3)").find("div").attr("tip")); //商品价格
            }
            var price2 = $.trim($(".tabLine tbody tr").eq(i).find("td:eq(3)").find(".hidPrice").val()); //商品原始价格
            var num = $.trim($(".tabLine tbody tr").eq(i).find(".txtGoodsNum ").val()); //商品购买数量
            var remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink a").text()); //商品备注
            var id = $.trim($(".tabLine tbody tr").eq(i).attr("id")); //商品id
            if (remark == "添加") {
                remark = "";
            } else {
                remark = $.trim($(".tabLine tbody tr").eq(i).find(".alink .cur").text());
            }
            if (i == 0) {
                json += "{\"id\":\"" + id + "\",\"disid\":\"" + disId + "\",\"goodsinfoid\":\"" + tip + "\",\"price\":\"" + price + "\",\"price2\":\"" + price2 + "\",\"remark\":\"" + remark + "\",\"goodsnum\":\"" + num + "\"}";
            } else {
                json += ",{\"id\":\"" + id + "\",\"disid\":\"" + disId + "\",\"goodsinfoid\":\"" + tip + "\",\"price\":\"" + price + "\",\"price2\":\"" + price2 + "\",\"remark\":\"" + remark + "\",\"goodsnum\":\"" + num + "\"}";
            }
        }
    })
    json += "]}]";
    $.ajax({
        type: "post",
        url: "../../Handler/orderHandle.ashx",
        data: { ck: Math.random(), ActionType: "insertOrder", json: json, keyid: keyid == undefined ? 0 : keyid, goodsinfoId: $.trim($("#hidGoodsInfoId").val()), utype: $(".btn-area").attr("tip"), type: type2, DisId: disId, compID: compID },
        dataType: "text",
        success: function (data) {
            var result = eval('(' + data + ')');
            var ds = result["ds"];
            if (ds == "True") {
                location.href = "orderdetail.aspx?KeyID=" + $.trim(result["KeyID"]) + "&top=1"; //edit by hgh 0改为1
            } else {
                layerCommon.msg(result["msg"], IconOption.哭脸, 4000);
            }
            return false;
        }, complete: function () {
            $(".po-bg2").addClass("none");
            $(".p-delete2").addClass("none");
        }, error: function () {
            layerCommon.msg("提交失败", IconOption.错误);
            return false;
        }
    })
}
function send_data() {
    $(".tabLine .search-opt").hide(); //隐藏商品列表
}
function send_data2() {
    $(".jxs-box .search-opt").hide(); //隐藏代理商列表
}
//dis绑定
function disBind(disId, disname) {
    disname= stripscript(disname)
    // disId = disIds;
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
            var type = $("#hidType").val(); //2再次购买
            if (type == "2") {
                addre($.trim(disId), 0);
            } else {
                addre($.trim(disId), keyId);
            }
        }
    })
}
//更多商品（当前行索引，当前行商品id，每一行的商品id拼接,代理商id）
function gengduo(indexs, goodsInfoId, goodsInfoIdList, disId) {
    if (disId == undefined || disId == "" && $("#hidDisID").val() == "") {
        layerCommon.msg("请选择代理商", IconOption.错误);
        return false;
    } else {
        if (disId == undefined || disId == "") {
            disId = $("#hidDisID").val();
        }
    }
    var Utype = $("#hidUtype").val();
    var compId = $("#hidCompId").val();
    var index = layerCommon.openWindow("选择商品", "../../Distributor/newOrder/selectgoods.aspx?disId=" + disId + "&CompId=" + compId + "&index=" + indexs + "&goodsInfoId=" + goodsInfoId + "&goodsInfoIdList=" + goodsInfoIdList + "&Utype=" + Utype, "985px", "630px");  //记录弹出对象
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
                $("#hidAddName").val($.trim(data.split('@@')[0])); //联系人
                $("#hidAddPhone").val($.trim(data.split('@@')[1])); //联系人手机
                $("#hidAdder").val($.trim(data.split('@@')[2])); //收货地址
                $("#hrAdder").val($.trim(data.split('@@')[3])); //收货地址
                if ($.trim(data.split('@@')[0]) != "") {
                    var str = "收货人：" + $.trim(data.split('@@')[0]) + "，联系电话：" + $.trim(data.split('@@')[1]) + "，收货地址：" + $.trim(data.split('@@')[2]);
                    $(".site .site-if").text(str)
                };
                $(".edit-ok .txt").html("可用返利￥ <label id=\"lblRebate\">" + formatMoney(parseFloat(data.split('@@')[4].replace(/,/gm, '')).toFixed(2), 2) + "</label><i class=\"sus-i  seebate\"></i>"); //返利金额
            }
        }
    })
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
function remarkinfo(type, remark, goodsInfoId, index, disId) {
    remark=stripscript(remark)
    goodsInfoId=stripscript(goodsInfoId)
    index=stripscript(index)
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
        onchengSum(goodsInfoId, index, 0, disId);
    }
}
//返利
function bate() {
    var zyunf = $.trim($("#lblRebate").text().replace(/,/gm, '')); //总的可用返利
    var yunf = $.trim($("#lblPostFee").text().replace(/,/gm, '')); // $.trim($("#hidPostFree").val()); //运费
    var batePrice = $.trim($(".txtRebate").val()); //使用的返利
    if (isNaN(batePrice)) {
        batePrice = "0.00";
    }
    if (parseFloat(zyunf) < parseFloat(batePrice)) {
        layerCommon.msg("使用返利不能大于可用返利", IconOption.错误);
        return false;
    }
    var Total = $("#lblTotalAmount").text();  //订单总价
    var cux = $("#lblCux").text(); //促销
    if (parseFloat(batePrice) == parseFloat(0) || batePrice.toString() == "") {
        batePrice = 0;
        $(".txtRebate").val("0.00");
    }
    Total = Total.replace(/,/gm, '');
    Total = parseFloat(Total) + parseFloat($.trim(yunf)) - parseFloat($.trim(cux));
    $("#lblFanl").text(formatMoney(parseFloat(batePrice).toFixed(2), 2));
    if (parseFloat(Total) - parseFloat(batePrice) > 0) {
        $("#lblYFPrice").text(formatMoney(parseFloat(parseFloat(Total) - parseFloat(batePrice)).toFixed(2), 2));
    } else {
        $("#lblYFPrice").text("0.00");
    }
}
//回调收货地址
function addr_info(id, principal, phone, Address) {
    principal=stripscript(principal)
    phone=stripscript(phone)
    Address=stripscript(Address)
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
    val=stripscript(val)
    LookUp=stripscript(LookUp)
    Context=stripscript(Context)
    Bank=stripscript(Bank)
    Account=stripscript(Account)
    RegNo=stripscript(RegNo)
    $("#hrOrderInv").val(DisAccID); //发票id
    $("#hidVal").val(val); //判断开的什么发票
    $("#hidLookUp").val(LookUp); //抬头
    $("#hidContext").val(Context); //发票内容
    $("#hidBank").val(Bank); //银行
    $("#hidAccount").val(Account); //帐号
    $("#hidRegNo").val(RegNo); //登记号
    if (val == 0) {
        $(".invoice .in-if").text("不开票");
    } else if (val == 1) {
        $(".invoice .in-if").text("发票抬头：" + LookUp + "，发票内容：" + Context + "，纳税人登记号：" + RegNo + "");
    } else if (val == 2) {
        $(".invoice .in-if").text("发票抬头：" + LookUp + "，发票内容：" + Context + "，开户银行：" + Bank + "，开户账户：" + Account + "，纳税人登记号：" + RegNo + "");
    }
}
//运费
function amount_info(type, tatol, num, num2) {
    type=stripscript(type)
    tatol=stripscript(tatol)
    num=stripscript(num)
    num2=stripscript(num2)
    var postfee = $.trim($("#lblPostFee").text().replace(/,/gm, '')); // $.trim($("#hidPostFree").val());
    $("#lblPostFee").text(formatMoney(parseFloat($.trim(tatol)).toFixed(2), 2));
    $("#hidPostFree").val(parseFloat($.trim(tatol)).toFixed(2));
    var Total = $("#lblTotalAmount").text();  //订单总价
    var cux = $("#lblCux").text(); //促销
    var batePrice = $(".txtRebate").val(); //使用的返利
    Total = Total.replace(/,/gm, '');
    Total = parseFloat(Total) + parseFloat(tatol) - parseFloat($.trim(cux)) - parseFloat($.trim(batePrice));
    if (parseFloat(Total) < 0) {
        Total = "0.00";
    }
    $("#lblYFPrice").text(formatMoney(parseFloat(Total).toFixed(2), 2));
}
//关闭弹出窗口
function CloseGoods() {
    layerCommon.layerClose("hid_Alert");
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