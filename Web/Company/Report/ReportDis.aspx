<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportDis.aspx.cs" Inherits="Company_Report_ReportDis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BO报表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
     <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var h = $(document).height();
            $("#reportDis").height(h-95);
        });
    </script>
</head>
<body id="body" runat="server">
    <form id="form1" runat="server">
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面</a></li>
            <li><a href="#">我的报表</a></li>
            <li><a href="#">BO报表</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server"
                        class="textBox" maxlength="50" /></li>
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="Text1" runat="server" class="textBox"
                        maxlength="50" /></li>
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="Text2" runat="server" class="textBox"
                        maxlength="50" /></li>
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="Text3" runat="server" class="textBox"
                        maxlength="50" /></li>
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="Text4" runat="server" class="textBox"
                        maxlength="50" /></li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <iframe src="<%= url %>" name="reportDis" id="reportDis" title="reportDis" scrolling="no"
            style="width: 100%; height: auto;" ></iframe>
    </form>
</body>
</html>
