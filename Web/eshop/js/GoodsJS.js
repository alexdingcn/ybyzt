if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt /*, from*/) {
        var len = this.length >>> 0;
        var from = Number(arguments[1]) || 0;
        from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
        if (from < 0)
            from += len;
        for (; from < len; from++) {
            if (from in this &&
          this[from] === elt)
                return from;
        }
        return -1;
    };
};

var GoodsCoomon = {
    Compid: 0,
    GoodsClassexPand: function () {
        //商品一级分类点击事件　start
        var CateGoryOne = $(".fl1 a[data-Classid='" + $.trim($("#HidCategoryOne").val()) + "']", ".goods-gn .classify"), CateGoryTow;
        var HidCateGoryCheck = $("#HidCategoryCheckId");
        var HidCateGoryThree = $("#HidCategoryThree");
        if ($.trim(HidCateGoryCheck.val()) == "" || $.trim(HidCateGoryCheck.val()) == "0") {
            $(".fl1 a[data-Classid=0]", ".goods-gn .classify").addClass("hover");
        }
        $(".fl1 a[data-Classid=0]", ".goods-gn .classify").on("click", function (e, data) {
            $("input:hidden.CheckClass").val("");
            $(".btn_SearchCategory").trigger("click");
        })
        $(".fl1 a[data-Classid!=0]", ".goods-gn .classify").on("click", function (e, data) {
            var $This = $(this);
            if (!$This.hasClass("hover")) {
                //获取二级分类数据 start
                data == undefined && $("#HidClassMoreShow").val("");
                var DataId = $(this).data("data-Classid") || $(this).attr("data-Classid");
                data ? GoodsCoomon.GetClassFly(DataId, function (ReturnData) {
                    var No2Class = $("<div class='fl2'></div>");
                    $This.addClass("hover");
                    ($("ul.fl3", ".goods-gn .classify").remove(), $This.parent().data("CKId", DataId)) && ReturnData.ListSource.length > 0 && (
                        $.each(ReturnData.ListSource, function (index, data) {
                            No2Class.append($("<a href=\"javascript:\" data-Classid='" + data.ID + "'>" + data.CategoryName + "</a>").data("data-Classid", data.ID));
                        }),
                        $(".goods-gn .classify").append(No2Class), data.CallBack());
                }) : function () {
                    return data ? !1 : (HidCateGoryCheck.val(DataId), $("#HidCategoryOne").val(DataId), $("#HidCategoryTow").val(""), HidCateGoryThree.val(""), $(".btn_SearchCategory").trigger("click"))
                } ();
                //获取二级分类数据 end
            } else {
                data == undefined && function () {
                    HidCateGoryCheck.val(""), $("#HidCategoryOne").val(""), $("#HidCategoryTow").val(""), HidCateGoryThree.val(""),
                    $(".btn_SearchCategory").trigger("click")
                } ();
            }
        }),
        //商品一级分类点击事件　end
        //商品二级分类点击事件 start
        $(".goods-gn .classify").delegate(".fl2 a", "click", function (e, data) {
            var DataId = $(this).data("data-Classid") || $(this).attr("data-Classid");
            var $This = $(this);
            if (!$This.hasClass("hover")) {
                if (DataId != undefined && DataId != "undefined") {
                    data ? GoodsCoomon.GetClassFly(DataId, function (ReturnData) {
                        var No3Class = $("<ul class=\"fl3\"></ul>");
                        data == undefined && $("#HidClassMoreShow").val("");
                        $This.addClass("hover");
                        ReturnData.ListSource.length > 0 &&
                        ($.each(ReturnData.ListSource, function (index, data) {
                            var LI = $("<li></li>");
                            var Span = $("<span class=\"fxx\"></span>");
                            Span.append($("<input type=\"checkbox\" id=\"checkbox-" + index + "\" class=\"regular-checkbox\">").val(data.ID).data("data-Classid", data.ID)).
                            append("<label for=\"checkbox-" + index + "\" class=\"labelkbox\"></label>"),
                            LI.append(Span), LI.append($("<a href=\"javascript:\">" + data.CategoryName + "</a>")), No3Class.append(LI);
                        }), $(".goods-gn .classify").append(No3Class), (No3Class.height() > 62 ? $(".goods-gn .classify").append("<div class=\"more maxf13\"><a href='javascript:;'><i class=\"xx-icon2\"></i>更多</a></div>") : !1), No3Class.attr("max-height", No3Class.height()).css("max-height", "62px"), data.CallBack())
                    }) : function () {
                        return data ? !1 : HidCateGoryCheck.val(DataId), HidCateGoryThree.val(""), $("#HidCategoryTow").val(DataId),
                            $(".btn_SearchCategory").trigger("click");
                    } ();
                }
            } else {
                data == undefined && function () {
                    HidCateGoryCheck.val($("#HidCategoryOne").val()), $("#HidCategoryTow").val(""), HidCateGoryThree.val(""),
                    $(".btn_SearchCategory").trigger("click")
                } ();
            }
        }),
        //商品二级分类点击事件 end
        //商品三级分类点击事件 start
        $(".goods-gn .classify").delegate(".fl3 input:checkbox", "click", function (e) {
            var $This = $(this), DataId = "", CheckItem = $(".fl3 input:checkbox:checked", ".goods-gn .classify");
            CheckItem.length == 0 ? function () {
                HidCateGoryThree.val(""), $("#HidClassMoreShow").val("hide"), HidCateGoryCheck.val($("#HidCategoryTow").val());
            } () : function () {
                $.each(CheckItem, function (index, Contorl) {
                    DataId == "" ? (DataId = $(Contorl).data("data-Classid")) : (DataId += "," + $(Contorl).data("data-Classid"));
                })
                HidCateGoryThree.val(DataId);
                HidCateGoryCheck.val(DataId);
            } ();
            $(".btn_SearchCategory").trigger("click");
        });
        //商品三级分类点击事件 start
        $(".fl1 a", ".goods-gn .classify").each(function (index, Control) {
            $(Control).data("data-Classid", $(Control).attr("data-Classid"));
        }),
        CateGoryOne.length > 0 && CateGoryOne.trigger("click", [{ CallBack: function () {
            CateGoryTow = $(".fl2 a[data-Classid='" + $.trim($("#HidCategoryTow").val()) + "']", ".goods-gn .classify");
            CateGoryTow.length > 0 &&
            //获取三级分类数据
                        function () {
                            CateGoryTow.trigger("click", [{ CallBack: function () {
                                if ($.trim(HidCateGoryThree.val()) != "") {
                                    var $Ul = $(".fl3", ".goods-gn .classify");
                                    $.each(HidCateGoryThree.val().split(","), function (index, data) {
                                        var CheckConrol = $(".fl3 input:checkbox[value=" + data + "]", ".goods-gn .classify");
                                        CheckConrol.length > 0 && CheckConrol.prop("checked", true) && CheckConrol.parent().parent().addClass("hover2");
                                    }), ($("#HidClassMoreShow[value=show]").val().length > 0 && $Ul.css("max-height", ($Ul.attr("max-height") + "px")) && $(".more", ".goods-gn .classify").removeClass("maxf13").find("a").html("<i class=\"xs-icon\"></i>收缩"));
                                }
                            }
                            }]);
                        } ();
            return true;
            //获取三级分类数据
        }
        }]);
        // 
        //点击展开更多分类 start
        $(".goods-gn .classify").delegate(".more", "click", function () {
            var $This = $(this), $Ul = $(".fl3", ".goods-gn .classify");
            ($This.hasClass("maxf13") && $This.find("a").html("<i class=\"xs-icon\"></i>收缩") && $("#HidClassMoreShow").val("show") && $Ul.animate({ "maxHeight": ($Ul.attr("max-height") + "px") }, 500, function () {
                $This.removeClass("maxf13");
            })) || ($This.find("a").html("<i class=\"xx-icon2\"></i>更多"), $("#HidClassMoreShow").val("hide"), $(".fl3", ".goods-gn .classify").animate({ "maxHeight": "62px" }, 500, function () {
                $This.addClass("maxf13");
            }));
        })
        //点击展开更多分类 end 
        $("ul#Div_BigGoods").delegate("input:text.txtKeyInt", {
            keyup: function () {
                KeyInt(this, "");
            },
            blur: function () {
                KeyInt(this, "1");
            }
        });
    },
    //页面初始化 显示已选中的商品分类 end
    //获取分类数据 start
    GetClassFly: function (dataId, CallBack) {
        $.ajax({
            type: 'post',
            url: '/Controller/GetDataSource.ashx?action=GetGoodsClass',
            data: { Compid: GoodsCoomon.Compid, CategoryId: dataId },
            dataType: 'json',
            success: function (ReturnData) {
                if (ReturnData.Result && !ReturnData.Error) {
                    CallBack(ReturnData);
                } else {
                    layerCommon.msg(ReturnData.Msg, IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
//                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        });
    } //获取分类数据 end
        , //获取商品规格属性
    GetArryAttribute: function (GoodIdArry, Callback) {
        if (GoodIdArry && GoodIdArry.length > 0) {
            $.ajax({
                type: 'get',
                url: '/Controller/GetDataSource.ashx?action=GetGoodsAttribute',
                data: { GoodIdArrr: GoodIdArry, Compid: GoodsCoomon.Compid },
                dataType: 'json',
                traditional: true,
                success: function (ReturnData) {
                    if (ReturnData.Result) {
                        Callback(ReturnData.Code);
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
//                    layerCommon.msg("添加失败，请求错误或超时,请重试", IconOption.错误);
                }
            })
        }
    }
    //获取商品规格属性 end
    //初始化大图商品规格属性以及商品价格
        , GetBigGoodsAttribute: function (GoodsControlSelector, Suceesback) {
            var $Li = $(GoodsControlSelector).find("li");
            var GoodsIdArry = new Array();
            $Li.each(function (intdex, li) {
                $(li).data("data-Goodsid", $(li).attr("data-goodsid"));
                $(li).data("data-Goodsname", $(li).attr("data-Goodsname"));
                GoodsIdArry.push($(li).attr("data-Goodsid"));
            });
            if (typeof Suceesback == "function" && $Li.length == 0) {
                Suceesback();
            }
            GoodsCoomon.GetArryAttribute(GoodsIdArry, function (ReturnData) {
                var AttrJsonArry = {};
                AttrJsonArry["GoodsInfoArry"] = new Array();
                if (ReturnData) {
                    ReturnData = eval('(' + ReturnData + ')');
                    $Li.each(function (index, Li) {
                        var literal = $(Li).find(".literal");
                        literal.html("");
                        var dataid = $(Li).data("data-Goodsid") || $(Li).attr("data-Goodsid");
                        var ArryValue = ReturnData[dataid];
                        var DIVDemo = $("<div class='specs'></div>");
                        literal.append(DIVDemo);
                        if (ArryValue.length > 0) {
                            if (literal.length > 0) {
                                var PushAttribute = new Array();
                                var AtValue = {};
                                var ArrrValue = "";
                                //属性值放进Json数组里
                                $.each(ArryValue, function (index, DataValue) {
                                    if (AtValue[DataValue.AttributeName] == undefined) {
                                        ArrrValue += (DataValue.AttributeName + ":" + DataValue.AttrValue + "；");
                                        AtValue[DataValue.AttributeName] = new Array();
                                    }
                                    var ArrtValueControl = $("<a href=\"javascript:;\"  " + (AtValue[DataValue.AttributeName].length == 0 ? "class=\"hover\"" : "") + ">" + DataValue.AttrValue + "</a>").data("Attr", DataValue.AttributeName).data("AttrValue", DataValue.AttrValue);
                                    ArrtValueControl.on("click", function () {
                                        GoodsCoomon.ArrtValueControlGetdate(Li, ArrtValueControl);
                                    });
                                    AtValue[DataValue.AttributeName].push(ArrtValueControl);
                                });
                                //属性值放进Json数组里 end

                                $(Li).data("AttrValue", ArrrValue);
                                AttrJsonArry["GoodsInfoArry"].push({ GoodsID: dataid, AttrValue: "" + ArrrValue + "" });
                                //循环规格属性数组，并创建规格属性Demo 显示到页面
                                var LIDemo = null;
                                $.each(ArryValue, function (index, DataValue) {
                                    if (PushAttribute.indexOf(DataValue.AttributeName) == -1) {
                                        LIDemo = $("<div class=\"li\"><div class='t' title='" + DataValue.AttributeName + "'>" + (DataValue.AttributeName.length > 4 ? DataValue.AttributeName.substring(0, 4)  : DataValue.AttributeName) + "：</div></div>");
                                        var AttrvDiv = $("<div class='n'></div>");
                                        $.each(AtValue[DataValue.AttributeName], function (index, AtValueInfo) {
                                            AttrvDiv.append(AtValueInfo);
                                        })
                                        PushAttribute.push(DataValue.AttributeName);
                                        DIVDemo.append(LIDemo.append(AttrvDiv));
                                    }
                                });

                            }
                        } else {
                            AttrJsonArry["GoodsInfoArry"].push({ GoodsID: dataid, AttrValue: "" });
                        }
                    });
                }

                //获取商品价格后把数据绑定到指定的行 
                GoodsCoomon.GetGoodsInfoIdPrice(AttrJsonArry, function (InfoArryData) {
                    $Li.each(function (index, Li) {
                        var DivSpec = $(Li).find(".literal div.specs");
                        var dataid = $(Li).data("data-Goodsid") || $(Li).attr("data-Goodsid");
                        var InfoData = InfoArryData[dataid];
                        if (InfoData != undefined) {
                            InfoData = InfoData[0];
                            for (var Item in InfoData) {
                                $(Li).data(Item, InfoData[Item]);
                            }
                            $(Li).find(".wrapper div.price b").html("¥" + InfoData["finalPrice"].toFixed(2)).removeClass("txt");
                            var DivCarttxt = $("<div class=\"li\"><div class=\"t\">选择数量：</div><div class=\"b\"><input  id='GoodsBig_CartNum' maxlength='4' value='1' type=\"text\" class=\"box txtKeyInt\" /><i>件</i></div></div>");
                            DivSpec.append(DivCarttxt);
                        } else {
                            $(Li).find(".wrapper div.price b").html("¥" + "0.00").removeClass("txt");
                        }
                    });
                }, 1, Suceesback);
                //获取商品价格后把数据绑定到指定的行 end

            });

        }
    //初始化大图商品规格属性以及商品价格 end
        ,
    //获取商品InfoId和价格
    GetGoodsInfoIdPrice: function (AttrJsonArry, CallBack, Isall, Suceesback) {
        Isall == undefined && (Isall = 0);
        if (AttrJsonArry) {
            $.ajax({
                type: 'post',
                url: '/Controller/GetDataSource.ashx?action=GetGoodsInfoIdPrice',
                data: { GoodsInfoArry: JSON.stringify(AttrJsonArry), Compid: GoodsCoomon.Compid, IsAll: Isall },
                dataType: 'json',
                cache: false,
                timeout: 3500,
                success: function (ReturnData) {
                    if (ReturnData.Result) {
                        try {
                            var Json = eval('(' + ReturnData.Code + ')');
                            CallBack(Json);
                        }
                        catch (e) {
                        }
                    }
                    else if (ReturnData.Error) {
                        layerCommon.msg(ReturnData.Msg, IconOption.错误);
                    }
                    if (typeof Suceesback == "function") {
                        Suceesback();
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (typeof Suceesback == "function") {
                        Suceesback();
                    }
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            })
        }
    } //获取商品InfoId和价格end
        ,
    //商品规格属性选中事件（点击后获取选中的商品属性的价格）
    ArrtValueControlGetdate: function (ParentControl, Control) {
        var dataid = $(ParentControl).data("data-Goodsid");
        var AttrJsonArry = {};
        AttrJsonArry["GoodsInfoArry"] = new Array();
        var AttrValue = $(ParentControl).data("AttrValue");
        if ($.trim(AttrValue) != "" && AttrValue != undefined) {
            var reg = eval("/" + $(Control).data("Attr") + ":[^\；]{1,}[\；]{1}/");
            AttrValue = AttrValue.replace(reg, $(Control).data("Attr") + ":" + $(Control).data("AttrValue") + "；");
        }
        AttrJsonArry["GoodsInfoArry"].push({ GoodsID: dataid, AttrValue: AttrValue });
        if (AttrJsonArry["GoodsInfoArry"].length > 0) {
            //获取选中某属性商品的价格
            GoodsCoomon.GetGoodsInfoIdPrice(AttrJsonArry, function (InfoArryData) {
                var InfoData = InfoArryData[dataid];
                if (InfoData != undefined) {
                    InfoData = InfoData[0];
                    $(ParentControl).find(".wrapper div.price b").html("¥" + InfoData["finalPrice"].toFixed(2));
                    for (var Item in InfoData) {
                        $(ParentControl).data(Item, InfoData[Item]);
                    }
                    if ($(ParentControl).find("#GoodsBig_CartNum").length == 0) {
                        var DivSpec = $(ParentControl).find(".literal div.specs");
                        var DivCarttxt = $("<div class=\"li\"><div class=\"t\">选择数量：</div><div class=\"b\"><input  id='GoodsBig_CartNum' maxlength='4'  value='1' type=\"text\" class=\"box txtKeyInt\" /><i>件</i></div></div>");
                        DivSpec.append(DivCarttxt);
                    }
                    $(Control).addClass("hover").siblings("a").removeClass("hover");
                    $(ParentControl).data("AttrValue", AttrValue);
                }
            })
            //获取选中某属性商品的价格 end
        }


    } //商品规格属性点击事件 end
    ,

    GoodsAddCollect: function (IsComp, Islogin, Comid) {
        //Ul start
        Islogin = Islogin || false;
        $("ul#Div_BigGoods,ul#Div_BigGoodsHot").each(function (intdex, ul) {
            //添加收藏点击事件 start
            $(ul).find("li #GoodsBig_AddCollect").on("click", function () {
                if (!Islogin) {
                    layerCommon.openWindow("用户登录", "/WindowLogin.aspx?ShowRegis=show&Comid=" + Comid, "400px", "345px", function () {
                        $.ajax({
                            type: "POST",
                            url: '/Controller/login.ashx',
                            data: { SubmitAcion: "CloseAccuntSwitch" },
                            cache: false,
                            success: function (ReturnData) {
                            },
                            error: function () {
                            }
                        });
                    }, false);
                    return;
                }
                var $This = $(this);
                var dataid = $This.parents("li:eq(0)").data("data-Goodsid") || $This.attr("data-goodsid");
                if (dataid != undefined) {
                    $.ajax({
                        type: "post",
                        url: "/Controller/AddUpDataSource.ashx?action=AddCart",
                        data: { Goodsid: dataid },
                        dataType: 'json',
                        timeout: 3000,
                        cache: false,
                        success: function (data) {
                            if (data.Result) {
                                var IsHotBig = $This.parents("ul#Div_BigGoodsHot:eq(0)").length > 0;
                                if ((!$("input:checkbox#CK_Collect").prop("checked") || $("input:checkbox#CK_Pro").prop("checked")) || IsHotBig) {
                                    var ShowI = $This.attr("title", data.Code).find("i");
                                    switch (data.Code) {
                                        case "收藏": ShowI.removeAttr("style"); IsHotBig ? $("ul#Div_BigGoods #GoodsBig_AddCollect[data-goodsid='" + dataid + "']").attr("title", data.Code).find("i").removeAttr("style") : $("ul#Div_BigGoodsHot #GoodsBig_AddCollect[data-goodsid='" + dataid + "']").attr("title", data.Code).find("i").removeAttr("style"); break;
                                        case "取消收藏": ShowI.attr("style", "background-position: -0px -73px;"); IsHotBig ? $("ul#Div_BigGoods #GoodsBig_AddCollect[data-goodsid='" + dataid + "']").attr("title", data.Code).find("i").attr("style", "background-position: -0px -73px;") : $("ul#Div_BigGoodsHot #GoodsBig_AddCollect[data-goodsid='" + dataid + "']").attr("title", data.Code).find("i").attr("style", "background-position: -0px -73px;"); break;
                                    }
                                }
                                else {
                                    $($This).parents("li:eq(0)").remove();
                                }
                                layerCommon.msg(data.Msg, IconOption.正确);
                            } else {
                                if (data.Code == "Login") {
                                    layerCommon.openWindow("用户登录", "/WindowLogin.aspx?ShowRegis=show&Comid=" + Comid, "400px", "345px", function () {
                                        $.ajax({
                                            type: "POST",
                                            url: '/Controller/login.ashx',
                                            data: { SubmitAcion: "CloseAccuntSwitch" },
                                            cache: false,
                                            success: function (ReturnData) {
                                            },
                                            error: function () {
                                            }
                                        });
                                    }, false);
                                } else {
                                    layerCommon.msg(data.Msg, IconOption.错误, 2000);
                                }
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layerCommon.msg("请求错误或超时,请重试", IconOption.错误, 2000);
                        }
                    })
                }
            });
            //添加收藏点击事件 end

            //购物车点击事件 start
            $(ul).find("li #GoodsBig_AddCart").on("click", function () {
                if (!Islogin) {
                    layerCommon.openWindow("用户登录", "/WindowLogin.aspx?ShowRegis=show&Comid=" + Comid, "400px", "345px", function () {
                        $.ajax({
                            type: "POST",
                            url: '/Controller/login.ashx',
                            data: { SubmitAcion: "CloseAccuntSwitch" },
                            cache: false,
                            success: function (ReturnData) {
                            },
                            error: function () {
                            }
                        });
                    }, false);
                    return;
                }
                var PrentControl = $(this).parents("li:eq(0)");
                var infoId = PrentControl.data("Infoid");
                var dataid = PrentControl.data("data-Goodsid");
                var CartNum = PrentControl.find("#GoodsBig_CartNum").length == 0 ? "1" : PrentControl.find("#GoodsBig_CartNum").val();
                var Obj = this;
                $.ajax({
                    type: 'post',
                    url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
                    data: { GoodsInfoID: infoId, ProID: "", Num: CartNum, Price: "", TPrice: "", CartType: 1, Goodsid: dataid },
                    dataType: 'json',
                    success: function (ReturnData) {
                        if (ReturnData.Result) {
                            //                                var CloneImg = $("#Top_CartImg").clone().css({ "position": "absolute", "z-index": "10000", "left": $(Obj).offset().left + "px", "top": ($(Obj).offset().top - 25) + "px" }).addClass("Top_CartImgClone");
                            //                                $('body').append(CloneImg);
                            layerCommon.msg("商品已成功添加到购物车", IconOption.正确);
                            $(".TopE_CartNum").html(ReturnData.SumCart);

                            $(".cur a i[tip=\"title\"]").length > 0 ? $(".cur a i[tip=\"title\"]").parent("a").remove() : ""

                            var CartControl = $(".cur a[infoid='" + infoId + "']  span.goodsnum", ".fore2");

                            if ($("li[class=\"fore2\"] .cur a").length < 4) {
                                (CartControl.length == 0 && $(".cur", ".fore2").append("<a target='_blank' Infoid='" + infoId + "' href=\"/e" + infoId + "_" + GoodsCoomon.Compid +"_.html"+ "\" ><i class=\"GoGoodsInfo name\">" + PrentControl.data("data-Goodsname") + "</i><span class=\"goodsnum num\">x" + CartNum + "</span></a>")) ||
                            (CartControl.text("x" + (parseInt(CartControl.text().replace("x", "")) + parseInt(CartNum))));
                                return;
                            } else {
                                if ($("li[class=\"fore2\"] .cur div[class=\"border\"]").length > 0) {
                                    if (CartControl.length > 0) {
                                        CartControl.text("x" + (parseInt(CartControl.text().replace("x", "")) + parseInt(CartNum)))
                                    } else {
                                        var sum = $("li[class=\"fore2\"] .cur div[class=\"border\"] span span[class=\"red\"]").text();
                                        $("li[class=\"fore2\"] .cur div[class=\"border\"] span span[class=\"red\"]").text(parseInt(sum) + parseInt(CartNum));
                                    }
                                } else {
                                    $("li[class=\"fore2\"] .cur").append("<div class=\"border\"><span>购物车还有<span class='red' id=\"num\">" + CartNum + "</span></span>个商品<a class\"cklink\" style='float:right' href=\"../Distributor/Shop.aspx\">去购物车</a></div>");
                                }
                            }
//                            CloneImg.animate({ "left": ($(".shop-i").offset().left) + "px", "top": ($(".shop-i").offset().top) + "px" }, 1000, function () {
//                                /*CloneImg.attr("class", "gw-icon2 Top_CartImgClone");*/
//                                setTimeout(function () { CloneImg.remove(); $(".TopE_CartNum").html(ReturnData.SumCart); }, 400);
//                            });
                        } else {
                            if (ReturnData.Code == "Login") {
                                layerCommon.openWindow("用户登录", "/WindowLogin.aspx?ShowRegis=show&Comid=" + Comid, "400px", "345px", function () {
                                    $.ajax({
                                        type: "POST",
                                        url: '/Controller/login.ashx',
                                        data: { SubmitAcion: "CloseAccuntSwitch" },
                                        cache: false,
                                        success: function (ReturnData) {
                                        },
                                        error: function () {
                                        }
                                    });
                                }, false);
                            } else {
                                if (infoId != undefined || ReturnData.IsRegi) {
                                    layerCommon.msg(ReturnData.Msg, IconOption.错误);
                                }
                                else {
                                    layerCommon.msg("该属性商品已下架，无法加入购物车，<br/>如果该商品有其它属性，请重新选择。", IconOption.错误);
                                }
                            }
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("添加失败，请求错误或超时,请重试", IconOption.错误);
                    }
                });
                return false;
            });
            //购物车点击事件 end
        });
        //Ul end
    }
    //商品购物车/收藏点击事件 end
    ,
    //绑定推荐商品数据 start
    BindGoodsHot: function (ParentControl, HidPageIndex, HidPageCount, IsComp, ImgPath, Comid, CallBack) {
        $.ajax({
            type: 'post',
            url: '/Controller/GetDataSource.ashx?action=GetGoodsHot',
            data: { PageIndex: HidPageIndex.val(), Comid: Comid },
            dataType: "text",
            success: function (ReturnData) {
                try {
                    ReturnData = eval('(' + ReturnData + ')');
                    var index = 0;
                    if (ReturnData.Source.length > 0) {
                        ParentControl.html("");
                        for (var item in ReturnData.Source) {
                            item = ReturnData.Source[item];
                            if (typeof item != "object") {
                                break;
                            }
                            var LiHTML = '<li id="LIBig_' + (index++) + '"  data-goodsid="' + item.id + '" data-goodsname="' + item.goodsname + '">\
                                        <div class="wrapper">\
                                            <div class="pic"><a target="_blank" href="/e' + item.id + '_' + Comid + '.html"><img src="' + (ImgPath + item.pic2) + '" onerror="this.src=\'/images/Goods400x400.jpg\'" "></a></div>\
                                            <div class="price"><b ' + (IsComp ? "" : "class=\"txt\"") + '>' + (IsComp ? "¥0.00" : "代理商可见") + '</b> ' + (item.type == "0" || item.type == "1" ? "<div class=\"sale-box\"><i class=\"sale\">促销</i><div class=\"sale-txt\"><i class=\"arrow\"></i>" + item.proinfomation + "</div></div>" : "") + ' </div>\
                                            <div class="txt2"><a target="_blank" title="' + item.goodsname + '" href="/e' + item.id + '_' + Comid + '.html">' + item.goodsname + '</a></div>\
                                            <div id="Ltr_' + (index) + '" class="literal"></div>\
                                            <div class="btn"><a href="javascript:;" data-goodsid="' + item.id + '" id="GoodsBig_AddCollect" title="' + (item.bdcid != "" ? "取消收藏" : "收藏") + '" class="keep"><i style="' + (item.bdcid != "" ? "background-position: -0px -73px;" : "") + '" class="sc-icon" ></i>收藏</a><a id="GoodsBig_AddCart" href="javascript:;" class="addCart"><i class="gwc-icon"></i>加入购物车</a></div>\
                                        </div>\
                                    </li>';
                            ParentControl.append(LiHTML);
                        }
                        HidPageCount.val(ReturnData.PageCount);
                    } else {
                        $("#DivHotGoods,#DivHotGoodsHeader").hide();
                    }
                }
                catch (e) {

                }
                if (typeof CallBack == "function") {
                    CallBack();
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (typeof CallBack == "function") {
                    CallBack();
                }
            }
        });

    }
    //绑定推荐商品数据 end

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


(function ($, h, c) { var a = $([]), e = $.resize = $.extend($.resize, {}), i, k = "setTimeout", j = "resize", d = j + "-special-event", b = "delay", f = "throttleWindow"; e[b] = 250; e[f] = true; $.event.special[j] = { setup: function () { if (!e[f] && this[k]) { return false } var l = $(this); a = a.add(l); $.data(this, d, { w: l.width(), h: l.height() }); if (a.length === 1) { g() } }, teardown: function () { if (!e[f] && this[k]) { return false } var l = $(this); a = a.not(l); l.removeData(d); if (!a.length) { clearTimeout(i) } }, add: function (l) { if (!e[f] && this[k]) { return false } var n; function m(s, o, p) { var q = $(this), r = $.data(this, d); r.w = o !== c ? o : q.width(); r.h = p !== c ? p : q.height(); n.apply(this, arguments) } if ($.isFunction(l)) { n = l; return m } else { n = l.handler; l.handler = m } } }; function g() { i = h[k](function () { a.each(function () { var n = $(this), m = n.width(), l = n.height(), o = $.data(this, d); if (m !== o.w || l !== o.h) { n.trigger(j, [o.w = m, o.h = l]) } }); g() }, e[b]) } })(jQuery, this);
