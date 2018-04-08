<%@ Page Language="C#" AutoEventWireup="true" CodeFile="platform.aspx.cs" EnableViewState="false" Inherits="platform"  %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>平台介绍-核心功能、多供应链、应用场景、商业智能、数据安全-医站通</title>
    <meta name="keywords" content="我的医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的医站通,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>    
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

<uc1:Top ID="top1" runat="server" />
<uc1:TopSec ID="top2" runat="server" />

<!--banner 开始-->
<div class="panel-banner">
 <div class="p-bannerbg1">
   <div class="panel-w1200">
   <div class="pic-phone"></div>
   <div class="gg-wen"></div>
   <div class="panel-btn">
     <a class="anzhuo" target="_blank" href="http://a.app.qq.com/o/simple.jsp?pkgname=com.yzt.app.h5"></a>
     <a class="phone" target="_blank" href="https://itunes.apple.com/cn/app/%E5%8C%BB%E7%AB%99%E9%80%9A/id1355304363"></a>
   </div>
    <div class="erwei"></div>
  </div>
 </div>
</div>
<!--banner 结束-->


<!--核心功能一览 start-->
<div class="panel1">
	<div class="section">
    	<h2 class="panel-t">核心功能一览</h2>
        <div class="panel-w">分为代理商订货和厂商后台管理，支持订货、支付、发货等各种业务。</div>
        <ul class="p-list1">
        	<li><i class="hxgn-i1"></i><h3>订货</h3><i>代理商下单，并可通过订单查询、短息提醒等方式实时掌握订单状态。</i></li>
           <!-- <li><i class="hxgn-i2"></i><h3>支付</h3><i>支持快捷支付、网银支付等多种支付方式。</i></li>!-->
            <li><i class="hxgn-i3"></i><h3>发货</h3><i>厂商发货，代理商随时查看发货信息和物流信息。</i></li>  
            <li><i class="hxgn-i5"></i><h3>商品</h3><i>支持自定义商品规格属性，提供上下架、促销、等多种功能。</i></li>
            <li><i class="hxgn-i6"></i><h3>代理商</h3><i>提供上级厂商的商品共享。</i></li>
            <li><i class="hxgn-i7"></i><h3>促销</h3><i>支持商品促销、订单促销等多种促销方式。</i></li>
            <li><i class="hxgn-i8"></i><h3>价格政策</h3><i>支持按商品、按规格、按代理商等多种方式定价。</i></li>
            <li><i class="hxgn-i9"></i><h3>商业智能</h3><i>强大的数据分析，为厂商提供决策依据。</i></li>
        </ul>  
    </div>
</div>
<!--核心功能一览 end-->

<!--多供应链 start-->
<div class="panel2">
	<h2 class="panel-t">多供应链</h2>
	<div class="panel-w">适用于厂商与它的代理商之间的订货、支付、发货、收货等，也适用于厂商和代理商之间的业务。</div>
	<div class="pic"><img src="images/panel2.png" alt="暂无图片"/></div>
</div>
<!--多供应链  end-->

<!--应用场景 start-->
<div class="panel3">
	<h2 class="panel-t">应用场景</h2>
	<div class="panel-w">医站通适用于拥有稳定订货关系的企业。其业务形态可为</div>
    <div class="market"></div>
    <div class="line"></div>
    <div class="blank20"></div>
    <div class="panel-w">医站通同样适用于业务员跟单模式，可有业务员或跟单员为订货客户代为下单订货</div>
    <div class="market2"></div>
</div>
<!--应用场景 end-->

<!--第三方系统集成 start-->
<!--<div class="panel4">
	<h2 class="panel-t">第三方系统集成</h2>
	<div class="panel-w">开放API接口，并已实现与U8、NC、U9、SAP、金蝶等ERP系统的对接。</div>
    <div class="Interface"></div>     
</div>-->
<!--第三方系统集成 end-->

<!--商业智能 start-->
<!--<div class="panel4">
	<h2 class="panel-t">商业智能</h2>
	<div class="panel-w">让数据说话，通过医站通强大的数据分析功能，展示各种直观明了的图表，为厂商的商业决策提供依据。</div>
    <div class="data"></div>
</div>-->
<!--商业智能 end-->

<!--数据安全 start-->
<div class="panel4">
	<h2 class="panel-t">数据安全</h2>
	<div class="panel-w">数据安全是我们的核心竞争力，保障用户的商业机密，是我们的生存底线。</div>
    <ul class="safe">
    	<li><i class="safe-i1"></i><p>二级域名登录，加强厂商对代理商的管理。</p></li>
        <li><i class="safe-i2"></i><p>采用阿里云服务器，通过云安全生态圈，实现安全业务全覆盖。</p></li>
        <li><i class="safe-i3"></i><p>对入驻企业严格把关，对医站通员工进行严格管理，保证用户数据安全。</p></li>
    </ul>
</div>
<!--数据安全 end-->

<uc1:Bottom ID="Bottom1" runat="server" />
<script type="text/javascript" src="js/menu.js"></script>  
<script src="js/layer/layer.js" type="text/javascript"></script>
<script src="js/layerCommon.js" type="text/javascript"></script>
</form>
</body>
</html>
