
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderCreateInfo.aspx.cs" Inherits="Company_Order_OrderCreateInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>销售管理</title>
    
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            //返回
            $("#cancel").click(function () {
                history.go(-1);
            });

            $("#Edit").click(function () {
                window.location.href = 'OrderCreateAdd.aspx?Id=<%=Request.QueryString["Id"] %>';
            });

            //审核
            $("#Audit").on("click", function () {
                $("#btnAudit").trigger("click");
            });

            //提交
            $("#Submit").on("click", function () {
                $("#btnSubmit").trigger("click");
            });

            //退回
            $("#Return").on("click", function () {
                $("#btnReturn").trigger("click");
            });

             //生成订单
            $("#CopyOrder").on("click", function () {
                $("#btnCopyOrder").trigger("click");
            });

            //日志
            $("#Log").on("click", function () {

                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度

                var index = showDialog('订单日志', '../../BusinessLog.aspx?LogClass=Order&ApplicationId=' + <%=Id %> + '&CompId=', '750px', '450px', layerOffsetY); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">销售管理</a><i>></i>
            <a href="#">销售订单生成</a>
        </div>
        <!--当前位置 end-->

        <asp:Button ID="btnAudit" Text="审核" runat="server" onclick="btnAudit_Click" style=" display:none;"  />
        <asp:Button ID="btnSubmit" Text="提交" runat="server" onclick="btnSubmit_Click" style=" display:none;"  />
        <asp:Button ID="btnReturn" Text="退回" runat="server" onclick="btnReturn_Click" style=" display:none;"  />
        <asp:Button ID="btnCopyOrder" Text="生成订单" runat="server" onclick="btnCopyOrder_Click" style=" display:none;"  />
        <input type="hidden" id="hid_Alert" />
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="Submit" runat="server"><span><img src="../../Company/images/t08.png" /></span>提交</li>
                    <li id="Audit" runat="server"><span></span>审核</li>
                    <li id="Return" runat="server"><span></span>退回</li>
                    <li id="Shipping" runat="server"><span></span>发货</li>
                    <%--<li id="Li1" runat="server"><span></span>申请退款</li>--%>
                    <li id="Clearing" runat="server"><span></span>去结算</li>
                    <li id="PrePayMonery" runat="server"><span></span>预收款申请</li>
                    <li id="Remove" runat="server"><span></span>取消订单</li>
                    <li id="Edit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                    <li id="Del" runat="server"><span><img src="../../Company/images/t03.png" /></span>删除</li>
                    <li id="CopyOrder" runat="server"><span></span>生成订单</li>
                    <li id="Log" runat="server"><span></span>日志</li>
                    <li id="cancel" runat="server"><span></span>返回</li>
                </ul>
            </div>
            <!--功能按钮 end-->
            <div class="tipinfo">
                <!--销售订单主体 start-->
                <%--<div class="lb">
                    <span>订单编号：</span>
                    <label id="lblReceiptNo" runat="server"></label>

                    <span>代理商名称：</span>
                    <label id="lblDisName" runat="server"></label>
                    <input id="hidDisId" type="hidden" runat="server"/>

                    <span>收货地址：</span>
                    <label id="lblAddr" runat="server"></label>
                    <input id="hidAddrId" type="hidden" runat="server"/>
                </div>
                <div class="lb">
                    <span>订单类型：</span>
                    <label id="lblOtype" runat="server"></label>

                    <span>订单总价：</span>
                    <label id="lblTotalPrice" runat="server"></label>

                    <span>下单时间：</span>
                    <label id="lblCreateDate" runat="server"></label>
                </div>
                <div class="lb">
                    <span>订单状态：</span>
                    <label id="lblOState" runat="server"></label>

                    <span>支付状态：</span>
                    <label id="lblPayState" runat="server"></label>

                    <span>支付金额：</span>
                    <label id="lblPayedPrice" runat="server"></label>
                </div>
                <div class="lb" style="height:50px;">
                    <span>订单备注：</span>
                    <label id="lblRemark" runat="server"></label>
                </div>--%>
                <div style="padding-left: 10px; ">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                               <td><span>订单编号：</span></td>
                               <td><label id="lblReceiptNo" runat="server"></label></td>

                               <td><span>代理商名称：</span></td>
                               <td>
                                    <label id="lblDisName" runat="server"></label>
                                    <input id="hidDisId" type="hidden" runat="server"/>
                                </td>

                                <td><span>收货地址：</span></td>
                                <td>
                                    <label id="lblAddr" runat="server"></label>
                                    <input id="hidAddrId" type="hidden" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td><span>订单类型：</span></td>
                                <td><label id="lblOtype" runat="server"></label></td>

                                <td><span>订单总价：</span></td>
                                <td><label id="lblTotalPrice" runat="server"></label></td>

                                <td><span>下单时间：</span></td>
                                <td><label id="lblCreateDate" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>订单状态：</span></td>
                                <td><label id="lblOState" runat="server"></label></td>

                                <td><span>支付状态：</span></td>
                                <td><label id="lblPayState" runat="server"></label></td>

                                <td><span>支付金额：</span></td>
                                <td><label id="lblPayedPrice" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>订单备注：</span></td>
                                <td colspan="5"><label id="lblRemark" runat="server"></label></td>
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
                        <div class="edittable2">
                            <asp:Repeater ID="rpDtl" runat="server">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr class="list-title">
                                            <th>序 号</th>
                                            <%--<th>产品名称</th>
                                            <th>产品代码</th>
                                            <th>属 性</th>--%>
                                            <th>单 价</th>
                                            <th>数 量</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="list-title" style=" text-align:center;">
                                        <td>
                                            <asp:Label ID="lblGoodsId" Text='<%# Eval("GoodsinfoID") %>' runat="server" Style="display: none;"></asp:Label>
                                            <%# Container.ItemIndex + 1 %>
                                        </td>
                                        <%--<td><%# Eval("GoodsName")%></td>
                                        <td><%# Eval("GoodsSpec")%></td>
                                        <td><%# Eval("Ntype")%></td>--%>
                                        <td><%# Convert.ToDecimal(Eval("Price")).ToString("0.00")%></td>
                                        <td><%# Convert.ToInt32(Eval("GoodsNum")) %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
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
