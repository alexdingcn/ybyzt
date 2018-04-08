<%@ Page Language="C#" AutoEventWireup="true" CodeFile="news.aspx.cs" Inherits="news" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="bottom" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">  
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title id="title"><%= GetTitle() %></title>
    <meta name="keywords" content="我的医站通网_医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的医站通网,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <%--<script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?779e9b3d086d94ec0ead28ec3dd99190";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <!--顶部导航栏 start-->
    <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->

    <!--页头 start-->
    <uc1:TopSec ID="top2" runat="server" />
   <!--页头 end-->


    <div class="guideTitle">
        <div class="nr">
	        <div class="menu"><a class="hover">新闻资讯</a></div>
        </div>
    </div>
    <div class="help-ad"><img src="images/news-ad.jpg" /></div>
    <div class="blank20"></div>

    <div class="wrap">
        <!--新闻左侧菜单 start-->
        <div class="help-sideBar fl">
	        <div class="title"><h2>新闻资讯</h2></div>
	        <ul class="menu new-menu">
                <li class="flbt" runat="server" id="LiNews_1"><a href="/news_1.html"" class="t"><i class="arrw-i"></i>行业新闻</a></li>
                <li class="flbt" runat="server" id="LiNews_3"><a href="/news_3.html" class="t"><i class="arrw-i"></i>资讯</a></li>
                <li class="flbt" runat="server" id="LiNews_4"><a href="/news_4.html" class="t"><i class="arrw-i"></i>生意经</a></li>
                <li class="flbt" runat="server" id="LiNews_2"><a href="/news_2.html" class="t"><i class="arrw-i"></i>公告</a></li>
	        </ul>
        </div>
        <!--新闻左侧菜单 end-->

        <div class="help-ricon fr about-ri">
            <!--新闻资讯 start-->
            <div class="about">
		        <div class="place"><i  class="t">新闻资讯</i>><i runat="server"  id="Inewstext"  class="t">行业新闻</i></div>
                <!--新闻列表 start-->
       	        <ul class="news-list">
                    <asp:Repeater ID="Rpt_News" runat="server">
                       <ItemTemplate>
                         <li>
            	            <div class="date"><i class="day"><%#Eval("CreateDate","{0:dd}")%></i><i class="year"><%#Eval("CreateDate","{0:yyyy/MM}")%></i></div>
            	            <div class="title"><a href="/newsinfo_<%#Eval("ID")%>.html"><%#Eval("NewsTitle")%></a></div>
                            <div class="txt"><a href="/newsinfo_<%#Eval("ID")%>.html"><%# NoHTML(Eval("NewsContents").ToString(),Eval("Newsinfo"))%></a></div>
                        </li>
                       </ItemTemplate>
                    </asp:Repeater>
                    
                </ul>
                <!--新闻列表 end-->
                <div class="blank20"></div>
                <!--分页 start-->
                 <webdiyer:AspNetPager ID="Pager_List" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    FirstLastButtonClass="tf" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                    NextPrevButtonClass="tf" PageSize="10" PrevPageText="上一页" AlwaysShow="True" UrlPaging="false"
                  
                    MoreButtonClass="tf" PagingButtonClass="tf"  CssClass="page" CurrentPageButtonClass="cur" NumericButtonCount="3"
                    OnPageChanged="PagerList_PageChanged">
                </webdiyer:AspNetPager>
               <!--分页 start--> 

            </div>
            <!--新闻资讯 end-->
   

        </div>
     </div>
    <div class="blank20"></div>
    <uc1:bottom runat="server" ID="bottom" />

    </form>
</body>
</html>
