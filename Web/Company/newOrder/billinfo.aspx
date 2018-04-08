<%@ Page Language="C#" AutoEventWireup="true" CodeFile="billinfo.aspx.cs" Inherits="Company_newOrder_billinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发票信息</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--发票信息 start-->
        <div class="popup po-billing">
	        <%--<div class="po-title">发票信息<a href="" class="close"></a></div>--%>
	        <div class="billing-box">
    	        <div class="at">
                    <div class="bt left">发票号：</div>
                    <input name="" id="txtbill" runat="server" maxlength="50" autocomplete="off" type="text" class="box" />
                </div>
                <div class="xz">
                    <i class="dx" style=" float:left;">
                        <input type="checkbox" id="checkbox_4_1" runat="server" class="regular-checkbox" />
                        <label for="checkbox_4_1"></label>
                    </i>
                    <i style=" float:left;">是否已开完</i>
                </div>
            </div> 
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>
                <a href="#" runat="server" class="btn-area" id="btnConfirm">确定</a>
            </div>
        </div>
        <!--发票信息 end-->
    <script src="../../Distributor/newOrder/js/ordercommon.js?v=201608170930" type="text/javascript"></script>

    <script>

        $(function () {
            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });

            //确定
            $("#btnConfirm").click(function () {
                var bill =stripscript( $("#txtbill").val());
                  var KeyID='<%=Request["KeyID"]%>';
                var isbill = 0;
                if ($("#checkbox_4_1").is(":checked"))
                    isbill = 1;

                $.ajax({
                    type: "Post",
                    url: "billinfo.aspx/Edit",
                    data: "{ 'KeyID':'" + KeyID + "', 'bill':'" + bill + "', 'IsBill':'" + isbill + "'}",
                    dataType: "json",
                    timeout: 5000,
                    contentType: "application/json; charset=utf-8",
                    success: function (ReturnData) {
                        var Json = eval('(' + ReturnData.d + ')');
                        if (Json.result) {
                            //发票信息
                            window.parent.billinfo(bill, isbill);
                            window.parent.CloseGoods();
                        } else {
                            layerCommon.msg(Json.code, IconOption.错误);
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        var a;
                    }

                });
            });
        });
    </script>
    </form>
</body>
</html>
