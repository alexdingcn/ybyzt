(function ($) {
    $_defaults = { ID: "", Type: "", Item: "" }

    //添加到购物车
    $addCart = function () {
        var GoodsID = $($_defaults.ID).attr("tip_ID");
        var GoodsinfoId = $("#tr_" + GoodsID).find("td:first").find("input[type=hidden]").val();
        var Num = 1;
        var ProID = "";
        var TPrice = "";
        var Price = "";
        var CartType = 1;
        var GoodsName = $($_defaults.ID).parents().siblings("td").attr("GoodsName");
        var GoodsNameSub = GoodsName.length > 15 ? GoodsName.substring(0, 15) + "..." : GoodsName;

        if (GoodsinfoId.toString() == "0" || GoodsinfoId.toString() == "") {
            layerCommon.msg("商品出错了,无法加入购物车！", IconOption.错误);
            return;
        }

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
            data: { GoodsInfoID: GoodsinfoId, ProID: ProID, Num: Num, Price: Price, TPrice: TPrice, CartType: CartType },
            async: true, //true: 异步 false:同步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    var are = "";
                    $("ul.topNav div[id*=tgnCar]").find("i").each(function () {
                        var cartGoodsID = $(this).attr("tip");
                        var goods_tip = $(this).attr("goods_tip");
                        if (cartGoodsID == "title") {
                            $(this).remove();
                            return false;
                        }
                        if (GoodsinfoId == cartGoodsID) {
                            //var goodsnum = $(this).parents("div[class=\"tgnCart\"]").siblings("b[class=\"number\"]").text();
                            //$(this).parents("div[class=\"tgnCart\"]").siblings("b[class=\"number\"]").text(parseInt(goodsnum) + parseInt(Num));
                            are = "True";
                        }
                    })
                    if (are == "") {
                        if ($("ul.topNav div[id*=tgnCar]").find("i").length >= 4) {
                            if ($("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").length > 0) {
                                var sum = $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=\"num\"]").text();
                                $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=\"num\"]").text(parseInt(sum) + parseInt(1));
                            } else {
                                $("ul.topNav div[id*=tgnCar]").append("<div class=\"border\"><span>购物车还有<span id=\"num\">1</span>个商品</span><a class=\"cklink\" href=\"../Distributor/Shop.aspx\">去购物车</a></div>");
                            }
                        }
                        else {
                            $("ul.topNav div[id*=tgnCar]").append("<i class=\"GoGoodsInfo\" goods_tip=\"" + GoodsID + "\" tip=\"" + GoodsinfoId + "\">" + GoodsNameSub + "<span class=\"goodsnum\">" + Num + "</span><span>x</span></i>");
                        }
                    } else {
                        var num = $("ul.topNav div[id*=tgnCar]").find("i[tip=\"" + GoodsinfoId + "\"]").find("span[class=\"goodsnum\"]").text();
                        $("ul.topNav div[id*=tgnCar]").find("i[tip=\"" + GoodsinfoId + "\"]").find("span[class=\"goodsnum\"]").text(parseFloat(num) + parseFloat(1));
                    }

                    var CloneImg = $("#Top_CartImg").clone().css({ "position": "absolute", "z-index": "10000", "left": $($_defaults.ID).offset().left + "px", "top": ($($_defaults.ID).offset().top - 25) + "px" }).addClass("Top_CartImgClone");
                    $('body').append(CloneImg);
                    CloneImg.animate({ "left": ($("#Top_CartImg").offset().left) + "px", "top": ($("#Top_CartImg").offset().top) + "px" }, 1000, function () {
                        CloneImg.attr("class", "shop-icon2 Top_CartImgClone");
                        setTimeout(function () { CloneImg.remove(); $("ul.topNav b[id*=Top_CartNum]").html(data.SumCart); }, 400);
                    });
                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    //加减购物车商品数量
    $AddCartNum = function () {
        var type = $($_defaults.ID).attr("class");
        var proID = $($_defaults.ID).parent().attr("data-proID");
        var goodsinfoid = $($_defaults.ID).parent().attr("data-GoodsInfoid");
        var snum = $($_defaults.ID).siblings("input[class*=txtGoodsNum]").val();
        var Digits = $("#hidsDigits").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var num = parseFloat(snum).toFixed(sDigits);
        var IsInve = $("#hidIsInve").val();

        if (type == "minus") {
            var minusnum = parseFloat(num) - parseFloat(1);
            if (parseFloat(minusnum) <= parseFloat(0))
                $($_defaults.ID).val(num);
            else
                num = minusnum.toFixed(sDigits);
        }
        else if (type == "add") {
            num = (parseFloat(num) + parseFloat(1)).toFixed(sDigits);
            if (IsInve == 0) {
                //启用商品库存
                var Inventory = $($_defaults.ID).parent().attr("Inventory"); //商品库存
                if (parseFloat(num) > parseFloat(Inventory)) {
                    num = Inventory;
                    $($_defaults.ID).val(parseFloat(Inventory).toFixed(sDigits));
                }
            } else {
                //商品数量不能大于100万
                if (parseFloat(num) >= parseFloat(1000000)) {
                    num = 1;
                    $($_defaults.ID).val(num);
                }
            }
        }
        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
            data: { GoodsInfoID: goodsinfoid, ProID: proID, Num: num, CartType: "2" },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    $("ul.topNav b[id*=Top_CartNum]").html(data.SumCart);
                    $($_defaults.ID).siblings("input[class*=txtGoodsNum]").val(num);
                    $("#sumPrice").html(formatMoney(data.SumAmount, 2));
                    $($_defaults.ID).parent().parent().siblings("td").find("div.tprice").html("￥" + formatMoney(data.TPrice, 2));
                    $($_defaults.ID).parent().parent().siblings("td").find("div.sumAmount").html("￥" + formatMoney(data.AuditAmount, 2));

                    $("ul.topNav div[id*=tgnCar]").find("i").each(function () {
                        var cartGoodsID = $(this).attr("tip");
                        if (cartGoodsID == goodsinfoid) {
                            $(this).find("span[class=\"goodsnum\"]").text(num);
                            return false;
                        }
                    });

                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                    $($_defaults.ID).siblings("input[class*=txtGoodsNum]").val(data.Num);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    //修改购物车商品数量
    $ChangeCart = function () {
        var type = $($_defaults.ID).attr("class");
        var proID = $($_defaults.ID).parent().attr("data-proID");
        var goodsinfoid = $($_defaults.ID).parent().attr("data-GoodsInfoid");
        var snum = $($_defaults.ID).val();
        var Digits = $("#hidsDigits").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var num = parseFloat(snum).toFixed(sDigits);
        var IsInve = $("#hidIsInve").val();

        if (isNaN(num)) {
            num = 1;
        }
        if (parseFloat(num) <= parseFloat(0) || num.toString() == "") {
            num = 1;
            $($_defaults.ID).val(num);
        } else {
            if (IsInve == 0) {
                //启用商品库存
                var Inventory = $($_defaults.ID).parent().attr("Inventory"); //商品库存
                if (parseFloat(num) > parseFloat(Inventory)) {
                    num = Inventory;
                    $($_defaults.ID).val(parseFloat(Inventory).toFixed(sDigits));
                }
            } else {
                //商品数量不能大于100万
                if (parseFloat(num) >= parseFloat(1000000)) {
                    num = 1;
                    $($_defaults.ID).val(num);
                } else
                    $($_defaults.ID).val(num);
            }
        }

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
            data: { GoodsInfoID: goodsinfoid, ProID: proID, Num: num, CartType: "2" },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    $("ul.topNav b[id*=Top_CartNum]").html(data.SumCart);
                    $($_defaults.ID).parent().parent().siblings("td").find("div.tprice").html("￥" + formatMoney(data.TPrice, 2));
                    $($_defaults.ID).parent().parent().siblings("td").find("div.sumAmount").html("￥" + formatMoney(data.AuditAmount, 2));
                    $("#sumPrice").html(formatMoney(data.SumAmount, 2));

                    $("ul.topNav div[id*=tgnCar]").find("i").each(function () {
                        var cartGoodsID = $(this).attr("tip");
                        if (cartGoodsID == goodsinfoid) {
                            $(this).find("span[class=\"goodsnum\"]").text(num);
                            return false;
                        }
                    });

                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });

    }
    $DelCart = function () {
        //删除购物车商品信息
        var cartID = "";
        var da = "";
        var GoodsInfoID = $($_defaults.ID).attr("tip");
        if (parseInt($_defaults.Type) == 0) {
            //删除单个购物车商品
            cartID = $($_defaults.ID).attr("tip_ID");
            da = { cartID: cartID, deltype: $_defaults.Type, CartType: "4" };
        }
        else {
            da = { deltype: $_defaults.Type, CartType: "4" };
        }

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
            data: da,
            async: true, // false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    if (parseInt($_defaults.Type) == 0) {
                        //删除单个购物车商品
                        $("#tr_" + cartID).remove();
                        $("ul.topNav b[id*=Top_CartNum]").html(data.SumCart);
                        $("#sumPrice").html(formatMoney(data.SumAmount, 2));
                        var are = "";
                        $("ul.topNav div[id*=tgnCar]").find("i").each(function () {
                            var cartGoodsID = $(this).attr("tip");
                            if (cartGoodsID == GoodsInfoID) {
                                $(this).remove();
                                are = "True";
                                return false;
                            }
                        });

                        if ($("ul.topNav div[id*=tgnCar]").find("i").length <= 0) {
                            if ($("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").length <= 0) {
                                $("ul.topNav div[id*=tgnCar]").append("<i tip=\"title\">购物车中还没有商品，赶紧选购吧！</i>");
                                $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").remove();
                            } else {
                                if (are == "") {
                                    var num = $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=num]").text();
                                    if (parseInt(num) == 1) {
                                        $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").remove();
                                        $("ul.topNav div[id*=tgnCar]").append("<i tip=\"title\">购物车中还没有商品，赶紧选购吧！</i>");
                                    } else
                                        $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=num]").text(parseInt(num) - parseInt(1));
                                }
                            }
                        } else {
                            if (are == "") {
                                var num = $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=num]").text();
                                parseInt(num) == 1 ? $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").remove() : $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").find("span").find("span[id=num]").text(parseInt(num) - parseInt(1));
                            }
                        }
                        layerCommon.msg("删除购物车商品成功", IconOption.笑脸);
                    } else {
                        //清空购物车
                        $(".tabNr tbody tr").remove();
                        $("#sumPrice").html(0);
                        $("ul.topNav b[id*=Top_CartNum]").html(0);

                        $("ul.topNav div[id*=tgnCar]").find("i").remove();
                        $("ul.topNav div[id*=tgnCar]").append("<i tip=\"title\">购物车中还没有商品，赶紧选购吧！</i>");
                        $("ul.topNav div[id*=tgnCar]").find("div[class=\"border\"]").remove();

                        layerCommon.msg("清空购物车成功", IconOption.笑脸);
                    }
                } else {
                    layerCommon.msg("删除失败", IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    $CartToOrder = function () {
        //判断购物车是否有商品
        if ($(".tabNr tbody tr").length <= 0)
            return;
        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=CartToOrder',
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    window.location.href = 'ShopInfo.aspx';
                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    $_OrderDelCart = function () {
        //删除单个购物车商品
        cartID = $($_defaults.ID).attr("tip_ID");
        da = { cartID: cartID, deltype: $_defaults.Type, CartType: "4" };

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=DelShopCart',
            data: da,
            async: true, // false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    if (parseFloat(data.SumAmount) == 0) {
                        window.location.href = '/Distributor/GoodsList.aspx';
                    }
                    //删除单个购物车商品
                    $("#tr_" + cartID).remove();
                    $("ul.topNav b[id*=Top_CartNum]").html(data.SumCart);
                    //总价
                    $("#lblsumPrice").text(formatMoney(data.AuditAmount, 2));
                    $("#divProType3").html(data.Code);
                    //合计
                    $("#hidprice").val(formatMoney(data.SumAmount, 2));
                    $("#lblprice").text(formatMoney(data.SumAmount, 2));
                    layerCommon.msg("删除成功", IconOption.笑脸);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    //添加购物车商品备注
    $_AddNoteOrderCart = function () {
        var cart_ID = $($_defaults.ID).attr("tip_Rvedf");
        var Re = $("#txtNote").val();
        if (Re == "") {
            $(".addRVedf").attr("style", "display:none;");
            return;
        }

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddNoteOrderCart',
            data: { cart_ID: cart_ID, Re: Re },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    $(".addRVedf").attr("style", "display:none;");
                    $("#txtNote").val("");

                    $("#" + cart_ID).siblings("div.addVedf").attr("style", "display:none;");
                    $("#" + cart_ID).attr("style", "display:block;");
                    $("#" + cart_ID).find("div.txt").html(Re);
                    $("#" + cart_ID).find("div.adthover").find("span").html(Re);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    //商品收藏
    $_AdddcGoods = function () {
        var GoodsId = $($_defaults.ID).attr("TipGoods");
        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=AddDcGoods',
            data: { GoodsId: GoodsId },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    $($_defaults.ID).removeClass("dcGoods");
                    $($_defaults.ID).addClass("CanceldcGoods");
                    $($_defaults.ID).html("取消收藏");
                    layerCommon.msg("收藏成功", IconOption.笑脸);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
    //取消商品收藏
    $_CanceldcGoods = function () {
        var GoodsId = $($_defaults.ID).attr("TipGoods");
        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx?ActionType=CalDcGoods',
            data: { GoodsId: GoodsId },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    layerCommon.msg("取消收藏成功", IconOption.笑脸);
                    if ($_defaults.Type == 0) {
                        $($_defaults.ID).removeClass("CanceldcGoods");
                        $($_defaults.ID).addClass("dcGoods");
                        $($_defaults.ID).html("收藏");
                    } else {
                        //$($_defaults.ID).parents().parents("tr").remove();
                        window.location.reload(); //刷新当前页面
                    }
                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }

    //立即下单修改数量
    $OrderNum = function () {
        var type = $($_defaults.ID).attr("class");
        var proID = $($_defaults.ID).parent().attr("data-proID");
        var goodsinfoid = $($_defaults.ID).parent().attr("data-GoodsInfoid");
        var snum = $($_defaults.ID).siblings("input[class*=txtGoodsNum]").val();
        var Digits = $("#hidsDigits").val();
        var type1 = $("#type").val();
        var OrderID = $("#OrderID").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var num = parseFloat(snum).toFixed(sDigits);
        var IsInve = $("#hidIsInve").val(); //是否启用商品库存
        var batePrice = $("#txtRebate").val(); //返利

        if (parseFloat(batePrice) == parseFloat(0) || typeof (batePrice) == "undefined" || batePrice == "") {
            batePrice = 0;
            $(".txtRebate").val("0");
        }

        if (type == "minus") {
            var minusnum = parseFloat(num) - parseFloat(1);
            if (parseFloat(minusnum) <= parseFloat(0))
                $($_defaults.ID).val(num);
            else
                num = minusnum.toFixed(sDigits);
        }
        else if (type == "add") {
            num = (parseFloat(num) + parseFloat(1)).toFixed(sDigits);
            if (IsInve == 0) {
                //启用商品库存
                var Inventory = $($_defaults.ID).parent().attr("Inventory"); //商品库存
                if (parseFloat(num) > parseFloat(Inventory)) {
                    num = Inventory;
                    $($_defaults.ID).val(parseFloat(Inventory).toFixed(sDigits));
                }
            } else {
                //商品数量不能大于100万
                if (parseFloat(num) >= parseFloat(1000000)) {
                    num = 1;
                    $($_defaults.ID).val(num);
                }
            }
        }
        $.ajax({
            type: 'post',
            url: 'ShopInfo.aspx?ActionType=ShopNum',
            data: { GoodsInfoID: goodsinfoid, ProID: proID, Num: num, type: type1, KeyID: OrderID },
            success: function (data) {
                var result = eval('(' + data + ')');
                var ds = result["ds"];
                if (ds == "True") {
                    var SumTotal = result["SumTotal"];
                    var ProAmount = result["ProAmount"];
                    var price = parseFloat(SumTotal) - (parseFloat(ProAmount) + parseFloat(batePrice));

                    //商品数量
                    $($_defaults.ID).siblings("input[class*=txtGoodsNum]").val(num);
                    //商品小计
                    $($_defaults.ID).parent().parent().siblings("td").find("div.sumAmount").html("￥" + formatMoney(result["AuditAmount"], 2));
                    //商品总价
                    $("#lblsumPrice").text(formatMoney(result["SumTotal"], 2));
                    //合计
                    $("#hidprice").val(formatMoney(price, 2));
                    $("#lblprice").text(formatMoney(price, 2));

                } else {
                    layerCommon.msg(result["msg"], IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }

    //立即下单修改商品数量
    $OrderChangeCart = function () {
        var type = $($_defaults.ID).attr("class");
        var proID = $($_defaults.ID).parent().attr("data-proID");
        var goodsinfoid = $($_defaults.ID).parent().attr("data-GoodsInfoid");
        var snum = $($_defaults.ID).val();
        var Digits = $("#hidsDigits").val();
        var type1 = $("#type").val();
        var OrderID = $("#OrderID").val();
        var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
        var num = parseFloat(snum).toFixed(sDigits);
        var IsInve = $("#hidIsInve").val(); //是否启用商品库存
        var batePrice = $("#txtRebate").val(); //返利

        if (parseFloat(batePrice) == parseFloat(0) || typeof (batePrice) == "undefined" || batePrice == "") {
            batePrice = 0;
            $(".txtRebate").val("0");
        }

        if (isNaN(num)) {
            num = 1;
        }
        if (parseFloat(num) <= parseFloat(0) || num.toString() == "") {
            num = 1;
            $($_defaults.ID).val(num);
        } else {
            if (IsInve == 0) {
                //启用商品库存
                var Inventory = $($_defaults.ID).parent().attr("Inventory"); //商品库存
                if (parseFloat(num) > parseFloat(Inventory))
                    num = Inventory;
                $($_defaults.ID).val(num);
            } else {
                //商品数量不能大于100万
                if (parseFloat(num) >= parseFloat(1000000)) {
                    num = 1;
                    $($_defaults.ID).val(num);
                } else
                    $($_defaults.ID).val(num);
            }
        }
        $.ajax({
            type: 'post',
            url: 'ShopInfo.aspx?ActionType=ShopNum',
            data: { GoodsInfoID: goodsinfoid, ProID: proID, Num: num, type: type1, KeyID: OrderID },
            success: function (data) {
                var result = eval('(' + data + ')');
                var ds = result["ds"];
                if (ds == "True") {
                    var SumTotal = result["SumTotal"];
                    var ProAmount = result["ProAmount"];
                    var price = parseFloat(SumTotal) - (parseFloat(ProAmount) + parseFloat(batePrice));

                    //商品小计
                    $($_defaults.ID).parent().parent().siblings("td").find("div.sumAmount").html("￥" + formatMoney(result["AuditAmount"], 2));
                    //商品总价
                    $("#lblsumPrice").text(formatMoney(result["SumTotal"], 2));
                    //合计
                    $("#hidprice").val(formatMoney(price, 2));
                    $("#lblprice").text(formatMoney(price, 2));

                } else {
                    layerCommon.msg(result["msg"], IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }

    ////下单删除商品
    $OrderDel = function () {
        cartID = $($_defaults.ID).attr("tip_ID");
        var type = $("#type").val();
        var OrderID = $("#OrderID").val();
        var del = $("#delGoodsinfo").val();
        var IsInve = $("#hidIsInve").val();

        da = { cartID: cartID, type: type, KeyID: OrderID };

        $.ajax({
            type: 'post',
            url: 'ShopInfo.aspx?ActionType=DelShop',
            data: da,
            success: function (data) {
                var result = eval('(' + data + ')');
                var ds = result["ds"];
                if (ds == "True") {
                    //删除单个购物车商品
                    $("#tr_" + cartID).remove();

                    if ($(".tabNr tbody tr").length <= 0) {
                        var str = '';
                        if (IsInve == 0)
                            str = '<tr class="trSy"><td class="t8"><div class="addg"><a href="#" class="minus2"></a><a href="#" class="add2 OrderDelCart" tip_ID=""></a></div></td><td><div class="sPic wpd"><span><a href="#" class="seleGoods" ><img src="" alt="选择商品" width="65" height="65"></a></span></div></td><td><div class="tc"></div></td><td><div class="tc"></div></td><td><div class="sl OrderNum"></div></td><td><div class="tc"></div></td><td><div class="tc sumAmount"></div></td></tr>';
                        else
                            str = '<tr class="trSy"><td class="t8"><div class="addg"><a href="#" class="minus2"></a><a href="#" class="add2 OrderDelCart" tip_ID=""></a></div></td><td><div class="sPic wpd"><span><a href="#" class="seleGoods" ><img src="" alt="选择商品" width="65" height="65"></a></span></div></td><td><div class="tc"></div></td><td><div class="sl OrderNum"></div></td><td><div class="tc"></div></td><td><div class="tc sumAmount"></div></td></tr>';

                        $(".tabLine tbody").append(str);
                    }

                    if (result["ld"] == "1") {
                        del += cartID + ",";
                        $("#delGoodsinfo").val(del);
                    }

                    var SumTotal = result["SumTotal"];
                    var ProAmount = result["ProAmount"];
                    var price = parseFloat(SumTotal) - parseFloat(ProAmount);
                    //商品总价
                    $("#lblsumPrice").text(formatMoney(result["SumTotal"], 2));
                    $("#divProType3").html(result["proOrderType"]);
                    //合计
                    $("#hidprice").val(formatMoney(price, 2));
                    $("#lblprice").text(formatMoney(price, 2));
                    layerCommon.msg("删除成功", IconOption.笑脸);
                } else {
                    if (cartID.toString() == "") {
                        if ($(".tabNr tbody tr").length > 1)
                            $($_defaults.ID).parent().parent().parent().remove();
                    }
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }

    $AddGoodsInfo = function () {
        var DisID = $("#hidDisId").val();
        var CompId = $("#hidCompId").val();

        if (DisID.toString() == "") {
            layerCommon.msg("请选择代理商", IconOption.感叹);
            return false;
        }

        //转向网页的地址; 
        var url = '/Company/Order/GoodsAdd.aspx?DisId=' + DisID + "&CompId=" + CompId;
        var name = '选购商品';                     //网页名称，可为空; 
        var iWidth = 920;                          //弹出窗口的宽度; 
        var iHeight = 600;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
        //获得窗口的水平位置 
        var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;

        //2016-02-24修改
        var height = document.body.clientHeight; //计算高度
        var layerOffsetY = (height - 450) / 2; //计算宽度
        var index = layerCommon.openWindow('订单选购商品', url, '1000px', '550px'); //记录弹出对象

        $("#hid_Alert").val(index); //记录弹出对象
        return false;
    }
    //选择商品回调方法
    selectGoods = function (info) {
        var type = $("#type").val();
        var OrderID = $("#OrderID").val();
        layerCommon.layerClose("hid_Alert"); //2016-02-24修改
        if (info.toString() == "") {
            //没有选中商品，直接返回
            return;
        }

        $.ajax({
            type: 'post',
            url: 'ShopInfo.aspx',
            data: { ck: Math.random(), "ActionType": "AddShop", "type": type, "KeyID": OrderID },
            success: function (data) {
                var result = eval('(' + data + ')');
                var ds = result["ds"];
                if (ds == "True") {

                    //绑定商品
                    if (result["str"] != "") {
                        $(".trSy").remove();
                        $(".tabLine tbody").append(result["str"]);
                    }

                    var SumTotal = result["SumTotal"];
                    var ProAmount = result["ProAmount"];
                    var Rebate = $("#lblbate").text();

                    if (parseFloat(Rebate) == parseFloat(0) || Rebate.toString() == "")
                        Rebate = 0;
                    var price = parseFloat(SumTotal) - (parseFloat(ProAmount) + parseFloat(Rebate));
                    //商品总价
                    $("#lblsumPrice").text(formatMoney(result["SumTotal"], 2));
                    $("#divProType3").html(result["proOrderType"]);
                    //合计
                    $("#hidprice").val(formatMoney(price, 2));
                    $("#lblprice").text(formatMoney(price, 2));
                }
                //layerCommon.msg("删除成功", IconOption.笑脸);
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    }
})(jQuery);

$(function () {
    //添加到购物车
    $(".btnAddCart").on("click", function () {
        $_defaults.ID = this;
        $addCart();
    });

    $("input:text.txtGoodsNum").on({
        "keyup": function () {
            var r = /^[0-9]\d*?\.?\d*?$/;
            if (!$(this).val().match(r)) {
                $(this).val(1);
            }
        },
        "blur": function () {
            var r = /^[0-9]\d*?\.?\d*?$/;
            if (!$(this).val().match(r)) {
                $(this).val(1);
            }
        }
    });

    //立即下单
    $(".btnSave").on("click", function () {
        /*$_defaults.ID = this;
        $CartToOrder();*/
        //判断购物车是否有商品
        if ($(".tabNr tbody tr").length <= 0) {
            layerCommon.msg("购物车是空的，不能下单！", IconOption.哭脸);
            return;
        }
        window.location.href = 'newOrder/orderbuy.aspx?type=1';
    });

    //购物车商品加减
    $(".sl #minus,.sl #add").on("click", function () {
        $_defaults.ID = this;
        $AddCartNum();
    });

    //修改数量
    $(".txtGoodsNum").on("change", function () {
        $_defaults.ID = this;
        $ChangeCart();
    });

    //删除购物车商品
    $(document).on("click", ".delcart", function () {
        $_defaults.ID = this;
        $_defaults.Type = 0;  //0、删除单个购物车商品  1、清空购物车
        $DelCart();
    });

    //清空购物车商品
    $(document).on("click", " .btnEmpty", function () {
        //清空购物车
        if ($(".tabNr tbody tr").length <= 0)
            return;
        $_defaults.ID = this;
        $_defaults.Type = 1;  //0、删除单个购物车商品  1、清空购物车
        layerCommon.confirm("是否清空购物车？", function () { $DelCart(); });
    });

    //下单修改数量
    $(document).on("click", ".OrderNum #orderminus,.OrderNum #orderadd", function () {
        $_defaults.ID = this;
        $OrderNum();
    });

    //下单修改数量
    $(document).on("change", "#txtOrderNum", function () {
        $_defaults.ID = this;
        $OrderChangeCart();
    });

    //加行
    $(document).on("click", ".minus2", function () {
        var IsInve = $("#hidIsInve").val();
        var str = '';
        if (IsInve == 0)
            str = '<tr class="trSy"><td class="t8"><div class="addg"><a href="#" class="minus2"></a><a href="#" class="add2 OrderDelCart" tip_ID=""></a></div></td><td><div class="sPic wpd"><span><a href="#" class="seleGoods" ><img src="" alt="选择商品" width="65" height="65"></a></span></div></td><td><div class="tc"></div></td><td><div class="tc"></div></td><td><div class="sl OrderNum"></div></td><td><div class="tc"></div></td><td><div class="tc sumAmount"></div></td></tr>';
        else
            str = '<tr class="trSy"><td class="t8"><div class="addg"><a href="#" class="minus2"></a><a href="#" class="add2 OrderDelCart" tip_ID=""></a></div></td><td><div class="sPic wpd"><span><a href="#" class="seleGoods" ><img src="" alt="选择商品" width="65" height="65"></a></span></div></td><td><div class="tc"></div></td><td><div class="sl OrderNum"></div></td><td><div class="tc"></div></td><td><div class="tc sumAmount"></div></td></tr>';

        $(".tabLine tbody").append(str);
    });

    //下单选择商品
    $(document).on("click", ".seleGoods", function () {
        $_defaults.ID = this;
        $AddGoodsInfo();
    });

    //删除购物车商品
    $(document).on("click", ".OrderDelCart", function () {
        $_defaults.ID = this;
        $_defaults.Type = 0;  //0、删除单个购物车商品  1、清空购物车
        $OrderDel();
    });

    //下单类型显示
    $(".goods-gn .t").on({
        "mouseover": function (e) {
            $(this).find(".menu").show();
        },
        "mouseout": function (e) {
            $(this).find(".menu").hide();
        }
    });

    //选择订单类型
    $(".menu a").on("click", function () {
        //单击选中的订单类型
        var tip = $(this).attr("tip");
        var tipText = $(this).text();
        //上次选中的订单类型
        var setip = $(this).parents().siblings("a.dx").attr("tip");
        var setipText = $(this).parents().siblings("a.dx").find("span").text();

        $(this).attr("tip", setip);
        $(this).text(setipText);

        $(this).parents().siblings("a.dx").attr("tip", tip);
        $(this).parents().siblings("a.dx").find("span").text(tipText);
        $(this).parents("div.menu").hide();
    });

    //订单info页面添加商品备注
    $(".addVedf,.aupRe").on("click", function () {
        $(".addRVedf").attr("style", "display:block;position:fixed;top:50%; left:63%; z-index:9999");
        var upRe = $(this).siblings("span").text();
        if (upRe != "") {
            $("#txtNote").val(upRe);
            $(".btn_Rvedf").attr("tip_Rvedf", $(this).attr("tip_ID"));
        } else {
            $(".btn_Rvedf").attr("tip_Rvedf", $(this).attr("cart_ID"));
        }
    });

    //购物车修改商品备注
    $(".btn_Rvedf").on("click", function () {
        $_defaults.ID = this;
        $_AddNoteOrderCart();
    });

    //添加到收藏
    $(document).on("click", ".dcGoods", function () {
        $_defaults.ID = this;
        $_AdddcGoods();
    });
    //取消收藏
    $(document).on("click", ".CanceldcGoods", function () {
        $_defaults.ID = this;
        //商品信息列表
        $_defaults.Type = 0;
        $_CanceldcGoods();
    });
    //收藏商品列表取消收藏
    $(document).on("click", ".txtn", function () {
        $_defaults.ID = this;
        //收藏商品列表
        $_defaults.Type = 1;
        $_CanceldcGoods();
    });

    /*收货地址 region*/
    //修改收货地址

    $("#OrderAddr").on("click", function () {
        var ID = $("#hidAddrID").val();
        //var height = document.body.clientHeight; //计算高度
        //var layerOffsetY = (height - 450) / 2; //计算宽度
        //var index = showDialog("修改地址", "../Distributor/AddDisAddr.aspx?KeyID=" + ID, "650px", "530px", layerOffsetY);

        var index = layerCommon.openWindow("修改地址", "../Distributor/AddDisAddr.aspx?KeyID=" + ID, "650px", "530px");
        $("#hid_Alert").val(index);
    });

    $(".modify").attr("style", "margin:-245px 0 0 -325px; width:647px;");
    $(".list").find("li input[value=\"" + $("#hidKeyID").val() + "\"]").prop("checked", true);

    //修改地址
    $(document).on("click", ".upAddr", function () {
        var ID = $.trim($(this).siblings("input[name=\"addr\"]").val());
        var principal = $.trim($(this).siblings("label[class=\"principal\"]").text());
        var phone = $.trim($(this).siblings("label[class=\"phone\"]").text());

        var Address = $.trim($(this).siblings("label[class=\"Address\"]").text());
        var Province = $.trim($(this).siblings("input[class=\"pr\"]").val());
        var City = $.trim($(this).siblings("input[class=\"ci\"]").val());
        var Area = $.trim($(this).siblings("input[class=\"ar\"]").val());

        $(this).siblings("input:radio[name=\"addr\"]").prop("checked", true);
        $("#txtprincipal").val(principal);
        $("#txtprincipal").attr("tip_id", ID);
        $("#txtphone").val(phone);
        $_Addr(Province, City, Area, Address);
        $(".add .li input[type=\"text\"]").siblings("label").html("");
    });

    //收货地址验证
    $(".add .li input[type=\"text\"]").on({
        "focus": function () {
            $(this).siblings("label").html("");
        },
        "blur": function () {
            var ltxt = "- " + $(this).siblings("div[class=\"bt\"]").text();
            ltxt += "不能为空";
            var val = $(this).val();
            if (val == "") {
                $(this).siblings("label").html(ltxt);
            }
        }
    });

    //保存地址
    $(".btnSaveAddr").on("click", function () {
        var id = $.trim($("#txtprincipal").attr("tip_id"));
        var principal = $.trim($("#txtprincipal").val());
        var phone = $.trim($("#txtphone").val());
        var Address = $.trim($("#txtAddress").val());
        var Province = $.trim($("#txtProvince").val());
        var City = $.trim($("#txtCity").val());
        var Area = $.trim($("#txtArea").val());
        var phoneCode = $.trim($("#txtPhoneCode").text());
        var disPhone = $.trim($("#lblDisPhone").text());
        var str = "";
        //判断是修改、新增地址
        var ActionType = "AddAddr";
        if (id != "") {
            ActionType = "UpAddr";
        }
        //非空判断
        if (principal == "") {
            str += "- 收货人不能为空。<br/>";
            $("#lblprincapal").html("- 收货人不能为空");
        }
        if (phone == "") {
            str += "- 手机不能为空。<br/>";
            $("#lblphone").html("- 手机不能为空");
        }
        if (Address == "") {
            str += "- 详细地址不能为空。<br/>";
            $("#lblAddrress").html("- 详细地址不能为空");
        }
        if (phoneCode == "") {
            str += "- 手机验证码不能为空。<br/>";
            $("#spancode").html("- 手机验证码不能为空");
        }
        if (str != "") {
            return false;
        }

        $.ajax({
            type: 'post',
            url: '../Handler/ShopCart.ashx',
            data: { ActionType: ActionType, principal: principal, phone: phone, Address: Address, Province: Province, City: City, Area: Area, id: id, phoneCode: phoneCode, disPhone: disPhone },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    if (id != "") {
                        //修改完成
                        var th = $("input:radio[name=\"addr\"][value=\"" + id + "\"]");
                        $(th).siblings("label[class=\"principal\"]").text(principal);
                        $(th).siblings("label[class=\"phone\"]").text(phone);
                        $(th).siblings("label[class=\"Address\"]").text(Address);
                        $(th).siblings("input[class=\"pr\"]").val(Province);
                        $(th).siblings("input[class=\"ci\"]").val(City);
                        $(th).siblings("input[class=\"ar\"]").val(Area);
                    } else {
                        //新增完成
                        var th = $("input:radio[name=\"addr\"]:checked")
                        var newli = "<li><input name=\"addr\" type=\"radio\" checked=\"true\" value=\"" + data.Code + "\" class=\"dx\" />";
                        newli += "<label class=\"principal\">" + principal + "</label>&nbsp";
                        newli += "<label class=\"Address\">" + Address + "</label>&nbsp";
                        newli += "<label class=\"phone\">" + phone + "</label>&nbsp";
                        newli += "<a href=\"javascript:void(0);\" class=\"link upAddr\">修改</a>";
                        newli += "<input type=\"hidden\" class=\"pr\" value=\"" + Province + "\" />";
                        newli += "<input type=\"hidden\" class=\"ci\" value=\"" + City + "\" />";
                        newli += "<input type=\"hidden\" class=\"ar\" value=\"" + Area + "\" /></li>";

                        $(th).parents("li").siblings().eq(0).append(newli);

                    }
                } else {
                    layerCommon.msg(data.Msg, IconOption.错误);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    });

    //确认地址
    $(".btn_AddrSave").on("click", function () {
        //获取radio已经选中的ID
        var rdo = $(".list").find("li input[name=\"addr\"]:checked");

        var id = $(rdo).val();
        var principal = $.trim($(rdo).siblings("label[class=\"principal\"]").text());
        var phone = $.trim($(rdo).siblings("label[class=\"phone\"]").text());
        var Address = $.trim($(rdo).siblings("label[class=\"Address\"]").text());
        parent.$_seleAddr(id, principal, phone, Address);
    });

    //取消
    $(".btn_Cancel").on("click", function () {
        parent.layerCommon.layerClose("hid_Alert");
    });

    //修改订单、购物车结算的地址
    $_seleAddr = function (id, principal, phone, Address) {
        //关闭弹出层
        layerCommon.layerClose("hid_Alert");
        var type = $("#hidAddrID").attr("tip_AddrID");

        if (type == "OrderInfo") {
            var KeyID = $(".btn_OrderNote").attr("tip");
            $.ajax({
                type: 'post',
                url: '../Handler/ShopCart.ashx',
                data: { ActionType: "AddOrderNote", type: "UpdateAddr", AddrID: id, principal: principal, phone: phone, Address: Address, ID: KeyID },
                async: true, //false:同步 true: 异步
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        $("#hidAddrID").val(id);
                        $("#lblAddrName").text(principal);
                        $("#lblAddrPhone").text(phone);
                        $("#lblAddrAddr").text(Address);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });
        } else {
            $("#hidAddrID").val(id);
            $("#lblAddrName").text(principal);
            $("#lblAddrPhone").text(phone);
            $("#lblAddrAddr").text(Address);
        }
    }
    /*收货地址 end*/

    //新增订单
    $(".btnOrderAdd").on("click", function () {
        var str = "";
        var Order_type = $("#Order_Type").attr("tip");
        var drlen = $(".tabLine tbody tr").length;
        var hidAddrID = $("#hidAddrID").val();
        var Remark = $("#OrderNote").val();
        var Date = $("#txtDate").val();
        var type = $("#type").val();
        var batePrice = $("#txtRebate").val();
        var bate = $("#lblRebate").text();
        var OrderID = $("#OrderID").val();
        var del = $("#delGoodsinfo").val();
        bate = bate.replace(/,/gm, '');
        batePrice = batePrice.replace(/,/gm, '');

        if (Order_type == "") {
            str += "- 请选择订单类型。<br>";
        }
        if (drlen <= 0) {
            str += "- 您未选中任何商品。<br>";
        }
        if (hidAddrID == "") {
            str += "- 请选择收货信息。<br>";
        }
        if (parseFloat(batePrice) > parseFloat(bate)) {
            str += "- 返利金额小于可用返利金额。<br>";
        }
        if (Remark != "") {
            if (Remark.length > 400) {
                str += "- 订单备注不能大于400个字符。<br>";
            }
        }

        if (str != "") {
            layerCommon.msg(str, IconOption.错误);
            return false;
        }

        $.ajax({
            type: 'post',
            url: '../Distributor/ShopInfo.aspx',
            data: { ActionType: "AddOrder", Order_type: Order_type, hidAddrID: hidAddrID, Remark: Remark, Date: Date, bateAmount: batePrice, del: del, type: type, KeyID: OrderID },
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                if (data.Result) {
                    window.location.href = '../Distributor/neworder/orderdetail.aspx.aspx?KeyID=' + data.Code;
                } else
                    layerCommon.alert(data.Msg, IconOption.错误);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    });
});


//查看返利
function ReBateLog() {
    var height = document.body.clientHeight; //计算高度
    var layerOffsetY = (height - 450) / 2; //计算宽度

    var index = layerCommon.openWindow('查看返利', '../Distributor/DisReBate.aspx', '750px', '380px'); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}

function bate() {
    var batePrice = $(".txtRebate").val();//使用的返利
    var Rebate = $("#lblRebate").text(); //返利总额
    var Total = $("#hidprice").val();  //订单总价

    if (parseFloat(batePrice) == parseFloat(0) || batePrice.toString() == "") {
        batePrice = 0;
        $(".txtRebate").val("0");
    }
    
    var re = Rebate.replace(/,/gm, '');
    Total = Total.replace(/,/gm, '');

    if (parseFloat(batePrice) > parseFloat(re))
        if (parseFloat(re) > parseFloat(Total)) {
            $("#txtRebate").val($.trim(parseFloat(Total).toFixed(2)));
            $("#lblbate").text($.trim(parseFloat(Total).toFixed(2)));
            $("#lblprice").text("0");
        } else {
            $("#txtRebate").val($.trim(parseFloat(re).toFixed(2)));
            $("#lblbate").text($.trim(parseFloat(re).toFixed(2)));
        }
    else {
        if (parseFloat(batePrice) >= parseFloat(Total)) {
            $("#txtRebate").val($.trim(parseFloat(Total).toFixed(2)));
            $("#lblbate").text($.trim(parseFloat(Total).toFixed(2)));
            $("#lblprice").text("0");
        } else {
            var Total_bate = parseFloat(Total) - parseFloat(batePrice);
            $("#lblprice").text(formatMoney(Total_bate, 2));
            $("#lblbate").text($.trim(parseFloat(batePrice).toFixed(2)));
            $("#txtRebate").text($.trim(parseFloat(batePrice).toFixed(2)));
        }
    }
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