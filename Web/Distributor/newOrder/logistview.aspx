<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logistview.aspx.cs" Inherits="Distributor_newOrder_logistview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>物流信息</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--查看物流信息 start-->
        <div class="popup po-logist">
	        <%--<div class="po-title">物流信息<a href="" class="close"></a></div>--%>
	        <div class="logist-box">
    	        <div class="line"></div>
    	        <ul class="list" id="logislist" runat="server">
        	        <%--<li><i class="day">2016-07-15 周五</i><i class="time">09:56:49</i>【铁岭市】辽宁省铁岭市 已发出<i class="circle cur"></i></li>
        	        <li><i class="time">09:56:49</i>【沈阳市】快件已到达 沈阳转运中心<i class="circle"></i></li>
        	        <li><i class="time">09:56:49</i>【沈阳市】沈阳转运中心 已发出<i class="circle"></i></li>
        	        <li><i class="time">09:56:49</i>【武汉市】快件已到达 武汉转运中心<i class="circle"></i></li>
        	        <li><i class="time">09:56:49</i>【武汉市】武汉转运中心 已发出<i class="circle"></i></li>
        	        <li><i class="time">09:56:49</i>【铁岭市】圆通速递 辽宁省铁岭市收件员 已揽件<i class="circle"></i></li>
                    <li><i class="day">2016-07-14 周四</i><i class="time">09:56:49</i>商家正通知快递公司揽件<i class="circle"></i></li>
                    <li><i class="time">09:56:49</i>商家正通知快递公司揽件<i class="circle"></i></li>--%>
                </ul>
            </div> 
            <div class="po-btn">
                <a href="javascript:void(0);" class="btn-area" id="btnConfirm">取消</a>
            </div>
        </div>
        <!--查看物流信息 end-->
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>

    <script>
        

        $(function () {
            $(".list").find("li:first").find("i[class=\"circle\"]").attr("class", "circle cur");

            //确定
            $("#btnConfirm").click(function () {
                window.parent.CloseGoods();
            });
        });
    </script>
    </form>
</body>
</html>
