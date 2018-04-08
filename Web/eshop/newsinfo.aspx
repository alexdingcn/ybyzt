<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newsinfo.aspx.cs" Inherits="newsinfo" %>

<%@ Register TagName="Header" TagPrefix="uc" Src="UserControl/EshopHeader.ascx" %>
<%@ Register TagName="Bttom" TagPrefix="uc" Src="UserControl/EshopBttom.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>店铺新闻-分销、批发就上医站通</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <%--<meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />--%>
    <link href="/eshop/css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="/eshop/css/goods.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("#lblNewContent").find("a").attr("target", "_blank");
        })
    </script>
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
<style>
.newsNr{ width:1000px; margin:20px auto 0px auto; border:1px solid #ddd; background:#fff;}
.newsNr .ntitle{ font-size:20px; color:#666; text-align:center; padding:20px 80px ; line-height:30px; border-bottom:1px solid #ddd; background:#fdfdfd;}
.newsNr .ntime{ text-align:center; padding:20px 0; color:#999;}
.newsNr .nnr{ line-height:40px; color:#666; font-size:14px;padding:0px 40px;  padding-bottom:20px;}

</style>
  
<body class="root3">
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="Header" />
    <div class="newsNr">
	<div class="ntitle"> <label id="lblNewTitle" runat="server"></label></div>
    <div class="ntime">  <label id="lblCreateDate" runat="server"></label>   </div>
    <div class="nnr">
    	 <label id="lblNewContent" runat="server"></label>
    </div>
    </div>
    <div class="blank20"></div>
    <uc:Bttom runat="server" ID="Bttom" />
    </form>
</body>
</html>
