<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompOrderInfo_ZD.aspx.cs" Inherits="Company_Report_CompOrderInfo_ZD" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>销售管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
        <%--<div class="place" id="btntitle" runat="server">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li>
                <li><a href="#">我要管账</a></li>
                <li><a href="#">订单收款明细</a></li>
            </ul>
        </div>--%>
        <!--当前位置 end-->

        <div class="rightinfo" style=" margin-top:0px; margin-left:0px; width:auto;">
            <!--功能按钮 start-->
            <%--<div class="tools" id="btn" runat="server" style="padding-left:5px;">
                <ul class="toolbar left">
                    <li></li>
                </ul>
            </div>--%>
            <!--功能按钮 start-->

            <!--销售订单主体 start-->
            <div class="div_content" >
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width:25%;">
                                <span>账单编号</span>
                            </td>
                            <td>
                                <label id="lblReceiptNo" runat="server">
                                </label>&nbsp;
                            </td>
                            <td style="width:25%;">
                                <span>账单状态</span>
                            </td>
                            <td>
                                <label id="lblOState" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>支付状态</span>
                            </td>
                            <td>
                                <label id="lblPayState" runat="server"></label>
                                &nbsp;
                            </td>
                            <td>
                                <span>收款日期</span>
                            </td>
                            <td>
                                <label id="lblArriveDate" runat="server"></label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>账单金额</span>
                            </td>
                            <td>
                                <label id="lblTotalPrice" runat="server"></label>
                                &nbsp;
                            </td>
                            <td>
                                <span>本次支付金额</span>
                            </td>
                            <td>
                                <label id="lblPayedPrice" runat="server"></label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <span> 账单已支付金额</span>
                            </td>
                            <td>
                                <label id="lblPayAuomet" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                               <span> 支付方式</span>
                            </td>
                            <td>
                                <label id="lblPaySource" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>制单人</span>
                            </td>
                            <td>
                                <label id="lblDisUser" runat="server"></label>
                                &nbsp;
                            </td>
                            <td>
                                <span>下单日期</span>
                            </td>
                            <td>
                                <label id="lblCreateDate" runat="server"></label>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--销售订单主体 end-->

        </div>
    </form>
</body>
</html>
