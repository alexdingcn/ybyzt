<%@ Page Language="C#" AutoEventWireup="true" CodeFile="productsview.aspx.cs" Inherits="productsview" %>

<%@ Register TagName="Header" TagPrefix="uc" Src="UserControl/EshopHeader.ascx" %>
<%@ Register TagName="Bttom" TagPrefix="uc" Src="UserControl/EshopBttom.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="baidu-site-verification" content="IdU3LryeUL" />
    <meta http-equiv="Cache-Control" content="no-transform" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,user-scalable=yes" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= GetTitle() %></title>
    <meta name="keywords" runat="server" id="mKeyword" />
    <%--<meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />--%>
    <link href="/eshop/css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="/eshop/css/goods.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="/js/superslide.2.1.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="/js/InputSearchData.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <link href="/css/lanrenzhijia.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function(){
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
        })
    </script>
</head>
<style type="text/css">
    @media(max-width:960px)
    {
        
    }
    table {
        border-collapse: collapse;
        white-space: normal;
        line-height: normal;
        font-weight: normal;
        font-size: medium;
        font-variant: normal;
        font-style: normal;
        color: -webkit-text;
    }

    tr {
        display: table-row;
        vertical-align: inherit;
        border-color: inherit;
    }

    .red {
        color: #f40000;
    }

    i, em {
        font-style: normal;
    }

    .fl {
        float: left;
    }

    .fr {
        float: right;
    }

    .label .cur, .label .t:hover {
        border: 1px solid #608cd2;
        color: #608cd2;
    }

    .label .t {
        min-width: 50px;
        height: 28px;
        line-height: 28px;
        display: inline-block;
        border-radius: 5px;
        border: 1px solid #ddd;
        padding: 0px 10px;
        text-align: center;
        margin-right: 10px;
        cursor: pointer;
        position: relative;
    }
