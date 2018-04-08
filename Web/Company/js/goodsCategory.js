$(function () {
    var myObj = "";
    //搜索商品类别文本框
    $(".txtCateName").focus(function () {
        var name = $.trim($(".txtCateName").val()); //商品类别文本框
        if (name == "搜索名称") {
            $(this).val("");
            return;
        }
    })
    $(".txtCateName").blur(function () {
        var name = $.trim($(".txtCateName").val()); //商品类别文本框
        if (name == "") {
            $(this).val("搜索名称");
            return;
        }
    })
    //商品类别搜索
    $(".txtCateName").keyup(function () {
        var name = $.trim($(".txtCateName").val()); //商品类别文本框
        if (name == "搜索名称") {
            name = "";
        }
        $(".menu .li i").each(function (index, obj) {
            var html = $.trim($(this).html());
            if (name != "") {
                if (html.indexOf(name) == -1) {
                    $(this).parentsUntil('.li').hide();
                } else {
                    $(this).parentsUntil('.li').show();
                }
            } else {
                $(this).parentsUntil('.li').show();
            }
        })
    })
    //商品类别一级新增
    $(".addCategoryBtn").click(function () {
        $(".tip .tiptop span").text("新增一级分类");
        $(".tip").fadeIn(200);
        $(".Layer").fadeIn(200);
        $(".trParent").hide();
        $(".tip").css("height", "200px");
        $(".tip").css("width", "530px");
        $(".txtCategoryName").val("");
        $(".hideCategoryId").val("");
        $(".ddlGoodsType").attr("disabled", false);
        $(".ddlGoodsType").change(function () {
            $(".hideGoodsType").val($.trim($(this).val())); //赋值大类
        })
        $(".ddlGoodsType").val("");
    })
    //商品类别二级新增
    $(document).on("click", ".addyi", function () {
        $(".lblCategory").find(".hover").removeClass("hover");
        $(this).parent().addClass("hover");
        $(".catediv").html("位置：" + $.trim($(this).find("i").text())); //规格属性列 标题显示
        var id = $(this).find(".edit").attr("tips"); //分类id
        CategoryAttr(id, 0);
    })
    //商品类别三级新增
    $(document).on("click", ".adder", function () {
        $(".lblCategory").find(".hover").removeClass("hover");
        $(this).addClass("hover");
        var str = $.trim($(this).parent().prevAll(".addyi").find("i").text()); //父级分类
        $(".catediv").html("位置：" + str + "&nbsp;>&nbsp;" + $.trim($(this).find("i").text())); //规格属性列 标题显示
        var id = $(this).find(".edit").attr("tips"); //分类id
        CategoryAttr(id, 0);
    })
    //商品类别三级选中
    $(document).on("click", "dd", function () {
        $(".lblCategory").find(".hover").removeClass("hover");
        $(this).addClass("hover");
        var str = $.trim($(this).parent().prevAll(".addyi").find("i").text()); //max父级分类
        var str2 = $.trim($(this).prevAll(".adder").find("i").text()); //父级分类
        $(".catediv").html("位置：" + str + "&nbsp;>&nbsp;" + str2 + "&nbsp;>&nbsp;" + $.trim($(this).find("i").text())); //规格属性列 标题显示
        var id = $(this).find(".edit").attr("tips"); //分类id
        CategoryAttr(id, 0);
    })

    //商品类型下级新增弹出层
    $(document).on("click", ".add", function () {
        var id = $(this).next(".edit").attr("tips"); //分类id
        CategoryAttr(id, 1);
        if ($.trim($(".lblAttribute li i").text()) != "该分类暂无规格属性" && $.trim($(".lblAttribute li i").text()) != "") {
            alert("- 该分类已绑定规格属性不能新增下级分类。");
            return;
        }
        if ($(this).parent().parent().attr("class").indexOf("adder") != -1) {
            $(".tip .tiptop span").text("新增三级分类");
        }
        if ($(this).parent().parent().attr("class").indexOf("addyi") != -1) {
            $(".tip .tiptop span").text("新增二级分类");
        }
        $(".txtCategoryName").val("");
        $(".tip").fadeIn(100);
        $(".Layer").fadeIn(100);
        $(".tip").css("height", "230px");
        var typeid = $.trim($(this).attr("tip")); //大类id
        var id = $.trim($(this).attr("tips")); //父类id
        $(".trParent").show();
        $(".hideCategoryId").val(id);
        var name = $.trim($(this).parent().prev("i").text());
        $(".lblCategoryName").val(name);
        $(".ddlGoodsType").val(typeid);
        $(".ddlGoodsType").attr("disabled", "disabled");
        $(".hideGoodsType").val(typeid);
    })
    //商品类型下级编辑弹出层
    $(document).on("click", ".menu .edit", function () {
        if ($(this).parent().parent()[0].tagName == "DD") {
            $(".tip .tiptop span").text("编辑三级分类");
        } else if ($(this).parent().parent()[0].tagName == "DT") {
            $(".tip .tiptop span").text("编辑二级分类");
        } else if ($(this).parent().parent()[0].tagName == "DIV") {
            $(".tip .tiptop span").text("编辑一级分类");
        }

        $(".trParent").hide();
        $(".tip").fadeIn(100);
        $(".Layer").fadeIn(100);
        $(".tip").css("height", "200px");
        $(".tip").css("width", "530px");
        if ($(this).attr("tipss") == 0) {
            $(".ddlGoodsType").attr("disabled", "disabled");
        } else {
            $(".ddlGoodsType").attr("disabled", false);
        }
        $(".ddlGoodsType").change(function () {
            $(".hideGoodsType").val($.trim($(this).val())); //赋值大类
        })

        var typeid = $.trim($(this).attr("tip")); //大类id
        var id = $.trim($(this).attr("tips")); //父类id
        $(".hideCategoryId").val(id);
        var name = $.trim($(this).parent().prev("i").text());
        var indexs = $.trim($(this).attr("tipsss")); //排序
        $(".txtCategoryName").val(name);
        // $(".txtSortIndex").val(indexs);
        $(".ddlGoodsType").val(typeid);
        $(".hideGoodsType").val(typeid);

    })
    //取消关闭
    $(".tip .cancel,.tip .tiptop a").click(function () {
        $(".tip").fadeOut(100);
        $(".tip2").fadeOut(200);
        $(".Layer").fadeOut(100);
        //        $(".lblCategory").find(".hover").removeClass("hover");
        //        $(".lblAttribute").text("");
    });

    //取消关闭
    $(".tip2 .cancel,.tip2 .tiptop a").click(function () {
        $(".tip").fadeOut(100);
        $(".tip2").fadeOut(200);
        $(".Layer").fadeOut(100);
        $(".hover").trigger("click");
    });
    //取消关闭
    $(".tip3 .cancel,.tip3 .tiptop a").click(function () {
        $(".tip").fadeOut(100);
        $(".tip2").fadeOut(200);
        $(".tip3").fadeOut(200);
        $(".Layer").fadeOut(100);
        $(".hover").trigger("click");
    });
    //取消关闭
    $(".tip4 .cancel,.tip4 .tiptop a").click(function () {
        $(".tip").fadeOut(100);
        $(".tip2").fadeOut(200);
        $(".tip3").fadeOut(200);
        $(".tip4").fadeOut(200);
        $(".Layer").fadeOut(100);
        $(".hover").trigger("click");
    });
    //编辑、添加商品分类验证用
    $(".btnEdit").click(function () {
        var name = $.trim($(".txtCategoryName").val()); //商品分类名称
        var typeid = $.trim($(".hideGoodsType").val()); //商品大类id
        if (typeid == "") {
            alert("- 请选择商品大类。");
            return false;
        }

        if (name == "") {
            alert("- 分类名称不能为空。");
            return false;
        }

        // var sortindex = $.trim($(".txtSortIndex").val()); //排序
        var categoryid = $.trim($(".hideCategoryId").val()); //商品分类id
        var type = $.trim($(".tip .tiptop span").text()); //区分编辑还是新增
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            data: { ck: Math.random(), action: "editCategory", name: name, typeid: typeid, categoryid: categoryid, type: type },
            dataType: "text",
            success: function (data) {
                if (data == "sb") {
                    if (type.indexOf("新增") != -1) {
                        alert("- 商品分类新增失败。");
                    } else {
                        alert("- 商品分类编辑失败。");
                    }
                } else if (data == "ycz") {
                    alert("- 商品分类已存在。");
                } else {
                    $(".tip").fadeOut(100);
                    $(".Layer").fadeOut(100);
                    $(".lblCategory").html(data);
                    $(".txtCategoryName").val("");
                    // $(".txtSortIndex").val("");
                    $(".hideGoodsType").val("");
                    if (type.indexOf("新增") != -1) {// && type == "新增一级分类"
                        //  $(".lblCategory div").eq(0).find(".addyi").trigger("click");
                        $(".lblCategory a[tipname='" + name + "']").parent().parent().trigger("click");
                        if (type == "新增一级分类") {
                            $(".tip3 .tipinfo .lb").text("一级分类添加成功，请继续新增同级分类或新增下级分类或添加规格属性。");
                            $(".btnxzxj").show();
                        } else if (type == "新增二级分类") {
                            $(".tip3 .tipinfo .lb").text("二级分类添加成功，请继续新增同级分类或新增下级分类或添加规格属性。");
                            $(".btnxzxj").show();
                        } else if (type == "新增三级分类") {
                            $(".tip3 .tipinfo .lb").text("三级分类添加成功，请继续新增同级分类或新增下级分类或添加规格属性。");
                            $(".btnxzxj").hide();
                        }
                        $(".tip3 .tiptop span").text("提示");
                        $(".tip3").fadeIn(200);
                        $(".Layer").fadeIn(200);
                        $(".tip3").css("height", "200px");
                        $(".tip3").css("width", "530px");

                    }
                }
            }, error: function () { }
        })
    })
    //删除
    $(document).on("click", ".menu .del", function () {
        var id = $(this).attr("tips"); //类别id
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            data: { ck: Math.random(), action: "del", id: id },
            dataType: "text",
            success: function (data) {
                if (data == "") {
                    alert("- 删除失败。");
                } else if (data == "cz") {
                    alert("- 请先删除分类最小分类。");
                } else if (data == "czfl") {
                    alert("- 请先删除分类规格属性。");
                } else if (data == "czgoods") {
                    alert("- 分类已被使用，不能删除。");
                } else {
                    $(".lblCategory").html(data);
                    $(".catediv").html("位置："); //规格属性列 标题显示
                }
            }, error: function () {

            }
        })
    })
    //新增同级分类
    $(".btnxztj").click(function () {
        $(".tip3").fadeOut(200);
        $(".Layer").fadeOut(200);
        if ($(".lblCategory .hover")[0].tagName == "DIV") { //一级分类下 同级新增
            $(".addCategoryBtn").trigger("click");
        } else if ($(".lblCategory .hover")[0].tagName == "DD") {//三级分类 同级新增
            $(".lblCategory .hover").prev().find(".add").trigger("click");
        } else if ($(".lblCategory .hover")[0].tagName == "DT") {
            $(".lblCategory .hover").parent().prev().find(".add").trigger("click"); //二级分类 同级新增
        }
    })
    //新增下级分类
    $(".btnxzxj").click(function () {
        $(".tip3").fadeOut(200);
        $(".Layer").fadeOut(200);
        $(".lblCategory .hover .add").trigger("click");
    })
    //添加规格属性
    $(".btntjgg").click(function () {
        $(".tip3").fadeOut(200);
        $(".Layer").fadeOut(200);
        $(".addAttributeBtn").trigger("click");
    })
    /////////////////////////////////////////////////////规格属性////////////////////////////////////////////////
    //新增规格属性
    $(".addAttributeBtn").click(function () {
        if ($(".lblCategory  .hover").index() == -1) {
            alert("- 请选中对应的商品分类。");
            return;
        }
        if ($(".menu .hover").find("dl").index() != -1) {
            alert("- 请选中该商品分类最小分类。");
            return;
        } else {
            if ($(".menu .hover").prevAll("dt").index() == -1) {
                if ($(".menu .hover").next("dd").index() != -1) {
                    alert("- 请选中该商品分类最小分类。");
                    return;
                }
            }
        }
        if ($(".menu2 .lblAttribute li").length >= 3) {
            alert("- 商品规格属性最多3个。");
            return;
        }
        if (confirm('确定选中的是商品分类最小分类？')) {
            $(".tip2 .tiptop span").text("新增规格属性");
            $(".tip2").fadeIn(200);
            $(".Layer").fadeIn(200);
            $(".tip2").css("height", "400px");
            $(".divedit").html("");
            $(".txtAttributeName").val("");
            $(".txtAddValue").val("");
            var category = $.trim($(".lblCategory  .hover i").text()); //分类名称
            var categoryID = $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
            $(".hideCategoryAttrID").val(categoryID);
            $(".txtCategoryAttr").val(category);
            $(".txtCategoryAttr").attr("disabled", "disabled");
            //            $.ajax({
            //                type: "post",
            //                url: "GoodsCategory.aspx",
            //                data: { ck: Math.random(), action: "sortindex" },
            //                dataType: "text",
            //                success: function (data) {
            //                    $(".txtSortIndexs").val(data);
            //                }, error: function () { }
            //            })
        } else {
            if ($(".tip3").attr("style") != "display: none;") {
                $(".tip3").fadeIn(200);
                $(".Layer").fadeIn(200);
            }
        }
    })
    //商品分类规格属性编辑
    $(document).on("click", ".menu2 .edit", function () {
        $(".tip2").fadeIn(200);
        $(".Layer").fadeIn(200);
        $(".tip2").css("height", "400px");
        $(".tip2 .tiptop span").text("编辑规格属性");
        $(".tip2 .tiptop span").attr("tip", $(this).attr("tip"));
        var category = $.trim($(".lblCategory  .hover i").text()); //分类名称
        var categoryID = $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
        $(".hideCategoryAttrID").val(categoryID);
        $(".txtCategoryAttr").val(category);
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
            url: "GoodsCategory.aspx",
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
            url: "GoodsCategory.aspx",
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
                url: "GoodsCategory.aspx",
                data: { ck: Math.random(), action: "Del", name: name, id: id, attributeID: AttributeID },
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
            $.ajax({
                type: "post",
                url: "GoodsCategory.aspx",
                data: { ck: Math.random(), action: "DelCategoryAttr", id: id, categoryAttrid: AttributeID },
                dataType: "text",
                success: function (data) {
                    if ($.trim(data) == "bsy") {
                        alert("- 规格属性已被使用，不能删除。");
                    }
                    if ($.trim(data) == "") {
                        alert("- 规格属性删除失败。");
                    }
                    if ($(".hover").attr("class").indexOf("li") != -1) {
                        $(".hover").find(".addyi").trigger("click"); //一级分类
                    }
                    if ($(".hover").attr("class").indexOf("adder") != -1) {
                        $("dt[class='adder hover']").trigger("click"); //二级分类
                    }
                    if ($.trim($(".hover").attr("class")) == "hover") {
                        $("dd[class='hover']").trigger("click"); //三级分类
                    }
                }, error: function () { }
            })
        }
    })
    //商品规格属性新增、(添加)新增属性值
    $(".btnEdit2,.btnBg1").click(function () {
        var str = "";
        var attr = $.trim($(".txtAttributeName").val());
        var value = $.trim($(".txtAddValue").val());
        if (attr == "") {
            str = str + "- 规格属性不能为空。\r\n";
        }
        if (value == "") {
            str = str + "- 规格属性值不能为空。\r\n";
        }
        var categoryID = $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
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
            var divhtml = $.trim($(".divedit").text());
            if (divhtml == "") {
                alert("- 规格属性值不能为空。");
                return false;
            }
            if ($.trim($(".tip2 .tiptop span").text()) == "编辑规格属性") {
                var id = $.trim($(".tip2 .tiptop span").attr("tip"));
                // var sort = $.trim($(".txtSortIndexs").val());
                ShowAddValue(1, id, attr, value, categoryID);
            }
            $(".tip").fadeOut(100);
            $(".tip2").fadeOut(200);
            $(".Layer").fadeOut(100);
            if ($(".hover").attr("class").indexOf("li") != -1) {
                $(".hover").find(".addyi").trigger("click"); //一级分类
            }
            if ($(".hover").attr("class").indexOf("adder") != -1) {
                $("dt[class='adder hover']").trigger("click"); //二级分类
            }
            if ($.trim($(".hover").attr("class")) == "hover") {
                $("dd[class='hover']").trigger("click"); //三级分类
            }
            var category = $.trim($(".lblCategory  .hover i").text()); //分类名称
            $(".tip4 .tipinfo .lb").html("您已成功添加<i style='color:red'>" + category + "</i>的规格属性，请继续添加规格属性或录入商品。");
            $(".tip4 .tiptop span").text("提示");
            $(".tip4").fadeIn(200);
            $(".Layer").fadeIn(200);
            $(".tip4").css("height", "200px");
            $(".tip4").css("width", "530px");

        }

    })
    //选中属性加背景颜色
    $(document).on("click", ".menu2 li .test .i", function () {
        var ind = $(this).parent().parent("li").index(); //当前li索引
        if ($.trim($(this).attr("style")) == "") {
            $(".menu2 li").eq(ind).find("i").css("background-color", "");
            $(this).css("background-color", "#ff6c00");
        } else {
            $(this).css("background-color", "");
        }
    })
    //根据分类ID得出规格属性
    function CategoryAttr(id, type) {
        if (type == 1) {
            $.ajax({
                type: "post",
                url: "GoodsCategory.aspx",
                data: { ck: Math.random(), action: "SelectAttr", id: id },
                dataType: "text",
                async: false,
                success: function (data) {
                    $(".lblAttribute").html(data);
                }, error: function () { }
            })
        } else {
            $.ajax({
                type: "post",
                url: "GoodsCategory.aspx",
                data: { ck: Math.random(), action: "SelectAttr", id: id },
                dataType: "text",
                success: function (datass) {
                    $(".lblAttribute").html(datass);
                }, error: function () { }
            })
        }
    }
    //显示添加后的属性以及属性值
    //1代表编辑  ，2代表添加
    function ShowAddValue(type, id, attrName, value, categoryID) {
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            data: { ck: Math.random(), action: "addValue", attrName: attrName, value: value, id: id, type: type, categoryid: categoryID },
            dataType: "text",
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
    //显示属性值列表
    function ShowAttributeValue(id) {
        // $(".btnBg2").attr("tip", id); //隐藏当前属性的id，用于新增属性值
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            data: { ck: Math.random(), action: "show", id: id },
            dataType: "json",
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
    //导入商品
    $(".btndrsp").click(function () {
        window.open("GoodsAdd2.aspx");
    })
    //添加规格属性
    $(".btntjgg2").click(function () {
        $(".tip4").fadeOut(200);
        $(".Layer").fadeOut(200);
        $(".tip2 .tiptop span").text("新增规格属性");
        $(".tip2").fadeIn(200);
        $(".Layer").fadeIn(200);
        $(".tip2").css("height", "400px");
        $(".divedit").html("");
        $(".txtAttributeName").val("");
        $(".txtAddValue").val("");
        var category = $.trim($(".lblCategory  .hover i").text()); //分类名称
        var categoryID = $.trim($(".lblCategory  .hover .edit").attr("tips")); //分类名称id
        $(".hideCategoryAttrID").val(categoryID);
        $(".txtCategoryAttr").val(category);
        $(".txtCategoryAttr").attr("disabled", "disabled");
    })

})
//只能输入数字验证
function KeyInt(val) {
    val.value = val.value.replace(/[^\d]/g, '');
}

