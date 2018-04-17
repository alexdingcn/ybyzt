<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printout.aspx.cs" Inherits="Distributor_newOrder_printout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发货单打印</title>
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
        .deli-if .t
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
        .goods-zs .sPic .name,.deli-if .gray
        {
            color: #000;
        }
        .goodsPrint .sPic
        {
            padding-left: 1px;
        }
        .goods-zs .sPic
        {
            padding: 0px 0 0 10px;
            min-height: 20px;
        }
        /*.goodsPrint .deli-if{ background:none; border:none; height:auto; line-height:24px; padding:0px 0 10px 10px;color: #000;}*/
        .deli-if{ background:none; border:none; height:auto; line-height:24px; padding:0px 0 10px 10px;color: #000;}
        .goodsPrint .print-y
        {
            height: 800px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="popup goodsPrint">
        <div class="print-y" id="printcontent">
            <div class="goods-zs">
                <div style="margin:0 auto; text-align:center"><h1>发货单</h1></div>
               <div style="margin-top:18px"></div>
                <!--发货单号 start -->
                 <div class="deli-if"><i class="t">发货单号：<b class="gray"><label id="lblReceiptNo" runat="server"></label></b></i></div>
                <div class="deli-if">
                    <i class="t">订单编号：<b class="gray"><%=orderno %></b></i>
                    
                    <i class="t">发货日期：<b class="gray"><label id="lblArrDate" runat="server"></label></b></i>
                </div>
                   <div class="deli-if">
                    <i class="t"><label id="name" runat="server"></label><label id="lblName" runat="server"></label></i>
                </div>
                <div class="deli-if">
                    <i class="t">物流信息： 
                        <b class="gray">
                            <label id="lblLogistics" runat="server">
                            </label>
                        </b>
                    </i>
                </div>
                <%--<div style="position:absolute;right:30px;top:40px;"> <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>--%>
               <div style="position:absolute;right:15px;top:20px;">  <asp:Image ID="Image1"  runat="server" /></div>
                <br />
                <!--发货单号 end -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t1">
                                    商品名称
                                </th>
                                <th class="t5">
                                    规格属性
                                </th>
                                <th class="t5">
                                    单位
                                </th>
                                <th class="t3">
                                    订购数量
                                </th>
                                <th class="t3">
                                    已发货数量
                                </th>
                                <th class="t3">
                                    本次发货
                                </th>
                                <th class="t3">
                                    批次号
                                </th>
                                <th class="t3">
                                    有效期
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rptOrderD">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="sPic">
                                                <i class="code"><%# Eval("GoodsCode")%>&nbsp;<%# Eval("GoodsName")%></i> 
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
                                                <%# SelectGoodsInfo.GetNum(Eval("GoodsNum").ToString(), Digits)%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("odOutNum").ToString(), Digits)%>
                                                </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetNum(Eval("oNum").ToString(), Digits)%>
                                                </div>
                                        </td>
                                        <td>
                                            <div class="tc"><%# Eval("BatchNO") %></div>
                                        </td>
                                         <td>
                                            <div class="tc"><%# Eval("validDate", "{0:yyyy-MM-dd}")%></div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <%--<div class="po-btn oraOrder">
            <a href="javascript:;" class="gray-btn cancel">取消</a> 
            <a href="javascript:;" class="btn-area" onclick="testprint();">打印</a>
        </div>--%>
    </div>
    </form>
</body>
</html>
