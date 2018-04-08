<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderZDInfo.aspx.cs" Inherits="Distributor_OrderZDInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>账单信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../Company/js/order.js" type="text/javascript"></script>
     <script src="../Company/js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            //编辑
            $("#editIcon").click(function () {
                window.location.href = 'OrderAdd.aspx?type=6&KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>';
            });

            //提交
            $("#prnIcon").click(function (){
                layerCommon.confirm("确认提交订单吗？", function () { $("#btnPrn").trigger("click"); });
            });

            //删除
            $("#DelIcon").click(function (){
                layerCommon.confirm("确认删除订单吗？", function () { $("#btnDel").trigger("click"); });
            });

            //取消订单
            $("#offIcon").click(function (){
                layerCommon.confirm("确认取消订单吗？", function () { $("#btnOff").trigger("click"); });
            });

            //复制
            $("#copyIcon").click(function (){
                layerCommon.confirm("确认复制订单吗？", function () { $("#btnCopy").trigger("click"); });
            });

             //签收
            $("#SingIcon").click(function (){
                layerCommon.confirm("确认签收订单吗？", function () { $("#btnSing").trigger("click"); });
            });


            //退货
            $("#removeIcon").click(function (){
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('申请退货', 'ReturnOrderAdd.aspx?KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>', '480px', '230px');  //记录弹出对象
                $("#hid_Alert2").val(index);
            });

             //物流信息
            $("#Exp").click(function (){
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index =layerCommon.openWindow('物流信息', 'OrderExpress.aspx?KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>', '750px', '380px');  //记录弹出对象
                $("#hid_Alert").val(index);
            });

            //支付
            $("#payIcon").click(function (){
                window.location.href='pay/Pay.aspx?isDBPay=<%=Common.PaySetingsValue(this.CompID) %>&KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>';
            });

             

            //支付查询
            /*
            $("#payInfo").click(function(){
                $("#btnPayInfo").trigger("click");
            });*/

            //日志
            $("#Log").on("click", function () {
                var KeyId=<%=KeyID %>;
                var CompId=<%=this.CompID %>;
                Zd_Log(KeyId,CompId);
            });

            var type='<%=Request["type1"] %>';

            $("#returnIcon").click(function(){
                
                if (type == "")
                {
                   // location.href="orderZDList.aspx";
                    location.href="Pay/orderZDList.aspx";
                }
                if (type=="orderDzfzdList")
                {
                    location.href="Pay/orderDzfzdList.aspx";
                }  
                if (type=="RepZdzfDetailsList")
                {
                    history.go(-1);
                    return false;
                }              
                if(type=="orderZDList")
                {
                    history.go(-1);
                    return false;
                }
            });
        });

        function a_addordreturn()
        {
            if($.trim($("#txtremark1").val())=="")
            {
              layerCommon.alert("请填写退货理由",IconOption.错误);
                return false;
            }
            else
            {
                $("#txtremark").val($.trim($("#txtremark1").val()));
            }
        }


         //cust_by_ggh   begin --  添加支付明细

                 function showPayDetail() 
                 {
                    var height = document.body.clientHeight; //计算高度
                    var layerOffsetY = (height - 450) / 2; //计算宽度
                    var index = layerCommon.openWindow('账单支付明细', 'Pay/PayDetail.aspx?KeyID= <%= Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)%>', '750px', '380px'); //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                  }

                function CloseDialog() 
                {
                    var showedDialog = $("#hid_Alert").val(); //获取弹出对象
                     layerCommon.layerClose(showedDialog); //关闭弹出对象
                }

             //cust_by_ggh   end --  添加支付明细


    </script>
</head>
<body class="root3">
    
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head1" runat="server" />
    <%--<textarea style="display:none" id="txtremark" runat="server"></textarea>--%>
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
        <input type="hidden" id="hid_Alert" />
        <%--<asp:Button ID="btnPayInfo" runat="server" OnClick="btnPayInfo_Click" Text="支付查看" Style="display: none;" />--%>
        <asp:Button ID="btnPrn" runat="server" OnClick="btnPrn_Click" Text="提交" Style="display: none;" />
        <asp:Button ID="btnDel" runat="server" OnClick="btnDel_Click" Text="删除" Style="display: none;" />
        <asp:Button ID="btnOff" runat="server" OnClick="btnOff_Click" Text="取消" Style="display: none;" />
        <asp:Button ID="btnCopy" runat="server" OnClick="btnCopy_Click" Text="复制" Style="display: none;" />
        <asp:Button ID="btnSing" runat="server" OnClick="btnSing_Click" Text="签收" Style="display: none;" />
        <%--<asp:Button ID="btnRefund" runat="server" OnClick="btnRefund_Click" Text="申请退款" Style="display: none;" />--%>
        <input id="hid_Alert2" type="hidden" />
        <div class="rightCon">
            <div class="info">
                 <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">账单信息</a></div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                    <a href="javascript:void(0)" class="btnOr" runat="server" id="payIcon"><i class="payIcon">
                    </i>账单支付</a> <a href="javascript:void(0)" class="btnOr" id="SingIcon" runat="server">
                        <i class="editIcon"></i>签收</a> <a href="javascript:void(0)" class="btnOr" id="removeIcon"
                            runat="server"><i class="removeIcon"></i>申请退货</a>
                    <%--<a href="javascript:void(0)" class="btnBl" runat="server" id="payInfo">
                            <i class="payIcon"></i>支付查询</a>--%>
                    <a href="javascript:void(0)" class="btnOr" id="prnIcon" runat="server"><i class="prnIcon">
                    </i>提交</a> <a href="javascript:void(0)" class="btnBl" id="editIcon" runat="server"><i
                        class="editIcon"></i>编辑</a> <a href="javascript:void(0)" class="btnBl" id="offIcon"
                            runat="server"><i class="offIcon"></i>取消</a> <a href="javascript:void(0)" style="display:none;" class="btnBl"
                                id="copyIcon" runat="server"><i class="copyIcon"></i>复制</a> <a href="javascript:void(0)"
                                    class="btnBl" id="DelIcon" runat="server"><i class="offIcon"></i>删除</a>
                    <a href="javascript:void(0)" class="btnBl" id="Exp" runat="server" style="display: none;">
                        <i class="copyIcon"></i>物流信息</a> <a href="javascript:void(0)" class="btnBl" id="Log"
                            runat="server"><i class="dailyIcon"></i>日志</a> <a href="#" class="btnBl" id="returnIcon"
                                runat="server"><i class="returnIcon"></i>返回</a>
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
                            <td class="head" style="width: 13%">
                                账单编号
                            </td>
                            <td style="width: 37%">
                                <label id="lblReceiptNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 13%">
                                账单状态
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
                            </td>
                            <td>
                                <label id="lblPayState" runat="server">
                                </label>
                                &nbsp;
                                 <img src="../Company/images/zhifu.png" title="查看支付清单" style="cursor: pointer; position: absolute;"
                                    id="img_zhifu" runat="server" onclick="showPayDetail()" class="logisticsShow" />
                              
                            </td>
                            <td class="head">
                                有效截止日期
                            </td>
                            <td>
                                <label id="lblArriveDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                账单金额
                            </td>
                            <td>
                                <label id="lblTotalPrice" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                已支付金额
                            </td>
                            <td>
                                <label id="lblPayedPrice" runat="server">
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
                                账单日期
                            </td>
                            <td>
                                <label id="lblCreateDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                           <td class="head">
                                费用名称
                            </td>
                            <td colspan="5" >
                                <label id="lblfymc" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                账单备注
                            </td>
                            <td colspan="5" style="word-wrap: break-word; word-break: break-all;">
                                <label id="lblRemark" runat="server" style="line-height: 20px;">
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
