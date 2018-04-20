<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="EShop_index" %>

<%@ Register TagName="Header" TagPrefix="uc" Src="UserControl/EshopHeader.ascx" %>
<%@ Register TagName="Bttom" TagPrefix="uc" Src="UserControl/EshopBttom.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=shopname%></title>
    <meta name="keywords" runat="server" id="mKeyword"   />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="baidu-site-verification" content="IdU3LryeUL" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/eshop/css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="/eshop/css/goods.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script>
//        function browserRedirect() {
//            var sUserAgent = navigator.userAgent.toLowerCase();
//            var bIsIpad = sUserAgent.match(/ipad/i) == "ipad";
//            var bIsIphoneOs = sUserAgent.match(/iphone os/i) == "iphone os";
//            var bIsMidp = sUserAgent.match(/midp/i) == "midp";
//            var bIsUc7 = sUserAgent.match(/rv:1.2.3.4/i) == "rv:1.2.3.4";
//            var bIsUc = sUserAgent.match(/ucweb/i) == "ucweb";
//            var bIsAndroid = sUserAgent.match(/android/i) == "android";
//            var bIsCE = sUserAgent.match(/windows ce/i) == "windows ce";
//            var bIsWM = sUserAgent.match(/windows mobile/i) == "windows mobile";
//            if (bIsIpad || bIsIphoneOs || bIsMidp || bIsUc7 || bIsUc || bIsAndroid || bIsCE || bIsWM) {
//                //app
//                window.location.href = 'http://www.yibanmed.com:86/eshop/index.aspx?Compid=<%=Request["Comid"] %>';
//            } else {
//                //pc
//                //window.location.href="http://www.yibanmed.com";
//            }
//        }
//        if ('<%=Request["index"] %>' != '1') {
//            browserRedirect();
//        }
    </script>

