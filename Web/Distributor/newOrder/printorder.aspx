<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printorder.aspx.cs" Inherits="Distributor_newOrder_printorder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单打印</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>

        $(document).ready(function () {
            layerCommon.msg("您的浏览器需要您同时下CTRL+P来打印", IconOption.笑脸);

            $(".cancel").click(function () {
                window.close();
            });
            $(".orangeBtn").click(function () {
                $(".oraOrder").css("display", "none");
                javascript: window.print();
                $(".oraOrder").css("display", "block");
            });
        });

        function testprint() {
            var body = document.getElementById("printcontent");
            document.body.innerHTML = body.innerHTML
            window.print();
        }
    </script>
    <style>
        .popup
        {
            top: 0;
            left: 0;
        }
        body
        {
            color: #000;
        }
        .goods-info .ok
        {
            color: #000;
        }
        .tabLine table
        {
            border: 1px solid #000;
        }
        .goods-zs thead th
        {
            border-right: 1px solid #000;
        }
        .tabLine td
        {
            border-top: 1px solid #000;
            border-right: 1px solid #000;
        }
        .goods-zs .sPic .name
        {
            color: #000;
        }
        .goodsPrint .sPic
        {
            padding-left: 1px;
        }
        .goods-zs .sPic
        {
            padding: 0px;
            min-height: 20px;
        }
        .goodsPrint .print-y
        {
            height: 800px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--订单打印 start-->
    <div class="popup goodsPrint">
        <div class="print-y" id="printcontent">
            <!--[if !IE]>下单信息 start<![endif]-->
            <div class="goods-info">
                <div style="margin: 0 auto; text-align: center">
                    <h1>订单</h1></div>
              <div style="margin-top:15px"></div>
                <div class="bh">
                    <div class="left deli">
                        <i class="bt2">订单编号：</i><i class="ok "><label id="lblReceiptNo" runat="server"></label></i>
                    </div>
                    <div class="left deli">
                        <i class="bt2">订单日期：</i><i class="ok "><label id="lblCreateDate" runat="server"></label></i>
                    </div>
                    <div class="left deli">
                        <i class="bt2">交货日期：</i><i class="ok "><label id="lblArrDate" runat="server"></label></i>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="bz site">
                    <i class="bt"><label id="name" runat="server">
                            </label></i><div class="txtbox">
                        <i class="ok">
                            <label id="lblDisName" runat="server">
                            </label>
                        </i>
                    </div>
                </div>
                <div class="bz site">
                    <i class="bt">配送方式：</i><div class="txtbox">
                        <i class="ok">
                            <label id="lblGiveMode" runat="server">
                            </label>
                        </i>
                    </div>
                </div>
                <div class="bz site">
                    <i class="bt">收货信息：</i><div class="txtbox">
                        <i class="ok" id="iaddr" runat="server">收货人：<label id="lblPrincipal" runat="server"></label>
                            ，联系电话：<label id="lblPhone" runat="server"></label>
                            ，收货地址：<label id="lblAddress" runat="server"></label>
                        </i>
                    </div>
                </div>
                <div class="bz remark">
                    <i class="bt">订单备注：</i><div class="txtbox" id="divRemark">
                        <i class="ok">
                            <label class="ok" id="iRemark" runat="server">
                            </label>
                        </i>
                    </div>
                </div>
                <div style="position:absolute;right:15px;top:20px">  <asp:Image ID="Image1"  runat="server" /></div>
                <%--<div style="position:absolute;right:50px;top:65px"><asp:Literal ID="Literal1" runat="server"></asp:Literal></div>--%>
                <%--<div class="bz invoice">
                    <i class="bt">发票信息：</i><div class="txtbox">
                        <i class="ok">发票号：<label id="lblBillNo" runat="server"></label>
                            ，是否已开完：<label id="lblIsBill" runat="server"></label></i></div>
                </div>--%>
            </div>
            <!--[if !IE]>下单信息 end<![endif]-->
            <!--[if !IE]>商品展示区start<![endif]-->
            <div class="goods-zs">
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="">
                                    商品名称
                                </th>
                                <th class="t2">
                                    规格属性
                                </th>
                                <th class="t5">
                                    单位
                                </th>
                                <th class="t3">
                                    单价
                                </th>
                                <th class="t5">
                                    数量
                                </th>
                                <th class="t3">
                                    小计
                                </th>
                                <%--<th class="t3">
                                    备注
                                </th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptOrderD">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="sPic">
                                                <i class="code"><%# Eval("GoodsCode")%>&nbsp; <%# Eval("GoodsName")%></i> 
                                                <i class="name"></i>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetGoodsInfos(Eval("GoodsInfos").ToString()) %></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("Unit")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                ￥<%# Eval("AuditAmount", "{0:f2}")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("GoodsNum").ToString(), Digits)%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                ￥<%# Eval("sumAmount","{0:f2}")%></div>
                                        </td>
                                        <%--<td>
                                            <div class="tc">
                                                <%# Eval("Remark").ToString() %> </div>
                                        </td>--%>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>                    </table>
                </div>
                <!--[if !IE]>订单金额 start<![endif]-->
                <div class="options-box price-box">
                    <div class="right">
                        <table>
                            <tr>
                                <td class="li">
                                    商品总额：
                                </td>
                                <td>
                                    ¥<label id="lblTotalAmount" runat="server">0.00</label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    促销优惠：
                                </td>
                                <td>
                                    ¥<label id="lblProAmount" runat="server">0.00</label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    返利抵扣：
                                </td>
                                <td>
                                    ¥<label id="lblbateAmount" runat="server">0.00</label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    运费：
                                </td>
                                <td>
                                    ¥<label id="lblPostFee" runat="server">0.00</label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="li">
                                    <i class="rz-icon"></i>应付总额：
                                </td>
                                <td>
                                    <div class="price-sum li">
                                        <i class="price">￥<label id="lblAuditAmount" runat="server">0.00</label>&nbsp;</i></div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!--[if !IE]>订单金额 end<![endif]-->
            </div>
            <!--[if !IE]>商品展示区start<![endif]-->
        </div>
        <%--<div class="po-btn oraOrder">
            <a href="javascript:;" class="gray-btn cancel">取消</a> <a href="javascript:;" class="btn-area"
                onclick="testprint();">打印</a>
        </div>--%>
    </div>
    <!--订单打印 end-->
    </form>
</body>
</html>
