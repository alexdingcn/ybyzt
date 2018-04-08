<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrePayInfo.aspx.cs" Inherits="Distributor_Pay_PrePayInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>基本资料</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            //返回
            $(".btnBl").click(function () {                   
                 history.go(-1);
                // window.history.back;

            });

          
            //支付
            $("#payIcon").click(function (){
                window.location.href = "pay/Pay.aspx?KeyID='<%= Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>'";
            });

            //支付查询
            $("#payInfo").click(function(){
                $("#btnPayInfo").trigger("click");
            });
        });
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="PrePayList" />
    <%--<asp:Button ID="btnPayInfo" runat="server" OnClick="btnPayInfo_Click" Text="支付查询"
        Style="display: none;" />--%>
    <div class="rightCon">
     <div class="info">
                <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
            <a id="navigation2" href="/Distributor/pay/PrePayList.aspx" class="cur">我的钱包</a>>
            <a id="navigation3" href="#" class="cur">钱包详情</a></div> 
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <%-- <a href="javascript:void(0)" class="btnBl" runat="server" id="payInfo">
                    <i class="payIcon"></i>支付查询</a>--%>
                <a href="javascript:void(0)" class="btnBl" id="returnIcon" runat="server"><i class="returnIcon">
                </i>返回</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理详细 start-->
        <div class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td class="head" style="width: 10%">
                            名  称
                        </td>
                        <td style="width: 23%">
                            <label id="lbldis" runat="server">
                            </label>
                            &nbsp;
                        </td>
                        <td class="head" style="width: 10%">
                            金  额
                        </td>
                        <td style="width: 30%;">
                            <label id="lblprice" runat="server">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="head" style="width: 10%">
                            款项类型
                        </td>
                        <td style="width: 23%">
                            <label id="lblpaytype" runat="server">
                            </label>
                            &nbsp;
                        </td>
                        <td class="head" style="width: 10%">
                            制单日期
                        </td>
                        <td style="width: 30%;">
                            <label id="lblcreatetime" runat="server">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            制单人
                        </td>
                        <td style="width: 30%;" colspan="3">
                            <label id="lblcreateuser" runat="server">
                            </label>
                        </td>
                        <%--<td class="head">
                            结算状态
                        </td>
                        <td style="width: 30%;">
                            <label id="lbljs" runat="server">
                            </label>
                        </td>--%>
                    </tr>
                    <%--<tr id="hide" class="hide" runat="server">
                        <td class="head">
                            审批状态
                        </td>
                        <td style="width: 30%;"  colspan="3">
                            <label id="lblauditstate" runat="server">
                            </label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="head">
                            备  注
                        </td>
                        <td colspan="3" style="word-wrap: break-word; word-break: break-all;">
                            <label id="lblRemark" runat="server">
                            </label>
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
