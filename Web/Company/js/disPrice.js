var imgurl = "http://118.178.88.33:902/GoodsImg/";

//检查代理商是否调过价 如果调过价则选择最后一次的调价记录选中显示
function IsChkGoods(compId) {
    $.ajax({
        type: "post",
        url: "/Handler/GetPrice.ashx",
        data: { ck: Math.random(), action: "IsChkGoods", compId: compId },
        dataType: "text",
        success: function (data) {
            if (data != "") {
                //单个代理商each
                $(".dis-filter li").eq(4).find("a").each(function (index, obj) {
                    //匹配到与数据库相同的再click读取数据
                    if ($(this).attr("tips") == data) {
                        $(this).trigger("click");
                    }
                })
            }
        }, error: function () { }
    })
}
$(function () {
    $(".txt_txtTypename").addClass("box");
    $(".showDiv3 .ifrClass").css("width", "135px");
    $(".showDiv3").css("width", "135px");
    $('iframe').load(function () {
        $('iframe').contents().find('.pullDown').css("width", "130px");
    });
    var compId = $(".hidCompId").val(); //厂商Id
    GtDisBind(""); //加载top10个代理商
    // IsChkGoods(compId); //检查代理商是否调过价 如果调过价则选择最后一次的调价记录选中显示
    //调价类型切换
    $(".disprice a").click(function () {
        $(this).addClass("cur").siblings().removeClass("cur"); //当前选中样式，同级隐藏
        var index = $(this).index(); //a标签的索引
        if (index == 2) { //个体代理商
            GtDisBind(""); //加载top10个代理商
            // IsChkGoods(compId); //检查代理商是否调过价 如果调过价则选择最后一次的调价记录选中显示
            $(".dis-filter li:gt(2)").show(); //大于索引2的显示,区分代理商分类、区域和单个代理商
            $(".dis-filter li:lt(3)").hide(); //小于索引3的隐藏,区分代理商分类、区域和单个代理商
            $(".tabLine tbody").html("<tr><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><div class=\"search\"><input name=\"txtGoods\" type=\"text\" class=\"box txtGoods\"><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice none\" value=\"\" onfocus=\"InputFocus(this)\"    onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>"); //清空商品绑定列表
        } else {
            if (index == 0) {
                //默认一级分类（当前，厂商id,父级id,所在等级、区域行的li 索引,区分代理商区域、等级）并绑定商品列表
                BindDis(this, compId, 0, 0, "getDisType");
            } else {
                //默认一级区域 （当前，厂商id,父级id,所在等级、区域行的li 索引,区分代理商区域、等级）并绑定商品列表
                BindDis(this, compId, 0, 0, "getDisArea");
            }
            $(".hid_TypeId").val("");
            $(".txt_txtTypename").val("");
            $(".dis-filter li:gt(2)").hide(); //大于索引2的隐藏,区分代理商分类、区域和单个代理商
            $(".dis-filter li:lt(3)").show(); //小于索引3的显示,区分代理商分类、区域和单个代理商
            $(".dis-filter li:lt(4)").find(".nr").html("<span style=\" height: 24px;\"></span>"); //单个代理商列表清空
        }
    })
    //分类、区域点击事件
    $(document).on("click", ".dis-filter li a", function () {
        var parentId = $(this).attr("tip"); //父级标识
        var disId = $(this).attr("tips"); //代理商Id
        var index = $(this).parent().parent().attr("liindex"); //li所在行的索引
        var aindex; //代理商分类的索引
        $(".disprice a").each(function (indexx, obj) {
            if ($(this).attr("class") == "cur") {//根据样式判断选中的是什么
                aindex = indexx;
                return false;
            }
        })
        if (aindex == 0) {//等级 （当前，厂商id,父级id,所在等级、区域行的li 索引,区分代理商区域、等级）并绑定商品列表
            BindDis(this, compId, parentId, parseInt(index) + 1, "getDisType");
        } else if (aindex == 1) {//区域 （当前，厂商id,父级id,所在等级、区域行的li 索引,区分代理商区域、等级）并绑定商品列表
            BindDis(this, compId, parentId, parseInt(index) + 1, "getDisArea");
        } else {//单个代理商 (厂商id，代理商id)
            if (disId != "" && disId != undefined) {
                BindDiss(compId, disId); //绑定商品列表
            }
        }
    })
    //代理商查询
    $(".seaBox .btnSelect").click(function () {
        var str = ""; //传入的值
        var dis = $.trim($(".txtDis").val()); //代理商名称或者编码
        var distypeid = $.trim($(".hid_TypeId").val()); //代理商分类
        if (dis != "" || distypeid != "") {
            //   layerCommon.msg("请先输入或者选择查询条件", IconOption.哭脸);
            //   $(".txtDis").focus();
            //   return false;
            str = dis + "," + distypeid;
        }
        Qingk(); //默认初始状态
        GtDisBind(str); //根据输入的内容进行查询并绑定代理商列表
    })
    //个体代理商选中
    $(document).on("click", ".dis-filter li:eq(4) a", function () {
        $(this).addClass("cur").siblings().removeClass("cur");
    })
    //保存按钮
    $(".toolbar li:eq(0)").click(function () {
        var parentidlist = ""; //拼接选中的代理商区域或者等级（160，0，0）
        var disidlist = ""; //获取选中的单个代理商id
        //判断是否选中具体的范围或者代理商进行调价
        $(".dis-filter a").each(function (index, obj) {
            if ($(this).attr("class") == "cur") {//根据样式判断
                if ($(this).attr("tip") != undefined) {
                    parentidlist += $(this).attr("tip") + ",";
                }
                if ($(this).attr("tips") != undefined) {//根据tips是否有值判断
                    disidlist = $(this).attr("tips");
                }
            }
        })
        if (parentidlist != "" || disidlist != "") {
            var aindex; //代理商分类的索引
            $(".disprice a").each(function (indexx, obj) {
                if ($(this).attr("class") == "cur") {
                    aindex = indexx;
                    return false;
                }
            })
            //0 1 属于代理商区域、等级
            if (aindex == 1 || aindex == 0) {
                var isprice = "";
                parentidlist = parentidlist.substring(0, parentidlist.length - 1);
                //每行商品数据的拼接
                var json = "[";
                $(".tabLine tbody tr").each(function (index, obj) {
                    if ($(this).attr("tip") != undefined) {
                        var name = $.trim($(this).find(".divname").text());
                        var code = $.trim($(this).find(".divcode").text());
                        var info = $.trim($(this).find(".divinfo").text());
                        var unit = $.trim($(this).find(".divunit").text());
                        var price = $.trim($(this).find(".txtDisPrice").val());
                        if (price == "" || price == undefined) {
                            isprice = "-1";
                            return false;
                        }
                        json += "{\"goodsinfoid\":\"" + $(this).attr("tip") + "\",\"name\":\"" + name + "\",\"code\":\"" + code + "\",\"info\":\"" + info + "\",\"unit\":\"" + unit + "\",\"price\":\"" + price + "\"},";
                    }
                })
                if (json != "[") {
                    json = json.substring(0, json.length - 1);
                }
                json += "]";
                if (isprice == "-1") {
                    layerCommon.msg("商品价格不能为空", IconOption.哭脸);
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx",
                    data: { ck: Math.random(), action: "inertDisPrice", compId: compId, parentidlist: parentidlist, type: aindex, json: json },
                    dataType: "text",
                    success: function (data) {
                        if (data == "ycz") {
                            layerCommon.msg("不能上下级同时调整价格", IconOption.哭脸);
                        } else if (data == "yczl") {
                            layerCommon.msg("代理商分类价格和代理商区域价格不得同时生效", IconOption.哭脸, 5000);
                        } else if (data == "cg") {
                            //                            if ($(".tools .toolbar li").eq(3).attr("class") == "") {
                            //                                $(".tools .toolbar li").eq(2).addClass("none"); //禁用隐藏
                            //                            } else {
                            //                                $(".tools .toolbar li").eq(2).removeClass("none"); //启用显示
                            //                            }
                            layerCommon.msg("保存成功", IconOption.笑脸);
                        } else {
                            var num = $(".tabLine tbody tr").length; //获取商品数据的行数 
                            var bol = false;
                            for (var i = 0; i < num; i++) {
                                var tip = $(".tabLine tbody tr").eq(i).attr("tip");
                                if (tip != undefined && tip != "") {//根据是否存在tip来判断是否选择了商品
                                    bol = true;
                                    break;
                                }
                            }
                            if (!bol) {
                                layerCommon.msg("请先选择商品", IconOption.哭脸);
                            } else {
                                layerCommon.msg("保存失败", IconOption.哭脸);
                            }
                        }
                        return false;
                    }, error: function () { layerCommon.msg("保存失败", IconOption.哭脸); return false; }
                })
            } else {
                var isprice = "";
                //属于单个代理商
                if (disidlist == "" || disidlist == undefined) {
                    layerCommon.msg("请先确定调价类型或者代理商进行调价", IconOption.哭脸);
                    return false;
                }
                //每行商品数据的拼接
                var json = "[";
                $(".tabLine tbody tr").each(function (index, obj) {
                    if ($(this).attr("tip") != undefined) {
                        var name = $.trim($(this).find(".divname").text());
                        var code = $.trim($(this).find(".divcode").text());
                        var info = $.trim($(this).find(".divinfo").text());
                        var unit = $.trim($(this).find(".divunit").text());
                        var price = $.trim($(this).find(".txtDisPrice").val());
                        if (price == "" || price == undefined) {
                            isprice = "-1";
                            return false;
                        }
                        json += "{\"goodsinfoid\":\"" + $(this).attr("tip") + "\",\"name\":\"" + name + "\",\"code\":\"" + code + "\",\"info\":\"" + info + "\",\"unit\":\"" + unit + "\",\"price\":\"" + price + "\"},";
                    }
                })
                if (json != "[") {
                    json = json.substring(0, json.length - 1);
                }
                json += "]";
                if (isprice == "-1") {
                    layerCommon.msg("商品价格不能为空", IconOption.哭脸);
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx",
                    data: { ck: Math.random(), action: "inertGoodsPrice", compId: compId, disId: disidlist, json: json },
                    dataType: "text",
                    success: function (data) {
                        if (data != "") {
                            //                            if ($(".tools .toolbar li").eq(3).attr("class") == "") {
                            //                                $(".tools .toolbar li").eq(2).addClass("none"); //禁用隐藏
                            //                            } else {
                            //                                $(".tools .toolbar li").eq(2).removeClass("none"); //启用显示
                            //                            }
                            layerCommon.msg("保存成功", IconOption.笑脸);
                        } else {
                            var num = $(".tabLine tbody tr").length; //获取商品数据的行数 
                            var bol = false;
                            for (var i = 0; i < num; i++) {
                                var tip = $(".tabLine tbody tr").eq(i).attr("tip");
                                if (tip != undefined && tip != "") {//根据是否存在tip来判断是否选择了商品
                                    bol = true;
                                    break;
                                }
                            }
                            if (!bol) {
                                layerCommon.msg("请先选择商品", IconOption.哭脸);
                            } else {
                                layerCommon.msg("保存失败", IconOption.哭脸);
                            }
                        }
                        return false;
                    }, error: function () { layerCommon.msg("保存失败", IconOption.哭脸); return false; }
                })
            }
        } else {
            layerCommon.msg("请先确定调价类型或者代理商进行调价", IconOption.哭脸);
            return false;
        }
    })
    //启用按钮
    $(".toolbar li:eq(2)").click(function () {
        var parentidlist = ""; //拼接选中的代理商区域或者等级（160，0，0）
        var disidlist = ""; //获取选中的单个代理商id
        //判断是否选中具体的范围或者代理商进行调价
        $(".dis-filter a").each(function (index, obj) {
            if ($(this).attr("class") == "cur") {
                if ($(this).attr("tip") != undefined) {
                    parentidlist += $(this).attr("tip") + ",";
                }
                if ($(this).attr("tips") != undefined) {
                    disidlist = $(this).attr("tips");
                }
            }
        })
        if (parentidlist != "" || disidlist != "") {
            parentidlist = parentidlist.substring(0, parentidlist.length - 1);
            var aindex; //代理商分类的索引
            $(".disprice a").each(function (indexx, obj) {
                if ($(this).attr("class") == "cur") {
                    aindex = indexx;
                    return false;
                }
            })
            //0 1 属于代理商区域、等级
            if (aindex == 1 || aindex == 0) {
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx", //type2=1 属于启用
                    data: { ck: Math.random(), action: "IsEnabledDisPrice", compId: compId, parentidlist: parentidlist, type: aindex, type2: 1 },
                    dataType: "text",
                    success: function (data) {
                        if (data == "sb") {
                            layerCommon.msg("启用失败,不能同时区域和等级调价", IconOption.哭脸);
                        } else if (data == "cg") {
                            $(".tools .toolbar li").eq(2).addClass("none"); //启用隐藏
                            $(".tools .toolbar li").eq(3).removeClass("none"); //禁用显示
                            layerCommon.msg("启用成功", IconOption.笑脸);
                        } else {
                            layerCommon.msg("启用失败", IconOption.哭脸);
                        }
                        return false;
                    }, error: function () { layerCommon.msg("启用失败", IconOption.哭脸); return false; }
                })
            } else {
                //单个代理商
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx", //type2=1 属于启用
                    data: { ck: Math.random(), action: "IsEnabledGoodsPrice", compId: compId, disid: disidlist, type2: 1 },
                    dataType: "text",
                    success: function (data) {
                        if (data == "cg") {
                            $(".tools .toolbar li").eq(2).addClass("none"); //启用隐藏
                            $(".tools .toolbar li").eq(3).removeClass("none"); //禁用显示
                            layerCommon.msg("启用成功", IconOption.笑脸);
                        } else {
                            layerCommon.msg("启用失败", IconOption.哭脸);
                        }
                        return false;
                    }, error: function () { layerCommon.msg("启用失败", IconOption.哭脸); return false; }
                })
            }
        }
    })
    //禁用按钮
    $(".toolbar li:eq(3)").click(function () {
        var parentidlist = ""; //拼接选中的代理商区域或者等级（160，0，0）
        var disidlist = ""; //获取选中的单个代理商id
        //判断是否选中具体的范围或者代理商进行调价
        $(".dis-filter a").each(function (index, obj) {
            if ($(this).attr("class") == "cur") {
                if ($(this).attr("tip") != undefined) {
                    parentidlist += $(this).attr("tip") + ",";
                }
                if ($(this).attr("tips") != undefined) {
                    disidlist = $(this).attr("tips");
                }
            }
        })
        if (parentidlist != "" || disidlist != "") {
            parentidlist = parentidlist.substring(0, parentidlist.length - 1);
            var aindex; //代理商分类的索引
            $(".disprice a").each(function (indexx, obj) {
                if ($(this).attr("class") == "cur") {
                    aindex = indexx;
                    return false;
                }
            })
            //0 1 属于代理商区域、等级
            if (aindex == 1 || aindex == 0) {
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx", //type2=0 属于禁用
                    data: { ck: Math.random(), action: "IsEnabledDisPrice", compId: compId, parentidlist: parentidlist, type: aindex, type2: 0 },
                    dataType: "text",
                    success: function (data) {
                        if (data != "") {
                            $(".tools .toolbar li").eq(3).addClass("none"); //禁用隐藏
                            $(".tools .toolbar li").eq(2).removeClass("none"); //启用显示
                            layerCommon.msg("禁用成功", IconOption.笑脸);
                        } else {
                            layerCommon.msg("禁用失败", IconOption.哭脸);
                        }
                        return false;
                    }, error: function () { layerCommon.msg("禁用失败", IconOption.哭脸); return false; }
                })
            } else {
                //单个代理商
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx", //type2=0 属于禁用
                    data: { ck: Math.random(), action: "IsEnabledGoodsPrice", compId: compId, disid: disidlist, type2: 0 },
                    dataType: "text",
                    success: function (data) {
                        if (data != "") {
                            $(".tools .toolbar li").eq(3).addClass("none"); //禁用隐藏
                            $(".tools .toolbar li").eq(2).removeClass("none"); //启用显示
                            layerCommon.msg("禁用成功", IconOption.笑脸);
                        } else {
                            layerCommon.msg("禁用失败", IconOption.哭脸);
                        }
                        return false;
                    }, error: function () { layerCommon.msg("禁用失败", IconOption.哭脸); return false; }
                })
            }
        }
    })
    //商品名称或者编码获取焦点事件
    $(document).on("keyup", ".txtGoods", function () {
        BindGoods(this); //绑定下拉商品
    })
    $(document).on("focus", ".txtGoods", function () {
        BindGoods(this); //绑定下拉商品
    })
    var t = "";
    $(document).on("blur", ".txtGoods", function () {
        clearTimeout(t);
        var inindex = $(this).parent().parent().parent().index(); //所选行的索引
        t = setTimeout("$(\".tabLine table tbody tr\").eq(" + inindex + ").find(\".search-opt\").addClass(\"none\")", 200);        //隐藏list
    })
    //选中商品
    $(document).on("click", ".search-opt .list li", function () {
        var inindex = $(this).parent().parent().parent().parent().parent().index(); //所选行的索引
        var goodsinfoId = $(this).attr("tip"); //商品goodsinfoId
        var bol = false;
        $(".tabLine table tbody tr").each(function (indexss, objs) {
            if ($(".tabLine table tbody tr").eq([indexss]).attr("tip") != undefined) {
                if (goodsinfoId == $(".tabLine table tbody tr").eq([indexss]).attr("tip")) {//根据当前行的goodsinfoid去匹配是否存在相同的商品
                    bol = true;
                    return false;
                } else {
                    bol = false;
                }
            }
        })
        if (bol) { layerCommon.msg("商品已存在", IconOption.哭脸); return false; }
        //绑定选中的商品
        $.ajax({
            type: "post",
            url: "/Handler/GetPrice.ashx",
            data: { ck: Math.random(), action: "selectgoods", compId: compId, goodsInfoId: goodsinfoId },
            dataType: "json",
            success: function (data) {
                $(data).each(function (index, obj) {
                    $(".tabLine tbody tr").eq(inindex).replaceWith("<tr tip=\"" + obj.goodsinfoid + "\"><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><img src=\"" + imgurl + "" + obj.Pic + "\" width=\"60\" height=\"60\" style=\"float:left\"><div class=\"search\"><div class=\"text\"><span class=\"divname\">" + GetGoodsName(obj.GoodsName, "", 1) + "</span><span class=\"divcode\">" + GetGoodsName(obj.BarCode, "", 1) + "</span></div><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc divinfo\">" + obj.ValueInfo + "</div></td><td><div class=\"tc divunit\">" + obj.Unit + "</div></td><td><div class=\"tc \">" + obj.SalePrice + "</div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice\" value=\"" + parseFloat(obj.TinkerPrice).toFixed(2) + "\" onfocus=\"InputFocus(this)\"  onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>");
                })
            }, error: function () { }
        })
    })
    //增加行
    $(document).on("click", ".minus2", function () {
        //动态加载一行html
        $(".tabLine tbody").append("<tr><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><div class=\"search\"><input name=\"txtGoods\" type=\"text\" class=\"box txtGoods\"><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice none\" value=\"\" onfocus=\"InputFocus(this)\"       onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>");
    })
    //删除行
    $(document).on("click", ".add2", function () {
        var num = $(".tabLine tbody tr").length;
        if (num == 1) {//删除最后一行的时候直接替换成空的html
            $(".tabLine tbody").html("<tr><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><div class=\"search\"><input name=\"txtGoods\" type=\"text\" class=\"box txtGoods\"><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice none\" value=\"\" onfocus=\"InputFocus(this)\"       onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>");
        } else {
            $(this).parent().parent().parent().remove(); //删除当前行
        }
    })
    //多选商品
    $(document).on("click", ".tabLine tbody tr .text", function () {
        var indexs = $(this).parent().parent().parent().index(); //所选行的索引
        var goodsInfoId = $(this).parent().parent().parent().attr("tip"); //所选行的商品id
        var goodsInfoIdList = ""; //拼接每行的商品id用于排除查询
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("tip") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        if (goodsInfoId != undefined) {
            gengduo(indexs, goodsInfoId, goodsInfoIdList); //（索引，当前行的商品id，每一行的商品id拼接）
        } else {
            gengduo(indexs, "", ""); //（索引，当前行的商品id，每一行的商品id拼接）
        }
    })
    //更多商品2
    $(document).on("click", ".tabLine .opt-i", function () {
        var indexs = $(this).parent().parent().index(); //所选行的索引
        var goodsInfoIdList = ""; //拼接每行的商品id用于排除查询
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("tip") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList); //（索引，当前行的商品id，每一行的商品id拼接）
    })
    //更多商品1
    $(document).on("click", ".tabLine .search-opt .opt", function () {
        var indexs = $(this).parent().parent().parent().parent().index(); //所选行的索引
        var goodsInfoIdList = ""; //拼接每行的商品id用于排除查询
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("tip") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList); //（索引，当前行的商品id，每一行的商品id拼接）
    })
    //清空筛选条件
    $(".btnqk").click(function () {
        $(".txtDis").val(""); //代理商名称或者编码
        $(".hid_TypeId").val(""); //代理商分类id
        $(".txt_txtTypename ").val(""); //代理商分类
        //   GtDisBind(""); //加载top10个代理商
    })
})
//更多商品 （索引，当前行的商品id，每一行的商品id拼接）
function gengduo(indexs, goodsInfoId, goodsInfoIdList) {
    var compId = $("#hidCompId").val();
    var index = layerCommon.openWindow("选择商品", "selectgoods.aspx?CompId=" + compId + "&index=" + indexs + "&goodsInfoId=" + goodsInfoId + "&goodsInfoIdList=" + goodsInfoIdList, "985px", "630px");  //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}
//关闭弹出窗口
function CloseGoods() {
    layerCommon.layerClose("hid_Alert");
}
//个体代理商绑定
function GtDisBind(dis) {
    var compId = $(".hidCompId").val(); //厂商id
    $.ajax({
        type: "post",
        url: "/Handler/GetPrice.ashx",
        data: { ck: Math.random(), action: "selectDis", compId: compId, objDis: dis },
        dataType: "json",
        success: function (data) {
            if (data != "") {
                var html = "";
                $(data).each(function (index, obj) {
                    html += "<a href=\"javascript:;\" tips=\"" + obj.ID + "\">" + obj.DisName + "</a>";
                })
                if (html != "") {
                    $(".dis-filter li").eq(4).find(".nr").html(html);
                } else {
                    $(".dis-filter li").eq(4).find(".nr").html("<span style=\" height: 24px;\">&nbsp;&nbsp;暂无数据</span>");
                }
            } else {
                $(".dis-filter li").eq(4).find(".nr").html("<span style=\" height: 24px;\">&nbsp;&nbsp;暂无数据</span>");
            }
        }, complete: function () {
            if (dis == "") {
                $(".dis-filter li").eq(4).find("a").eq(0).trigger("click"); //默认选择第一个代理商
            }
        }, error: function () { }
    })
}
//绑定下拉商品
function BindGoods(thiss) {
    var str = $.trim($(thiss).val()); //商品名称或者编码
    var inindex = $(thiss).parent().parent().parent().index(); //所在行的索引
    $(".tabLine table tbody tr").eq(inindex).find(".search-opt").removeClass("none"); //显示list
    var hid = eval('(' + $(".divGoodsName").text() + ')'); //隐藏加载的商品
    $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").remove(); //清空list
    var html = "<ul class=\"list\">";
    $(hid).each(function (index, obj) {
        if (index < 5) {
            if (str != "") {
                if (obj.GoodsName.indexOf(str) != -1 || obj.BarCode.toLocaleLowerCase().indexOf(str.toLocaleLowerCase()) != -1) {
                    html += "<li tip=\"" + obj.goodsinfoid + "\">" + GetGoodsName(obj.BarCode + obj.GoodsName, "", 1) + "<i>" + GetGoodsName(obj.BarCode + obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></li>";
                }
            } else {
                html += "<li tip=\"" + obj.goodsinfoid + "\">" + GetGoodsName(obj.BarCode + obj.GoodsName, "", 1) + "<i>" + GetGoodsName(obj.BarCode + obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></li>";
            }
        }
    })
    html + "</ul>";
    $(".tabLine table tbody tr").eq(inindex).find(".opt").before(html);
}
//绑定代理商分类 （当前，厂商id,父级id,所在等级、区域行的li 索引,区分代理商区域、等级）
function BindDis(thiss, compId, parentId, ind, action) {
    $(thiss).addClass("cur").siblings().removeClass("cur");
    $.ajax({
        type: "post",
        url: "/Handler/GetPrice.ashx",
        data: { ck: Math.random(), action: action, compId: compId, parentId: parentId },
        dataType: "json",
        success: function (data) {
            var html = "";
            if (data != "") {
                $(data).each(function (index, obj) {
                    if (action == "getDisType") {
                        html += "<a href=\"javascript:;\" tip=\"" + obj.ID + "\">" + obj.TypeName + "</a>";
                    } else {
                        html += "<a href=\"javascript:;\" tip=\"" + obj.ID + "\">" + obj.AreaName + "</a>";
                    }
                })

                if (html != "") {
                    $(".dis-filter li").eq(ind).find(".nr").html(html);
                    $(".dis-filter li:gt(" + ind + ")").find(".nr").html("<span style=\" height: 24px;\"></span>");
                } else {
                    $(".dis-filter li").eq(ind).find(".nr").html("<span style=\" height: 24px;\"></span>");
                }
            } else {
                $(".dis-filter li:gt(" + ind + ")").find(".nr").html("<span style=\" height: 24px;\"></span>");
                $(".dis-filter li").eq(ind).find(".nr").html("<span style=\" height: 24px;\"></span>");
            }

        }, complete: function () {
            var parentidlist = "";
            //判断是否选中具体的范围或者代理商进行调价
            $(".dis-filter a").each(function (index, obj) {
                if ($(this).attr("class") == "cur") {
                    if ($(this).attr("tip") != undefined) {
                        parentidlist += $(this).attr("tip") + ",";
                    }
                }
            })
            if (parentidlist != "") {
                parentidlist = parentidlist.substring(0, parentidlist.length - 1);
                $.ajax({
                    type: "post",
                    url: "/Handler/GetPrice.ashx",
                    data: { ck: Math.random(), action: "bindgoods", compId: compId, parentidlist: parentidlist, type: action },
                    dataType: "json",
                    success: function (data) {
                        var html = "";
                        $(data).each(function (index, obj) {
                            //                            if (obj.isenabled == 0) {//禁用的时候
                            //                                $(".tools .toolbar li").eq(2).removeClass("none"); //启用显示
                            //                                $(".tools .toolbar li").eq(3).addClass("none"); //禁用隐藏
                            //                            } else {
                            //                                $(".tools .toolbar li").eq(2).addClass("none"); //启用隐藏
                            //                                $(".tools .toolbar li").eq(3).removeClass("none"); //禁用显示
                            //                            }
                            html += "<tr tip=\"" + obj.goodsinfoid + "\"><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"> <img src=\"" + imgurl + "" + obj.Pic + "\" width=\"60\" height=\"60\" style=\"float:left\"><div class=\"search\"><div class=\"text\"><span class=\"divname\">" + GetGoodsName(obj.GoodsName, "", 1) + "</span><span class=\"divcode\">" + GetGoodsName(obj.BarCode, "", 1) + "</span></div><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc divinfo\">" + obj.ValueInfo + "</div></td><td><div class=\"tc divunit\">" + obj.Unit + "</div></td><td><div class=\"tc\">" + obj.SalePrice + "</div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice\" value=\"" + parseFloat(obj.TinkerPrice).toFixed(2) + "\" onfocus=\"InputFocus(this)\"  onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>";
                        })
                        $(".tabLine tbody").html(html);
                        // IsChkGoods(compId, (action == "getDisType" ? 1 : 2));

                    }, error: function () {
                        Qingk(); //默认初始状态
                    }
                })
            } else {
                Qingk(); //默认初始状态
            }
        }, error: function () { }
    })
}
//单个代理商商品列表的绑定
function BindDiss(compId, disid) {
    $.ajax({
        type: "post",
        url: "/Handler/GetPrice.ashx",
        data: { ck: Math.random(), action: "bindgoodss", compId: compId, disid: disid },
        dataType: "json",
        success: function (data) {
            var html = "";
            $(data).each(function (index, obj) {
                //                if (obj.IsEnabled == 0) {
                //                    $(".tools .toolbar li").eq(2).removeClass("none");
                //                    $(".tools .toolbar li").eq(3).addClass("none");
                //                } else {
                //                    $(".tools .toolbar li").eq(2).addClass("none");
                //                    $(".tools .toolbar li").eq(3).removeClass("none");
                //                }
                if (obj.GoodsName != "") {
                    html += "<tr tip=\"" + obj.GoodsInfoID + "\"><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><img src=\"" + imgurl + "" + obj.Pic + "\" width=\"60\" height=\"60\" style=\"float:left\"><div class=\"search\"><div class=\"text\"><span class=\"divname\">" + GetGoodsName(obj.GoodsName, "", 1) + "</span><span class=\"divcode\">" + GetGoodsName(obj.BarCode, "", 1) + "</span></div><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc divinfo\">" + obj.InfoValue + "</div></td><td><div class=\"tc divunit\">" + obj.Unit + "</div></td><td><div class=\"tc\">" + obj.SalePrice + "</div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice\" value=\"" + parseFloat(obj.TinkerPrice).toFixed(2) + "\" onfocus=\"InputFocus(this)\"  onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>";
                }
            })
            if (html != "") {
                $(".tabLine tbody").html(html);
            } else {
                Qingk(); //默认初始状态
            }
        }, error: function () {
            Qingk(); //默认初始状态
        }
    })
}
//绑定数据（商品id，厂商id,所在行的索引）
function GoodsList(goodsInfoId, compId, inindex) {
    $.ajax({
        type: "post",
        url: "/Handler/GetPrice.ashx",
        data: { ck: Math.random(), action: "selectgoods", compId: compId, goodsInfoId: goodsInfoId },
        dataType: "json",
        success: function (data) {
            var html = "";
            $(data).each(function (index, obj) {
                html += "<tr tip=\"" + obj.goodsinfoid + "\"><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><img src=\"" + imgurl + "" + obj.Pic + "\" width=\"60\" height=\"60\" style=\"float:left\"><div class=\"search\"><div class=\"text\"><span class=\"divname\">" + GetGoodsName(obj.GoodsName, "", 1) + "</span><span class=\"divcode\">" + GetGoodsName(obj.BarCode, "", 1) + "</span></div><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc divinfo\">" + obj.ValueInfo + "</div></td><td><div class=\"tc divunit\">" + obj.Unit + "</div></td><td><div class=\"tc\">" + obj.SalePrice + "</div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice\" value=\"" + parseFloat(obj.TinkerPrice).toFixed(2) + "\" onfocus=\"InputFocus(this)\"  onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>";
            })
            $(".tabLine tbody tr").eq(inindex).replaceWith(html);
        }, error: function () { }
    })
}
//默认初始状态
function Qingk() {
    // $(".tools .toolbar li").eq(2).addClass("none"); //启用隐藏
    // $(".tools .toolbar li").eq(3).addClass("none"); //禁用隐藏
    $(".tabLine tbody").html("<tr><td><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td class=\"c1\"><div class=\"search\"><input name=\"txtGoods\" type=\"text\" class=\"box txtGoods\"><div class=\"search-opt none\"><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt-i\"></i>选择商品</a></div></div></div><a class=\"opt-i\"></a></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"tc\"></div></td><td><div class=\"search\"><input name=\"txtDisPrice\" type=\"text\" class=\"box box2 txtDisPrice none\" value=\"\" onfocus=\"InputFocus(this)\"      onkeyup=\"KeyInt2(this)\" maxlength=\"9\"></div></td></tr>"); //清空商品绑定列表
}
//截取字符串
//商品名称，属性值，是否需要截取
function GetGoodsName(goodsName, valueInfo, type) {
    var str = "";
    var str2 = "";
    if (valueInfo != "") {
        str2 = valueInfo.toString().substring(0, valueInfo.length - 1).toString().replace('；', ',');
    } else {
        str2 = valueInfo;
    }
    str = goodsName + "&nbsp;" + str2;
    if (type == "1") {
        if (str.length > 38) {
            str = str.substring(0, 38) + "...";
        }
    }
    return str;
};