<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="subject_index" EnableViewState="false" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>7月精选店铺 医站通-B2B电子商务平台，分销、批发就上医站通</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <link href="../../css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/jquery-1.9.1.min.js"></script>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

<uc1:Top ID="top1" runat="server" />

<!--页头 start-->
<uc1:TopSec ID="top2" runat="server" />
<!--页头 end-->


<!--banner start-->
<div class="banner">
 <div class="w1170">
  <img src="images/banner01.jpg" alt="暂无图片" /> 
  <img src="images/banner02.jpg" alt="暂无图片" /> 
  <img src="images/banner03.jpg" alt="暂无图片" /> 
 </div>
</div>
<!--banner end-->
<!--四大店铺 start-->
<div class="store">
 <ul class="storenr">
  <li><a href="http://www.医站通.com/1151.html" target="_blank"><img src="images/dp1.jpg" alt="暂无图片"/></a></li>
  <li><a href="http://www.医站通.com/1123.html" target="_blank"><img src="images/dp2.jpg" alt="暂无图片"/ ></a></li>
  <li><a href="http://www.医站通.com/1154.html" target="_blank"><img src="images/dp3.jpg" alt="暂无图片"/></a></li>
  <li id="storezh"><a href="http://www.医站通.com/1153.html" target="_blank"><img src="images/dp4.jpg" alt="暂无图片"/></a></li>
 </ul>
</div>
<!--四大店铺 end-->

<!--第一精选店铺 start-->
<div class="store store1">
<div class="title"><img src="images/title1.jpg" alt="暂无图片" /></div>
<div class="store1nr">
 <ul>
  <li><a href="http://www.医站通.com/e97923_1151.html" target="_blank"><img src="images/lym01.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e98027_1151.html" target="_blank"><img src="images/lym02.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97927_1151.html" target="_blank"><img src="images/lym03.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97934_1151.html" target="_blank"><img src="images/lym04.jpg" width="270" height="304" alt="暂无图片" /></a></li>
 </ul>
 <ul>
  <li><a href="http://www.医站通.com/e97926_1151.html" target="_blank"><img src="images/lym05.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97922_1151.html" target="_blank"><img src="images/lym06.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97933_1151.html" target="_blank"><img src="images/lym07.jpg" width="270" height="304" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97921_1151.html" target="_blank"><img src="images/lym08.jpg" width="270" alt="暂无图片" height="304" /></a></li>
 </ul>
</div>
</div>
<!--第一精选店铺end-->

<!--第二精选店铺 start-->
<div class="store store2">
<div class="title"><img src="images/title2.jpg" alt="暂无图片" /></div>
<div class="store2nr">
 <ul>
  <li><a href="http://www.医站通.com/e98178_1123.html" target="_blank"><img src="images/mto01.jpg" width="122" height="380" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97849_1123.html" target="_blank"><img src="images/mto02.jpg" width="244" height="380" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97719_1123.html" target="_blank"><img src="images/mto03.jpg" width="122" height="380" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e98168_1123.html" target="_blank"><img src="images/mto04.jpg" width="244" alt="暂无图片" height="380" /></a></li>
 </ul>
 <ul>
  <li><a href="http://www.医站通.com/e97699_1123.html" target="_blank"><img src="images/mto05.jpg" width="244" height="380" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97803_1123.html" target="_blank"><img src="images/mto06.jpg" width="120" height="380" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97880_1123.html" target="_blank"><img src="images/mto07.jpg" width="244" height="380" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e98179_1123.html" target="_blank"><img src="images/mto08.jpg" width="120" alt="暂无图片" height="380" /></a></li>
 </ul>
</div>
</div>
<!--第二精选店铺end-->

<!--第三精选店铺 start-->
<div class="store store3">
<div class="title"><img src="images/title3.jpg" alt="暂无图片" /></div>
<div class="store1nr">
 <ul>
  <li><a href="http://www.医站通.com/e97981_1154.html" target="_blank"><img src="images/jh01.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97965_1154.html" target="_blank"><img src="images/jh02.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97966_1154.html" target="_blank"><img src="images/jh03.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97964_1154.html" target="_blank"><img src="images/jh04.jpg" width="270" alt="暂无图片" height="273" /></a></li>
 </ul>
 <ul>
  <li><a href="http://www.医站通.com/e97974_1154.html" target="_blank"><img src="images/jh05.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97981_1154.html" target="_blank"><img src="images/jh06.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e97978_1154.html" target="_blank"><img src="images/jh07.jpg" width="270" height="273" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97976_1154.html" target="_blank"><img src="images/jh08.jpg" width="270" alt="暂无图片" height="273" /></a></li>
 </ul>
</div>
</div>
<!--第三精选店铺end-->

<!--第四精选店铺 start-->
<div class="store store4">
<div class="title"><img src="images/title4.jpg" alt="暂无图片" /></div>
<div class="store1nr">
 <ul>
  <li><a href="http://www.医站通.com/e98209_1153.html" target="_blank"><img src="images/jfw01.jpg" width="270" height="275" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e98204_1153.html" target="_blank"><img src="images/jfw02.jpg" width="270" height="275" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e98201_1153.html" target="_blank"><img src="images/jfw03.jpg" width="270" height="275" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97945_1153.html" target="_blank"><img src="images/jfw04.jpg" width="270" alt="暂无图片" height="275" /></a></li>
 </ul>
 <ul>
  <li><a href="http://www.医站通.com/e98208_1153.html" target="_blank"><img src="images/jfw05.jpg" width="270" height="270" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e98203_1153.html" target="_blank"><img src="images/jfw06.jpg" width="270" height="270" alt="暂无图片" /></a></li>
  <li><a href="http://www.医站通.com/e98200_1153.html" target="_blank"><img src="images/jfw07.jpg" width="270" height="270" alt="暂无图片" /></a></li>
  <li id="storezh"><a href="http://www.医站通.com/e97943_1153.html" target="_blank"><img src="images/jfw08.jpg" width="270" alt="暂无图片" height="270" /></a></li>
 </ul>
</div>
</div>
<!--第四精选店铺end-->


<uc1:Bottom ID="Bottom1" runat="server" />
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
</form>
</body>
</html>
