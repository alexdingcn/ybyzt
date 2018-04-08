<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderCreateInfo.aspx.cs" Inherits="Company_Order_OrderCreateInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单收款补录详细</title>

    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            //返回
            $("#cancel").click(function () {
                history.go(-1);
            });

            //$("#Edit").click(function () {
            //    window.location.href = 'OrderCreateAdd.aspx?KeyID=<%=KeyID %>';
            //});

            //审核
            $("#Audit").on("click", function () {
                $("#btnAudit").trigger("click");
            });

            //提交
            //$("#Submit").on("click", function () {
            //    $("#btnSubmit").trigger("click");
            //});

            //退回
            //$("#Return").on("click", function () {
            //    $("#btnReturn").trigger("click");
            //});

             //生成订单
            //$("#CopyOrder").on("click", function () {
            //    $("#btnCopyOrder").trigger("click");
            //});

            //日志
            function Log(KeyId, CompId) {
                var index=layerCommon.openWindow('订单日志', '../../BusinessLog.aspx?LogClass=Order&ApplicationId=' + KeyId + '&CompId=' + CompId, '750px', '380px');
                $("#hid_Alert").val(index); //记录弹出对象
            }

            //日志
           $("#Log").on("click", function () {
                var KeyId=<%=KeyID %>;
                var CompId=<%=this.CompID %>;
                Log(KeyId,CompId);
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-2" />

        <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../pay/PayOrderList.aspx">订单收款补录</a></li><li>></li>
                <li><a href="../pay/PayOrderList.aspx">订单收款补录详细</a></li>
	        </ul>
        </div>
        <!--当前位置 end-->

        <asp:Button ID="btnAudit" Text="审核" runat="server" onclick="btnAudit_Click" style=" display:none;"  />
        <%--<asp:Button ID="btnSubmit" Text="提交" runat="server" onclick="btnSubmit_Click" style=" display:none;"  />--%>
        <%--<asp:Button ID="btnReturn" Text="退回" runat="server" onclick="btnReturn_Click" style=" display:none;"  />--%>
        <%--<asp:Button ID="btnCopyOrder" Text="复制" runat="server" onclick="btnCopyOrder_Click" style=" display:none;"  />--%>
        <input type="hidden" id="hid_Alert" />

        
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <%--<li id="Submit" runat="server"><span><img src="../images/t08.png" /></span>提交</li>--%>
                    <li id="Audit" runat="server"><span><img src="../images/t08.png" /></span>审核</li>
                    <%--<li id="Return" runat="server"><span><img src="../images/t06.png" /></span>退回</li>--%>
                    <li id="Shipping" runat="server"><span><img src="../images/t09.png" /></span>发货</li>
                    <%--<li id="Li1" runat="server"><span></span>申请退款</li>--%>
                    <%--<li id="Clearing" runat="server"><span></span>去结算</li>--%>
                    <%--<li id="PrePayMonery" runat="server"><span></span>预收款申请</li>--%>
                    <%--<li id="Remove" runat="server"><span><img src="../images/t11.png" /></span>取消订单</li>--%>
                    <%--<li id="Edit" runat="server"><span><img src="../images/t02.png" /></span>编辑</li>--%>
                    <%--<li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li>--%>
                    <%--<li id="CopyOrder" runat="server"><span><img src="../images/t02.png" /></span>复制</li>--%>
                    <li id="Log" runat="server"><span>
                    <img src="../images/tp2.png" /></span>日志</li>
                    <li id="cancel" runat="server"><span><img src="../images/tp3.png" /></span>返回</li>
                </ul>
            </div>
            <!--功能按钮 end-->
            <div class="div_content">
                <!--销售订单主体 start-->
                <div style="padding-left: 10px; ">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                               <td style="width:15%;"><span>订单编号</span></td>
                               <td style="width:30%;"><label id="lblReceiptNo" runat="server"></label></td>

                               <td style="width:15%;"><span>代理商名称</span></td>
                               <td style="width:30%;">
                                    <label id="lblDisName" runat="server"></label>
                                    <input id="hidDisId" type="hidden" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td><span>收货地址</span></td>
                                <td>
                                    <label id="lblAddr" runat="server"></label>
                                    <input id="hidAddrId" type="hidden" runat="server"/>
                                </td>

                                <td><span>订单类型</span></td>
                                <td><label id="lblOtype" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>订单总价</span></td>
                                <td><label id="lblTotalPrice" runat="server"></label></td>

                                <td><span>&nbsp;</span><%--<span>其它费用</span>--%></td>
                                <td><%--<label id="lblOtherAmount" runat="server"></label>--%></td>
                            </tr>
                            <tr>
                                <td><span>制单人</span></td>
                                <td><label id="lblDisUser" runat="server"></label></td>

                                <td><span>要求发货日期</span></td>
                                <td><label id="lblArriveDate" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>下单时间</span></td>
                                <td><label id="lblCreateDate" runat="server"></label></td>

                                <td><span>订单状态</span></td>
                                <td><label id="lblOState" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>支付状态</span></td>
                                <td><label id="lblPayState" runat="server"></label></td>

                                <td><span>支付金额</span></td>
                                <td><label id="lblPayedPrice" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td style=" background: #f6f6f6 none repeat scroll 0 0;"><span style=" height:auto;">订单备注</span></td>
                                <td colspan="3"><label id="lblRemark" runat="server"></label>&nbsp; </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!--销售订单主体 end-->

                <!--清除浮动-->
                <div style="clear: none;"></div>

                <!--销售订单明细 start-->
                <div style="padding-top: 10px;">
                    <!--新增商品列表 start-->
                    <div style="padding-left: 10px;">
                        <div class="tablelist">
                            <asp:Repeater ID="rpDtl" runat="server">
                                <HeaderTemplate>
                                    <table >
                                        <tr >
                                            <th class="t7">序 号</th>
                                            <th class="t6">商品名称</th>
                                             <th class="t6">商品描述 </th>
                                            <th class="t7">单 位</th>
                                            <th class="t5">单 价</th>
                                            <th class="t7">数 量</th>
                                            <th class="t5">小 计</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr >
                                        <td>
                                          <div class="tc"><asp:Label ID="lblGoodsId" Text='<%# Eval("GoodsinfoID") %>' runat="server" Style="display: none;"></asp:Label>
                                            <%# Container.ItemIndex + 1 %></div>
                                        </td>
                                        <td> <div class="tcle"><%# Eval("GoodsName")%></div></td>
                                        <td> <div class="tcle"><%# Eval("GoodsInfos")%></div></td>
                                        <td> <div class="tc"><%# Eval("Unit")%></div></td>
                                        <td><div class="tc"><%# Convert.ToDecimal(Eval("Price")).ToString("0.00")%></div></td>
                                        <td><div class="tc"><%# Convert.ToInt32(Eval("GoodsNum")) %></div></td>
                                        <td><div class="tc"><%# Convert.ToDecimal(Eval("Total")).ToString("N")%></div></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <tr id="trTotal" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count!=0).ToString())%>'>
                                        
                                            <%--<td style="text-align: center;">
                                                <span id="SumNum">
                                                    <%# SelectGoods.SumNum(this.DisId,this.CompID).ToString("0.00") %></span>
                                            </td>--%>
                                            <td colspan="7">
                                              
                                                     <div class="editmoney">合 计：<b id="SumTatol" class="size"> <%# SelectGoods.SumTotal(this.DisId, this.CompID).ToString("N")%></b></div>
                                            </td>
                                            
                                            
                                        </tr>
                                        <tr id="tr" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                            <td colspan="6" align="center">
                                                无匹配数据
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!--新增商品列表 end-->
                </div>
                <!--销售订单明细 end-->
            </div>
        </div>
    </form>
</body>
</html>
