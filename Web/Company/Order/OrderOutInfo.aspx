<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderOutInfo.aspx.cs" Inherits="Company_Order_OrderOutInfo" %>

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
            //返回
            $("#cancel").click( function(){
                var go='<%=Request["go"]+"" %>';
                if(go==1){
                   cancel();
                }
                else if(go==2){
                    window.location.href = 'OrderCreateList.aspx';
                }
                else{
                  window.location.href = 'OrderOutList.aspx';
                }
                //parent.CloseDialog();
            });
            
            //新增物流信息
            $("#li_Express").click(function (){
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                // 记录弹出对象
                var index = layerCommon.openWindow('新增物流', 'LogisticsEdit.aspx?orderId=<%=Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey) %>&orderOutId=<%=Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>&type=0', '550px', '280px');
                $("#hid_Alert").val(index); //记录弹出对象
            });
         function Layerclose(){
          layerCommon.layerClose($("#hid_Alert").val());
        }

             //var Isadd='<%=Request["add"]+"" %>';
             //if(Isadd==1){
             //   $("#li_Express").trigger("click");
             //}
               //编辑物流信息
            $("#editLogistics,.aLogisticsEdit").click(function () {
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                // 记录弹出对象
                var count =<%= types %>;// $(".tools ul li").length;
                var index = 0;
                if (count == 2) {//手动输入
                    index = layerCommon.openWindow('编辑物流信息', 'LogisticsEdit2.aspx?orderId=<%=Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey) %>&orderOutId=<%= Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>', '750px', '480px');
                } else if (count == 1) { //现有物流公司
                    index = layerCommon.openWindow('编辑物流', 'LogisticsEdit.aspx?orderId=<%= Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey) %>&orderOutId=<%=Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>&type=1', '550px', '280px');
                }
                $("#hid_Alert").val(index); //记录弹出对象
            });
            //日志
            $("#Log").on("click", function () {
                var KeyId=<%=this.OrderId %>;
                var CompId=<%=this.CompID %>;
                Log(KeyId,CompId);
            });
        });

          //关闭物流发货
        function LogisticsCancel(Oid,outID) {
            location.reload();
        }

          //删除物流
        function del(){
           layerCommon.confirm('确定删除物流', function () {
                        $("#btnDel").trigger("click");
                    });
        }
    </script>
    <style>
        #li_Express
        {
            ilter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#ffeaea');
            background: -webkit-linear-gradient(top, #ffffff, #ffeaea);
            background: -moz-linear-gradient(top, #ffffff, #ffeaea);
            background: -ms-linear-gradient(top, #ffffff, #ffeaea);
            background: linear-gradient(top, #ffffff, #ffeaea);
            border: 1px solid #f1d2d2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-1" />
    <input type="hidden" id="hid_Alert" />
    <%--<asp:Button ID="btnExpress" Text="物流信息" runat="server" OnClick="btnExpress_Click"
        Style="display: none;" />--%>
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../Order/OrderOutList.aspx">发货记录</a></li><li>></li>
                <li><a href="#">订单详细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools" style="padding-left: 5px;">
            <ul class="toolbar left">
                <li id="li_Express" runat="server"><span>
                    <img src="../images/t01.png"></span>物流信息</li>
                <li id="editLogistics" runat="server"><span>
                    <img src="../images/t02.png"></span><asp:Literal ID="lblwul" runat="server">编辑物流</asp:Literal></li>
                <li id="delLogistics" runat="server" onclick="return del()"><span>
                    <img src="../images/t03.png" /></span>删除物流</li>
                <asp:Button ID="btnDel" Style="display: none" runat="server" Text="删除" OnClick="btnDel_Click" />
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
                                <span>发货单号</span>
                            </td>
                            <td>
                                <label id="lblReceiptNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td style="width: 15%;">
                                <span>订单编号</span>
                            </td>
                            <td>
                                <label id="lblOrderNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td style="width: 15%;">
                                <span>代理商名称</span>
                            </td>
                            <td>
                                <label id="lblDisName" runat="server">
                                </label>
                                &nbsp;
                                <input id="hidDisId" type="hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>发货日期</span>
                            </td>
                            <td>
                                <label id="lblSendDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>经办人</span>
                            </td>
                            <td colspan="3">
                                <label id="lblActionUser" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <%--<td>
                                <span>物流公司</span>
                            </td>
                            <td>
                                <label id="lblExpress" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>物流单号</span>
                            </td>
                            <td>
                                <label id="lblExpressNo" runat="server">
                                </label>
                                &nbsp;
                            </td>--%>
                        </tr>
                        <%--<tr style="display: none">
                            <td>
                                <span>物流联系人</span>
                            </td>
                            <td>
                                <label id="lblExpressPerson" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>物流电话</span>
                            </td>
                            <td>
                                <label id="lblExpressTel" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>发货包数</span>
                            </td>
                            <td>
                                <label id="lblExpressBao" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>--%>
                        <%--<tr style="display: none">
                            <td>
                                <span>运费</span>
                            </td>
                            <td>
                                <label id="lblPostFee" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>经办人</span>
                            </td>
                            <td colspan="3">
                                <label id="lblActionUser" runat="server">
                                </label>
                                &nbsp;
                            </td>
                             <td>
                                <span>&nbsp;</span>
                            </td>
                            <td>
                                <label id="Label4" runat="server">
                                </label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span>备注</span>
                            </td>
                            <td colspan="5" style="word-wrap: break-word; padding-left: 5px; word-break: break-all;">
                                <label id="lblRemark" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>是否签收</span>
                            </td>
                            <td>
                                <label id="lblIsSign" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td>
                                <span>签收人</span>
                            </td>
                            <td>
                                <label id="lblSignUser" runat="server">
                                </label>
                                &nbsp;
                                <input type="hidden" id="hidSignUserId" runat="server" />
                            </td>
                            <td>
                                <span>签收日期</span>
                            </td>
                            <td>
                                <label id="lblSignDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>签收备注</span>
                            </td>
                            <td colspan="5" style="word-wrap: break-word; word-break: break-all;">
                                <label id="lblSignRemark" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
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
            <div style="padding-top: 10px; margin: 0px 5px;">
                <!--新增商品列表 start-->
                <div class="tablelist">
                        <asp:Repeater ID="rpDtl" runat="server">
                            <HeaderTemplate>
                                <table >
                                    <tr>
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
                                    <td style="text-align: left; text-indent: 0.5em; line-height: 20px;" title='<%# Eval("GoodsName").ToString() %>'>
                                    <div class="tcle">
                                        <%# Eval("GoodsName") %>
                                        <%# Eval("vdef1").ToString() != "0" ? "<i class=\"ProIcon\" tip_type=\"" + Eval("vdef3").ToString() + "\" tip=\"" + Eval("vdef1").ToString() + "\"></i>" : ""%>
                                        &nbsp;
                                        <%# Eval("BarCode")%>
                                        </div>
                                    </td>
                                    <td style="text-align: left; text-indent: 0.5em; line-height: 20px;" title='<%# Eval("GoodsInfos").ToString() %>'>
                                    <div class="tcle"> 
                                        <%# Common.MySubstring(Eval("GoodsInfos").ToString(), 50, "...") %>&nbsp;</div>
                                    </td>
                                    <td><div class="tc"> 
                                        <%# Eval("Unit")%>&nbsp;</div>
                                    </td>
                                    <td><div class="tc"> 
                                        <%# OrderInfoType.proTypePrce(Eval("vdef1").ToString(), Eval("vdef2").ToString(), Eval("Price").ToString())%>
                                       <i <%# Eval("vdef1").ToString()=="0"?"":"style=\"color:Red;\""  %>> <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></i> &nbsp;</div>
                                    </td>
                                    <td><div class="tc"> 
                                        <%# Eval("GoodsNum").ToString()%>&nbsp;
                                        <%# OrderInfoType.proType(Eval("vdef1").ToString() ,Eval("vdef2").ToString()) %></div>
                                    </td>
                                    <%--<td>
                                        <%# Eval("Remark")%>
                                    </td>--%>
                                    <td>
                                     <div class="tc">    <%# Convert.ToDecimal(Eval("Total")).ToString("N")%>&nbsp;</div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr id="trTotal" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count!=0).ToString())%>'>
                                    <%--<td style="text-align: center;">
                                        <span id="SumNum">
                                            <%# SelectGoods.SumNum(this.DisId,this.CompID).ToString("0.00") %></span>
                                    </td>--%>
                                    <td colspan="7">
                                        <div class="editmoney">
                                            <div class="mo-t"> 
                                                <%# OrderInfoType.proOrderType(ProIDD, ProPrice, ProType) %>   
                                            </div> 
                                            <div class="mo-t"> 
                                             合 计：<div>
                                                <span id="SumTatol">
                                                    <%# SelectGoods.SumTotal(this.DisId, this.CompID,ProPrice).ToString("N") %>
                                                </span></div>
                                            </div>
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
                <!--新增商品列表 end-->
                </div>
            </div>
            <!--销售订单明细 end-->
            <!--销售订单主体 start-->
            <div class="tools" style="padding-left: 5px; margin-top: 15px; font-size: 15px; font-weight: bold">
                物流信息:
            </div>
            <div>
                <div class="lbtb" style="margin-left: 50px" id="divInfomation" runat="server">
                    <b>物流公司</b>：
                    <label id="lblCompName" runat="server">
                        百世汇通
                    </label>
                    <b style="margin-left: 125px">物流单号</b>：
                    <label id="lblLogisticsNo" runat="server">
                        350358240660
                    </label>
                    <p style="margin-top: 5px;">
                    </p>
                    <b>物流跟踪</b>：
                    <label id="lblLogisticsInfo" runat="server">
                        以下跟踪信息由<a href="http://www.aikuaidi.cn/" style="color: Blue" target="_blank">爱快递提供</a>，如有疑问请到物流公司官网查询</label>
                    <br />
                    <br />
                    <div style="padding-left: 60px" class="lbtb2" id="divInfomation2" runat="server">
                        <table class="dh">
                            <asp:Repeater ID="rptLogistics" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="width: 15%;">
                                            <span>
                                                <%# Eval("time") %></span>
                                        </td>
                                        <td>
                                            <span style="text-align: left; padding-left: 15px;">
                                                <%# Eval("content")== null ? Eval("context") : Eval("content")%></span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
            <!--销售订单主体 end-->
        </div>
    </div>
    </form>
</body>
</html>