</style>
<body class="root">
    <form id="form1" runat="server">
        <uc:Header runat="server" ID="Header" />
        <input type="hidden" runat="server" id="hidycId" />
        <input type="hidden" runat="server" id="hidCompId" />
        <input type="hidden" id="hid_Alert" />
        <div class="blank10">
        </div>
        <div class="whiteBg w1200">
            <!--[if !IE]>商品展示 start<![endif]-->
            <div class="spDetails">

                <div class="dPic left" style="display: none">
                    <img src="../images/Goods400x400.jpg" width="350" height="350" alt="暂无图片" />
                </div>
                <div class="lanrenzhijia left">
                    <!-- 大图begin -->
                    <div id="preview" class="spec-preview">
                        <span class="jqzoom">
                            <img jqimg="../images/Goods400x400.jpg" src="../images/Goods400x400.jpg" id="imgPic"
                                runat="server" width="350" height="350" alt="暂无图片" /></span>
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
                                            <img bimg="<%# Common.GetPicURL(Eval("pic").ToString()) %>" src="<%# Common.GetPicURL(Eval("pic").ToString(), "resize400") %>"
                                                onmousemove="preview(this);" alt="暂无图片"></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li id="ImgShow" runat="server">
                                    <img bimg="../images/Goods400x400.jpg" src="../images/Goods400x400.jpg" id="img1"
                                        runat="server" onmousemove="preview(this);" alt="暂无图片"></li>
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
                    <label id="lblGoodsTitle" class="txt" runat="server"></label>
                    <div class="goodsSubtitle"></div>
                    <div style="height: 50px; line-height: 50px; padding-left: 5px;">
                        <i>商品编码：</i><i id="lblCode" runat="server"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>单位：</i><i id="lblunit" runat="server"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='<%= OrderInfoType.rdoOrderAudit("商品是否启用库存",compId)=="0"?"": "display:none" %>'><i>库存：</i><i id="lblinventory" runat="server"></i></span>
                    </div>

                    <div style="height: 50px; line-height: 50px; padding-left: 5px;">
                        <i class="bt2" style="float: left">商品标签：</i><i>
                            <div class="control-input label fl" id="DivLabel" runat="server" style="margin-top: 10px;">
                            </div>
                        </i>
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
                    <div class="btn">
                        <a href="javascript:;" class="keep"><i class="sc-icon"></i>加入收藏</a><a href="javascript:;"
                            class="addCart"><i class="gwc-icon"></i>加入购物车</a><a href="javascript:;"
                                class="addReg" style="color: #fff;">申请合作</a>
                    </div>
                </div>
                <div class="recommend">
                    <div class="title">
                        <h3>推荐商品</h3>
                        <a href="/<%=compId %>.html#All" class="more">更多</a>
                    </div>
                    <ul class="list">
                    </ul>
                    <div class="fy paging">
                    </div>
                </div>
            </div>
            <!--[if !IE]>商品展示 end<![endif]-->
            <!--[if !IE]>商品详细 start<![endif]-->
            <div class="blank20">
            </div>
            <div class="nr-deta">
                <div class="t-deta">
                    <b tip="lblGoodsDetali" class="cur">商品介绍</b>
                    <b tip="lblGoodsDetali1">注册证</b>
                </div>
                <div class="nr">
                    <div id="lblGoodsDetali" runat="server" style="display: block;min-height:300px">
                        <p style="padding-top: 20px; line-height: 40px; padding-left: 20px">
                            暂无数据
                        </p>
                    </div>
                    <div id="lblGoodsDetali1" runat="server" style="display:none;min-height:300px">
                    </div>
                </div>
            </div>
            <div class="blank20">
            </div>
            <!--[if !IE]>商品详细 end<![endif]-->
        </div>
        <div class="blank10">
        </div>
        <div class="ad1200 w1200">
            <a href="javsscript:;">
                <img src="../images/ad1200.jpg" alt="暂无图片"></a>
        </div>
        <div class="blank10">
        </div>
        <input type="hidden" id="hidGoodsInfoId" runat="server" />
        <!--footer start-->
        <uc:Bttom runat="server" ID="Bttom" />
        <!--footer end-->
    </form>
    <style>
        .zidingyi {
            border-radius: 3px;
            display: inline-block;
            height: 26px;
            line-height: 26px;
            padding: 1px 10px;
            position: relative;
            text-align: center;
        }

        .addReg {
            background: #2d5a9e none repeat scroll 0 0;
            border-radius: 5px;
            color: #fff;
            display: inline-block;
            font-size: 14px;
            height: 40px;
            line-height: 40px;
            margin-right: 10px;
            text-align: center;
            width: 140px;
        }

        .sp-deta .addReg:hover {
            background: #255295 none repeat scroll 0 0;
        }
    </style>
