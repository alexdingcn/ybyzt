<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompOrderInfo.aspx.cs" Inherits="Company_Report_CompOrderInfo" %>

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
        
        <div class="rightinfo" style=" margin-top:0px; margin-left:0px; width:auto;">
           
            <!--销售订单主体 start-->
            <div class="div_content" >
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width:25%;">
                                <span>订单编号</span>
                            </td>
                            <td>
                                <label id="lblReceiptNo" runat="server">
                                </label>&nbsp;
                            </td>
                            <td style="width:25%;">
                                <span>订单状态</span>
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
                                <span>订单金额</span>
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
                               <span> 订单已支付金额</span>
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
                          <tr>
                          <td>
                            <span>附件</span>
                                </td>
                                <td colspan="3">
                            <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>
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