</head>
<body class="root">
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="Header" />
    <div runat="server" id="DIvBodyHTML">
        <!--banner start-->
        <div class="fullSlide" runat="server" visible="false" id="Top_Banner">
            <div class="bd">
                <ul runat="server" id="BannerUl">
                </ul>
            </div>
            <div class="hd">
                <ul>
                </ul>
            </div>
            <span class="prev"></span><span class="next"></span>
        </div>
        <!--banner end-->
        <div class="boxa11" style="position: relative;">
            <!--广告区 start-->
            <div class="adbox" runat="server" visible="false" id="Top_Advertisement">
                <!--店铺推荐 start-->
                <div class="adMenu">
                    <div class="title">
                        店铺推荐 &nbsp; <i class="sale" style="font-size: 12px; height: 15px;">Hot</i></div>
                    <asp:Literal ID="lblHtml" runat="server"></asp:Literal>
                </div>
                <!--店铺推荐 end-->
                <!--产品图片推荐 start-->
                <div class="adImg">
                    <a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",1) %>">
                        <img height="450" width="410" src="<%=GetBannerTopImg("Img",1) %>" alt="暂无图片" /></a></div>
                <ul class="adImg2">
                    <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",2) %>">
                        <img alt="暂无图片" width="180" height="220" src="<%=GetBannerTopImg("Img",2) %>" /></a></li>
                    <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",3) %>">
                        <img alt="暂无图片" width="180" height="220" src="<%=GetBannerTopImg("Img",3) %>" /></a></li>
                    <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",4) %>">
                        <img alt="暂无图片" width="180" height="220" src="<%=GetBannerTopImg("Img",4) %>" /></a></li>
                    <li>
                        <a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",5) %>">
                            <img alt="暂无图片" width="180" height="220" src="<%=GetBannerTopImg("Img",5) %>" />
                        </a>
                    </li>
                </ul>
                <!--产品图片推荐 end-->
                <!--信息联系方式 start-->
                <div class="adInfo">
                    <div class="title">
                        <a href="" class="hover">新闻公告</a><a href="" style="display: none;">通知</a><a href=""
                            style="display: none;">公告</a><a target="_blank" href="javascript:;" id="Href_NewMore"
                                runat="server" class="more">更多</a></div>
                    <ul class="list" runat="server" id="NewsList">
                    </ul>
                    <div class="title">
                        <a class="hover">联系方式</a></div>
                    <ul class="cut">
                        <li>
                            <p runat="server" id="lblPrincipal" />
                        </li>
                        <li>
                            <p runat="server" id="lblPhone" />
                        </li>
                        <li>
                            <p runat="server" id="lblAddress" />
                        </li>
                        <li>
                            <p runat="server" id="lbllogin" />
                        </li>
                    </ul>
                    <!--<div class="applyBtn"><a href='../../CompRegister.aspx?comid=<%=Request["comid"] %>'><i>加盟 · 购买</i>加盟代理商，即刻享受会员专属优惠！</a></div>-->
                </div>
                <!--信息联系方式 end-->
            </div>
            <!--广告区 end-->
            <!--推荐商品 start-->
            <div class="ztitle w1200" visible="false" runat="server" id="DivHotGoodsHeader">
                <b class="z">推荐商品</b><i class="y">recommend Commodity</i><a runat="server" href="javascript:;"
                    class="change GoodsHotNext">换一批<i class="sx-icon"></i></a></div>
            <div class="goods-show w1200" visible="false" runat="server" id="DivHotGoods">
                <div class="blank10">
                </div>
                <!--[if !IE]>商品大图列表 start<![endif]-->
                <ul class="goods-li" id="Div_BigGoodsHot" style="height: 320px;">
                </ul>
                <!--[if !IE]>商品大图列表 end<![endif]-->
                <div class="clear">
                    <a name="All"></a>
                </div>
            </div>
            <!--推荐商品 end-->
            <!--全部商品 start-->
            <div class="ztitle w1200" id="allPro">
                <b class="z">全部商品</b><i class="y">Whole Commodity</i></div>
            <div class="goods-show w1200">
                <!--[if !IE]>商品功能区 start<![endif]-->
                <div class="goods-gn">
                    <ul class="fn left">
                        <li class="closeFl"><a href="javascript:;" class="qx-icon CloseGsClasss "></a><a
                            href="javascript:;" class="CloseGsClasss">展开商品分类</a></li>
                        <li><a href="javascript:;" runat="server" onserverclick="btnProMotion_Click" id="CKGoodsPromotion"
                            class="k-icon"></a><a runat="server" onserverclick="btnProMotion_Click">促销商品</a>
                            <input type="checkbox" style="display: none;" runat="server" id="CK_Pro" /></li>
                        <li runat="server" id="LiCollect"><a href="javascript:;" runat="server" onserverclick="btnCollect_Click"
                            id="CKGoodsCollect" class="k-icon"></a><a runat="server" onserverclick="btnCollect_Click">
                                收藏商品</a>
                            <input type="checkbox" style="display: none;" runat="server" id="CK_Collect" /></li>
                    </ul>
                    <div class="fy right" style="margin-right: 10px;">
                        <a href="javascript:;" runat="server" id="PagePrev" onserverclick="PagePrev_Click"
                            class="s-icon"></a><a href="javascript:;" onserverclick="PageNext_Click" runat="server"
                                id="PageNext" class="x-icon"></a>
                    </div>
                    <div class="clear">
                    </div>
                    <!--[if !IE]>商品功能区-商品分类 start<![endif]-->
                    <div class="classify">
                        <div class="fl1">
                            <asp:Repeater runat="server" ID="Rpt_GoodsClass">
                                <ItemTemplate>
                                    <a href="javascript:" data-classid="<%# Eval("id") %>">
                                        <%# Eval("CategoryName")%></a>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!--[if !IE]>商品功能区-商品分类 end<![endif]-->
                </div>
                <!--[if !IE]>商品功能区 start<![endif]-->
                <!--[if !IE]>商品大图列表 start<![endif]-->
                <ul class="goods-li" id="Div_BigGoods">
                    <asp:Repeater runat="server" ID="Rpt_GoodsBig">
                        <ItemTemplate>
                            <li id="LIBig_<%# Container.ItemIndex %>" data-goodsid="<%#Eval("ID") %>" data-goodsname="<%#Eval("GoodsName")%>">
                                <div class="wrapper">
                                    <div class="pic">
                                        <a target="_blank" href="/e<%#Eval("ID")%>_<%# ViewState["Compid"]%>.html">
                                            <img width="205" height="205" alt="暂无图片" src="<%# Common.GetPicURL(Eval("Pic").ToString(),"resize400") %>"
                                                onerror="this.src='<%=ResolveUrl("../images/Goods400x400.jpg") %>'" />
                                        </a>
                                    </div>
                                    <div class="price">
                                        <b <%= !IsComp ? "class=\"txt\"" : ""%>>
                                            <%# IsComp ?Eval("IsLS").ToString().ToInt(0)==0?Eval("SalePrice").ToString().ToDecimal(0).ToString("#0.00"):"零售价：¥"+Eval("LSPrice").ToString().ToDecimal(0).ToString("#0.00") :kfmoney? (Eval("IsLS").ToString().ToInt(0)==0?Eval("SalePrice").ToString().ToDecimal(0).ToString("#0.00"):"零售价：¥"+Eval("LSPrice").ToString().ToDecimal(0).ToString("#0.00")):
                                              (Eval("IsLS").ToString().ToInt(0)==0?"代理商可见":"零售价：¥"+Eval("LSPrice").ToString().ToDecimal(0).ToString("#0.00"))%></b>
                                        <%#Eval("Type").ToString() == "1" || Eval("Type").ToString() == "0" ? "<div class=\"sale-box\"><i class=\"sale\">促销</i><div class=\"sale-txt\"><i class=\"arrow\"></i>" + Eval("ProInfoMation") + "</div></div>" : ""%>
                                    </div>
                                    <div class="txt2">
                                        <a target="_blank" title="<%#Eval("GoodsName")%>" href="/e<%#Eval("ID")%>_<%#ViewState["Compid"] %>.html">
                                            <%#Eval("GoodsName")%></a>
                                    </div>
                                    <div id="Ltr_<%#Eval("ID") %>" class="literal">
                                    </div>
                                    <div class="btn">
                                        <a href="javascript:;" title="<%#Eval("BdcID").ToString() != "" ? "取消收藏" : "收藏"%>"
                                            data-goodsid="<%#Eval("ID")%>" id="GoodsBig_AddCollect" class="keep"><i class='sc-icon'
                                                style="<%#Eval("BdcID").ToString() != ""?"background-position: -0px -73px;": "" %>">
                                            </i>收藏 </a><a id="GoodsBig_AddCart" href="javascript:;" class="addCart"><i class="gwc-icon">
                                            </i>加入购物车</a>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <!--[if !IE]>商品大图列表 end<![endif]-->
                <div class="blank10">
                </div>
                <!--[if !IE]>分页 start<![endif]-->
                <webdiyer:AspNetPager ID="Pager_List" runat="server" EnableTheming="true"  ShowPageIndexBox="Never" PageIndexBoxType="TextBox" 
                    FirstLastButtonClass="tf" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                    NextPrevButtonClass="tf" PageSize="15" PrevPageText="上一页" AlwaysShow="True" UrlPaging="false"
                    MoreButtonClass="tf" PagingButtonClass="tf" CssClass="page" CurrentPageButtonClass="cur"
                    NumericButtonCount="3" OnPageChanged="PagerList_PageChanged">
                </webdiyer:AspNetPager>
                <div class="blank10">
                </div>
                <!--[if !IE]>分页 end<![endif]-->
            </div>
            <!--全部商品 end-->
        </div>
        <input type="hidden" runat="server" value="0" id="HidScrollTop" />
        <input type="hidden" runat="server" id="HidClassShow" />
        <input type="hidden" runat="server" id="HidClassMoreShow" />
        <input type="hidden" runat="server" value="1" id="HidHotPageIndex" />
        <input type="hidden" runat="server" value="1" id="HidHotPageCount" />
        <input type="hidden" runat="server" class="CheckClass" id="HidCategoryOne" value="" />
        <input type="hidden" runat="server" class="CheckClass" id="HidCategoryTow" value="" />
        <input type="hidden" runat="server" class="CheckClass" id="HidCategoryThree" value="" />
        <input type="hidden" runat="server" class="CheckClass" id="HidCategoryCheckId" value="" />
        <input type="button" class="btn_SearchCategory" runat="server" onserverclick="Category_SearchClick"
            style="display: none;" />
    </div>

 
    <uc:Bttom runat="server" ID="Bttom" />
    <script type="text/javascript">
        $(document).ready(function () {
        $(".goods-li li").mousemove(function(){
            $(this).find(".literal").hide()
        });

                        GoodsCoomon.Compid='<%=ViewState["Compid"] %>',  GoodsCoomon.GoodsClassexPand(),  GoodsCoomon.GetBigGoodsAttribute("ul#Div_BigGoods", function(){ GeAccountUserList(function(){ BindHotGoods();}) } )  ,
                        ($("#HidClassShow").val() == "show" ? ($(".classify").show(),$(".CloseGsClasss").parent().children("a:eq(1)").text("关闭商品分类")) : ($(".classify").hide(), $(".CloseGsClasss").parent().removeClass("closeFl").children("a:eq(0)").attr("class", "k-icon CloseGsClasss"))),
                        $("#CK_Pro").prop("checked") && $("#CKGoodsPromotion").attr("class", "k-icon2"),
                        parseInt($.trim($("#HidScrollTop").val())) > 0 && $(window).scrollTop(parseInt($("#HidScrollTop").val())),
                        $("#CK_Collect").prop("checked") && $("#CKGoodsCollect").attr("class", "k-icon2"),
                        $(".CloseGsClasss").on("click", function () {
                            if ($(this).parent().hasClass("closeFl")) {
                                $(this).parent().children("a:eq(0)").attr("class", "k-icon").next("a").text("展开商品分类");
                                var Control = this;
                                $(".classify").slideUp(200, function () {
                                    $(Control).parent().removeClass("closeFl");
                                });
                                $("#HidClassShow").val("hide");
                            } else {
                                $(this).parent().addClass("closeFl");
                                $(this).parent().children("a:eq(0)").attr("class", "qx-icon").next("a").text("关闭商品分类");
                                $(".classify").slideDown(200);
                                $("#HidClassShow").val("show");
                            }
                        });

            $(".GoodsHotNext").on("click", function () {
                var Pageindex = parseInt($("#HidHotPageIndex").val());
                var PggeCount = parseInt($("#HidHotPageCount").val());
                if (isNaN(Pageindex) || isNaN(PggeCount)) {
                    $("#HidHotPageIndex").val("1");
                } else {
                    if (Pageindex < PggeCount) {
                         Pageindex++;
                        $("#HidHotPageIndex").val(Pageindex);
                    } else {
                        $("#HidHotPageIndex").val("1");
                    }
                }
                BindHotGoods();
            })

            $("#PagePrev[disbled],#PageNext[disbled]").removeAttr("href");

            $(window).scroll(function () {
                $("#HidScrollTop").val($(window).scrollTop());
            })

            function BindHotGoods(){
//                    GoodsCoomon.BindGoodsHot($("ul#Div_BigGoodsHot"),$("#HidHotPageIndex"),$("#HidHotPageCount"),<%=IsComp.ToString().ToLower() %>,"<%= Common.GetPicURL("test") %>",<%=ViewState["Compid"] %>,function(){ GoodsCoomon.GetBigGoodsAttribute("ul#Div_BigGoodsHot"); <%=ShowJs %>; });
             <%=ShowJs %>;
            }

            $(".fullSlide").hover(function () {
                $(this).find(".prev,.next").stop(true, true).fadeTo("show", 0.1)
            },
	function () {
	    $(this).find(".prev,.next").fadeOut()
	});
            $(".prev,.next").hover(function () {
                $(this).fadeTo("show", 0.5);
            }, function () {
                $(this).fadeTo("show", 0.1);
            })

            $(".fullSlide").slide({
                titCell: ".hd ul",
                mainCell: ".bd ul",
                effect: "fold",
                autoPlay: true,
                autoPage: true,
                trigger: "click",
                startFun: function (i) {
                    var curLi = jQuery(".fullSlide .bd li").eq(i);
                    if (!!curLi.attr("_src")) {
                        curLi.css("background-image", curLi.attr("_src")).removeAttr("_src")
                    }
                }
            });
        });
    </script>
    </form>
    <script src="/js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script type="text/javascript" src="/js/superslide.2.1.js"></script>
    <script src="/js/InputSearchData.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="/EShop/js/GoodsJS.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="/EShop/js/json2.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
</body>
</html>
