<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderPrint.aspx.cs" Inherits="Company_Order_OrderPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单打印</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $(".cancel").click(function () {
                window.close();
            });

            $(".orangeBtn").click(function () {
                $(".footerBtn").css("display", "none");
                javascript:window.print();
                $(".footerBtn").css("display", "block");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="div_content" id="printcontent">
        <div style="margin: 0 auto; text-align: center; margin-top: 15px;">
            <h5 style="font-size: 16px;">
                订单打印</h5>
        </div>
        <!--销售订单主体 start-->
        <div class="lbtb" style="margin-top: 5px;">
            <label style="font-size: 13px;">
                打印日期：</label>
            <label style="font-size: 13px;" id="PrintDate" runat="server">
            </label>
            <table class="dh">
                <tr>
                    <td style="width: 110px; text-align: center;">
                        订单编号
                    </td>
                    <td style="width: 250px;">
                        <label id="lblReceiptNo" runat="server">
                        </label>&nbsp;
                    </td>
                    <td style="width: 110px; text-align: center;">
                        代理商名称
                    </td>
                    <td style="width: 250px;">
                        <label id="lblDisName" runat="server">
                        </label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        订单类型
                    </td>
                    <td>
                        <label id="lblOtype" runat="server">
                        </label>&nbsp;
                    </td>
                    <td style="text-align: center;">
                        下单时间
                    </td>
                    <td>
                        <label id="lblCreateDate" runat="server">
                        </label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        制单人
                    </td>
                    <td>
                        <label id="lblDisUser" runat="server">
                        </label>&nbsp;
                    </td>
                    <td style="text-align: center;">
                        订单总价
                    </td>
                    <td>
                        <label id="lblTotalPrice" runat="server">
                            </label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        支付状态
                    </td>
                    <td>
                        <label id="lblPayState" runat="server">
                        </label>&nbsp;
                    </td>
                    <td style="text-align: center;">
                        支付金额
                    </td>
                    <td>
                        <label id="lblPayPrice" runat="server">
                            </label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">收货地址</td>
                    <td colspan="3">
                        <label id="lblAddr" runat="server">
                                </label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        订单备注
                    </td>
                    <td colspan="3" style="word-wrap: break-word; word-break: break-all;">
                        <label id="lblRemark" runat="server" style="line-height: 20px;">
                        </label>&nbsp;
                        <%--<p runat="server" id="lblRemark" style=" line-height:20px;"></p>--%>
                    </td>
                </tr>
            </table>
        </div>
        <!--销售订单主体 end-->

        <!--清除浮动-->
        <div style="clear: none;"></div>

        <!--销售订单明细 start-->
        <div style="padding-top: 10px; margin:0px 5px;">
                <!--新增商品列表 start-->
                <div class="tablelist">
                    <asp:Repeater ID="rpDtl" runat="server">
                        <HeaderTemplate>
                            <table >
                                <tr>
                                    <th class="t8">
                                        序 号
                                    </th>
                                    <th>
                                        商品名称
                                    </th>
                                    <th class="t6">
                                        商品描述
                                    </th>
                                    <th class="t8">
                                        单 位
                                    </th>
                                    <th class="t5">
                                        单 价
                                    </th>
                                    <th class="t8">
                                        数 量
                                    </th>
                                    <th class="t5">
                                        小 计
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="list-title" style="text-align: center;">
                                <td><div class="tc">
                                    <asp:Label ID="lblGoodsId" Text='<%# Eval("GoodsinfoID") %>' runat="server" Style="display: none;"></asp:Label>
                                    <%# Container.ItemIndex + 1 %>
                                    </div>
                                </td>
                                <td>
                                    <div class="tcle"> 
                                    <%# Common.GetName(Eval("GoodsName").ToString()) %>&nbsp;
                                    </div>
                                </td>
                                <td>
                                <div class="tcle"> 
                                    <%# Eval("GoodsInfos").ToString() %>&nbsp;
                                    </div>
                                </td>
                                <td><div class="tc"> 
                                    <%# Eval("Unit")%>&nbsp;</div>
                                </td>
                                <td><div class="tc"> 
                                    <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%>&nbsp;</div>
                                </td>
                                <td>
                                <div class="tc"> 
                                    <%# Eval("GoodsNum").ToString()%>&nbsp;</div>
                                </td>
                                <td><div class="tc"> 
                                    <%# Convert.ToDecimal(Eval("Total")).ToString("N")%>&nbsp;</div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count!=0).ToString())%>'>
                                <td colspan="7">
                                 <div class="editmoney">
                                 <div class="mo-t" >
                                    合 计： <div><span id="SumTatol">
                                        <%# SelectGoods.SumTotal(this.DisID, this.CompID).ToString("N")%></span></div></div>
                                        </div>
                                </td>
                            </tr>
                            <tr id="tr" runat="server" visible='<%# bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                <td colspan="9" align="center">
                                    无匹配数据
                                </td>
                            </tr>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <!--新增商品列表 end-->
            </div>
        <!--销售订单明细 end-->
    </div>
    <div class="footerBtn">
        <input name="" type="button" class="orangeBtn" value="打印" />
         &nbsp;
        <input name="" type="button" class="cancel" value="关闭" />
    </div>
    </form>
</body>
</html>
