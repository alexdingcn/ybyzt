<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" EnableViewState="false" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>备战双十一,消费者最爱的源头好货，疯狂采购中 </title>
<meta name="keywords" content="生活用品批发商城,家居家电批发, 家居批发商城,家电批发商城,食品批发商城,生活用品采购" />
<meta name="description" content="备战双十一，消费者最爱的家居礼品，食品，酒水饮料，五金器械装备，货源地直接采购，速速来备货" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/jquery-1.9.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

<uc1:Top ID="top1" runat="server" />

<!--页头 start-->
<uc1:TopSec ID="top2" runat="server" />
<!--页头 end-->
<div style="background:#0b182a;">
<!--banner start-->
<div class="banner">
	<div class="bg1"></div><div class="bg2"></div><div class="bg3"></div><div class="bg4"></div><div class="bg5"></div><div class="bg6"></div>
</div>
<!--1楼 start-->
<div class="title1"></div>
<div class="fbox f1-p1">
	<a href="https://www.yibanmed.com/e97551_1107.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99308_1292.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e97427_1086.html" target="_blank"></a>
</div>
<div class="fbox f1-p2">
	<a href="https://www.yibanmed.com/e97487_1098.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e98313_1170.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99563_1357.html" target="_blank"></a>
</div>
<div class="f1-bot"></div>
<!--1楼 end-->

<!--2楼 start-->
<div class="title2"></div>
<div class="fbox f2-p1">
	<a href="https://www.yibanmed.com/e99131_1247.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99277_1284.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99340_1290.html" target="_blank"></a>
</div>
<div class="fbox f2-p2">
	<a href="https://www.yibanmed.com/e99625_1373.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99135_1266.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99467_1295.html" target="_blank"></a>
</div>
<div class="f2-bot"></div>
<!--2楼 end-->

<!--3楼 start-->
<div class="title3"></div>
<div class="fbox f3-p1">
	<a href="https://www.yibanmed.com/e99418_1348.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99434_1348.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99437_1348.html" target="_blank"></a>
</div>
<div class="fbox f3-p2">
	<a href="https://www.yibanmed.com/e98896_1218.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99385_1306.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e97884_1149.html" target="_blank"></a>
</div>
<div class="f3-bot"></div>
<!--3楼 end-->

<!--4楼 start-->
<div class="title4"></div>
<div class="fbox f4-p1">
	<a href="https://www.yibanmed.com/e99637_1378.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e99641_1378.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e97914_1145.html" target="_blank"></a>
</div>
<div class="fbox f4-p2">
	<a href="https://www.yibanmed.com/e97715_1118.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e97696_1123.html" target="_blank"></a>
    <a href="https://www.yibanmed.com/e91430_1056.html" target="_blank"></a>
</div>
<!--4楼 end-->
<div class="footer">
</div>
</div>
<uc1:Bottom ID="Bottom1" runat="server" />
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script> 
</form>
</body>
</html>
