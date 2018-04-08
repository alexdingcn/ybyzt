<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="subject_dec_index" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>6月年中惠报</title>
    <meta name="keywords" content="6月年中惠报" />
    <meta name="description" content="食品酒水招商,机械五金招商,家电产品招商,全行业产品批发招商" />
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

        <!--banner star-->
		<div class="banner">
			<div class="banner_show w1200 wrapper">
				<img src="img/banner01.jpg" />
				<img src="img/banner02.jpg" />
				<img src="img/banner03.jpg" />
			</div>
		</div>
		<!--banner end-->
		
		<!--sec01 star-->
		<div class="section01">
			
				<div class="title01 w1200 wrapper">
					<img src="img/title01.png" />
				</div>
				<ul class="sec01_show w1200 wrapper">
					<li class="s01"><a href="https://www.yibanmed.com/e107305_2472.html" target="_blank"><img src="img/sec01.jpg" alt="电源连接器"></a></li>
					<div class="center">
						<li><a href="https://www.yibanmed.com/e107427_2489.html" target="_blank"><img src="img/sec02.jpg" alt="LED筒灯外壳配件"></a></li>
						<li><a href="https://www.yibanmed.com/e107616_2508.html" target="_blank"><img src="img/sec03.jpg" alt="明纬电源CNMW"></a></li>
						<li><a href="https://www.yibanmed.com/e107315_2462.html" target="_blank"><img src="img/sec04.jpg" alt="穿墙插拔式接线端子"></a></li>
					</div>
				</ul>
			
		</div>
		<!--sec01 end-->
		
		<!--sec02 star-->
		<div class="section02">
			
				<div class="title02 w1200 wrapper">
					<img src="img/title02.png" />
				</div>
				<ul class="sec02_show w1200 wrapper">
					<li class="s01"><a href="https://www.yibanmed.com/e101587_1780.html" target="_blank"><img src="img/sec05.jpg" alt="九阳豆浆机"></a></li>
					<div class="center">
						<li><a href="https://www.yibanmed.com/e97898_1149.html" target="_blank"><img src="img/sec06.jpg" alt="奔腾电压力锅"></a></li>
						<li><a href="https://www.yibanmed.com/e101576_1768.html" target="_blank"><img src="img/sec07.jpg" alt="好太太智能搅拌机"></a></li>
						<li><a href="https://www.yibanmed.com/e101579_1768.html" target="_blank"><img src="img/sec08.jpg" alt="好太太 家用嵌入式消毒碗柜"></a></li>
					</div>
				</ul>
			
		</div>
		<!--sec02 end-->
		
		<!--sec03 star-->
		<div class="section03">
			
				<div class="title03 w1200 wrapper">
					<img src="img/title03.png" />
				</div>
				<ul class="sec03_show w1200 wrapper">
					<li class="s01"><a href="https://www.yibanmed.com/e101314_1466.html" target="_blank"><img src="img/sec09.jpg" alt="爵味蜂蜜枇杷雪梨桂花茶"></a></li>
					<div class="center">
						<li><a href="https://www.yibanmed.com/e100751_1603.html" target="_blank"><img src="img/sec10.jpg" alt="百草味东北松子"></a></li>
						<li><a href="https://www.yibanmed.com/e102812_1938.html" target="_blank"><img src="img/sec11.jpg" alt="简妹五彩萌椒"></a></li>
						<li><a href="https://www.yibanmed.com/e100662_1582.html" target="_blank"><img src="img/sec12.jpg" alt="三只松鼠拉面丸子"></a></li>
					</div>
				</ul>
			
		</div>
		<!--sec03 end-->
		
		<!--sec04 star-->
		<div class="section04">
				<div class="title04 w1200 wrapper">
					<img src="img/title04.png" />
				</div>
				<ul class="sec04_show w1200 wrapper">
					<li class="s01"><a href="https://www.yibanmed.com/e99278_1247.html" target="_blank"><img src="img/sec13.jpg" alt="阳光味道100%桑葚果汁"></a></li>
					<div class="center">
						<li><a href="https://www.yibanmed.com/e89543_1045.html" target="_blank"><img src="img/sec14.jpg" alt="酒邦文化白金干红葡萄酒"></a></li>
						<li><a href="https://www.yibanmed.com/e99579_1295.html" target="_blank"><img src="img/sec15.jpg" alt="酒玩家奔富407"></a></li>
						<li><a href="https://www.yibanmed.com/e99625_1373.html" target="_blank"><img src="img/sec16.jpg" alt="拉莱娜城堡干红葡萄酒"></a></li>
					</div>
				</ul>
		</div>
		<!--sec04 end-->

    <!--bottom start-->
    <div class="bottom_bg">
    </div>
    <!--bottom end-->
    <uc1:Bottom ID="Bottom1" runat="server" />
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    </form>
</body>
</html>
