<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceInfo.aspx.cs" Inherits="Distributor_newOrder_InvoiceInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开票信息</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--开票信息 start-->
    <div class="popup po-invoice">
        <%--<div class="po-title">开票信息<a href="" class="close"></a></div>--%>
        <input type="hidden" id="hidDisAccID" runat="server" />
        <input type="hidden" id="hidDisID" runat="server" />
        <div class="invoice-li">
            <div class="li">
                <div class="bt">
                    发票类型：</div>
                <ul class="xx">
                    <li><i class="dx">
                        <input type="radio" value="0" name="radioinvoice" id="checkbox_2_1" runat="server"
                            class="regular-checkbox" />
                        <label for="checkbox_2_1">
                        </label>
                    </i><i class="t">不开发票</i> </li>
                    <li><i class="dx">
                        <input type="radio" value="1" name="radioinvoice" id="checkbox_2_2" runat="server"
                            class="regular-checkbox" />
                        <label for="checkbox_2_2">
                        </label>
                    </i><i class="t">普通发票</i> </li>
                    <li><i class="dx">
                        <input type="radio" value="2" name="radioinvoice" id="checkbox_2_3" runat="server"
                            class="regular-checkbox" />
                        <label for="checkbox_2_3">
                        </label>
                    </i><i class="t">增值税发票</i> </li>
                </ul>
            </div>
            <div class="AccType1">
            <div class="li none">
                <div class="bt">
                    发票抬头：</div>
                <input type="text" id="txtLookUp" runat="server" maxlength="100" autocomplete="off"
                    class="box" />
            </div>
            <div class="li none">
                <div class="bt">
                    发票内容：</div>
                <input type="text" id="txtContext" runat="server" maxlength="200" autocomplete="off"
                    class="box" value="" />
            </div>
            </div>
            <div class="AccType2">
                <div class="li none">
                    <div class="bt">
                        开户银行：</div>
                    <input type="text" id="txtBank" runat="server" maxlength="100" autocomplete="off"
                        class="box" />
                </div>
                <div class="li none">
                    <div class="bt">
                        开户账户：</div>
                    <input type="text" id="txtAccount" runat="server" maxlength="100" autocomplete="off"
                        class="box" />
                </div>
                <div class="li none">
                    <div class="bt">
                        纳税人登记号：</div>
                    <input type="text" id="txtRegNo" runat="server" maxlength="100" autocomplete="off"
                        class="box" />
                </div>
            </div>
        </div>
        <div class="po-btn">
            <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="javascript:void(0);"
                class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <!--开票信息 end-->
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>

    <script>


        $(function () {

            var tipa = '<%=Request["val"] %>';
            if (parseInt(tipa) == 2) {
                $("div.AccType1 div.none").attr("class", "li");
                $("div.AccType2 div.none").attr("class", "li");
            }
            else if (parseInt(tipa) == 1) {
                $("div.AccType1 div.none").attr("class", "li");
                $("div.AccType2 div.li:eq(0),div.AccType2 div.li:eq(1)").attr("class", "li none");
                $("div.AccType2 div.li:eq(2)").attr("class", "li");
            }
            else {
                $("div.AccType1 div.li").attr("class", "li none");
                $("div.AccType2 div.li").attr("class", "li none");
            }


            $(document).on("click", "input[type=\"radio\"][name=\"radioinvoice\"]", function () {
                var tip = $(this).val();
                if (parseInt(tip) == 2) {
                    $("div.AccType1 div.none").attr("class", "li");
                    $("div.AccType2 div.none").attr("class", "li");
                }
                else if (parseInt(tip) == 1) {
                    $("div.AccType1 div.none").attr("class", "li");
                    $("div.AccType2 div.li:eq(0),div.AccType2 div.li:eq(1)").attr("class", "li none");
                    $("div.AccType2 div.li:eq(2)").attr("class", "li");
                }
                else {
                    $("div.AccType1 div.li").attr("class", "li none");
                    $("div.AccType2 div.li").attr("class", "li none");
                }
            });

            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });

            //确定
            $("#btnConfirm").click(function () {
                var DisAccID = $("#hidDisAccID").val();
                var val = $(".xx li").find("input[type=\"radio\"][name=\"radioinvoice\"]:checked").val();
                var LookUp = $("#txtLookUp").val();
                var Context = $("#txtContext").val();
                var Bank = $("#txtBank").val();
                var Account = $("#txtAccount").val();
                var RegNo = $("#txtRegNo").val();

//                if (DisAccID == "" && val != "0") {
//                    $.ajax({
//                        type: 'post',
//                        url: '../../Handler/orderHandle.ashx',
//                        data: { ck: Math.random(), ActionType: "AddInvoi", disID: $.trim($("#hidDisID").val()), Rise: LookUp, Content: Context, OBank: Bank, OAccount: Account, TRNumber: RegNo },
//                        dataType: 'json',
//                        success: function (data) {
//                            if (data.Result) {
//                                DisAccID = data.Code;
//                            }
//                        },
//                        error: function (XMLHttpRequest, textStatus, errorThrown) {
//                            layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
//                        }
//                    });
//                }

                window.parent.invinfo(DisAccID, val, LookUp, Context, Bank, Account, RegNo);
                window.parent.CloseGoods();
            });
        });
    </script>
    </form>
</body>
</html>
