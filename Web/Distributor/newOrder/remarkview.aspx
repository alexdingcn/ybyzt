<%@ Page Language="C#" AutoEventWireup="true" CodeFile="remarkview.aspx.cs" Inherits="Distributor_newOrder_remarkview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备注</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <!--备注 start-->
    <div class="popup po-remark">
        <input type="hidden" id="hidDisId" runat="server" value="" />
        <input type="hidden" id="hidType" runat="server" value="" />
        <input type="hidden" id="hidGoodsInfo" runat="server" value="" />
        <input type="hidden" id="hidIndex" runat="server" value="" />
        <%--<div class="po-title">备注<a href="" class="close"></a></div>--%>
        <div class="remark-box">
            <textarea id="txtremark" runat="server" class="box" placeholder="填写备注信息"></textarea>
        </div>
        <div class="po-btn">
            <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="#"
                runat="server" class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <!--备注 end-->
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>
    <script>
        $(function () {
            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });
            //确定
            $("#btnConfirm").click(function () {
                var type = $("#hidType").val();
                var KeyID = '<%=Request["KeyID"] %>';
                var remark = $.trim($("#txtremark").val());
                var goodsInfoId = $("#hidGoodsInfo").val();
                var index = $("#hidIndex").val();
                var disId = $("#hidDisId").val();
                if (type == "1") {
                    if (remark.length > 150) {
                        layerCommon.msg("请输入不超过150字的商品描述。", IconOption.哭脸);
                        return false;
                    }
                } else {
                    if (remark.length > 200) {
                        layerCommon.msg("请输入不超过200字的订单备注。", IconOption.哭脸);
                        return false;
                    }
                }

                if (~ ~type != 2) {
                    window.parent.remarkinfo(type, KeyID, remark, goodsInfoId, index, disId);
                } else {
                    window.parent.remarkinfo(type, remark, goodsInfoId, index, disId);
                    window.parent.CloseGoods();
                }
            });
        });

        function type3() {
            var index = $("#hidIndex").val();
            $("#txtremark").val("");
            $("#txtremark").val($.trim(window.parent.$(".divremark" + index).next(".cur").text()));
        }
    </script>
    </form>
</body>
</html>
