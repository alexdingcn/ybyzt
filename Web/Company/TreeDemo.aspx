<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeDemo.aspx.cs" Inherits="Company_TreeDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <style>
        .pullDown
        {
            border: 1px solid #e5e5e5;
            width: 300px;
            background: #fff;
            height: 203px;
            overflow-y: scroll;
            position: relative;
        }
        .pullDown .itemTitle
        {
            height: 26px;
            line-height: 26px;
            margin-bottom: 1px;
            padding-top: 2px;
            background: #f5f5f5;
            padding-left: 30px;
            position: relative;
        }
        .pullDown .itemTitle.cur
        {
            background: #f5f5f5;
        }
        .pullDown .itemTitle a
        {
            width: 100%;
            height: 100%;
            display: inline-block;
        }
        .pullDown .pdaIcon
        {
            background: url(images/pullDown.png) no-repeat 0 0;
            width: 13px;
            height: 13px;
            position: absolute;
            top: 8px;
            left: 10px;
            cursor: pointer;
        }
        .pullDown .pdjIcon
        {
            background: url(images/pullDown.png) no-repeat -14px 0;
        }
        .pullDown .item
        {
            margin-left: 18px;
            padding-top: 3px;
            border-left: 1px solid #d6d6d6;
        }
        .pullDown .title
        {
            line-height: 20px;
            height: 20px;
            position: relative;
            padding-left: 17px;
        }
        .pullDown .title a
        {
            display: block;
        }
        .pullDown .title .lineIcon
        {
            background: #d6d6d6;
            width: 10px;
            height: 1px;
            overflow: hidden;
            position: absolute;
            top: 10px;
            left: 0;
        }
        .pullDown .title a
        {
            color: #494949;
        }
        .pullDown .title a:hover, .pullDown .itemTitle a:hover, .pullDown .itemCon li a:hover
        {
            color: #0490cd;
        }
        .pullDown .itemCon
        {
            line-height: 22px;
            padding-left: 20px;
        }
        .pullDown .itemCon li
        {
            background: url(images/pullDown.png) no-repeat -58px 6px;
            padding-left: 10px;
        }
        .pullDown .itemCon li a
        {
            color: #888;
            display: block;
        }
        
        .none
        {
            display: none;
        }
    </style>
    <script>
        $(function () {
            var type = '<%= type %>';
            //            var inde = window.parent.location.pathname.lastIndexOf("/");
            //            var str = window.parent.location.pathname.substring(parseInt(inde) + 1);
            //            str = $.trim(str);
            //            if (str == "GoodsAdd.aspx") {
            //                window.parent.$(".ifrClass").css("width", "175px");
            //                window.parent.$(".showDiv").css("width", "170px");
            //                window.parent.$(".txt_product_class").css("width", "170px");
            //                $(".pullDown").css("width", "170px");
            //            }
            //            if (str == "CompGoodsList.aspx") {
            //                window.parent.$(".ifrClass").css("width", "175px");
            //                window.parent.$(".showDiv").css("width", "170px");
            //                window.parent.$(".txt_product_class").css("width", "170px");
            //                $(".pullDown").css("width", "170px");
            //            }
            if (type == 1) {
                $(".pullDown").width("220px");
            }
            $(".pullDown  .pdaIcon").click(function () {
                if ($.trim($(this).parent().next("div").html()) != "") {
                    if ($(this).attr("class") == "pdaIcon") {
                        $(this).addClass("pdjIcon");
                        $(this).parent().next("div").find("ul").removeClass("none");
                    } else {
                        $(this).removeClass("pdjIcon");
                        $(this).parent().next("div").find("ul").addClass("none");
                    }
                }
            })
            //父页面赋值
            $(".pullDown a").click(function () {
                var $This = $(this), IsEidt = false;
                window.parent.IsLock = false;
                if (type == 1) {
                    var inde = window.parent.location.pathname.lastIndexOf("/");
                    var str = window.parent.location.pathname.substring(parseInt(inde) + 1);
                    if ($.trim(str) == "GoodsEdit.aspx") {
                        var str = "";
                        IsEidt = true;
                        //查询是否有下级分类ajax start
                        $.ajax({
                            type: "post",
                            data: { ck: Math.random(), action: "yanz", id: $.trim($This.attr("tip")) },
                            dataType: "text",
                            success: function (data) {
                                str = data;
                                if (str == "y") {
                                    window.parent.layerCommon.msg("请选择最小的分类", window.parent.IconOption.错误);
                                    return;
                                }

                                $.ajax({
                                    url: "/Handler/GoodsAttr.ashx",
                                    type: "post",
                                    data: { ck: Math.random(), action: "attr", id: $.trim($This.attr("tip")), compId: '<%=CompID %>' },
                                    dataType: "text",
                                    success: function (data) {
                                        if (data != "") {
                                            if ($.trim(data.split("@")[0]) == "") {
                                                window.parent.$("#lblAttr").text("该商品分类没有属性信息"); //属性
                                            } else {
                                                window.parent.$("#lblAttr").text(data.split("@")[0]); //属性
                                            }
                                            window.parent.$(".divheight").html(data.split("@")[1]); //属性值
                                            window.parent.$(".showCate2").show();
                                            window.parent.$(".lblPricelist").html('<input name=\"txtPrices\" type=\"text\" id=\"txtPrice\" style=\"width: 100px;\"   class=\"textBox txtPricess txtPrices\" onkeyup=\"KeyInt2(this);\" />');
                                            SettxtValue($This);
                                            window.parent.IsLock = false;
                                        }
                                    }, error: function () {
                                        window.parent.IsLock = false;
                                        var a;
                                    }
                                });

                            },
                            error: function () { }
                        });
                        //查询是否有下级分类ajax end
                    }
                    else if ($.trim(str) == "GoodsAreasList.aspx" || $.trim(str) == "GoodsAreasEdit.aspx") {
                        var str = "";
                        $.ajax({
                            type: "post",
                            data: { ck: Math.random(), action: "yanz", id: $.trim($This.attr("tip")) },
                            dataType: "text",
                            success: function (data) {
                                str = data;
                                if (str == "y") {
                                    window.parent.layerCommon.msg("请选择最小的分类", window.parent.IconOption.错误);
                                    return;
                                }
                                SettxtValue($This);
                            }, error: function () { }
                        })
                    }
                    else {
                        SettxtValue($This);
                    }
                } else if (type == 4) {
                    window.parent.$(".hid_product_class").val($.trim($(this).attr("tip")));
                    window.parent.$(".txt_product_class").val($.trim($(this).text()));
                    window.parent.$(".showDiv4").hide();
                    window.parent.$(".showDiv4").removeClass("xy");
                }
                else if (type == 2) {
                    window.parent.$(".hid_AreaId").val($.trim($(this).attr("tip")));
                    window.parent.$(".txt_txtAreaname").val($.trim($(this).text()));
                    window.parent.$(".showDiv2").hide();
                    window.parent.$(".showDiv2").removeClass("xy");
                } else if (type == 3) {
                    window.parent.$(".hid_TypeId").val($.trim($(this).attr("tip")));
                    window.parent.$(".txt_txtTypename").val($.trim($(this).text()));
                    window.parent.$(".showDiv3").hide();
                    window.parent.$(".showDiv3").removeClass("xy");
                }
                else if (type == 5) {
                    window.parent.$(".hid_DisId").val($.trim($(this).attr("tip")));
                    window.parent.$(".txt_txtDisname").val($.trim($(this).text()));
                    window.parent.$(".showDiv5").hide();
                    window.parent.$(".showDiv5").removeClass("xy");
                }
                else if (type == 6) {
                    window.parent.$(".hid_DisId").val($.trim($(this).attr("tip")));
                    window.parent.$(".txt_txtDisname").val($.trim($(this).text()));
                    window.parent.DefaultAddr($.trim($(this).attr("tip")), 0);
                    window.parent.$(".showDiv6").hide();
                    window.parent.$(".showDiv6").removeClass("xy");
                }
                if (!IsEidt) {
                    window.parent.IsLock = true;
                }
            });

            function SettxtValue(obj) {
                var text = $.trim($(obj).text()).split(">");
                text = $.trim(text[text.length - 1]);
                window.parent.$(".hid_product_class").val($.trim($(obj).attr("tip")));
                window.parent.$(".txt_product_class").val(text);
                window.parent.$(".hid_product_CateGoryName").val(text);
                window.parent.$(".showDiv").hide();
                window.parent.$(".showDiv").removeClass("xy");
            }
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pullDown">
       <%=WriteHTML %>
    </div>
    </form>
</body>
</html>
