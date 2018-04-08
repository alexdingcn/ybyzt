<%@ Page Language="C#" AutoEventWireup="true" CodeFile="po_deli.aspx.cs" Inherits="Distributor_newOrder_po_deli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%= title %></title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="popup po-deli">
            <input type="hidden" id="hidType" runat="server" value="" />
            <%--<div class="po-title"><a href="" class="close"></a></div>--%>
            <!--修改交货日期 start-->
            <div class="deli-box" id="senddate" runat="server">
                <i class="bt2 left">交货日期：</i>
                <%--<div class="date left">
                    <a href="">2015-05-07<i class="rl-icon"></i></a>
                </div>--%>
                <input type="text" class="Wdate" id="txtDate" runat="server" readonly="readonly"
                    onclick="WdatePicker({ minDate: '%y-%M-%d' })" />
            </div>
            <!--修改交货日期 end-->
            <!--修改配送方式 start-->
            <div class="invoice-li carry-box" id="GiveMode" runat="server">
                <div class="li">
                    <div class="bt">
                        配送方式：
                    </div>
                    <ul class="xx">
                        <li><i class="dx">
                            <input type="radio" name="rdogive" runat="server" id="checkbox_5_1" value="送货" class="regular-checkbox" />
                            <label for="checkbox_5_1">
                            </label>
                        </i><i class="t">送货</i> </li>
                        <li><i class="dx">
                            <input type="radio" name="rdogive" runat="server" id="checkbox_5_2" value="自提" class="regular-checkbox" />
                            <label for="checkbox_5_2">
                            </label>
                        </i><i class="t">自提</i> </li>
                    </ul>
                </div>
            </div>
            <!--修改配送方式 end-->
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>
                <a href="#" runat="server" class="btn-area" id="btnConfirm">确定</a>
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

                //确定
                $("#btnConfirm").click(function () {
                    var type = $("#hidType").val();
                    var str = "";
                    var KeyID = '<%=Request["KeyID"] %>';
                   
                    if (parseInt(type) == 0)
                        //交货日期
                        str += $("#txtDate").val();
                    else {
                        //配送方式
                        if ($("#checkbox_5_1").is(':checked'))
                            str += $("#checkbox_5_1").val();
                        else
                            str += $("#checkbox_5_2").val();
                    }
                    $.ajax({
                            type: "Post",
                            url: "po_deli.aspx/Edit",
                            data: "{ 'KeyID':'" + KeyID + "', 'type':'" + type + "', 'tip':'" + str + "'}",
                            dataType: "json",
                            timeout: 5000,
                            contentType: "application/json; charset=utf-8",
                            success: function (ReturnData) {
                                var Json = eval('(' + ReturnData.d + ')');
                                if (Json.result) {
                                    //交货日期、配送方式
                                    window.parent.pedeli_info(type, str);
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
