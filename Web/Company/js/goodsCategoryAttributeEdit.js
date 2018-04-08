$(function () {
    var cateName = decodeURIComponent(request("cateName"));
    var cateId = request("cateId");
    $(".listNr .title").text("商品规格属性维护(新增)");
    if (cateName != "" && cateId != "") {
        $(".hideCategoryAttrID").val(cateId);
        $(".txtCategoryAttr").val(cateName);
        $(".txtCategoryAttr").attr("disabled", "disabled");
    }
    CategoryAttr(cateId);
    //    var myObj = {};
    /////////////////////////////////////////////////////规格属性////////////////////////////////////////////////
    //取消
    $(".cancel").click(function () {
        var compId = $(".hideComId").val();
        $(".divedit").html("");
        $(".txtAttributeName").val("");
        $(".txtAddValue").val("");
        $(".listNr .title").text("商品规格属性维护(新增)");
        // CategoryAttr(cateId);
        ShowParentAttr(cateId, compId);
        window.parent.CloseDialog("2");
    })
    //商品分类规格属性编辑
    $(document).on("click", ".menu2 .edit", function () {
        $(".lblAttribute").find("li").removeClass("ss");
        $(this).parent().parent().addClass("ss");
        $(".lblAttribute").find(".but").removeAttr("style")
        $(this).parent().css("display", "block");
        $(".listNr .title").text("商品规格属性维护(编辑)");
        $(".listNr .title").attr("tip", $(this).attr("tip"));
        //   var category = $.trim($(".lblCategory  .hover i").text()); //分类名称
        //    var categoryID = $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
        $(".hideCategoryAttrID").val(cateId);
        $(".txtCategoryAttr").val(cateName);
        $(".txtCategoryAttr").attr("disabled", "disabled");
        var id = $(this).attr("tips"); //分类属性id
        ShowAttributeValue(id);
    })
    //编辑属性值
    $(document).on("click", ".Save", function () {
        var id = $(this).attr("tip"); //属性值ID
        var AttributeID = $(this).attr("tips"); //属性ID
        var value = $("#txtAttrValue" + id).val(); //属性值
        $.ajax({
            type: "post",
            url: "GoodsCategoryAttributeEdit.aspx",
            data: { ck: Math.random(), action: "Save", id: id, value: value, categoryAttrId: AttributeID },
            dataType: "text",
            success: function (data) {
                if ($.trim(data) == "") {
                    alert("- 规格属性值保存失败。");
                }
                if ($.trim(data) == "ycz") {
                    alert("- 规格属性值已存在。");
                }
                if ($.trim(data) == "cg") {
                    alert("- 规格属性值保存成功。");
                }
                ShowAttributeValue(AttributeID);
            }, error: function () { }
        })
    })
    //禁用属性值
    $(document).on("click", ".Enable", function () {
        var id = $(this).attr("tip"); //属性值ID
        var AttributeID = $(this).attr("tips"); //属性ID
        var name = $("#txtAttrValue" + id).val();
        $.ajax({
            type: "post",
            url: "GoodsCategoryAttributeEdit.aspx",
            data: { ck: Math.random(), action: "Enable", id: id, name: name },
            dataType: "text",
            success: function (data) {
                if ($.trim(data) == "jycg") {//禁用
                    $("#txtAttrValue" + id).attr("disabled", "disabled");
                }
                if ($.trim(data) == "qycg") {//启用
                    $("#txtAttrValue" + id).removeAttr("disabled");
                }
                //                if ($.trim(data) == "bsy") {//禁用
                //                    alert("- 规格属性值被使用不能禁用。");
                //                }
                ShowAttributeValue(AttributeID);
            }, error: function () { }
        })
    })
    //删除属性值
    $(document).on("click", ".Dels", function () {
        var obj = $(this);
        if (confirm("确定删除规格属性值？")) {
            var id = obj.attr("tip"); //属性值ID
            var AttributeID = obj.attr("tips"); //属性ID
            var name = $("#txtAttrValue" + id).val();
            $.ajax({
                type: "post",
                url: "GoodsCategoryAttributeEdit.aspx",
                data: { ck: Math.random(), action: "Del", name: name, id: id },
                dataType: "text",
                success: function (data) {
                    if ($.trim(data) == "bsy") {
                        alert("- 规格属性值已被使用，不能删除。");
                    }
                    if ($.trim(data) == "") {
                        alert("- 规格属性值删除失败。");
                    }
                    ShowAttributeValue(AttributeID);
                }, error: function () { }
            })
        }
    })
    //删除属性
    $(document).on("click", ".lblAttribute li .del", function () {
        var obj = $(this);
        if (confirm("确定删除规格属性？")) {
            var id = obj.attr("tip"); //属性值ID
            var AttributeID = obj.attr("tips"); //属性ID
            var compId = $(".hideComId").val();
            var categoryID = $.trim(cateId);
            $.ajax({
                type: "post",
                url: "GoodsCategoryAttributeEdit.aspx",
                data: { ck: Math.random(), action: "DelCategoryAttr", id: id, categoryAttrid: AttributeID },
                dataType: "text",
                success: function (data) {
                    if ($.trim(data) == "bsy") {
                        alert("- 规格属性已被使用，不能删除。");
                    }
                    if ($.trim(data) == "") {
                        alert("- 规格属性删除失败。");
                    }
                    CategoryAttr(cateId);
                    ShowParentAttr(categoryID, compId);
                }, error: function () { }
            })
        }
    })
    //显示添加后的属性以及属性值
    //1代表编辑  ，2代表添加
    function ShowAddValue(type, id, attrName, value, categoryID) {
        $.ajax({
            type: "post",
            url: "GoodsCategoryAttributeEdit.aspx",
            data: { ck: Math.random(), action: "addValue", attrName: attrName, value: value, id: id, type: type, categoryid: categoryID },
            dataType: "text",
            async: false,
            success: function (data) {
                if (data != "") {
                    if (data == "ycz") {
                        alert("- 规格属性值已存在。");
                    } else if (data == "sxycz") {
                        alert("- 规格属性已存在。");
                    } else {
                        ShowAttributeValue(data); //商品分类属性ID
                        if (type == 2) {
                            $("#hideadd").val(data);
                            if ($(".divedit tr").length > 6) {
                                $(".addattr").css("overflow", "hidden");
                            }
                        }
                    }
                    $(".txtAddValue").val("");
                } else {
                    alert("出错了");
                }
            }, error: function () {
            }
        })
    }
    //商品规格属性新增、(添加)新增属性值
    $(".btnEdit2,.btnBg1").click(function () {
        var str = "";
        var attr = $.trim($(".txtAttributeName").val());
        var value = $.trim($(".txtAddValue").val());
        var compId = $(".hideComId").val();
        if (attr == "") {
            str = str + "- 规格属性不能为空。\r\n";
        }
        if (value == "") {
            str = str + "- 规格属性值不能为空。\r\n";
        }
        var categoryID = $.trim(cateId); // $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
        if ($(this).attr("class").indexOf("btnBg1") != -1) {
            if (str != "") {
                alert(str);
                return false;
            }
            //  var sort = $.trim($(".txtSortIndexs").val());
            ShowAddValue(2, "", attr, value, categoryID);
        } else {
            if (attr == "") {
                alert("- 规格属性不能为空。");
                return false;
            }
            var divhtml = $.trim($(".divedit").html());
            if (divhtml == "") {
                alert("- 规格属性值不能为空。");
                return false;
            }
            if ($.trim($(".listNr .title").text()) == "商品规格属性维护(编辑)") {
                var id = $.trim($(".listNr .title").attr("tip"));
                // var sort = $.trim($(".txtSortIndexs").val());
                ShowAddValue(1, id, attr, value, categoryID);
            }
            //  ShowParentAttr(categoryID, compId);
            //  CategoryAttr(cateId);
            // setTimeout(function () {
            $(".cancel").trigger("click");
            //  }, 500);
        }

    })
    //根据分类ID得出规格属性
    function CategoryAttr(id) {
        $.ajax({
            type: "post",
            url: "GoodsCategoryAttributeEdit.aspx",
            data: { ck: Math.random(), action: "SelectAttr", id: id },
            dataType: "text",
            success: function (datass) {
                $(".lblAttribute").html(datass);
            }, error: function () { }
        })
    }

    //显示属性值列表
    function ShowAttributeValue(id) {
        // $(".btnBg2").attr("tip", id); //隐藏当前属性的id，用于新增属性值
        $.ajax({
            type: "post",
            url: "GoodsCategoryAttributeEdit.aspx",
            data: { ck: Math.random(), action: "show", id: id },
            dataType: "json",
            async: false,
            success: function (data) {
                var html = "<table style=\"width:100%;\">";
                if (data != "") {
                    $(data).each(function (index, obj) {
                        $(".txtAttributeName").val(obj.AttributeName);
                        // $(".txtSortIndexs").val(obj.SortIndex);
                        if ($.trim(obj.AttrValue) != "") {
                            var strhtml = "";
                            if (obj.IsEnabled == "0") {
                                strhtml = "disabled=\"disabled\"";
                            }
                            html += "<tr><td style=\"width: 20px;\">" + parseInt(index + 1) + "、</td>" +
                                                               "<td styel=\"width: 190px; height:24px; vertical-align:middle; text-align:left;\">" +
                                                               "<input  style=\"width:100px\" type=\"text\" " + strhtml + " id=\"txtAttrValue" + obj.ID + "\"" +
                                    "class=\"textBox txtAttrValue\" value=\"" + obj.AttrValue + "\" tip=\"" + obj.ID + "\"/></td>" +
                                                               "<td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Save\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"保存规格属性值\" src=\"../images/icon_save.png\" style=\"cursor: pointer;\"" +
                                                               " /></td><td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Enable\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"禁用规格属性值\" src=\"../images/icon_enable.png\" style=\"cursor: pointer;\"" +
                                                               " /></td><td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Dels\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"删除规格属性值\" src=\"../images/icon_del.png\" style=\"cursor: pointer;\"" +
                                                               " /></td></tr>";
                        }
                    })
                }
                html += "</table>";
                $(".divedit").html(html);
            }, error: function () {

            }
        })
    }
    //刷新父页面
    function ShowParentAttr(categoryID, compId) {
        var url = window.parent.location.href;
        var goodsId = "";
        if (url.indexOf("=") != -1) {
            goodsId = url.substring(url.lastIndexOf("=") + 1);
        }
        //编辑
        if (goodsId == "") {
            $.ajax({
                type: "post",
                url: "../../Handler/GoodsAttr.ashx",
                data: { ck: Math.random(), action: "attr", id: categoryID, compId: compId },
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data != "") {
                        var html2 = "<input name=\"txtPricess\" type=\"text\" id=\"txtPricess\" style=\"width: 100px; \" class=\"textBox txtPricess\" onkeyup=\"KeyInt2(this);\" />";
                        window.parent.$(".lblPricelist").html(html2); //价格
                        if ($.trim(data.split("@")[0]) == "") {
                            window.parent.$("#lblAttr").text("该商品分类没有属性信息"); //属性
                        } else {
                            window.parent.$("#lblAttr").text(data.split("@")[0]); //属性
                        }
                        window.parent.$(".divheight").html(data.split("@")[1]); //属性值

                    }
                }, error: function () {

                }
            })
        } else {
            $.ajax({
                type: "post",
                url: "../../Handler/GoodsAttr.ashx",
                data: { ck: Math.random(), action: "attr2", id: categoryID, compId: compId, keyId: goodsId },
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data != "") {
                        if ($.trim(data.split("@")[0]) == "") {
                            window.parent.$("#lblAttr").text("该商品分类没有属性信息"); //属性
                        } else {
                            window.parent.$("#lblAttr").text(data.split("@")[0]); //属性
                        }
                        window.parent.$(".divheight").html(data.split("@")[1]); //属性值
//                        window.parent.$(".divheight .dh3 tr").each(function (index, obj) {
//                            if ($(this).find("input[type=checkbox]").attr("checked") != "checked") {
//                                window.parent.$(".divheight .dh3 tr").eq(index).hide();
//                                window.parent.$(".divheight .dh3 tr").eq(index).find("input").attr("name", "");
//                            }
//                        })

                    }
                }, error: function () {

                }
            })
        }
    }
})

//只能输入数字验证
function KeyInt(val) {
    val.value = val.value.replace(/[^\d]/g, '');
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