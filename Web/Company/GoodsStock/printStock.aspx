<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printStock.aspx.cs" Inherits="Company_GoodsStock_printStock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                    <h1><%=TitleType %>单</h1></div>
              <div style="margin-top:15px"></div>
                <div class="bh">
                    <div class="left deli">
                        <i class="bt2">单 号：</i><i class="ok "><label id="lblReceiptNo" runat="server"></label></i>
                    </div>
                    <div class="left deli">
                        <i class="bt2">制单时间：</i><i class="ok "><label id="lblCreateDate" runat="server"></label></i>
                    </div>
                    <div class="left deli">
                        <i class="bt2"><%=TitleType %>类型：</i><i class="ok "><label id="lblType" runat="server"></label></i>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                
                <div class="bz remark">
                    <i class="bt">备注：</i><div class="txtbox" id="divRemark">
                        <i class="ok">
                            <label class="ok" id="iRemark" runat="server">
                            </label>
                        </i>
                    </div>
                </div>
                <div style="position:absolute;right:15px;top:20px">  <asp:Image ID="Image1"  runat="server" /></div>
                
                
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
                                                <i class="code"><%# Eval("GoodsCode")%>&nbsp; <%# Eval("GoodsName")%></i> 
                                                <i class="name"></i>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# SelectGoodsInfo.GetGoodsInfos(Eval("ValueInfo").ToString())%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("Unit")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                ￥<%# Eval("TinkerPrice", "{0:f2}")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("StockNum").ToString()%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("BatchNO")%></div>
                                        </td>
                                        <td>
                                            <div class="tc">
                                                <%# Eval("validDate")%></div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>                    
                    </table>
                </div>
            </div>
            <!--[if !IE]>商品展示区start<![endif]-->
        </div>
    </div>
    <!--订单打印 end-->
    </form>
</body>
</html>

