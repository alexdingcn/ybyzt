<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderReturnInfo.aspx.cs"
    Inherits="Company_Order_OrderReturnInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
     <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
             //审核
            $("#Audit").click(function(){
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度

                //记录弹出对象
                var index = layerCommon.openWindow('退货审核', 'OrderReturnAudit.aspx?KeyID=<%=Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) %>', '650px', '350px'); 
                $("#hid_Alert").val(index); //记录弹出对象
            });

            //返回 
            $("#cancel").click(function () {
                var go='<%=Request["go"]+"" %>';
                var type='<%=Request.QueryString["type"]+"" %>';
                //alert(type);
                if(go!=1){
                    window.location.href = 'OrderReturnList.aspx?type='+type;
                }else{
                   cancel();
                }
            });

            //确认退款
            $("#ReturnMoney").click(function () {
               layerCommon.confirm("确认退款吗？", function () { $("#btnReturnMoney").trigger("click"); });
            });

            //日志
            $("#Log").on("click", function () {
                var KeyId=<%=OrderId %>;
                var CompId=<%=this.CompID %>;
                Log(KeyId,CompId);
            });
        });
    </script>
    <style>
        #ReturnMoney{text-align:center;color:#fff; padding:0 15px;line-height: 28px;height:28px;background:#ff4e02;font-weight:normal; border:1px solid #ea5211; font-size:13px;}
		#ReturnMoney:hover{ color:#fff;background:#f14900; border:1px solid #dd4a0a;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-1" />
    <input type="hidden" id="hid_Alert" />
    <asp:Button ID="btnAudit" Text="审核" runat="server" OnClick="btnAudit_Click" Style="display: none;" />
    <asp:Button ID="btnReturnMoney" Text="退款" runat="server" OnClick="btnReturnMoney_Click"
        Style="display: none;" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../Order/OrderReturnList.aspx"  id="title" runat="server">退货审核</a></li><li>></li>
                <li><a href="#">订单详细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools" style="padding-left:5px;">
            <ul class="toolbar left">
                <li id="Audit" runat="server"><span>
                    <img src="../images/t15.png" /></span><font>审核</font></li>
                <li id="ReturnMoney" runat="server"><font>确认退款</font></li>
                <li id="Log" runat="server"><span>
                    <img src="../images/tp2.png" /></span>日志</li>
                <li id="cancel" runat="server"><span>
                    <img src="../images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <!--功能按钮 end-->
        <div class="div_content">
            <!--销售订单主体 start-->
            <div>
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width: 15%;">
                                <span>退单编号</span>
                            </td>
                            <td>
                                <label id="lblReceiptNo" runat="server">
                                </label>&nbsp;
                            </td>
                            <td style="width: 15%;">
                                <span>代理商名称</span>
                            </td>
                            <td>
                                <label id="lblDisName" runat="server">
                                </label>&nbsp;
                                <input id="hidDisId" type="hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>订单来源</span>
                            </td>
                            <td>
                                <label id="lblAddType" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>订单类型</span>
                            </td>
                            <td>
                                <label id="lblOtype" runat="server">
                                </label>&nbsp;
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <span>订单总价</span>
                            </td>
                            <td>
                                <label id="lblTotalPrice" runat="server">
                                </label>&nbsp;
                            </td>
                             <td>
                                <span>订单状态</span>
                            </td>
                            <td>
                                <label id="lblOState" style=" color:Red;" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>支付状态</span>
                            </td>
                            <td>
                                <label id="lblPayState" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>退货状态</span>
                            </td>
                            <td>
                                <label id="lblReturnState" style=" color:Red;" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <span>物流公司</span>
                            </td>
                            <td>
                                <label id="lblExpress" runat="server">
                                </label>
                            </td>
                            <td>
                                <span>物流单号</span>
                            </td>
                            <td>
                                <label id="lblExpressNo" runat="server">
                                </label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <span>退货日期</span>
                            </td>
                            <td>
                                <label id="lblReturnDate" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>退货申请人</span>
                            </td>
                            <td>
                                <label id="lblReturnUserID" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span>退货说明</span>
                            </td>
                            <td colspan="3" style="padding-left:5px; line-height: 20px;">
                                <label id="lblReturnContent" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr  style=" display:none;" id="trAuditUser" runat="server">
                            <td>
                                <span>审核人</span>
                            </td>
                            <td>
                                <label id="lblAuditUser" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>审核日期</span>
                            </td>
                            <td>
                                <label id="lblAuditDate" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr  style=" display:none;" id="trAuditRemark" runat="server">
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span>审核备注</span>
                            </td>
                            <td colspan="3" style=" padding-left:5px; word-wrap: break-word; word-break: break-all;">
                                <label id="lblAuditRemark" runat="server" style="line-height: 20px;">
                                </label>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--销售订单主体 end-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--销售订单明细 start-->
            <div style="padding-top: 10px; margin:0px 5px;">
                <!--新增商品列表 start-->
                 <div class="tablelist">
                        <asp:Repeater ID="rpDtl" runat="server">
                            <HeaderTemplate>
                                <table class="list">
                                    <tr class="list-title">
                                        <th class="t8">
                                            序号
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
                                    <td>
                                     <div class="tc"> 
                                        <asp:Label ID="lblGoodsId" Text='<%# Eval("GoodsinfoID") %>' runat="server" Style="display: none;"></asp:Label>
                                        <%# Container.ItemIndex + 1 %>
                                        </div>
                                    </td>
                                     <td style="text-align:left; text-indent:0.5em; " title='<%# Eval("GoodsName").ToString() %>'>
                                        <div class="tc"> 
                                        <%# Eval("GoodsName") %>
                                        <%# Eval("vdef1").ToString() != "0" ? "<i class=\"ProIcon\" tip_type=\"" + Eval("vdef3").ToString() + "\" tip=\"" + Eval("vdef1").ToString() + "\"></i>" : ""%>
                                        &nbsp;
                                        <%# Eval("BarCode")%>
                                        </div>
                                    </td>
                                    <td style="text-align:left; text-indent:0.5em; " title='<%# Eval("GoodsInfos").ToString() %>'>
                                        <div class="tc"> 
                                        <%# Common.MySubstring(Eval("GoodsInfos").ToString(), 50, "...") %>&nbsp;
                                        </div>
                                    </td>
                                    <td>
                                     <div class="tc"> 
                                        <%# Eval("Unit")%>&nbsp;
                                        </div>
                                    </td>
                                    <td>
                                     <div class="tc"> 
                                        <%# OrderInfoType.proTypePrce(Eval("vdef1").ToString(), Eval("vdef2").ToString(), Eval("Price").ToString())%>
                                       <i <%# Eval("vdef1").ToString()=="0"?"":"style=\"color:Red;\""  %>> <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></i>&nbsp;
                                       </div>
                                    </td>
                                    <td>
                                     <div class="tc"> 
                                        <%# Eval("GoodsNum") %>&nbsp;
                                        <%# OrderInfoType.proType(Eval("vdef1").ToString() ,Eval("vdef2").ToString()) %>
                                        </div>
                                    </td>
                                    <%--<td>
                                        <%# Eval("Remark")%>
                                    </td>--%>
                                    <td>
                                     <div class="tc"> 
                                        <%# Convert.ToDecimal(Eval("Total")).ToString("N")%>&nbsp;
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr id="trTotal" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count!=0).ToString())%>'>
                                    <td style="text-align: center;">
                                     <div class="tc"> 
                                        合 计
                                        </div>
                                    </td>
                                    <td colspan="4">
                                    </td>
                                    <%--<td style="text-align: center;">
                                        <span id="SumNum">
                                            <%# SelectGoods.SumNum(this.DisId,this.CompID).ToString("0.00") %></span>
                                    </td>--%>
                                    <td style="text-align: center;" colspan="2">
                                     <div class="tc"> 
                                        <div class="mo-t"> 
                                            <%# OrderInfoType.proOrderType(ProIDD, ProPrice,ProType) %>   
                                         <div> 
                                        <span id="SumTatol">
                                            <%# SelectGoods.SumTotal(this.DisId, this.CompID).ToString("N")%>
                                        </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="tr" runat="server" visible='<%# bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                    <td colspan="9" align="center">
                                     <div class="tc"> 
                                        无匹配数据
                                        </div>
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
    </div>
    </form>
</body>
</html>
