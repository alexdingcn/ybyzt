<%@ Page Language="C#" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="EShop_about" %>
<%@ Register TagName="Header" TagPrefix="uc" Src="UserControl/EshopHeader.ascx" %>
<%@ Register TagName="Bttom" TagPrefix="uc" Src="UserControl/EshopBttom.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="baidu-site-verification" content="IdU3LryeUL" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=shopname%>-店铺介绍</title>
    <meta name="keywords" runat="server" id="mKeyword"   />
    <%--<meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />--%>
    <link href="/eshop/css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="/eshop/css/goods.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css">
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
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
<body class="root">
    <form id="form1" runat="server">
        <uc:Header runat="server" ID="Header" />
        <div class="about-pic"></div>
        <div class="blank10"></div>
        <div class="about w1200">
	        <div class="blank20"></div>
	        <div class="title"></div>
            <div class="nr" style="padding: 30px 20px;" id="DivContent" runat="server">
    	        
            </div>
            <div class="blank10"></div>
	        <div class="title2"></div>
            <div class="nr2">
    	        <p runat="server" id="lblPrincipal">联系人：</p>
		        <p runat="server" id="lblPhone">电话：</p>
		        <p runat="server" id="lblAddress">地址：</p>
                <p runat="server" id="lbllogin" ></p>
            </div>
    
        </div>
         <uc:Bttom runat="server" ID="Bttom" />
    <script src="/js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    </form>
</body>
</html>
