<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayOrderInfo.aspx.cs" Inherits="Distributor_Rep_PayOrderInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
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

            $("#btnPrint").on("click", function () {
                var url = '../PayPrint/PayInfoPrint.aspx?KeyId=<%=Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) %>';                             //转向网页的地址; 
                var name = '电子回单打印';                     //网页名称，可为空; 
                var iWidth = 950;                          //弹出窗口的宽度; 
                var iHeight = 724;                         //弹出窗口的高度; 
                //获得窗口的垂直位置 
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
                //获得窗口的水平位置 
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
                window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
            });
        });

    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
        <Head:Head ID="Head1" runat="server" />
        <div class="w1200">
             <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
             <div class="rightCon">
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
                <div class="blank10"></div>

                <!--订单管理详细 start-->
                <div class="orderNr">
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
                        </tbody>
                     </table>
                </div>
                <!--订单管理详细 end-->
            </div>
        </div>
    </form>
</body>
</html>
