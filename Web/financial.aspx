<%@ Page Language="C#" AutoEventWireup="true" CodeFile="platform.aspx.cs" EnableViewState="false" Inherits="platform"  %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>金融服务-医站通</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
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
    <style>
        @media only screen and (max-device-width : 736px) and (orientation : portrait) {
            .topnav, .plat-header,.foot1,.foot2, .financial-banner {
                display: none;
            }
            .w1200 {
                width: 100%;
            }
            .textContent {padding:0 30px;margin-top:70px;}
            .f-con1 { padding: 0 }
            .f-nr { width: 60%; line-height:24px; }
            .f-icon1, .f-icon2, .f-icon3, .f-icon4 { width: 30%; margin-top:0;}
            header{
                display: block!important;
                background-color:#008fde;
                position:fixed; 
                z-index:2;
                top:0; left:0;
                width:100%;
                height:60px;
                padding:0;
            }
            header a, header a:visited, header a:link { color:white; font-size: 24px; line-height: 60px; text-decoration:none; padding-left:15px;}
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

<uc1:Top ID="top1" runat="server" />
<uc1:TopSec ID="top2" runat="server" />

<header style="display:none">
    <a href="javascript:;" onclick="history.go(-1);">返回</a>
</header>

<!--banner 开始-->
<div class="financial-banner">
 <div class="f-bannerbg1">
 </div>
</div>
<!--banner 结束-->


<!--医贷通 start-->
<div class="textContent">	
<div class="f-con1 w1200">
	<div class="f-title"><h3>医贷通</h3>医疗人自己的“微粒贷”“蚂蚁借呗”<p class="f-line"></p></div>
	<div class="f-nr fl"><b>【产品简介】</b>全线上操作、纯信用（无抵押担保），有发票数据就能贷款，T+1工作日放款，最高额度100万元，最长期限6个月。<br><br>
	    <b>【产品简介】</b>
		<p> <b class="green"> √ </b> 医疗器械相关企业</p>
		<p> <b class="green"> √ </b> 成立时间1年及以上</p>
		<p> <b class="green"> √ </b> 有1年及以上的连续增值税开票记录</p>
		<p> <b class="green"> √ </b> 年销售额100万及以上</p>
		<p> <b class="green"> √ </b> 企业无不良经营记录，法定代表人及实际控制人无不良征信记录</p>
	</div>
	<div class="f-icon1 fr"></div>
	
</div>
<!--医贷通 end-->
<!--发票贷 start-->
<div class="b-bg">
<div class="f-con1 w1200 ">
	<div class="f-title"><h3>发票贷</h3>代理商的福音，有发票就能贷款<p class="f-line"></p></div>
	<div class="f-icon2 fl"></div>
	<div class="f-nr fr"><b>【产品简介】</b>医疗代理商与医院合作有应收账款，凭发票就能贷款，最高额度1000万元，最长期限1年。<br><br>
	    <b>【产品简介】</b>
		<p> <b class="green"> √ </b> 医疗器械相关企业</p>
		<p> <b class="green"> √ </b> 成立时间1年及以上</p>
		<p> <b class="green"> √ </b> 年销售额300万及以上</p>
		<p> <b class="green"> √ </b> 企业无不良经营记录，法定代表人及实际控制人无不良征信记录</p>
	</div>
	
	
</div>
</div>	
<!--发票贷 end-->		
<!--订单贷 start-->
<div class="f-con1 w1200">
	<div class="f-title"><h3>订单贷</h3>有订单，但没钱买设备？找医伴金服就对了！<p class="f-line"></p></div>
	<div class="f-nr fl"><b>【产品简介】</b>医疗代理商已有医院订单但缺少资金采购设备，凭中标通知书、采购合同就能贷款，最高额度1000万元，最长期限1年。<br><br>
	    <b>【产品简介】</b>
		<p> <b class="green"> √ </b> 医疗器械相关企业</p>
		<p> <b class="green"> √ </b> 成立时间1年及以上</p>
		<p> <b class="green"> √ </b> 年销售额300万及以上</p>
		<p> <b class="green"> √ </b> 企业无不良经营记录，法定代表人及实际控制人无不良征信记录</p>
	</div>
	<div class="f-icon3 fr"></div>
	
</div>
<!--订单贷 end-->
<!--设备贷 start-->
<div class="b-bg">
<div class="f-con1 w1200 ">
	<div class="f-title"><h3>设备贷</h3>以租代买，找医伴金服！设备变钱，找医伴金服！<p class="f-line"></p></div>
	<div class="f-icon4 fl"></div>
	<div class="f-nr fr"><b>【产品简介】</b>通过融资租赁（直租）的形式，为有采购设备需求的医院，提供资金解决方案。通过融资租赁（回租）的形式，帮助代理商设备变钱，解决资金周转问题。<br><br>
	    <b>【产品简介】</b>
		<p> <b class="green"> √ </b> 医院、医疗器械相关企业</p>
		<p> <b class="green"> √ </b> 医院成立时间3年及以上，医疗代理商成立时间1年及以上</p>
		<p> <b class="green"> √ </b> 医院年销售额5000万及以上，医疗代理商年销售额500万及以上</p>
		<p> <b class="green"> √ </b> 无不良经营记录，法定代表人及实际控制人无良征信记录</p>
	</div>
	
	
</div>
</div>	
<!--设备贷 end-->	




<uc1:Bottom ID="Bottom1" runat="server" />
<script type="text/javascript" src="js/menu.js"></script>  
<script src="js/layer/layer.js" type="text/javascript"></script>
<script src="js/layerCommon.js" type="text/javascript"></script>
</form>
</body>
</html>
