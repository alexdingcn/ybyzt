<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderOutInfo.aspx.cs" Inherits="Company_Order_OrderOutInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>收货信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <%--<link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../js/layer.js" type="text/javascript"></script>--%>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../Company/js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#returnIcon").click(function () {
                //window.location.href = 'ReceivingList1.aspx';
                window.history.go(-1);
            });
            //物流信息
            $("#Exp").click(function () {
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('物流信息', 'OrderExpress.aspx?KeyID=<%=Common.DesEncrypt(OrderId.ToString(),Common.EncryptKey) %>', '750px', '480px');  //记录弹出对象
                $("#hid_Alert").val(index);
            });
        });
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <input type="hidden" id="hid_Alert" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="ReceivingList1" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="navigation2" href="#"
                    class="cur">收货信息</a></div>
            <div class="userFun">
                <div id="A_btn" runat="server" class="left">
                    <a href="javascript:void(0)" class="btnBl" id="Exp" runat="server"><i class="copyIcon">
                    </i>物流信息</a> <a href="javascript:void(0);" class="btnBl" id="returnIcon" runat="server">
                        <i class="returnIcon"></i>返回</a>
                </div>
            </div>
            <div class="blank10">
            </div>
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="head" style="width: 10%">
                                发货编号
                            </td>
                            <td style="width: 23%">
                                <label id="lblReceiptNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                订单编号
                            </td>
                            <td style="width: 23%">
                                <label id="lblOrderNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                代理商名称
                            </td>
                            <td style="width: 23%">
                                <label id="lblDisName" runat="server">
                                </label>
                                &nbsp;
                                <input id="hidDisId" type="hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="head" style="width: 10%">
                                发货日期
                            </td>
                            <td style="width: 23%">
                                <label id="lblSendDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                经办人
                            </td>
                            <td style="width: 23%">
                                <label id="lblActionUser" runat="server">
                                </label>
                                &nbsp;
                            </td>
                             <td>
                                &nbsp;
                            </td>
                             <td>
                                &nbsp;
                            </td>
                            <%--<td class="head" style="width: 10%">
                                物流公司
                            </td>
                            <td style="width: 23%">
                                <label id="lblExpress" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                物流单号
                            </td>
                            <td style="width: 23%">
                                <label id="lblExpressNo" runat="server">
                                </label>
                                &nbsp;
                            </td>--%>
                        </tr>
                        <%--<tr style="display: none">
                            <td class="head" style="width: 10%">
                                物流联系人
                            </td>
                            <td style="width: 23%">
                                <label id="lblExpressPerson" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                物流电话
                            </td>
                            <td style="width: 23%">
                                <label id="lblExpressTel" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                发货包数
                            </td>
                            <td style="width: 23%">
                                <label id="lblExpressBao" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>--%>
                        <tr style="display: none">
                            <%--<td class="head" style="width: 10%">
                                运费
                            </td>
                            <td style="width: 23%">
                                <label id="lblPostFee" runat="server">
                                </label>
                                &nbsp;
                            </td>--%>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <label id="Label4" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head" style="width: 10%">
                                备注
                            </td>
                            <td colspan="5" style="word-wrap: break-word; word-break: break-all;">
                                <label id="lblRemark" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head" style="width: 10%">
                                是否签收
                            </td>
                            <td style="width: 23%">
                                <label id="lblIsSign" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 10%">
                                签收人
                            </td>
                            <td style="width: 23%">
                                <label id="lblSignUser" runat="server">
                                </label>
                                &nbsp;
                                <input type="hidden" id="hidSignUserId" runat="server" />
                            </td>
                            <td class="head" style="width: 10%">
                                签收日期
                            </td>
                            <td style="width: 23%">
                                <label id="lblSignDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head" style="width: 10%">
                                签收备注
                            </td>
                            <td colspan="5" style="word-wrap: break-word; word-break: break-all;">
                                <label id="lblSignRemark" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="orderLiv">
                    <asp:Repeater ID="rpDtl" runat="server">
                        <HeaderTemplate>
                            <table class="PublicList" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th style="width: 50px;">
                                        序号
                                    </th>
                                    <%--<th>
                                            商品编号
                                        </th>--%>
                                    <th style="width: 240px;">
                                        商品名称
                                    </th>
                                    <th style="width: 300px;">
                                        商品描述
                                    </th>
                                    <th style="width: 100px;">
                                        单 位
                                    </th>
                                    <th>
                                        单 价
                                    </th>
                                    <th>
                                        数 量
                                    </th>
                                    <%--<th>
                                            备 注
                                        </th>--%>
                                    <th>
                                        小 计
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGoodsId" Text='<%# Eval("GoodsinfoID") %>' runat="server" Style="display: none;"></asp:Label>
                                    <%# Container.ItemIndex + 1 %>
                                </td>
                                <td title='<%# Eval("GoodsName").ToString() %>'>
                                    <span class="txt"><a class="txt" href="../e<%# Common.GetGoodsID(Eval("goodsinfoid").ToString()) %>_<%# Eval("compid")%>.html"
                                        target="_blank">
                                        <%# Eval("GoodsName") %>
                                    </a>
                                        <%# Eval("vdef1").ToString() != "0" ? "<i class=\"ProIcon\" tip_type=\"" + Eval("vdef3").ToString() + "\" tip=\"" + Eval("vdef1").ToString() + "\"></i>" : ""%>
                                    </span>&nbsp;<br />
                                    <%# Eval("BarCode")%>
                                </td>
                                <td title='<%# Eval("GoodsInfos").ToString() %>'>
                                    <span class="txt">
                                        <%# Common.MySubstring(Eval("GoodsInfos").ToString(), 50, "...") %></span>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("Unit")%>&nbsp;
                                </td>
                                <td>
                                    <%# OrderInfoType.proTypePrce(Eval("vdef1").ToString(), Eval("vdef2").ToString(), Eval("Price").ToString())%>
                                    <i <%# Eval("vdef1").ToString()=="0"?"":"style=\"color:Red;\""  %>>
                                        <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></i>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("GoodsNum").ToString()%>&nbsp;
                                    <%# OrderInfoType.proType(Eval("vdef1").ToString() ,Eval("vdef2").ToString()) %>
                                </td>
                                <%--<td>
                                        <%# Eval("Remark")%>
                                    </td>--%>
                                <td>
                                    <%# Convert.ToDecimal(Eval("Total")).ToString("N")%>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr id="trTotal" runat="server" style="line-height: 20px;" visible='<%#bool.Parse((rpDtl.Items.Count!=0).ToString())%>'>
                                <td style="text-align: center;">
                                    合 计
                                </td>
                                <td colspan="3">
                                </td>
                                <%--<td style="text-align: center;">
                                        <span id="SumNum">
                                            <%# SelectGoods.SumNum(this.DisId,this.CompID).ToString("0.00") %></span>
                                    </td>--%>
                                <td style="text-align: center;" colspan="3">
                                    <span>
                                        <%# OrderInfoType.proOrderType(ProIDD, ProPrice, ProType)%>
                                    </span>
                                    <br />
                                    <span id="SumTatol">
                                        <%# SelectGoods.SumTotal(this.DisID, this.CompID,ProPrice).ToString("N") %>
                                    </span>
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
            </div>
        </div>
    </div>
    </form>
</body>
</html>