</body>
<link href="/css/root.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
<script src="/js/jquery.jqzoom.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="/js/lanrenzhijia.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="/EShop/js/jquery.myPagination.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script>
    $(function () {
        Pagination();//推荐商品
        $("#PrevPageText[disbled],#NextPageText[disbled]").removeAttr("href");
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
        $(".btn .keep").click(function(){
            if('<%=islogin %>'=="False" && <%=compId %>!=0){
                layerCommon.openWindow("用户登录", "/WindowLogin_<%=compId%>.html", "400px", "345px", undefined, false);
             return false;
         }
            if(<%=goodsId %>!=0){
                $.ajax({
                    type: "post",
                    url: "../../Controller/AddUpDataSource.ashx?action=AddCart",
                    data: { Goodsid: <%=goodsId %> },
            dataType: 'json',
            timeout: 3000,
            cache: false,
            success: function (data) {
                if (data.Result) {
                    switch (data.Code) {
                        case "收藏":$(".btn .keep").html("<i class=\"sc-icon\"></i>加入收藏"); break;
                        case "取消收藏":$(".btn .keep").html("<i class=\"sc-icon\" style=\"background-position:0 -73px;\"></i>取消收藏");  break;
                    }
                    layerCommon.msg(data.Msg, IconOption.正确);
                } else {
                    if (data.Code == "Login") {
                        layerCommon.openWindow("用户登录", "/WindowLogin_" + comid+".html", "400px", "345px", undefined, false);
                    } else {
                        layerCommon.msg(data.Msg, IconOption.错误);
                    }
                    // layerCommon.msg(data.Msg, IconOption.错误);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
            }
        })
    }
            return false;
        })
        var hidCompId=$.trim($("#hidCompId").val());
        var hidycId=$.trim($("#hidycId").attr("yc-tip"));
        if('<%=string.IsNullOrWhiteSpace(Request["ycid"]) %>'!="False"){
            $(".addReg").hide();
        }else
        {
            if('<%= yc_CompT == "1" %>'=="True"){
                  if(hidCompId!=hidycId){
                      $(".addReg").show();
                  }else{
                      $(".addReg").hide();
                  }
              }
          }

        //申请合作
        $(document).on("click", ".addReg", function () {
            $.ajax({
                type: 'post',
                data: { action: "applyCooperation", CompId: $("#hidCompId").val(),ycCompID:$("#hidycId").attr("yc-tip") },
                dataType: "json",
                success: function (ReturnData) {
                    if (ReturnData.Result) {
                        //转向网页的地址; 
                        var url = '/Distributor/CMerchants/FirstCampAdd.aspx?KeyID=' + $("#hidycId").val();
                        var index = layerCommon.openWindow("申请合作", url, '950px', '615px'); //记录弹出对象
                        $("#hid_Alert").val(index);
                    } else {
                        if (ReturnData.Code == "Login") {
                            layerCommon.openWindow("用户登录", "/WindowLogin.aspx", "400px", "345px", function () {
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
                            layerCommon.msg(ReturnData.Msg, IconOption.错误);
                        }
                    }
                },
            })

        });

        //加入购物车
        $(".btn .addCart").click(function(){
            if('<%=islogin %>'=="False" && <%=compId %>!=0){
                 layerCommon.openWindow("用户登录", "/WindowLogin_<%=compId %>.html", "400px", "345px", undefined, false);
             return false;
         }
             if ('<%=userCompId %>' == '<%=compId %>') {
                 //登录的代理商是该企业的代理商
                 if ('<%=type %>'== '3' || '<%=type %>' == '4' || '<%=type %>'=='0' ) { //3：企业用户  4：企业管理员 
                      layerCommon.msg('对不起，您不能购买，请先加盟！', IconOption.错误);
                      return false;
                  }
              } else {
                  //登录的代理商不是该企业的代理商
                  if ('<%=type %>'!='2' ) { //3：企业用户  4：企业管理员 
                      layerCommon.msg('对不起，您不能购买，请先加盟！', IconOption.错误);
                      return false;
                  }
              }
             var GoodsinfoId = $("#hidGoodsInfoId").val();
             var GoodsName=$("#lblGoodsName").text();
             var Num = $.trim($(".sl .box").val());
             var ProID = "";
             var TPrice = Price = TagPrice = "";
             var CartType = 1;
             var Obj = this;
             if (GoodsinfoId != undefined && GoodsinfoId!="") {
                 $.ajax({
                     type: 'post',
                     url: '../Handler/ShopCart.ashx?ActionType=AddShopCart',
                     data: { GoodsInfoID: GoodsinfoId, ProID: ProID, Num: Num, Price: Price, TPrice: TPrice, CartType: CartType, Goodsid: <%=goodsId %> },
                        dataType: 'json',
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                layerCommon.msg("商品已成功添加到购物车", IconOption.正确);
                                $(".TopE_CartNum").html(ReturnData.SumCart);

                                $(".cur a i[tip=\"title\"]").length > 0 ? $(".cur a i[tip=\"title\"]").parent("a").remove() : ""

                                var CartControl = $(".cur a[infoid='" + GoodsinfoId + "']  span.goodsnum", ".fore2");

                                if ($("li[class=\"fore2\"] .cur a").length < 4) {
                                    (CartControl.length == 0 && $(".cur", ".fore2").append("<a target='_blank' Infoid='" + GoodsinfoId + "' href=\"/e" + GoodsinfoId + "_" + <%=compId %> +"_.html"+ "\" ><i class=\"GoGoodsInfo name\">" + GoodsName + "</i><span class=\"goodsnum num\">x" + Num + "</span></a>")) ||
                                (CartControl.text("x" + (parseInt(CartControl.text().replace("x", "")) + parseInt(Num))));
                                    return;
                                } else {
                                    if ($("li[class=\"fore2\"] .cur div[class=\"border\"]").length > 0) {
                                        if (CartControl.length > 0) {
                                            CartControl.text("x" + (parseInt(CartControl.text().replace("x", "")) + parseInt(Num)))
                                        } else {
                                            var sum = $("li[class=\"fore2\"] .cur div[class=\"border\"] span span[class=\"red\"]").text();
                                            $("li[class=\"fore2\"] .cur div[class=\"border\"] span span[class=\"red\"]").text(parseInt(sum) + parseInt(Num));
                                        }
                                    } else {
                                        $("li[class=\"fore2\"] .cur").append("<div class=\"border\"><span>购物车还有<span class='red' id=\"num\">" + Num + "</span></span>个商品<a class\"cklink\" style='float:right' href=\"../Distributor/Shop.aspx\">去购物车</a></div>");
                                    }
                                }
                                //                            CloneImg.animate({ "left": ($(".shop-i").offset().left) + "px", "top": ($(".shop-i").offset().top) + "px" }, 1000, function () {
                                //                                /*CloneImg.attr("class", "gw-icon2 Top_CartImgClone");*/
                                //                                setTimeout(function () { CloneImg.remove(); $(".TopE_CartNum").html(ReturnData.SumCart); }, 400);
                                //                            });
                            } else {
                                if (ReturnData.Code == "Login") {
                                    layerCommon.openWindow("用户登录", "/WindowLogin_" + comid+".html", "400px", "345px", undefined, false);
                                } else {
                                    if (GoodsinfoId != undefined) {
                                        layerCommon.msg(ReturnData.Msg, IconOption.错误);
                                    }
                                    else {
                                        layerCommon.msg("该属性商品已下架，无法加入购物车，<br/>如果该商品有其它属性，请重新选择。", IconOption.错误);
                                    }
                                }
                                //  layerCommon.msg(ReturnData.Msg, IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layerCommon.msg("添加失败，请求错误或超时,请重试", IconOption.错误);
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
                data: { ck: Math.random(), action: "price", value: valuelist ,compId:<%=compId %>, goodsId:<%=goodsId %>},
                success: function (data) {
                    var reg = /^[\u4e00-\u9fa5]{0,}$/; //中文正则表达式
                    var goodsParent = $("#lblGoodsTitle").parent();
                    $("#divHideInfo1,br",goodsParent).remove();
                    if(data.indexOf(",")==-1) {
                        if (reg.exec(data)) {
                            if($.trim(data)!="代理商可见") {
                                if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && ('<%= hideInfo2 %>'==undefined || '<%= hideInfo2 %>'=="")){
                                    $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                                }
                                if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && '<%= hideInfo2 %>'!=undefined && '<%= hideInfo2 %>'!="" ){
                                    $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                                }
                            }
                            $(".price #lblPrice").text(data);
                            $(".price #lblPrice").css("font-size","13px");
                        } else {
                            if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && ('<%= hideInfo2 %>'==undefined || '<%= hideInfo2 %>'=="")){
                                $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                            }
                            if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && '<%= hideInfo2 %>'!=undefined && '<%= hideInfo2 %>'!="" ){
                                $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                            }
                            var price = formatMoney(data, 2);
                            $(".price #lblPrice").text("￥" + price);
                            $(".price #lblPrice").css("font-size","20px");
                        }
                        $("#hidGoodsInfoId").val("");
                    } 
                    else
                    {
                        if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && ('<%= hideInfo2 %>'==undefined || '<%= hideInfo2 %>'=="")){
                            $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\"><%= hideInfo1 %></label>");
                        }
                        if('<%= hideInfo1 %>'!=undefined && '<%= hideInfo1 %>'!="" && '<%= hideInfo2 %>'!=undefined && '<%= hideInfo2 %>'!="" ){
                            $(".goodsSubtitle").html("<br /><label id=\"divHideInfo1\" class=\"txt\" ><%= hideInfo1 %></label><br /><label id=\"divHideInfo2\" class=\"txt\" ><%= hideInfo2 %></label>");
                        }
                        if (reg.exec(data.split(',')[0])) {
                            $(".price #lblPrice").text(data.split(',')[0]);
                            $(".price #lblPrice").css("font-size","13px");
                        } else {
                            var price = formatMoney(data.split(',')[0], 2);
                            $(".price #lblPrice").text("￥" + price);
                            $(".price #lblPrice").css("font-size","20px");
                        }
                        $("#hidGoodsInfoId").val(data.split(',')[1]);
                    }

                    $.ajax({
                        type: "post",
                        data: { ck: Math.random(), action: "code", value: valuelist },
                        dataType: "text",
                        success: function (data) {
                            if(data!=""){
                                $("#lblCode").text(data.split(',')[0]);
                                $("#lblinventory").text(data.split(',')[1]);
                            }
                        }, error: function () { }
                    })
            }, error: function () { }
        })
    }
function Pagination() {
    var paras = { action: 'getData',compId:<%=compId %> };
        $(".paging").myPagination({
            currPage: 1,
            pageCount: 1,
            pageSize:10,
            cssStyle: 'myClasspaging',
            info: {
                msg_on: false,
                first_on: false,
                last_on: false
            },
            ajax: {
                on: true,
                callback: 'InitData',
                url: "../Handler/GetPrice.ashx?" + jQuery.param(paras),
                dataType: 'json',
                ajaxStart: function () {
                    $(".recommend .list").addClass("load");
                }, ajaxStop: function () {
                    $(".recommend .list").removeClass("load");
                }
            }
        })
    }
    function InitData(data) {
        var json = data.rows;
        var html = "";
        if (json.length != 0) {
            $(json).each(function (index, obj) {
                    var src="";
                    if (!obj.pic){
                        src="../images/Goods200x200.jpg";
                    } else {
                        src="<%=Common.GetPicBaseUrl() %>" + obj.pic + "?x-oss-process=style/resize400";
                    }
                    $.ajax({
                        type: "post",
                        dataType: "text",
                        async: false,
                        data: { ck: Math.random(), action: "price2", value: "" ,compId:<%=compId %>,goodsId2:obj.id},
                    success: function (data) {
                        var str="";
                        var reg = /^[\u4e00-\u9fa5]{0,}$/; //中文正则表达式
                        if (reg.exec(data)) {
                            str=data;
                        } else {
                            var price = formatMoney(data, 2);
                            str="￥" + price;
                        }
                       
                        html += "<li><div class=\"pic\"><a href=\"/e"+obj.id+"_"+obj.compId+".html"+"\" title=\""+obj.goodsname+"\"><img src=\""+src+"\" width=\"85\" alt=\"暂无图片\"></a></div><div class=\"txt2\"><a  href=\"/e"+obj.id+"_"+obj.compId+".html"+"\" title=\""+obj.goodsname+"\">" + obj.goodsname + "</a></div><div class=\"price\">"+str+"</div></li>"
                    }
                })
            })
        }else
        {
            html+="<li>暂无数据</li>";
        }
        $(".recommend .list").html(html);
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
</html>
