<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reporttest.aspx.cs" Inherits="Reporttest" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据分析报表</title>
	<link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>

<style>
.reportIcon{ width:100%; margin-top:0px;}
.reportIcon .li{ width:340px; height:220px;  float:left; background:#fdfdfd; margin:10px 10px 0 0; text-align:center;}
.reportIcon .li a{ display:block; width:100%; height:100%;}
.reportIcon .li i{ display:block; text-align:center;font-size:16px; color:#333; padding-top:10px;}
.reportIcon .li:hover{ background:#f4f9fb;}
.reportIcon .icon1{ width:136px; height:165px; background:url(../../images/reportIcon.png) no-repeat 0 40px; display:inline-block; }
.reportIcon .icon2{ width:125px; height:165px; background:url(../../images/reportIcon.png) no-repeat -188px 35px; display:inline-block; }
.reportIcon .icon3{ width:102px; height:165px; background:url(../../images/reportIcon.png) no-repeat -380px 35px; display:inline-block; }
.reportIcon .icon4{ width:125px; height:165px; background:url(../../images/reportIcon.png) no-repeat -536px 45px; display:inline-block; }
.reportIcon .icon5{ width:125px; height:165px; background:url(../../images/reportIcon.png) no-repeat -715px 45px; display:inline-block; }
.reportIcon .icon6{ width:135px; height:165px; background:url(../../images/reportIcon.png) no-repeat -895px 45px; display:inline-block; }
</style>
    <uc1:top ID="top1" runat="server" ShowID="nav-5" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
	        <ul class="placeul">
		        <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
		        <li><a href="Reporttest.aspx">数据分析报表</a></li>
	        </ul>
        </div>
        <!--当前位置 end-->

    <div class="reportIcon">
	    <div class="li"><a href="Report1.aspx"><span class="icon1"></span><i>综合分析</i></a></div>
        <div class="li"><a href="Report2.aspx"><span class="icon2"></span><i>订单分析</i></a></div>
        <div class="li"><a href="Report3.aspx"><span class="icon3"></span><i>代理商分析</i></a></div>
        <div class="li"><a href="Report4.aspx"><span class="icon4"></span><i>产品分析</i></a></div>
        <div class="li"><a href="Report5.aspx"><span class="icon5"></span><i>产品ABC分析</i></a></div>
        <div class="li"><a href="Report6.aspx"><span class="icon6"></span><i>应收分析</i></a></div>
    </div>
</div>
</body>
</html>
