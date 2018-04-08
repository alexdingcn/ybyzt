<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisReBate.aspx.cs" Inherits="Distributor_newOrder_DisReBate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>返利抵扣查询</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="popup po-rebate">
	    <%--<div class="po-title">返利抵扣查询<a href="" class="close"></a></div>--%>
        <div class="goods-zs"><div class="tabLine">
            <table border="0" cellspacing="0" cellpadding="0" >
                    <thead>
                    <tr>
                        <th class="t1">返利单号</th>
                        <th class="t3">返利金额</th>
                        <th class="t3">可用余额</th>
                        <th class="t4">有效期</th>
                        <th class="t3">已使用金额</th>
                    </tr>
                </thead>
                    <tbody>
                    <asp:Repeater runat="server" ID="rptbate">
                        <ItemTemplate>
                            <tr>
                                <td class="t5" align="center"><div class="tc"><%# Eval("ReceiptNo")%></div></td>
                                <td><div class="tc" style="word-wrap: break-word; word-break: break-all; ">￥<%# Eval("RebateAmount").ToString().ToDecimal(0).ToString("N") %></div></td>
                                <td class="t2"><div class="tc" style="word-wrap: break-word; word-break: break-all; ">￥<%# Eval("EnableAmount").ToString().ToDecimal(0).ToString("N")%></div></td>
                                <td class="t5"><div class="tc"><%#Eval("EndDate", "{0:yyyy-MM-dd}")%></div></td>
                                <td class="t4"><div class="tc" style="word-wrap: break-word; word-break: break-all; ">￥<%# Eval("UserdAmount").ToString().ToDecimal(0).ToString("N")%></div></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tbody>
            </table>
            </div>
        </div>
        <div class="po-btn">
            <a href="javascript:void(0);" class="btn-area" id="btnCancel">取消</a>
        </div>
    </div>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>

    <script>
        

        $(function () {
            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });
        });
    </script>
    </form>
</body>
</html>
