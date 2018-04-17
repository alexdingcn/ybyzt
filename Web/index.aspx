<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" EnableViewState="false"
    Inherits="index" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <meta charset="utf-8">
    <title>医站通</title>
    <link href="css/global-2.0.css?v=2.7.8.1" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="js/slide-simulate.js"></script>
	<script src="js/menu.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <!--顶部导航栏开始-->
        <uc1:Top ID="top1" runat="server" />


        <!--顶部导航结束-->
        <!--页头开始-->
        <!--顶部导航栏 end-->
        <uc1:TopNav ID="TopNav1" runat="server" />
        <!--页头结束-->
        <!--主导航开始-->
        
        <!--主导航结束-->
        <!--第一屏右侧 start-->
        <div class="wrap">
            <div class="fr me-fr">
				<div class="me-con">
                <div class="me-box">
                    <div class="me"><i class="me-i"></i></div>
                    <div class="t1 time">Hi,中午好！</div>
                    <div class="t2">欢迎来到医伴金服</div>
                </div>
                <div runat="server" id="index_Dvlogin" class="m-login"><a href="login.html" class="dl">登录</a> <a href="compordisregister.html" class="rz">注册</a> </div>
				</div>
                <div class="me-bg"></div>
                <div class="m-notice" runat="server" id="index_mNotice">
                    <div class="title">
                        <ul>
							<li class="hover" id="a1" onmouseover="setTab2('a',1,2)"><a href="/news_2.html">公告</a><i class="fg">|</i></li>
							<li id="a2" onmouseover="setTab2('a',2,2)"><a href="/news_3.html">新闻资讯</a><i class="fg">|</i></li>
                        </ul>
                        <a class="more fr" href="/news_1.html">更多 > </a>
                    </div>
                    <ul class="list hover" id="con_a_1" style="display: block;">
                        <asp:Repeater ID="Rpt_News" runat="server">
                            <ItemTemplate>
                                <li><a target="_blank" href="/newsinfo_<%# Eval("ID") %>.html" title="<%# Eval("NewsTitle") %>"><%# Eval("NewsTitle") %></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
					<ul class="list" id="con_a_2" style="display:none;">
                        <asp:Repeater ID="Rpt_News1" runat="server">
                            <ItemTemplate>
                                 <li><a target="_blank" href="/newsinfo_<%# Eval("ID") %>.html" title="<%# Eval("NewsTitle") %>"><%# Eval("NewsTitle") %></a></li>
                                <%--<li><a target="_blank" href="">系统优化升级公告 2017-08-10</a></li>
                               <li><a target="_blank" href="">系统优化升级公告 2017-07-12</a></li>
                               <li><a target="_blank" href="">系统优化更新公告—2017-03-03</a></li>
                               <li><a target="_blank" href="">系统优化更新公告 2016-12-28</a></li>--%>
                            </ItemTemplate>
                       </asp:Repeater>
                    </ul>


                </div>
				<div class="me-bg2"></div>
                <div class="blank15"></div>
            </div>
        </div>
        <!--第一屏右侧 end-->

        <!--第一屏-中间 start-->
        <div class="first-screen layout ">
            <div class="col-main super-slide" id="banner_super_slide">
                <!--代码开始-->
                <div class="slide_screen">
                    <ul class="list clearfix">
                        <li class="li1 i-ctrl-block">
                            <div class="window" id="banner">
                                <%--   首页Banner图--%>
                                <%= ConfigCommon.GetIndexConfig("IndexConfig.xml", "IndexBanner")%>
                            </div>
                        </li>
                    </ul>
                    <div class="i-ctrl">
                        <a class="ctrl-prev" href="javascript:void(0);"></a>
                        <a class="ctrl-next" href="javascript:void(0);"></a>
                    </div>
                    <div class="libBtn-Wrap">
                        <ul class="libtn clearfix" id="banner_ul1"></ul>
                    </div>
                </div>
                <!--代码结束-->
            </div>

            <script type="text/javascript">

                $(function () {
                    var c = "";
                    var a = 0;
                    var d = $("#banner").children();
                    for (var b = 0; b < d.length; b++) {
                        if (d[b] != null && d[b] != "undefined") {
                            if ($.trim(d[b].innerHTML).length != 0) {
                                c += "<li data-index='" + (a + 1) + "'></li>";
                                a++
                            }
                        }
                    }
                    if (a == 1) {
                        $("#banner_super_slide").mSlide({
                            isAuto: false,
                            moveWidth: 2560
                        });

                    } else {
                        $("#banner_ul1").html(c);
                        $("#banner_super_slide").mSlide({
                            isAuto: true,
                            moveWidth: 2560
                        })
                    }
                });
            </script>

            <!--top right-->

        </div>
        <!--第一屏-中间 end-->
        <div class="blank15"></div>

        <div class="w1200">
            <!--内容1 start-->
            <div class="con-t">
                <a href="/compnew.html" class="fr more">更多 ></a><h2>优品推荐</h2>
            </div>
            <div class="Optima">
                <%--   优品推荐--%>
                <%= ConfigCommon.GetIndexConfig("IndexConfig.xml", "ProductRecommend")%>
            </div>

            <!--内容1 end-->
            <div class="blank15"></div>
            <!--内容2 start-->
            <div class="con-t">
                <a href="/compnew.html" class="fr more">更多 ></a><h2>优惠促销</h2>
            </div>
            <div class="Sales">
                <ul>
                    <%--   优惠促销--%>
                    <%= ConfigCommon.GetIndexConfig("IndexConfig.xml", "ProductPromotion")%>
                </ul>
            </div>
            <!--内容2 end-->
        </div>
        <!--内容3 start-->
        <div class="bgf5">
            <div class="w1200">
                <div class="blank15"></div>
                <!--内容3 start-->
                <div class="con-t">
                    <a href="/compnew.html" class="fr more">更多 ></a><h2>家用常备</h2>
                </div>
                <%--   家用常备--%>
                <%= ConfigCommon.GetIndexConfig("IndexConfig.xml", "HomeStandby")%>
                <div class="blank25"></div>
                <div class="blank25"></div>
                <!--内容1 end-->
            </div>
        </div>
        <!--内容3 end-->
        <!--foot start-->
        <uc1:Bottom ID="Bottom1" runat="server" />
        <!--foot end-->
    </form>

    <script src="/js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
</body>

</html>
