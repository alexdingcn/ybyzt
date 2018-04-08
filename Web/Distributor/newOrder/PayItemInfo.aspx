<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayItemInfo.aspx.cs" Inherits="Distributor_neworder_PayItemInfo" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>订单支付明细</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("#returnIcon").on("click", function () {
                window.location.href = 'RepDetailsList.aspx';
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server" method="post">
        <%--<Head:Head ID="Head1" runat="server" />--%>
       <%-- <div class="w1200">
             <Left:Left ID="Left1" runat="server" ShowID="nav-3" />--%>
             <%--<div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="/Distributor/Rep/RepDetailsList.aspx" class="cur">订单支付</a>>
                    <a id="navigation3" href="#" class="cur">订单支付明细</a>
                </div>
                <!--功能条件 start-->
                <div class="userFun">
                    <div class="left">
                        <a href="#" class="btnBl" id="returnIcon" runat="server"><i class="returnIcon"></i>返回</a>
                    </div>
                </div>
                    <!--功能条件 end-->
                <div class="blank10"></div>--%>

                <!--订单管理详细 start-->
                <div  >
                     <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tbody> 
                            <tr>
                                <td class="head" style="width: 13%">
                                    订单编号
                                </td>
                                <td style="width: 37%">
                                    <label id="lblReceiptNo" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                                <td class="head">
                                    订单状态
                                </td>
                                <td>
                                    <label id="lblOState" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head">
                                    支付状态
                                    &nbsp;
                                </td>
                                <td>
                                    <label id="lblPayState" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                                <td class="head" style="width: 10%">
                                    支付日期
                                </td>
                                <td style="width: 23%">
                                    <label id="lblArriveDate" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head">
                                    订单金额
                                </td>
                                <td>
                                    <label id="lblTotalPrice" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                                <td class="head">
                                    本次支付金额
                                </td>
                                <td>
                                    <label id="lblPayedPrice" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head">
                                    订单已支付金额
                                </td>
                                <td>
                                    <label id="lblPayAuomet" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                               <td class="head">
                                    支付方式
                                </td>
                                <td>
                                    <label id="lblPaySource" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="head">
                                    制单人
                                </td>
                                <td>
                                    <label id="lblDisUser" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                                <td class="head">
                                    下单日期
                                </td>
                                <td>
                                    <label id="lblCreateDate" runat="server">
                                    </label>
                                    &nbsp;
                                </td>
                            </tr>
                           <tr>
                             <td class="head">
                                    附件
                                </td>
                                <td>
                            <asp:Panel runat="server" id="DFile" ></asp:Panel>
                           </td>
                           </tr>
                        </tbody>
                     </table>
                </div>
                <!--订单管理详细 end-->
           <%-- </div>
        </div>--%>
    </form>
</body>
</html>
