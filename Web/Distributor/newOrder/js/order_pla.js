/************************* 商品选择 start **************************************/

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
            var Digits = $("#hidsDigits").val();
            var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
            if (parseInt(ProType) == 3) {
                //商品促销满送
                str += "满" + parseFloat(proDiscount).toFixed(sDigits) + unit + " ，获赠商品（" + parseFloat(proGoodsPrice).toFixed(sDigits) + "）" + unit;
            }
            else if (parseInt(ProType) == 4) {
                //商品促销打折
                str += "在原订货价基础上打" + parseFloat(proDiscount / 10) + "折";
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

//保存选中的商品信息ID
function selectGoods() {
    var goodsinfoID = $("#hidSelectGoods").val();
    //分页选中的商品
    var str = "";
    if (goodsinfoID.toString() != "")
        str += goodsinfoID;

    var infoid = str.split(",");

    $("table.goods tbody tr").each(function (item) {
        var chkInfo = $(this).find(".chkbox").val();
        if ($(this).find(".chkbox").is(':checked')) {
            if ($.inArray(chkInfo, infoid) < 0)
                str += chkInfo + ",";
            //  if (str.indexOf(chkInfo) < 0)
            //  str += chkInfo + ",";
        }
        else {
            if (str.length > 0) {
                chkInfo = chkInfo + ",";
                str = str.replace(new RegExp(chkInfo), "");
            }
        }
    });
    $("#hidSelectGoods").val(str);
}

var SaleFunction = {
    BindSaleData: function (data) {
        var json = data.rows;
        var OutHTML = "";
        selectGoods();

        var Digits = $("#hidsDigits").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var Stock = $.trim($("#hidStock").val());

        var $IsInve = $("#hidIsInve").val();
        var $In = 6;
        if (parseInt($IsInve) == 1)
            $In = 5;

        if (json.length > 0) {
            $.each(json, function (index, item) {
                OutHTML += "<tr>";
                OutHTML += '<td class="t5" align="center"><input type="checkbox" id="checkbox-3-' + index + '" class="regular-checkbox chkbox" value="' + item.ID + '" /><label for="checkbox-3-' + index + '"></label></td>';
                OutHTML += '<td class=""><div class="sPic">';
                if (item.Pic) {
                    OutHTML += '<span><a href="javascript:;"><img src="' + $("#hidImgViewPath").val() + item.Pic + '?x-oss-process=style/resize200" width="40" height="40"></a></span>';
                } else {
                    OutHTML += '<span><a href="javascript:;"><img src="/images/Goods400x400.jpg" width="40" height="40"></a></span>';
                }
                OutHTML += '<a href="javascript:;" class="code">商品编码：' + item.BarCode + sale(item.ProID, item.proGoodsPrice, item.proDiscount, item.proTypes, item.ProType, item.unit) + '</a><a href="javascript:;" class="name">' + sub(item.GoodsName, 25, "...") + '<i>' + item.GoodsName + '</i></a></div></td>';
                OutHTML += '<td class="t2"><div class="tl">' + item.ValueInfo.replace(/:/g, "：").replace(/；/g, "，").substr(0, item.ValueInfo.length - 1) + '&nbsp;</div></td>';
                OutHTML += '<td class="t5"><div class="tc">' + item.Unit + '&nbsp;</div></td>';
                OutHTML += '<td class="t3"><div class="tc">￥' + item.pr.toFixed(2) + '&nbsp;</div></td>';
                if (parseInt($IsInve) == 0) {
                    var Inventory = Stock == "1" ? item.StockNum == "" ? "" : item.StockNum.toFixed(sDigits) : item.Inventory.toFixed(sDigits);
                    OutHTML += '<td class="t3"><div class="tc">' + Inventory + '&nbsp;</div></td>';
                }
                OutHTML += "</tr>";
            });

        } else {
            OutHTML += '<tr><td colspan="' + $In + '" class="t2"><div class="tc">暂无数据</div></td></tr>';
        }
        $("table.goods tbody").empty().append(OutHTML);

        //js判断ie8及以下版本
        var DEFAULT_VERSION = "8.0";
        var ua = navigator.userAgent.toLowerCase();
        var isIE = ua.indexOf("msie") > -1;
        var safariVersion;
        if (isIE) {
            safariVersion = ua.match(/msie ([\d.]+)/)[1];
        }
        if (safariVersion <= DEFAULT_VERSION) {
            $(".selectnoAll").click();
        } else {
            $(".selectnoAll").siblings("input[type=\"checkbox\"]").click();
        }
        $(".selectnoAll").attr("class", "regular-checkbox selectAll");

        //显示上页上次选中的商品信息
        var str = $("#hidSelectGoods").val();
        if (str.toString() != "" || typeof (str) != "undefined" || str != null) {
            //str = str.substr(0, str.length - 1);
            var infoid = str.split(",");
            if (infoid.length > 0) {
                $("table.goods tbody tr").each(function (item) {
                    var iiid = $(this).find(".chkbox").val();
                    //if (infoid.indexOf(iiid) >= 0)
                    if ($.inArray(iiid, infoid) > -1)
                        $(this).find(".chkbox").click();
                });
            }
        }

        return this;
    },
    GetSalePaginData: function () {
        $(".paging").myPagination({
            currPage: 1,
            pageCount: 1,
            pageSize: 8,
            btnsize: 5,
            IsShowOnePaging: true,
            cssStyle: 'myPagination',
            info: {
                msg_on: false,
                first_on: false,
                last_on: false,
                prev: "上一页",
                next: "下一页"
            },
            ajax: {
                on: true,
                callback: 'SaleFunction.BindSaleData',
                url: "../../Handler/GetPageDataSource.ashx?PageAction=GetGoods",
                dataType: 'json',
                params: { "CompId": $.trim($("#hidCompId").val()), "DisId": $.trim($("#hidDisId").val()), "goodsInfoIdList": $.trim($("#hidgoodsInfoIdList").val()), "txtGoods": $.trim($("#txtGoods").val()), "goodspro": $.trim($("#agoodspro").attr("tip")), "wwwtype": $.trim($("#hidtype").val()), "pro": $.trim($("#apro").attr("tip")), cat: $.trim($(".category").attr("tip")), Utype: $.trim($("#hidUtype").val()) },
                ajaxStart: function () {
                    //$("table.goods tbody").empty().append('<tr><td colspan="9" style="width:100%;">数据正在加载中请稍候....</td></tr>');
                }, ajaxStop: function () {
                },
                ajaxError: function () {
                    var $IsInve = $("#hidIsInve").val();
                    var $In = 6;
                    if (parseInt($IsInve) == 1)
                        $In = 5;

                    $("table.goods tbody").empty().append('<tr><td colspan="' + $In + '" class="t2"><div class="tc">暂无数据</div></td></tr>');
                }
            }
        });
        return this;
    },
    BindSaleBtnEvent: function () {
        $(".searchBtn").on("click", function () {
            SaleFunction.GetSalePaginData();
        })
        //促销商品
        $("#goodspro").on("click", function () {
            var ispro = $("#agoodspro").attr("tip");
            if (ispro.toString() == "" || typeof (ispro) == "undefined" || ispro == null) {
                $("#agoodspro").attr("class", "k-icon2");
                $("#agoodspro").attr("tip", "1");
            } else {
                $("#agoodspro").attr("class", "k-icon");
                $("#agoodspro").attr("tip", "");
            }
            SaleFunction.GetSalePaginData();
        });
        //特价促销
        //        $("#pro").on("click", function () {
        //            var ispro = $("#apro").attr("tip");
        //            if (ispro.toString() == "" || typeof (ispro) == "undefined" || ispro == null) {
        //                $("#apro").attr("class", "k-icon2");
        //                $("#apro").attr("tip", "0");
        //            } else {
        //                $("#apro").attr("class", "k-icon");
        //                $("#apro").attr("tip", "");
        //            }
        //            SaleFunction.GetSalePaginData();
        //        });
        //商品分类搜索
        $(document).on("click", "div.goodscat .sorts a,.sorts .sorts1 a", function () {
            var tip = $(this).attr("tip");
            var tipvla = $(this).text();

            $(".category").text(tipvla);
            $(".category").attr("tip", tip);
            $(this).parents().parents("div.goodscat").css("display", "none");
            SaleFunction.GetSalePaginData();
        });

        return this;
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

$(function () {
    SaleFunction.GetSalePaginData().BindSaleBtnEvent();

    //取消
    $(document).on("click", "#btnCancel", function () {
        window.parent.CloseGoods();
    });

    //确定
    $("#btnConfirm").click(function () {
        var str = "";
        $("table.goods tbody tr").each(function (item) {
            if ($(this).find(".chkbox").is(':checked'))
                str += $(this).find(".chkbox").val() + ",";
        });
        str = str + $("#hidSelectGoods").val();

        if (str != "") {
            str = str.substring(0, str.length - 1);
            var disId = $("#hidDisId").val();
            var compId = $("#hidCompId").val();
            var index = $("#hidIndex").val();
            var goodsInfoId = $("#hidgoodsInfoId").val();
            // window.parent.GoodsInfoList(str, disId, compId, index, goodsInfoId);
            if (goodsInfoId != "" && goodsInfoId != undefined) {
                window.parent.$("#hidGoodsInfoId").val(goodsInfoId + ",");
            }
            window.parent.GoodsList(str, compId, index, disId);
        }
        window.parent.CloseGoods();
    });

    //全选/反选
    $(document).on("click", ".selectAll", function () {
        $(this).attr("class", "regular-checkbox selectnoAll");
        $("table.goods tbody tr").each(function (item) {
            $chk = $(this).find(".chkbox");
            //if (!$($chk).is(':checked')) {
            //  $($chk).click();
            //} else {
            //  $($chk).prop("checked", false)
            //  }
            //$($chk).click();
            $($chk).prop("checked", true);
        });
    });
    $(document).on("click", ".selectnoAll", function () {
        $(this).attr("class", "regular-checkbox selectAll");
        // 取消全选 
        $("table.goods tbody tr").each(function (item) {
            $chk = $(this).find(".chkbox");
            $($chk).prop("checked", false);
            //$($chk).click();
        });
    })



    //tr选中商品
    $(document).on("click", "table.goods tbody tr td", function () {
        if ($(this).index() != 0) {
            //        var $chk = $(this).siblings("td:first").find("input[type=\"checkbox\"]");
            var $chk = $(this).parent().find("td:first").find("input[type=\"checkbox\"]");
            $($chk).click();
        }
    });

    //    $(document).on("click", "table.goods tbody tr td:first label", function () {
    //        var $chk = $(this).siblings("input[type=\"checkbox\"]");
    //        $($chk).click();
    //    });

    //商品分类折叠开
    $(document).on("click", "div.goodscat .sorts1 .arrow2,div.goodscat .sorts2 .arrow3", function () {
        var arrow = $(this).attr("class");
        if (arrow == "arrow2") {
            $(".sorts2").css("display", "none");
            var tipdis = $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis");
            if (tipdis == "no") {
                $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis", "ok");
                $(this).parent().siblings("ul[class=\"sorts2\"]").css("display", "block");
            }
            else {
                $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis", "no");
                $(this).parent().siblings("ul[class=\"sorts2\"]").css("display", "none");
            }
        } else {
            $(".sorts3").css("display", "none");
            var tipdis = $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis");
            if (tipdis == "no") {
                $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis", "ok");
                $(this).siblings("ul[class=\"sorts3\"]").css("display", "block");
            }
            else {
                $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis", "no");
                $(this).siblings("ul[class=\"sorts3\"]").css("display", "none");
            }
        }
    });

    //商品分类
    $(document).on({
        "mouseenter": function (e) {
            $(".menu2").show();
        },
        "mouseleave": function (e) {
            $(".menu2").hide();
        }
    }, ".ca-box");

});
/************************* 商品选择 end **************************************/