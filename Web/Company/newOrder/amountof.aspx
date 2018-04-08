<%@ Page Language="C#" AutoEventWireup="true" CodeFile="amountof.aspx.cs" Inherits="Company_newOrder_amountof" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%= title %></title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="popup po-billing">
        <input type="hidden" id="hidType" runat="server" value="" />
        <input type="hidden" id="hidOrderID" runat="server" value="" />
        <input type="hidden" id="hidT" runat="server" value="0" />
        <input type="hidden" id="hidts" runat="server" value="0" />

        <!--修改应付总额 start-->
        <%--<div class="po-title">修改应付总额<a href="" class="close"></a></div>--%>
        <div class="billing-box sum-box" runat="server" id="copetotal">
            <div class="at">
                <div class="bt left">
                    应付总额：</div>
                <input name="" type="text" id="txttotal" onkeyup="KeyInt2(this)" onfocus="InputFocus(this)"
                    onblur="priceBlur(this)" runat="server" maxlength="10" autocomplete="off" class="box"
                    value="0.00" />
            </div>
        </div>
        <!--修改应付总额 end-->
        <!--修改运费 start-->
        <div class="billing-box fare-box" runat="server" id="PostFee">
            <div class="at">
                <div class="bt left">
                    运费：</div>
                <input name="" type="text" id="txtPostFee" onkeyup="KeyInt2(this)" onfocus="InputFocus(this)"
                    onblur="priceBlur(this)" runat="server"
                    maxlength="8" autocomplete="off" class="box" value="0.00" />
            </div>
        </div>
        <!--修改运费 end-->
        <div class="po-btn">
            <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="javascript:void(0);"
                class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../Distributor/newOrder/js/ordercommon.js?v=201608170930" type="text/javascript"></script>
    <script>

        $(function () {
            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });

            $(document).on("focus", "#txtPostFee", function () {
                var val = $(this).val();

                if (parseFloat(val) == parseFloat(0.00))
                    $(this).val("");
            });

            //确定
            $("#btnConfirm").click(function () {
                var type = $("#hidType").val();
                var oID = $("#hidOrderID").val();
                //原有金额
                var t = $("#hidT").val();

                var tatol = 0;
                if (~ ~type == 0)
                    tatol = $("#txttotal").val();
                else
                    tatol = $("#txtPostFee").val();

                if (tatol == null || typeof (tatol) == "undefined" || tatol == "")
                    tatol = "0";

                if (~ ~type == 2) {
                    window.parent.amount_info(type, tatol, 0, 0);
                    window.parent.CloseGoods();
                } else {
                    var ts = $.trim($("#hidts").val());
                    $.ajax({
                        type: 'post',
                        url: '../../Handler/orderHandle.ashx',
                        data: { ck: Math.random(), ActionType: "amountof", oID: oID, type: type, tatol: tatol, t: t, ts: ts },
                        dataType: 'json',
                        success: function (data) {
                            if (data.Result) {
                                window.parent.amount_info(type, tatol, 0, data.Code);
                                window.parent.CloseGoods();
                            } else
                                layerCommon.msg(data.Msg, IconOption.错误);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                        }
                    });
                    return false;
                }
            });
        });
    </script>
    </form>
</body>
</html>
