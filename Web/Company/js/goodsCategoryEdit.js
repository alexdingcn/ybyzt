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
    //选中分类并赋值到父页面
    $(document).on("click", ".lblCategory i", function () {
        var cateName = $(this).text(); // text - decoration
        var cateId = $(this).attr("tip");
        var CompID = $(this).attr("tips");
        $.ajax({
            type: "post",
            url: "../../Handler/GoodsAttr.ashx",
            data: { ck: Math.random(), action: "minCate", id: cateId, compId: CompID },
            dataType: "text",
            async: false,
            success: function (data) {
                if (data == "cz") {
                    alert("请选择最小的商品分类");
                    return;
                } else {
                    if (data != "") {
                        if ($.trim(data.split("@")[0]) == "") {
                            window.parent.$("#lblAttr").text("该商品分类没有属性信息"); //属性
                        } else {
                            window.parent.$("#lblAttr").text(data.split("@")[0]); //属性
                        }
                        window.parent.$(".divheight").html(data.split("@")[1]); //属性值
                        window.parent.$(".hid_product_class").val(cateId);
                        window.parent.$(".txt_product_class").val(cateName);
                        window.parent.$(".showCate2").show();
                        window.parent.CloseDialog("2");

                    }
                }
            }, error: function () { }
        })
    })

    $(".ddlGoodsType").change(function () {
        $(".hideGoodsType").val($.trim($(this).val())); //赋值大类
    })
    //商品类型下级新增弹出层
    $(document).on("click", ".add", function () {
        $(".lblCategory").find(".hover").removeClass("hover");
        if ($(this).parent().parent().attr("class").indexOf("addyi") != -1) {
            $(this).parent().parent().parent().addClass("hover");
        }
        if ($(this).parent().parent().attr("class").indexOf("adder") != -1) {
            $(this).parent().parent().addClass("hover");
        }
        var id = $(this).next(".edit").attr("tips"); //分类id
        CategoryAttr(id);
        if (myObj == "1") {
            alert("- 该分类已绑定规格属性不能新增下级分类。");
            $(".cancel").trigger("click");
            return;
        }
        if ($(this).parent().parent().attr("class").indexOf("adder") != -1) {
            $(".catediv").text("新增三级分类");
        }
        if ($(this).parent().parent().attr("class").indexOf("addyi") != -1) {
            $(".catediv").text("新增二级分类");
        }
        $(".txtCategoryName").val("");
        //        $(".tip").fadeIn(100);
        //        $(".Layer").fadeIn(100);
        //        $(".tip").css("height", "230px");
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
        $(".lblCategory").find(".hover").removeClass("hover");
        if ($(this).parent().parent()[0].tagName == "DD") {
            $(this).parent().parent().addClass("hover");
        } else {
            if ($(this).parent().parent().attr("class").indexOf("addyi") != -1) {
                $(this).parent().parent().parent().addClass("hover");
            }
            if ($(this).parent().parent().attr("class").indexOf("adder") != -1) {
                $(this).parent().parent().addClass("hover");
            }
        }
        if ($(this).parent().parent()[0].tagName == "DD") {
            $(".catediv").text("编辑三级分类");
        } else if ($(this).parent().parent()[0].tagName == "DT") {
            $(".catediv").text("编辑二级分类");
        } else if ($(this).parent().parent()[0].tagName == "DIV") {
            $(".catediv").text("编辑一级分类");
        }
        $(".trParent").hide();
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
    $(".cancel").click(function () {
        $(".ddlGoodsType").val("");
        $(".txtCategoryName").val("");
        $(".hideGoodsType").val(""); //赋值大类
        $(".catediv").text("新增一级分类");
        $(".trParent").hide();
        $(".ddlGoodsType").attr("disabled", false);
        $(".lblCategory").find(".hover").removeClass("hover");
        window.parent.CloseDialog("2");
        //        $(".lblAttribute").text("");
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
        var type = $.trim($(".catediv").text()); //区分编辑还是新增
        $.ajax({
            type: "post",
            url: "GoodsCategoryEdit.aspx",
            data: { ck: Math.random(), action: "editCategory", name: name, typeid: typeid, categoryid: categoryid, type: type },
            dataType: "text",
            async: false,
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
                    $(".lblCategory").html(data);
                    $(".ddlGoodsType").val("");
                    $(".txtCategoryName").val("");
                    $(".hideGoodsType").val(""); //赋值大类
                    $(".hideCategoryId").val("");
                    $(".catediv").text("新增一级分类");
                    $(".trParent").hide();
                    $(".ddlGoodsType").attr("disabled", false);
                    $(".lblCategory").find(".hover").removeClass("hover");
                }
            }, error: function () { }
        })

    })
    //删除
    $(document).on("click", ".menu .del", function () {
        if (confirm('确定删除商品分类？')) {
            var id = $(this).attr("tips"); //类别id
            $.ajax({
                type: "post",
                url: "GoodsCategoryEdit.aspx",
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
                    }
                }, error: function () {

                }
            })
        }
    })
    //根据分类ID得出规格属性
    function CategoryAttr(id) {
        $.ajax({
            type: "post",
            url: "GoodsCategoryEdit.aspx",
            data: { ck: Math.random(), action: "SelectAttr", id: id },
            dataType: "text",
            async: false,
            success: function (data) {
                myObj = data;
            }, error: function () { }
        })
    }
})
