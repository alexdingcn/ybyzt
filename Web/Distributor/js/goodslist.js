$(function () {
    var isSc = $(".hidShouc").val(); //判断收藏页面还是列表页面
    if (isSc == "sc") {
        $("#navigation2").text("收藏商品");
    } else {
        $("#navigation2").text("商品列表");
    }
    SaleFunction.GetSalePaginData().BindSaleBtnEvent();
    //文本框
    $(".goods-li").delegate("input:text.txtKeyInt", {
        keyup: function () {
            KeyInt(this, "");
        },
        blur: function () {
            KeyInt(this, "1");
        }
    });
    //多规格切换
    $(document).on("click", ".specs .n a", function () {
        var thiss = this;
        $(this).addClass("hover").siblings().removeClass("hover");
        var attr = "";
        var str = ""; //列表和大图的区别
        //列表
        if ($.trim($(thiss).parents(".goods-zs").attr("class")) == "goods-zs back3") {
            str = "li";
        } else {
            //大图
            str = ".li";
        }
        $(this).parent().parent().parent().find(str).each(function (index, obj) {
            if ($.trim($(this).find(".t").text()) != "数量：") {
                attr = attr + $.trim($(this).find(".t").text());
                $(this).find(".n a").each(function (index2, obj) {
                    if ($(this).attr("class") == "hover") {
                        attr = attr + $.trim($(this).text()) + "；";
                        return true;
                    }
                })
            }
        })
        attr = attr.replace(/选择/gm, '').replace(/：/gm, ':');
        var goodsids = "";
        if ($.trim($(thiss).parents(".goods-zs").attr("class")) == "goods-zs back3") {//判断是否大图还是列表
            goodsids = $(this).parents("tr").attr("tip");
        } else {
            goodsids = $(this).parents("li").attr("tip");
        }
        $.ajax({
            type: "post",
            url: "/Handler/GetPageDataSource.ashx?PageAction=GetGoodsInfo1",
            data: { ck: Math.random(), CompId: $.trim($("#hidCompId").val()), DisId: $.trim($("#hidDisId").val()), valueinfo: attr, goodsId: goodsids },
            dataType: "json",
            success: function (data) {
                var url = window.location.href.lastIndexOf("?"); //判断是否收藏商品页面
                var can = ""; //是否拼接传递到info页面的字符串
                if (url != -1) {
                    can = "&sc=sc";
                }
                if ($.trim($(thiss).parents(".goods-zs").attr("class")) == "goods-zs back3") {
                    var Digits = $("#hidsDigits").val(); //小数位
                    var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
                    $(data).each(function (index, obj) {
                        $(thiss).parents("tr").find("td").eq(2).find(".tc").text(parseFloat(obj.Inventory).toFixed(sDigits)); //库存
                        $(thiss).parents("tr").find("td").eq(3).find(".tc").text(formatMoney(obj.pr, 2)); //当前行价格
                        $(thiss).parents("tr").find("td").eq(4).find("a").eq(0).attr("goodsinfoid", obj.ID); //保存goodsinfoid
                        $(thiss).parents("tr").find("td").eq(0).find(".pCode").text(obj.BarCode); //保存编码
                        $(thiss).parents("tr").find("td").eq(0).find("a").attr("href", "GoodsInfo.aspx?goodsId=" + obj.GoodsID + "&goodsInfoId=" + obj.ID + can); //url
                    })
                } else {
                    //大图
                    $(data).each(function (index, obj) {
                        $(thiss).parents("div").prevAll(".price").html("<b>¥" + parseFloat(obj.pr).toFixed(2) + "</b>");
                        $(thiss).parents("div").nextAll(".btn").find(".addCart").attr("goodsinfoid", obj.ID); //保存goodsinfoid
                        $(thiss).parents("div").prevAll(".pic").find("a").attr("href", "GoodsInfo.aspx?goodsId=" + obj.GoodsID + "&goodsInfoId=" + obj.ID + can); //url
                    })
                }
            }
        })
    })
    //收藏商品
    $(document).on("click", ".keep", function () {
        var thiss = $(this);
        var goodsid = $(this).attr("goodsid");
        $.ajax({
            type: "post",
            url: "/Controller/AddUpDataSource.ashx?action=AddCart",
            data: { Goodsid: goodsid },
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    //列表
                    if ($.trim($(thiss).parents(".goods-zs").attr("class")) == "goods-zs back3") {
                        switch (data.Code) {
                            case "收藏": thiss.text("收藏"); thiss.attr("title", "添加收藏"); break;
                            case "取消收藏": thiss.text("取消收藏"); thiss.attr("title", "取消收藏"); break;
                        }
                    } else {//大图
                        switch (data.Code) {
                            case "收藏": thiss.find("i").attr("style", "background-position:-0px -50px"); thiss.attr("title", "添加收藏"); break;
                            case "取消收藏": thiss.find("i").attr("style", "background-position:-0px -73px"); thiss.attr("title", "取消收藏"); break;
                        }
                    }
                    layerCommon.msg(data.Msg, IconOption.正确);
                } else {
                    layerCommon.msg(data.Msg, IconOption.错误, 2000);
                }
            }
        })
    })
    //加入购物车
    $(document).on("click", ".addCart", function () {
        $_defaults = this;
        var GoodsID = $(this).attr("goodsid");
        var GoodsinfoId = $(this).attr("goodsinfoid");
        var GoodsName = "";
        var Num = "1"; //默认1个
        if ($.trim($(this).parents(".goods-zs").attr("class")) == "goods-zs back3") {
            GoodsName = $(this).parents("tr").find("td").eq(0).find(".sPic").children("a").attr("title"); //商品名称
        } else {
            GoodsName = $(this).parent().prevAll(".txt2").find("a").text(); //商品名称
            Num = $.trim($(this).parent().prev().find(".txtKeyInt").val()); //购买的数量
        }
        var GoodsNameSub = GoodsName.length > 15 ? GoodsName.substring(0, 15) + "..." : GoodsName;
        $.ajax({
            type: 'post',
            url: '/Handler/ShopCart.ashx?ActionType=AddShopCart',
            data: { GoodsInfoID: GoodsinfoId, ProID: "", Num: (Num == "" ? 1 : Num), Price: "", TPrice: "", CartType: 1, Goodsid: GoodsID },
            dataType: 'json',
            async: true,
            success: function (ReturnData) {
                if (ReturnData.Result) {
                    var are = "";
                    $("ul.topNav div[id*=tgnCar]").find("i").each(function () {
                        var cartGoodsID = $(this).attr("tip");
                        var goods_tip = $(this).attr("goods_tip");
                        if (cartGoodsID == "title") {
                            $(this).remove();
                            return false;
                        }
                        if (GoodsinfoId == cartGoodsID) {
                            are = "True";
                        }
                    })
                    if (are == "") {
                        if ($("ul.topNav div[id*=tgnCar]").find("i").length >= 4) {
                            if ($("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").length > 0) {
                                var sum = $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=\"num\"]").text();
                                $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=\"num\"]").text(parseInt(sum) + parseInt(Num));
                            } else {
                                $("ul.topNav div[id*=tgnCar]").append("<div class=\"border\"><span>购物车还有<span id=\"num\">1</span>个商品</span><a class=\"cklink\" href=\"../Distributor/Shop.aspx\">去购物车</a></div>");
                            }
                        }
                        else {
                            $("ul.topNav div[id*=tgnCar]").append("<i class=\"GoGoodsInfo\" goods_tip=\"" + GoodsID + "\" tip=\"" + GoodsinfoId + "\">" + GoodsNameSub + "<span class=\"goodsnum\">" + Num + "</span><span>x</span></i>");
                        }
                    } else {
                        var num = $("ul.topNav div[id*=tgnCar]").find("i[tip=\"" + GoodsinfoId + "\"]").find("span[class=\"goodsnum\"]").text();
                        $("ul.topNav div[id*=tgnCar]").find("i[tip=\"" + GoodsinfoId + "\"]").find("span[class=\"goodsnum\"]").text(parseFloat(num) + parseFloat(Num));
                    }
                    var CloneImg = $("#Top_CartImg").clone().css({ "position": "absolute", "z-index": "10000", "left": $($_defaults).offset().left + "px", "top": ($($_defaults).offset().top - 25) + "px" }).addClass("Top_CartImgClone");
                    $('body').append(CloneImg);
                    CloneImg.animate({ "left": ($("#Top_CartImg").offset().left) + "px", "top": ($("#Top_CartImg").offset().top) + "px" }, 1000, function () {
                        CloneImg.attr("class", "shop-icon2 Top_CartImgClone");
                        setTimeout(function () { CloneImg.remove(); $("ul.topNav b[id*=Top_CartNum]").html(ReturnData.SumCart); }, 400);
                        layerCommon.msg("商品已成功添加到购物车", IconOption.正确);
                    });
                } else {
                    layerCommon.msg(ReturnData.Msg, IconOption.哭脸);
                }
            }
        })
    })
    //大图切换
    $(".dt-icon").click(function () {
        $(this).addClass("hover");
        $(".lb-icon").removeClass("hover");
        $(".back").show();
        $(".back3").hide();
    })
    //列表切换
    $(".lb-icon").click(function () {
        $(this).addClass("hover");
        $(".dt-icon").removeClass("hover");
        $(".back3").show();
        $(".back").hide();
    })
    //上一页
    $(".xy-icon").on("click", function () {
        $("a.pre").click();
    });
    //下一页
    $(".yy-icon").on("click", function () {
        $("a.next").click();
    });

})
var SaleFunction = {
    BindSaleData: function (data) {
        var json = data.rows;
        var OutHTML = ""; //大图
        var OutHTML2 = ""; //列表
        var Digits = $("#hidsDigits").val(); //小数位
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var iskeep = $("#iskeep").val(); //加入收藏
        var isaddCart = $("#isaddCart").val(); //加入购物车
        var file = $(".hidFlie").val(); //图片路径
        var CompId = $("#hidCompId").val();
        if (json.length > 0) {
            var url = window.location.href.lastIndexOf("?"); //判断是否收藏商品页面
            var can = ""; //是否拼接传递到info页面的字符串
            if (url != -1) {
                can = "&sc=sc";
            }
            $.each(json, function (index, item) {
                //--------------------------------------------列表---------------------------------------------
                OutHTML2 += "<tr tip=\"" + item.GoodsID + "\"><td><div class=\"sPic\"><span><a href=\"" + "GoodsInfo.aspx?goodsId=" + item.GoodsID + "&goodsInfoId=" + item.ID + "&CompId=" + CompId + can + "\" ><img src=\"" + (item.Pic == "" ? "/images/Goods400x400.jpg" : file + item.Pic) + "\" width=\"60\" height=\"60\" /></a></span><a href=\"" + "GoodsInfo.aspx?goodsId=" + item.GoodsID + "&goodsInfoId=" + item.ID + +can + "\" title=\"" + item.GoodsName + "\" >" + GetGoodsName(item.GoodsName, "", 1) + "</a><br /><label class=\"pCode\">" + item.BarCode + "</label>" + sale(item.ProID, item.proGoodsPrice, item.proDiscount, item.proTypes, item.ProType, item.unit) + "</div></td><td><ul class=\"specs\">";
                //--------------------------------------------列表---------------------------------------------
                //--------------------------------------------大图---------------------------------------------
                OutHTML += "<li  tip=\"" + item.GoodsID + "\"><div class=\"wrapper\"><div class=\"pic\" style=\"width:205px; height:205px;\"><a href=\"" + "GoodsInfo.aspx?goodsId=" + item.GoodsID + "&goodsInfoId=" + item.ID + "&CompId=" + CompId + can + "\" ><img src=\"" + (item.Pic2 == "" || item.Pic2 == "X" ? "/images/Goods400x400.jpg" : file + item.Pic2) + "\"  alt=\"\" width=\"205\" height=\"205\" /></a></div><div class=\"price\"><b>¥" + item.pr.toFixed(2) + "</b>" + sale(item.ProID, item.proGoodsPrice, item.proDiscount, item.proTypes, item.ProType, item.unit) + "</div><div class=\"txt2\"><a href=\"" + "GoodsInfo.aspx?goodsId=" + item.GoodsID + "&goodsInfoId=" + item.ID + can + "\" >" + item.GoodsName + "</a></div><div class=\"specs\">";
                //--------------------------------------------大图---------------------------------------------
                //--------------------------------------------大图、列表多规格、多属性---------------------------------------------
                if (item.ValueInfo != "" && item.ValueInfo != null) {
                    var html = ""; //大图多规格的拼接
                    var html2 = null; //大图用于存储需要追加规格属性的
                    var html3 = ""; //列表多规格的拼接
                    var html4 = null; //列表用于存储需要追加规格属性的
                    $.ajax({
                        type: "post",
                        url: "/Handler/GetPageDataSource.ashx?PageAction=GetGoods1",
                        data: { ck: Math.random(), CompId: $.trim($("#hidCompId").val()), DisId: $.trim($("#hidDisId").val()), goodsId: item.GoodsID, txtGoods: "", goodspro: "", shouc: $.trim($(".hidShouc").val()) },
                        dataType: 'json',
                        async: false,
                        success: function (data) {
                            $(data).each(function (index, obj) {
                                var arry = obj.ValueInfo.split("；");  //颜色:白色,CPU:i7；
                                for (var i = 0; i < arry.length; i++) {
                                    if (arry[i] != undefined && arry[i] != "") {
                                        var arry2 = arry[i].split(":"); //颜色:白色
                                        //                                        if (arry2[0].toString().substring(0, 2) == "选择") {
                                        //                                            arry2[0] = arry2[0].toString().substring(2); //判断是是否规格前面2个字是选择
                                        //                                        }
                                        //第一次进来把规格都绑上，后面不需要在绑定
                                        if (index == 0) {
                                            html += "<div class=\"li\"><div class=\"t\">" + arry2[0] + "：</div><div class=\"n\">";
                                            html3 += "<li><div class=\"t\">" + arry2[0] + "：</div><div class=\"n\">";
                                        }
                                        if (html2 != null || html4 != null) {
                                            //第一次之后都是追加
                                            var bol = false;
                                            //判断是否有重复的规格属性，如有就不在追加
                                            $(html2).find(".li").eq(i).find(".n a").each(function (index2, obj2) {
                                                if ($.trim($(this).text()) == arry2[1]) {
                                                    bol = true;
                                                    return false;
                                                }
                                            })
                                            if (!bol) {
                                                //往html2里面的n下面追加规格属性
                                                $(html2).find(".li").eq(i).find(".n").append("<a href=\"javascript:;\" >" + arry2[1] + "</a>");
                                                $(html4).find("li").eq(i).find(".n").append("<a href=\"javascript:;\" >" + arry2[1] + "</a>");
                                                html = html2.html(); //再把html2的值赋给html
                                                html3 = html4.html(); //再把html4的值赋给html3
                                            }
                                        } else {
                                            //第一次为空时，都是拼接
                                            html += "<a href=\"javascript:;\" class=\"" + (index == 0 ? "hover" : '') + "\">" + arry2[1] + "</a>";
                                            html3 += "<a href=\"javascript:;\" class=\"" + (index == 0 ? "hover" : '') + "\">" + arry2[1] + "</a>";
                                        }
                                        if (index == 0) {
                                            html += "</div></div>";
                                            html3 += "</div></li>";
                                        }
                                    }
                                }
                                html2 = $("<div>" + html + "</div>"); //把第一次的规格、规格属性保存到html2
                                html4 = $("<div>" + html3 + "</div>"); //把第一次的规格、规格属性保存到html2
                            })
                        }
                    })
                    OutHTML += html;
                    OutHTML2 += html3;
                }
                //--------------------------------------------大图、列表多规格、多属性---------------------------------------------
                //--------------------------------------------大图---------------------------------------------
                var souc = ""; //是否收藏商品
                var sctitle = "添加收藏"; //title
                var sctext = "收藏";
                if (item.collGoodsID != "" && item.collGoodsID != null) {
                    souc = "style=\"background-position: -0px -73px;\"";
                    sctitle = sctext = "取消收藏";
                }
                //OutHTML += "<div class=\"li\"><div class=\"t\">数量：</div><div class=\"b\"><input name=\"\" value=\"1\" type=\"text\" class=\"box txtKeyInt\" /><i>件</i></div></div></div><div class=\"btn\"><a href=\"javascript:;\" goodsid=\"" + item.GoodsID + "\" class=\"keep\" title=\"" + sctitle + "\"><i class=\"sc-icon\" " + souc + "></i>收藏</a><a href=\"javascript:;\"  class=\"addCart\" goodsinfoid=\"" + item.ID + "\" goodsid=\"" + item.GoodsID + "\"><i class=\"gwc-icon\"></i>加入购物车</a></div></div></li>";

                OutHTML += "<div class=\"li\"><div class=\"t\">数量：</div><div class=\"b\"><input name=\"\" value=\"1\" type=\"text\" class=\"box txtKeyInt\" /><i>件</i></div></div></div><div class=\"btn\">";
                if (iskeep == "0")
                    OutHTML += "<a href=\"javascript:;\" goodsid=\"" + item.GoodsID + "\" class=\"keep\" title=\"" + sctitle + "\"><i class=\"sc-icon\" " + souc + "></i>收藏</a>";
                if (isaddCart == "0")
                    OutHTML += "<a href=\"javascript:;\"  class=\"addCart\" goodsinfoid=\"" + item.ID + "\" goodsid=\"" + item.GoodsID + "\"><i class=\"gwc-icon\"></i>加入购物车</a>";
                OutHTML += "</div></div></li>"
                //--------------------------------------------大图---------------------------------------------
                //--------------------------------------------列表---------------------------------------------
                //OutHTML2 += "</ul></td><td><div class=\"tc\">" + parseFloat(item.Inventory).toFixed(sDigits) + "</div></td><td><div class=\"tc\">" + formatMoney(item.pr, 2) + "</div></td><td><div class=\"tc alink\"><a href=\"javascript:;\" class=\"addCart\" goodsinfoid=\"" + item.ID + "\" goodsid=\"" + item.GoodsID + "\">加入购物车</a><br /><a href=\"javascript:;\" goodsid=\"" + item.GoodsID + "\" title=\"" + sctitle + "\" class=\"keep\">" + sctext + "</a></div></td></tr>";

                OutHTML2 += "</ul></td><td><div class=\"tc\">" + parseFloat(item.Inventory).toFixed(sDigits) + "</div></td><td><div class=\"tc\">" + formatMoney(item.pr, 2) + "</div></td><td><div class=\"tc alink\">";
                if (isaddCart == "0")
                    OutHTML2 += "<a href=\"javascript:;\" class=\"addCart\" goodsinfoid=\"" + item.ID + "\" goodsid=\"" + item.GoodsID + "\">加入购物车</a><br />";
                if (iskeep == "0")
                    OutHTML2 += "<a href=\"javascript:;\" goodsid=\"" + item.GoodsID + "\" title=\"" + sctitle + "\" class=\"keep\">" + sctext + "</a>";
                OutHTML2 += "</div></td></tr>"
                //--------------------------------------------列表---------------------------------------------
            });
        } else {
            OutHTML = "暂无数据";
        }
        $(".back .goods-li").html(OutHTML);
        $(".tabNr-box tbody").html(OutHTML2);
        return this;
    },
    GetSalePaginData: function () {
        $(".paging").myPagination({
            currPage: 1,
            pageCount: 1,
            pageSize: 15,
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
                url: "/Handler/GetPageDataSource.ashx?PageAction=GetGoods1",
                dataType: 'json',
                params: { "CompId": $.trim($("#hidCompId").val()), "DisId": $.trim($("#hidDisId").val()), "goodsId": "", "txtGoods": $.trim($(".txtGoods").val()), "goodspro": $(".back1 ul li").find("a").attr("class") == "k-icon" ? "" : "1", shouc: $.trim($(".hidShouc").val()) },
                ajaxStart: function () {
                    $(".po-bg2").removeClass("none"); //等待跳转的层
                    $(".p-delete2").removeClass("none"); //等待跳转的层
                }, ajaxStop: function () {
                    $(".po-bg2").addClass("none");
                    $(".p-delete2").addClass("none");
                },
                ajaxError: function () {
                    $(".rightCon").html("暂无数据");
                }
            }
        });
        return this;
    },
    BindSaleBtnEvent: function () {
        //商品名称搜索
        $(".searchBtn").on("click", function () {
            SaleFunction.GetSalePaginData();
        })
        //促销商品
        $(".back1 ul li").click(function () {
            //判断是否选中
            if ($(this).find("a").eq(0).attr("class") == "k-icon") {
                $(this).find("a").eq(0).attr("class", "k-icon2"); //选中
            } else {
                $(this).find("a").eq(0).attr("class", "k-icon"); //取消选中
            }
            SaleFunction.GetSalePaginData();
        })
        $("#ddrComp").on("change", function () {
            $("#hidCompId").val($(this).val());
            SaleFunction.GetSalePaginData();
        });
        return this;
    }
}

function sale(ProID, proGoodsPrice, proDiscount, proTypes, ProType, unit) {
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
                str += "在原订货价基础上打" + parseFloat(proDiscount / 10).toFixed(sDigits) + "折";
            }
        }
    }
    str += "</div></div>";

    return str;

};
function KeyInt(val, defaultValue) {
    if (val.value == "0" || val.value == "") {
        if (defaultValue != undefined)
            val.value = defaultValue;
        else
            val.value = "";
    }
    else
        val.value = val.value.replace(/[^\d]/g, '');
};
//截取字符串
//商品名称，属性值，是否需要截取
function GetGoodsName(goodsName) {
    if (goodsName.length > 28) {
        return goodsName.substring(0, 28) + "...";
    } else {
        return goodsName;
    }
};

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