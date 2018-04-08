function RemoveList() {
    $(".pullDown2").removeClass("xy");
    $(".pullDown2").hide();
    $("#leftClick").remove();
}
//分类绑定
function Bind(can, can2, can3, can4) {
    $(".pullDown2").removeClass("xy");
    var count = $("." + can + " li").length;
    if ($.trim($("." + can).attr("class")) == can + " pullDown2 xy" || count == 0) {
        $("." + can).hide();
        $("." + can).removeClass("xy");
    } else {
        $("." + can).show();
        $("." + can).addClass("xy");
        $("." + can).css("display", "block");
    }
    var x = $(can2).offset().left;
    var y = $(can2).offset().top;
    $("." + can).css("position", "absolute");
    $("." + can).css("left", x - 5 + "px");
    $("." + can).css("top", y + 30 + "px");
    $(document.body).append("<div id='leftClick' style='height: 100%;width: 100%; position: absolute; z-index: 999998' onclick='RemoveList()'></div>");

    if ($.trim($("." + can).attr("class")) == can + " pullDown2 xy" || count == 0) {
        var ind = $(".tablelist").find("#" + can4).parent().parent().index();
        if (can == "categoryYi") {
            Trigger($(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val(), 0, "categoryYi");

        } else if (can == "categoryEr") {
            Trigger($(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val(), $(".hidCategoryYi" + parseInt(parseInt(ind) + 1)).val(), "categoryEr");

        } else if (can == "categorySan") {
            Trigger($(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val(), $(".hidCategoryEr" + parseInt(parseInt(ind) + 1)).val(), "categorySan");
        } else if (can == "unit") {
            $(".unit .addBtn").attr("tip", can3);
        }
    }
    $("." + can + " li").attr("tips", can3);
}
//下拉选项绑定
function Trigger(can, can2, can3) {
    if ($.trim(can) != "") {
        $.ajax({
            type: "post",
            url: "GoodsAdd2.aspx",
            data: { ck: Math.random(), action: "trigger", goodsTypeId: can, parentId: can2 },
            dataType: "Json",
            async: false,
            success: function (data) {
                var json = eval(data);
                if (data != "") {
                    var html = "";
                    $(json).each(function (index, obj) {
                        if (obj.CategoryName != undefined) {
                            var ss = "";
                            if (obj.CategoryName.toString().length > 6) {
                                ss = obj.CategoryName.substring(0, 6) + "...";
                            } else {
                                ss = obj.CategoryName;
                            }
                            html += "<li tip='" + obj.ID + "' title='" + obj.CategoryName + "' ><a href=\"javascript:;\">" + ss + "</a></li>";
                        }
                    })
                    $("." + can3 + " .list").html(html);
                } else {
                    $("." + can3 + " .list").html(data);
                }
            }
        })
    } else {
        $(".pullDown2").hide();
    }
}
//验证最小的分类
function ChkMinCategory(can, can2) {
    var id = "";
    var hidCategoryYi = $(".hidCategoryYi" + can).val();
    var hidCategoryEr = $(".hidCategoryEr" + can).val();
    var hidCategorySan = $(".hidCategorySan" + can).val();
    if (hidCategoryYi != "") {
        id = hidCategoryYi;
    }
    if (hidCategoryEr != "") {
        id = hidCategoryEr;
    }
    if (hidCategorySan != "") {
        id = hidCategorySan;
    }
    var str = "";
    if (id != "") {
        $.ajax({
            type: "post",
            url: "../TreeDemo.aspx",
            data: { ck: Math.random(), action: "yanz", id: id },
            dataType: "text",
            success: function (data) {
                if (data == "y") {
                    alert("请选择最小的分类");
                    $("." + can2).val("");
                    $("." + can2).focus();
                }
            }, error: function () { }
        })
    }
}
//商品图片上传层
function btnShowGoodsPic(index) {
    //    $("#hidIndex").val(index);
    //    $(".tip").fadeIn(200);
    //    $(".Layer").fadeIn(200);
    var height = document.documentElement.clientHeight; //计算高度
    var layerOffsetY = (height - 340) / 2; //计算宽度
    var index = showDialog('上传图片', 'Jcrop.aspx?picfile=&index=' + index, '818px', '483px', layerOffsetY); //记录弹出对象
    $("#hid_Alert").val(index); //记录弹出对象
}
//验证
function formCheck() {
    var jsonlist = "{\"Goods\": [";
    var json = ""; var json2 = ""; var json3 = ""; var json4 = "";
    var count = $(".tablelist >tbody").children("tr").length; //有多少行数据
    var z = 0; var x = 0; var bol = false;
    for (var i = 0; i < count; i++) {
        if ($.trim($(".tablelist >tbody").children("tr").eq(i).attr("style")) == "display: none;") {
            continue;
        }
        json = "";
        json2 = "\"GoodsAttrValues\": [";
        json3 = " \"GoodsInfo\": [";
        json4 = "";
        x = parseInt(i * 8 + 1);
        var goodstype = $.trim($(".TextBox" + x).val()); //商品分类
        var categoryyi = $.trim($(".TextBox" + parseInt(parseInt(x) + 1)).val()); //一级分类
        var categoryer = $.trim($(".TextBox" + parseInt(parseInt(x) + 2)).val()); //二级分类
        var categorysan = $.trim($(".TextBox" + parseInt(parseInt(x) + 3)).val()); //三级分类
        var goodsname = $.trim($(".TextBox" + parseInt(parseInt(x) + 4)).val()); //商品上架名称
        var pic2 = $.trim($("#Img" + parseInt(parseInt(i) + 1)).attr("src"));
        var unit = $.trim($(".TextBox" + parseInt(parseInt(x) + 6)).val()); //计量单位
        var detail = $.trim($(".TextBox" + parseInt(parseInt(x) + 7)).val()); //描述
        if (goodstype == "" || categoryyi == "") {
            z++;
        } else {
            if (goodsname == "") {
                alert("请填写第" + parseInt(parseInt(i) + 1) + "行的商品上架名称");
                bol = true;
                break;
            } else {
                json += "{\"GoodsName\": \"" + goodsname + "\",";
                var boll = false;
                for (var xy = 0; xy < i; xy++) {
                    var str = parseInt(parseInt(xy * 8) + 5);
                    if ($(".TextBox" + str).val() == goodsname) {
                        boll = true;
                        break;
                    }
                }
                if (boll) {
                    alert("商品上架名称有相同数据,请检查");
                    bol = true;
                    break;
                }
            }
            var CategoryID = "";
            var hidCategoryYi = $(".hidCategoryYi" + parseInt(parseInt(i) + 1)).val();
            var hidCategoryEr = $(".hidCategoryEr" + parseInt(parseInt(i) + 1)).val();
            var hidCategorySan = $(".hidCategorySan" + parseInt(parseInt(i) + 1)).val();
            if (hidCategoryYi != "") {
                CategoryID = hidCategoryYi;
            }
            if (hidCategoryEr != "") {
                CategoryID = hidCategoryEr;
            }
            if (hidCategorySan != "") {
                CategoryID = hidCategorySan;
            }
            json += "\"CategoryID\": \"" + CategoryID + "\",";
            if (unit == "") {
                alert("请填写第" + parseInt(parseInt(i) + 1) + "行的计量单位");
                bol = true;
                break;
            } else {
                json += "\"Unit\": \"" + unit + "\",";
            }
            var price = false;
            var pricelist = "";
            $(".lblPricelist" + parseInt(parseInt(i) + 1) + " .txtPrices").each(function (index, obj) {
                if ($.trim($(this).val()) == "") {
                    price = true;
                    return false;
                } else {
                    if (index == 0) {
                        json += "\"SalePrice\": \"" + $.trim($(this).val()) + "\",";
                    }
                    pricelist += $.trim($(this).val()) + ",";
                }
            })
            if (price) {
                alert("请填写第" + parseInt(parseInt(i) + 1) + "行所有的价格");
                bol = true;
                break;
            }

            if (pic2 != "") {
                pic2 = pic2.substring(parseInt(parseInt(pic2.lastIndexOf("/")) + 1)); //图片
                json += "\"Pic2\": \"" + pic2 + "\",";
            }
            json += "\"details\": \"" + detail + "\",";
            var str2 = new Array();
            var ii = 0;
            $(".divheight" + parseInt(parseInt(i) + 1) + " table input[type=checkbox]:checked").each(function (index, obj) {
                //规格属性值
                json2 += " {\"ValuesID\": \"" + $(this).attr("id") + "\"},"
                if (str2.length != 0) {
                    var bool = false;
                    for (var f = 0; f < str2.length; f++) {
                        if (str2[f] == $(this).parent().prev().text()) {
                            bool = true;
                            break;
                        }
                    }
                    if (!bool) {
                        str2[ii] = $(this).parent().prev().text(); //规格属性
                        ii++;
                    }
                } else {
                    str2[ii] = $(this).parent().prev().text();
                    ii++;
                }
            });
            if (json2 != "\"GoodsAttrValues\": [") {
                json2 = json2.substring(0, json2.length - 1) + "],";
            } else {
                json2 = json2 + "],";
            }

            var attrvalue = new Array();
            var count2 = $(".lblPricelist" + parseInt(parseInt(i) + 1) + " input[name='hideAttrValue']").length;
            if (count2 > 1) {
                for (var y = 0; y < count2; y++) {
                    var strlist = "";
                    json4 += "{";
                    attrvalue[y] = $($(".lblPricelist" + parseInt(parseInt(i) + 1) + " input[name='hideAttrValue']")[y]).val(); //属性值
                    for (var s = 1; s <= attrvalue[y].split("/").length; s++) {
                        if (attrvalue[y].split("/")[s - 1] != "") {
                            json4 += "\"Value" + s + "\": \"" + attrvalue[y].split("/")[s - 1] + "\",";
                            if (str2.length != 0) {
                                strlist += str2[s - 1].replace("：", ":") + attrvalue[y].split("/")[s - 1] + "；";
                            }
                        }
                    }
                    json4 += "\"ValueInfo\": \"" + strlist + "\",";
                    json4 += "\"SalePrice\": \"" + pricelist.split(",")[y] + "\"},";
                }
            } else {
                json4 += "{\"SalePrice\": \"" + pricelist.split(",")[0] + "\"},";
            }
            if (json4 != "") {
                json4 = json4.substring(0, json4.length - 1);
            }
            json4 = json4 + "]";
            jsonlist = jsonlist + json + json2 + json3 + json4 + "},";
        }
    }
    if (jsonlist != "{\"Goods\": [") {
        jsonlist = jsonlist.substring(0, jsonlist.length - 1) + "]}";
    }
    if (bol) {
        return false;
    } else {
        if (z == count) {
            alert("没有一条完成的数据，请填写完整");
            return false;
        } else {
            //alert("验证已通过");
            $.ajax({
                type: "post",
                url: "goodsadd2.aspx",
                data: { ck: Math.random(), action: "insert", json: jsonlist },
                dataType: "text",
                success: function (data) {
                    alert(data);
                    location = location;
                }, error: function () {
                    alert("数据有误,请检查");
                }
            })
            return false;
        }
    }
}
//排序
function Sort() {
    var trcount = $(".tablelist> tbody").children("tr").length;
    var z = 0;
    for (var i = 0; i < trcount; i++) {
        if ($(".tablelist >tbody").children("tr").eq(i).attr("style") != "display: none;") {
            z++;
            $(".tablelist> tbody").children("tr").eq(i).find("td:first").html(z);
        }
    }
}
//删除一行
function Del(can) {
    var trcount = $(".tablelist> tbody").children("tr").length;
    var z = 0;
    for (var i = 0; i < trcount; i++) {
        if ($(".tablelist >tbody").children("tr").eq(i).attr("style") != "display: none;") {
            z++;
        }
    }
    if (z == "1") {
        alert("只有一行了,不能删除");
        return false;
    } else {
        var str = can.substring(6);
        var count = $(".tablelist >tbody").children("tr").eq(parseInt(parseInt(str) - 1)).find(".textBox").length;
        var xy = 0;
        for (var i = 0; i < count; i++) {
            if ($.trim($(".tablelist >tbody").children("tr").eq(parseInt(parseInt(str) - 1)).find(".textBox").eq(i).val()) != "") {
                xy++;
                break;
            }
        }
        var pic = $("#Img" + parseInt(parseInt(str) - 1)).attr("src");
        if (xy != 0 || pic != "../../images/havenopicsmallest.gif") {
            if (confirm("确定删除？")) {
                $(".tablelist >tbody").children("tr").eq(parseInt(parseInt(str) - 1)).hide();
            }
        } else {
            $(".tablelist >tbody").children("tr").eq(parseInt(parseInt(str) - 1)).hide();
        }
    }
    Sort();
}
//新增一行

function addGoods() {
    var trcount = $(".tablelist> tbody").children("tr").length;
    var zcount = 0;
    var zindex = 0;
    for (var i = 0; i < trcount; i++) {
        if ($(".tablelist >tbody").children("tr").eq(i).attr("style") != "display: none;") {
            zcount++;
            zindex = $.trim($(".tablelist >tbody").children("tr").eq(i).find("td:first").html());
        }
    }
    if (zcount <= 9) {
        var rowscount = parseInt(parseInt(trcount) + 1);

        var i = parseInt(trcount * 8 + 1);
        var html = "<tr><td>" + parseInt(parseInt(zindex) + 1) + "</td>                                                                                                     "
             + "       <td><input ID=\"TextBox" + i + "\" type=\"text\" Class=\"textBox TextBox" + i + "\"    "
             + "       OnClick=\"Bind('goodsType',this,'TextBox" + i + "','TextBox" + i + "')\"               "
             + "       ReadOnly=\"true\" style=\"width:110px;\"  /></td>                                                                          " + "       <td><input ID=\"TextBox" + parseInt(parseInt(i) + 1) + "\" type=\"text\" Class=\"textBox TextBox" + parseInt(parseInt(i) + 1) + "\"    "
             + "       OnClick=\"Bind('categoryYi',this,'TextBox" + parseInt(parseInt(i) + 1) + "','TextBox" + i + "')\"                                    " + "       ReadOnly=\"true\" style=\"width:110px;\"/></td>                                                              "
             + "       <td><input ID=\"TextBox" + parseInt(parseInt(i) + 2) + "\" type=\"text\" Class=\"textBox TextBox" + parseInt(parseInt(i) + 2) + "\"    "
             + "       OnClick=\"Bind('categoryEr',this,'TextBox" + parseInt(parseInt(i) + 2) + "','TextBox" + i + "')\"              "
             + "       ReadOnly=\"true\" style=\"width:110px;\"/></td>                                                              "
             + "       <td><input ID=\"TextBox" + parseInt(parseInt(i) + 3) + "\" type=\"text\" Class=\"textBox TextBox" + parseInt(parseInt(i) + 3) + "\"    "
             + "       OnClick=\"Bind('categorySan',this,'TextBox" + parseInt(parseInt(i) + 3) + "','TextBox" + i + "')\"             "
             + "         ReadOnly=\"true\" style=\"width:110px;\" /></td>                                                            "
             + "     <td><div style=\"margin-left: 5px; margin-top: 0px; float: left; width: 400px\" class=\"divheight" + rowscount + "\"></div></td> "
             + "     <td><input ID=\"TextBox" + parseInt(parseInt(i) + 4) + "\" type=\"text\"                                                           "
             + "        Class=\"textBox TextBox" + parseInt(parseInt(i) + 4) + "\"  MaxLength=\"60\" Style=\"width: 250px\"                                                      "
             + "        onchange=\"ChkMinCategory('" + rowscount + "','TextBox" + parseInt(parseInt(i) + 4) + "')\"/></td>                 "
             + "     <td><asp:Label ID=\"lblPricelist" + rowscount + "\"                                                                              "
             + "        Class=\"lblPricelist" + rowscount + " lblPricelist\" Style=\"width: 353px; float: left;\" Text=\"\">                       "
             + "         <input name=\"txtPrices\" type=\"text\" id=\"TextBox" + parseInt(parseInt(i) + 5) + "\"                                        "
             + "   style=\"width: 60px;\"  class=\"textBox txtPricess txtPrices\" onkeyup=\"KeyInt2(this);\" /></asp:Label></td>                 " + "     <td><input ID=\"TextBox" + parseInt(parseInt(i) + 6) + "\" Class=\"textBox TextBox" + parseInt(parseInt(i) + 6) + "\"           "
             + "        OnClick=\"Bind('unit',this,'TextBox" + parseInt(parseInt(i) + 6) + "','TextBox" + i + "')\"                   "
             + "         ReadOnly=\"true\" style=\"width:60px;\" type=\"text\"/></td>                                                             "
             + "     <td><a class=\"tooltip tooltip" + rowscount + "\" href='javascript:;' style=\"display: inline-block;\">                          "
             + "         <img id='Img" + rowscount + "' class='pic' alt=\"暂无\" title=\"图片上传\" onclick=\"btnShowGoodsPic(" + rowscount + ");\"  " + "  src=\"../../images/havenopicsmallest.gif\" /></a></td>   "
             + "     <td><input  ID=\"TextBox" + parseInt(parseInt(i) + 7) + "\"         "
             + "         class=\"textBox TextBox" + parseInt(parseInt(i) + 7) + "\"/></td>                                                 "
             + "     <td><img id=\"ImgDel" + rowscount + "\"  title=\"删除改行\" style=\"cursor: pointer\" src=\"../images/t03.png\"                 "
             + "   onclick=\"Del('ImgDel" + rowscount + "\')\" /></td> </tr>";
        $(".tablelist>tbody").children("tr:last").after(html);
    } else {
        alert("最多添加十行");
    }
}
$(function () {
    //取消
    $(".cancel,.tiptop a").click(function () {
        $(".tip").fadeOut(100);
        $(".Layer").fadeOut(100);
    });
    $(".tiptop").LockMove({ MoveWindow: "#DLodIMG" });
    //移动图片展示
    for (var i = 1; i <= 10; i++) {
        $("a.tooltip" + i).ImgAmplify();
    }

    if ($(".pullDown2 .list li").length > 6) {//超过6个单位计量，则出现滚轴
        $(".pullDown2 .list").css("height", "156px");
    }
    var infoIdList2 = "";
    var valuesList = "";
    //属性值选中出对应的价格框
    $(document).on("click", "table input[type=checkbox]", function () {
        var str = $(this).parents("div").attr("class"); //规格属性div的class，根据class获取tr索引
        if (str != undefined) {
            str = str.substring(9);
            AllPrice(str);
        }

    });
    //赋值goods表价格
    $(document).on("blur", ".lblPricelist1", function () {
        var price = $(this).find(".txtPrices").eq(0).val();
        if ($.trim(price) != "") {
            $(".txt_Price").val(price);
        }
    })
    //删除价格文本框
    $(document).on("click", ".lblPricelist img", function () {
        var inde = $(this).parent().parent().attr("id");
        if (inde != "" && inde != undefined) {
            inde = inde.substring(12);

            var infoId = $(this).prev().attr("tip"); //删除的goodsinfo表id
            if (infoId != undefined) {
                infoIdList2 += infoId + ",";
            }
            if (infoIdList2 != "") {
                $("#hideInfoIdList").val(infoIdList2);
            }
            $(this).parent().remove();
            if ($.trim($(".lblPricelist" + inde).html()) == "") {
                $(".lblPricelist" + inde).html("<input id=\"txtPricess\" class=\"textBox txtPrices\" type=\"text\" value=\"\" onkeyup=\"KeyInt2(this);\" style=\"width: 100px; \" name=\"txtPricess\">");
            }
            $(".divheight" + inde + " table input[type=checkbox]:checked").each(function (index, obj) {
                var str = $(this).next().text();
                var str2 = $(".lblPricelist" + inde + " span").text();
                var str3 = str2.split('/');
                var bol = false;
                for (var i = 0; i < str3.length; i++) {
                    if ($.trim(str3[i]) == str) {
                        bol = true;
                        break;
                    }
                }
                if (!bol) {
                    $(this).removeAttr("checked");
                    valuesList += $(this).val() + ",";
                    if (valuesList != "") {
                        $("#hideValuesList").val(valuesList); //删除的goodsvlaues表valuesID
                    }
                }
            })
        }
    })
    //商品大类
    $(document).on("click", ".goodsType .list li", function () {
        $(".categoryYi .list").html("");
        $(".categoryEr .list").html("");
        $(".categorySan .list").html("");
        var str = $(this).attr("tips"); //赋值文本框Id
        $("." + str).val($.trim($(this).text()));
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 1)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 2)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 3)).val("");
        $(".divheight" + parseInt(str.substring(7))).html("");

        var ind = $(".tablelist").find("#" + str).parent().parent().index();
        $(".hidCategoryYi" + parseInt(parseInt(ind) + 1)).val("");
        $(".hidCategoryEr" + parseInt(parseInt(ind) + 1)).val("");
        $(".hidCategorySan" + parseInt(parseInt(ind) + 1)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 4)).removeAttr("readonly");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 4)).val("");
        $(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val($.trim($(this).attr("tip")));
        $(".goodsType").hide();
        $(".goodsType").removeClass("xy");
        Trigger($.trim($(this).attr("tip")), 0, "categoryYi");
        RemoveList();
    })
    //一级分类
    $(document).on("click", ".categoryYi .list li", function () {
        $(".categoryEr .list").html("");
        $(".categorySan .list").html("");
        var str = $(this).attr("tips"); //赋值文本框Id
        $("." + str).val($.trim($(this).text()));
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 1)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 2)).val("");
        var ind = $(".tablelist").find("#" + str).parent().parent().index();
        $(".hidCategoryEr" + parseInt(parseInt(ind) + 1)).val("");
        $(".hidCategorySan" + parseInt(parseInt(ind) + 1)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 3)).removeAttr("readonly");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 3)).val("");
        $(".hidCategoryYi" + parseInt(parseInt(ind) + 1)).val($.trim($(this).attr("tip")));
        $(".categoryYi").hide();
        $(".categoryYi").removeClass("xy");
        Trigger($(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val(), $.trim($(this).attr("tip")), "categoryEr");
        GlAttribute($.trim($(this).attr("tip")), parseInt(parseInt(ind) + 1));
        RemoveList();
    })
    //二级分类
    $(document).on("click", ".categoryEr .list li", function () {
        $(".categorySan .list").html("");
        var str = $(this).attr("tips"); //赋值文本框Id
        $("." + str).val($.trim($(this).text()));
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 1)).val("");
        var ind = $(".tablelist").find("#" + str).parent().parent().index();
        $(".hidCategorySan" + parseInt(parseInt(ind) + 1)).val("");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 2)).removeAttr("readonly");
        $(".TextBox" + parseInt(parseInt(str.substring(7)) + 2)).val("");
        $(".hidCategoryEr" + parseInt(parseInt(ind) + 1)).val($.trim($(this).attr("tip")));
        $(".categoryEr").hide();
        $(".categoryEr").removeClass("xy");
        Trigger($(".hidGoodsType" + parseInt(parseInt(ind) + 1)).val(), $.trim($(this).attr("tip")), "categorySan");
        GlAttribute($.trim($(this).attr("tip")), parseInt(parseInt(ind) + 1));
        RemoveList();
    })
    //三级分类
    $(document).on("click", ".categorySan .list li", function () {
        var str = $(this).attr("tips"); //赋值文本框Id
        $("." + str).val($.trim($(this).text()));
        var ind = $(".tablelist").find("#" + str).parent().parent().index();
        $(".hidCategorySan" + parseInt(parseInt(ind) + 1)).val($.trim($(this).attr("tip")));
        $(".categorySan").hide();
        $(".categorySan").removeClass("xy");
        GlAttribute($.trim($(this).attr("tip")), parseInt(parseInt(ind) + 1));
        RemoveList();
    })
    //计量单位
    $(document).on("click", ".unit .list li", function () {
        var str = $(this).attr("tips"); //赋值文本框Id
        $("." + str).val($.trim($(this).text()));
        $(".unit").hide();
        $(".unit").removeClass("xy");
        RemoveList();
    })
})