<%@ Page Language="C#" AutoEventWireup="true" CodeFile="recruit.aspx.cs" Inherits="recruit" EnableViewState="false" %>
<%@ OutputCache Duration="180" VaryByParam="none" Location="Server"%>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>活动页 医站通</title>
    <meta name="keywords" content="我的医站通网_医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的医站通网,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
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

<uc1:Top ID="top1" runat="server" />

<!--页头 start-->
<uc1:TopSec ID="top2" runat="server" />
<!--页头 end-->
<div class="rec-banner"></div>
<div class="rec-title"></div>
<div class="rec-nr">
	<div class="pic"><img src="images/recruit-map.jpg" alt="暂无图片"/></div>
    <h2 class="bt"><i>黎旭</i>医伴金服渠道&市场 VP</h2>
    <ul class="list">
    	<li><i class="tel-i"></i>联系电话：13817241023</li>
        <li><i class="mail-i"></i>邮　　箱：lx@moreyou.cn</li>
        <li><i class="map-i"></i>公司地址：上海市浦东新区耀华路488号信建大厦8楼 </li>
    </ul>
</div>

<uc1:Bottom ID="Bottom1" runat="server" />
    <script src="js/layer/layer.js" type="text/javascript"></script>
    <script src="js/layerCommon.js" type="text/javascript"></script>
<script type="text/javascript" src="js/menu.js"></script>  
</body>
</html>
