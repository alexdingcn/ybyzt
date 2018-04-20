<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsInfo.aspx.cs" Inherits="Distributor_GoodsInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品详情</title>
    <link href="/Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/lanrenzhijia.css" rel="stylesheet" type="text/css" />
    <link href="css/goodsinfo.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/jquery.jqzoom2.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/lanrenzhijia.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <style>
        * {
            font-size: inherit;
        }
        .nr-deta .t-deta .cur{ font-weight:normal; font-size:14px; padding:0px 15px; height:35px; line-height:35px; border-top:3px solid #2d5a9e; border-left:1px solid #dedfde;border-right:1px solid #dedfde; background:#fff; display:inline-block;}
    </style>
    <script>
        $(function () {

            $("img").each(function (index, obj) {
                var width = $(window).width() * 0.7;
                var attewidth = $(obj).width() * 1;
                if (attewidth >= width) {
                    var style = $(obj).attr("style");
                    if (style == undefined) {
                        $(obj).attr("style", "width:100%;")
                    }
                    else {
                        $(obj).attr("style", $(obj).attr("style").replace("width:" + attewidth + "px;", ""))
                        $(obj).width("100%");
                    }
                }

            })



            GetPrice();
            $("#litAttrVaue a").click(function () {
                $(this).addClass("hover").siblings().removeClass("hover");
                GetPrice();
            })
            //购买数量-
            $(".minus").click(function () {
                var num = $(".sl .txtNum").val();
                if ($.trim(num) >= 2) {
                    $(".txtNum").val(parseInt(num) - 1);
                }
                return false;
            })
            //购买数量+
            $(".add").click(function () {
                var num = $(".sl .txtNum").val();
                if ($.trim(num) >= 1) {
                    $(".txtNum").val(parseInt(num) + 1);
                }
                return false;
            })
            //加入、取消收藏
            $(".btns .keep").click(function () {
                $.ajax({
                    type: "post",
                    url: "/Controller/AddUpDataSource.ashx?action=AddCart",
                    data: { Goodsid:<%=goodsId %> },
                    dataType: 'json',
                    timeout: 3000,
                    cache: false,
                    success: function (data) {
                        if (data.Result) {
                            switch (data.Code) {
                                case "收藏": $(".btns .keep").html("<i class=\"sc-icon\" ></i>加入收藏"); break;
                                case "取消收藏": $(".btns .keep").html("<i class=\"sc-icon\" style=\"background-position:0 -73px;\"></i>取消收藏"); break;
                            }
                            layerCommon.msg(data.Msg, IconOption.正确);
                        } else {
                           
                            layerCommon.msg(data.Msg, IconOption.错误);
                           
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                    }
                })
            })
            //加入购物车
            $(".btns .addCart").click(function () {
                $_defaults = this;
                var GoodsinfoId = $("#hidGoodsInfoId").val();
                var GoodsName = $("#lblGoodsName").text();
                var GoodsNameSub = GoodsName.length > 15 ? GoodsName.substring(0, 15) + "..." : GoodsName;
                var Num = $.trim($(".sl .box").val());
                var ProID = "";
                var TPrice = Price = TagPrice = "";
                var CartType = 1;
                var Obj = this;
                if (GoodsinfoId != undefined && GoodsinfoId != "") {
                    $.ajax({
                        type: 'post',
                        url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
                        data: { GoodsInfoID: GoodsinfoId, ProID: ProID, Num: Num, Price: Price, TPrice: TPrice, CartType: CartType, Goodsid: <%=goodsId %> },
                        dataType: 'json',
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
                                            $("ul.topNav div[id*=tgnCar]").append("<div class=\"border\"><span>购物车还有<span id=\"num\">1</span>个商品</span><a class=\"cklink\" href=\"Shop.aspx\">去购物车</a></div>");
                                        }
                                    }
                                    else {
                                        $("ul.topNav div[id*=tgnCar]").append("<i class=\"GoGoodsInfo\" goods_tip=\<%=goodsId %>\" tip=\"" + GoodsinfoId + "\">" + GoodsNameSub + "<span class=\"goodsnum\">" + Num + "</span><span>x</span></i>");
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
                    });
                } else {
                    layerCommon.msg("该属性商品已下架或已删除，无法加入购物车，<br/>如果该商品有其它属性，请重新选择。", IconOption.错误);
                }
                return false;
            })
        })
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, "") == "" || val.value.replace(/[^\d]/g, "") == "0" ? 1 : val.value.replace(/[^\d]/g, "");
        }
        //获取价格
        function GetPrice() {
            var valuelist = ""; //属性值id
            $("#litAttrVaue a").each(function (index, obj) {
                if ($.trim($("#litAttrVaue a").eq(index).attr("class")) == "hover") {
                    var attr = $("#litAttrVaue a").eq(index).parent().siblings(".t").attr("tip");
                    valuelist += attr + ":" + $("#litAttrVaue a").eq(index).text() + ";";
                }
            });
            $.ajax({
                type: "post",
                dataType: "text",
                data: { ck: Math.random(), action: "price", value: valuelist,  goodsId: <%=goodsId %> },
        success: function (data) {
            var reg = /^[\u4e00-\u9fa5]{0,}$/; //中文正则表达式
            if (data.indexOf(",") == -1) {
                if (reg.exec(data)) {
                    if ($.trim(data) != "代理商可见") {
                        if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && ('<%= hideInfo2 %>' == undefined || '<%= hideInfo2 %>' == "")) {
                                    $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                                }
                                if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && '<%= hideInfo2 %>' != undefined && '<%= hideInfo2 %>' != "") {
                                    $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                                }

                            }
                            $(".price #lblPrice").text(data);
                            $(".price #lblPrice").css("font-size", "13px");
                        } else {
                            if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && ('<%= hideInfo2 %>' == undefined || '<%= hideInfo2 %>' == "")) {
                                $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                            }
                            if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && '<%= hideInfo2 %>' != undefined && '<%= hideInfo2 %>' != "") {
                                $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                            }
                            var price = formatMoney(data, 2);
                            $(".price #lblPrice").text("￥" + price);
                            $(".price #lblPrice").css("font-size", "20px");
                        }
                        $("#hidGoodsInfoId").val("");
                    } else {
                        if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && ('<%= hideInfo2 %>' == undefined || '<%= hideInfo2 %>' == "")) {
                            $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                        }
                        if ('<%= hideInfo1 %>' != undefined && '<%= hideInfo1 %>' != "" && '<%= hideInfo2 %>' != undefined && '<%= hideInfo2 %>' != "") {
                            $("#lblGoodsTitle").after("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                        }
                        if (reg.exec(data.split(',')[0])) {
                            $(".price #lblPrice").text(data.split(',')[0]);
                            $(".price #lblPrice").css("font-size", "13px");
                        } else {
                            var price = formatMoney(data.split(',')[0], 2);
                            $(".price #lblPrice").text("￥" + price);
                            $(".price #lblPrice").css("font-size", "20px");
                        }
                        $("#hidGoodsInfoId").val(data.split(',')[1]);
                    }
                    $.ajax({
                        type: "post",
                        data: { ck: Math.random(), action: "code", value: valuelist },
                        dataType: "text",
                        success: function (data) {
                            if (data != "") {
                                $("#lblCode").text(data.split(',')[0]);
                                $("#lblinventory").text(data.split(',')[1]);
                            }
                        }, error: function () { }
                    })

                }, error: function () { }
    })
        }
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


        $(document).on("click",".t-deta b",function(){
            $(this).siblings("b").removeClass("cur");
            $(this).addClass("cur");
            var tip=$.trim($(this).attr("tip"));
            $("#"+tip).siblings("div").hide();
            $("#"+tip).show();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input id="hid_Alert" type="hidden" />
        <Head:Head ID="Head2" runat="server" />
        <div class="rightinfo" style="overflow: visible;">
            <Left:Left ID="Left1" runat="server" ShowID="nav-2" />
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="A1" href="GoodsList.aspx"
                    runat="server" class="cur">商品列表</a>> <a id="navigation2" href="javascript:;" class="cur">商品详情</a>
            </div>
            <div class="ProdView">
                <div class="pic" style="display: none">
                    <img src="../images/Goods400x400.jpg" width="350" height="350" />
                </div>
                <div class="lanrenzhijia" style="position: absolute; top: auto;">
                    <!-- 大图begin -->
                    <div id="preview" class="spec-preview">
                        <span class="jqzoom">
                            <img jqimg="../images/Goods400x400.jpg" src="../images/Goods400x400.jpg" id="imgPic"
                                runat="server" width="350" height="350" /></span>
                    </div>
                    <!-- 大图end -->
                    <!-- 缩略图begin -->
                    <div class="spec-scroll">
                        <a class="prev">&lt;</a> <a class="next">&gt;</a>
                        <div class="items">
                            <ul>
                                <asp:Repeater ID="rptImg" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <img bimg="<%# Common.GetPicURL(Convert.ToString(Eval("pic"))) %>"
                                                src="<%# Common.GetPicURL(Convert.ToString(Eval("pic")), "resize400") %>"
                                                onmousemove="preview(this);"></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li id="ImgShow" runat="server">
                                    <img bimg="../images/Goods400x400.jpg" src="../images/Goods400x400.jpg" id="imgPic2"
                                        runat="server" onmousemove="preview(this);"></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="sp-deta left">
                    <h2 class="title" id="lblGoodsName" runat="server">
                        <div class="sale-box">
                            <i class="sale">促销</i>
                        </div>
                    </h2>
                    <label id="lblGoodsTitle" class="txt" runat="server">
                    </label>
                    <div style="height: 50px; line-height: 50px; padding-left: 5px;">
                        <i>商品编码：</i><i id="lblCode" runat="server"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>单位：</i><i
                            id="lblunit" runat="server"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='display: inline; <%= OrderInfoType.rdoOrderAudit("商品是否启用库存",this.CompID)=="0"?"": "display:none" %>'><i>库存：</i><i
                                id="lblinventory" runat="server"></i></span>
                    </div>
                    <div class="price" style="padding-left: 28px;">
                        <i>价格：</i><b class="red" id="lblPrice" runat="server"></b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <span id="YuanPrice" runat="server" style="display: inline-block;"></span>
                    </div>

                    <div class="specs">
                        <div id="litZiDingYi" runat="server">
                        </div>
                        <div id="litAttrVaue" runat="server">
                        </div>
                        <div>
                            <div class="li">
                                <div class="t">
                                    购买数量：
                                </div>
                                <div class="sl">
                                    <a href="javsscript:;" class="minus">-</a><input type="text" class="box txtNum" value="1"
                                        style="height: 23px;" onkeyup="KeyInt(this);" /><a href="javsscript:;" class="add">+</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="blank20">
                    </div>




                    
                    <%--<div class="specs" style="">
                        <i style="float: left;text-align:right;width:65px;">注册证：</i>
                        <i>
                            <div id="UpFileText2" runat="server" style="margin-left: 10px; width: 340px;">
                            </div>
                        </i>
                    </div>
                    <div class="blank10"></div>--%>




                    <div class="btns">
                        <a href="javascript:;" class="keep"><i class="sc-icon"></i>加入收藏</a><a href="javascript:;"
                            class="addCart"><i class="gwc-icon"></i>加入购物车</a><%--<a href="javascript:history.back(-1);" style="float:right;margin-top: 20px;" ><<返回</a>--%>
                    </div>
                </div>
                <div class="blank10">
                </div>
            </div>
            <div class="blank10">
            </div>
            <div class="nr-deta">
                <div class="t-deta">
                    <b tip="lblGoodsDetali" class="cur">商品介绍</b>
                    <b tip="lblGoodsDetali1" class="">注册证</b>
                </div>
                <div class="nr">
                    <div id="lblGoodsDetali" runat="server">
                        <p style="padding-top: 20px; line-height: 40px; padding-left: 20px">
                            暂无数据
                        </p>
                    </div>
                    <div id="lblGoodsDetali1" runat="server" style=" display:none;">
                    </div>
                </div>
            </div>
            <div class="blank20">
            </div>
            <input type="hidden" id="hidGoodsInfoId" runat="server" />
        </div>
    </form>
</body>
</html>
