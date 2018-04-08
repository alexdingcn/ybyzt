<%@ Page Language="C#" AutoEventWireup="true" CodeFile="guarantee.aspx.cs" EnableViewState="false" Inherits="guarantee" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="bottom" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>交易保障-安全体系、卖家权益、买家保障-医站通</title>
    <meta name="keywords" content="医站通-B2B电子商务平台,在线订货系统,手机订货系统,订货app,分销订货,代理商管理,网上下单系统,订货管理系统,订单管理系统,在线订单管理系统,网上订货系统,网络订货系统,网上订货平台,在线订货平台,订货软件,分销系统,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
     <!--顶部导航栏 start-->
        <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->

    <!--页头 start-->
    <uc1:TopSec ID="top2" runat="server" />
    <!--页头 end-->
    
    <div class="guideTitle"><div class="nr">
	    <div class="menu"><a href="" class="hover">交易保障</a></div>
    </div></div>
    <div class="guide-ad"><img src="images/guar-ad.jpg" alt="交易保障"/></div>
    
    
    <div class="guar-p1"></div>
    <div class="guar-p2"></div>
    <div class="guar-p3"></div>

       <uc1:bottom runat="server" ID="bottom" />
       </form>
</body>
</html>
