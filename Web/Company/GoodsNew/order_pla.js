/************************* 商品选择 start **************************************/
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
        if (json.length > 0) {
            $.each(json, function (index, item) {
                OutHTML += "<tr>";
                OutHTML += '<td class="t5" align="center"><input type="checkbox" id="checkbox-3-' + index + '" class="regular-checkbox chkbox" value="' + item.ID + '" /><label for="checkbox-3-' + index + '"></label></td>';
                OutHTML += '<td class=""><div class="sPic"><span><a href="javascript:;"><img src="' + $("#hidImgViewPath").val() + item.Pic + '" width="40" height="40"></a></span> <a href="javascript:;" class="code">商品编码：' + item.BarCode + '</a><a href="javascript:;" class="name">' + sub(item.GoodsName, 25, "...") + '<i>' + item.GoodsName + '</i></a></div></td>';
                OutHTML += '<td class="t2"><div class="tl">' + item.ValueInfo.replace(/:/g, "：").replace(/；/g, "，").substr(0, item.ValueInfo.length - 1) + '&nbsp;</div></td>';
                OutHTML += '<td class="t5"><div class="tc">' + item.Unit + '&nbsp;</div></td>';
                OutHTML += '<td class="t3"><div class="tc">￥' + item.TinkerPrice.toFixed(2) + '&nbsp;</div></td>';
                OutHTML += "</tr>";
            });

        } else {
            OutHTML += '<tr><td colspan="5" class="t2"><div class="tc">暂无数据</div></td></tr>';
        }
        $("table.goods tbody").empty().append(OutHTML);

        $(".selectnoAll").siblings("input[type=\"checkbox\"]").click();
        $(".selectnoAll").attr("class", "selectAll");

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
                url: "../../Handler/GetPageDataSource.ashx?PageAction=GetGoods2",
                dataType: 'json',
                params: { "CompId": $.trim($("#hidCompId").val()), "goodsInfoIdList": $.trim($("#hidgoodsInfoIdList").val()), "txtGoods": $.trim($("#txtGoods").val()), cat: $.trim($(".category").attr("tip")) },
                ajaxStart: function () {
                    //$("table.goods tbody").empty().append('<tr><td colspan="9" style="width:100%;">数据正在加载中请稍候....</td></tr>');
                }, ajaxStop: function () {
                },
                ajaxError: function () {
                    $("table.goods tbody").empty().append('<tr><td colspan="5" class="t2"><div class="tc">暂无数据</div></td></tr>');
                }
            }
        });
        return this;
    },
    BindSaleBtnEvent: function () {
        $(".searchBtn").on("click", function () {
            SaleFunction.GetSalePaginData();
        })
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
            var compId = $("#hidCompId").val();
            var index = $("#hidIndex").val();
            var goodsInfoId = $("#hidgoodsInfoId").val();
            window.parent.GoodsList(str, compId, index);
        }
        window.parent.CloseGoods();
    });

    //全选/反选
    $(document).on("click", ".selectAll", function () {
        $(this).attr("class", "selectnoAll");
        $("table.goods tbody tr").each(function (item) {
            $chk = $(this).find(".chkbox");
            //                if (!$($chk).is(':checked')) {
            //                    $($chk).click();
            //                } else {
            //                    $($chk).prop("checked", false)
            //                }
            //$($chk).click();
            $($chk).prop("checked", true);
        });
    });
    $(document).on("click", ".selectnoAll", function () {
        $(this).attr("class", "selectAll");
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
        "mouseover": function (e) {
            $(".menu2").css("display", "block");
        },
        "mouseout": function (e) {
            $(".menu2").css("display", "none");
        }
    }, ".ca-box");

});
/************************* 商品选择 end **************************************/