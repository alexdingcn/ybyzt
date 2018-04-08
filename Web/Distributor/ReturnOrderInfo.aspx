<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnOrderInfo.aspx.cs"
    Inherits="Distributor_ReturnOrderInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>退单详细</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <%--<script src="../js/layer.js" type="text/javascript"></script>
    <link href="../css/layer.css" rel="stylesheet" type="text/css" />--%>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../Company/js/order.js" type="text/javascript"></script>

    <script type="text/javascript">
        function offIcon()
         {
           layerCommon.confirm("是否取消退货单?",function(){
                $.ajax({
                    url: 'ReturnOrderInfo.aspx?OffIcon=true&KeyID=<%= Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>',
                    //data:{KeyID:<%= "\""+Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)+"\"" %> },
                    dataType: 'json',
                    success: function (img) {
                        if(img.str)
                        {
                            location.href="ReturnOrderList1.aspx";
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                       layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                    }
                });
            }); 
            
        }

        function editIcon()
        {
            $.ajax({
                url: 'ReturnOrderInfo.aspx?editIcon=true&KeyID=<%=Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>',
                //data:{ KeyID:<%= "\""+Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)+"\"" %> },
                dataType: 'json',
                success: function (img) {
                    if(img.str)
                    {
                        layerCommon.msg("您的退货申请已被受理，请耐心等待审核结果",IconOption.哭脸);
                        location.href="returnorderinfo.aspx?KeyID=<%=Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) %>";
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                }
            });
        }

        function areturn()
        {
            location.href='ReturnOrderList.aspx';
        }

        //订单日志
        function returnLog()
        {
            var KeyId=<%=KeyID %>;
            var CompId=<%=this.CompID %>;
            Log(KeyId,CompId);
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-4" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="ReturnOrderList.aspx" class="cur">退单查询</a>>
                <a id="navigation3" href="#" class="cur">退单详细</a>
            </div>
        <!--功能条件 start-->
        <div class="userFun">
            <div id="A_btn" runat="server" class="left">
                </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <input type="hidden" id="hid_Alert" />
        <!--订单管理详细 start-->
        <div class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td class="head" style="width: 10%">
                            退货单号
                        </td>
                        <td style="width: 23%">
                            <%=ReceiptNo %>&nbsp;
                        </td>
                        <td class="head" style="width: 10%">
                            退货进度
                        </td>
                        <td style="width: 23%">
                            <%=ReturnState %>&nbsp;
                        </td>
                        <td class="head" style="width: 10%">
                            申请时间
                        </td>
                        <td style="width: 23%">
                            <%=CreateDate %>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            订单总价
                        </td>
                        <td>
                            <label id="lblTotalPrice" runat="server">
                            </label>
                        </td>
                        <td class="head">
                            审核人
                        </td>
                        <td>
                            <%=AuditUserName %>&nbsp;
                        </td>
                        <td class="head">
                            审核时间
                        </td>
                        <td colspan="3">
                            <%=AuditDate %>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            退货备注
                        </td>
                        <td colspan="5">
                            <%=ReturnContent %>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            审核备注
                        </td>
                        <td colspan="5">
                            <%=AuditRemark %>&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            
        </div>
        
        <!--订单管理 end-->
        
        
        <div class="blank10"></div>
        <div class="orderLiv">
                <table class="PublicList" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t1" >商品图片</th>
                                <th >商品名称</th>
                                <th >商品描述</th>
                                <th class="t2">单价</th>
                                <th class="t2">数量</th>
                                <th class="t2">总价</th>
                            </tr>
                        </thead>
                        <tbody>
                        <asp:Repeater ID="rptgoods" runat="server">
                        <ItemTemplate>
                            
                                <tr>
                                 <td><a href="/e<%# Common.GetGoodsID(Eval("goodsinfoid").ToString()) %>_<%# Eval("compid")%>.html" target="_blank" class="pic"><img src='<%# Common.picUrl(Eval("GoodsinfoID").ToString())%>' style="width: 70px; height: 70px;" /></a></td>
                                  <td>
                                   <div class="tc tcle">
                                   		<div class="title"><a href="/e<%# Common.GetGoodsID(Eval("goodsinfoid").ToString()) %>_<%# Eval("compid")%>.html" target="_blank" >
										<%# Common.GetName(Eval("GoodsName").ToString()) %>
                                        <%# Eval("vdef1").ToString() != "0" ? "<i class=\"sale\" tip_type=\"" + Eval("vdef3").ToString() + "\" tip=\"" + Eval("vdef1").ToString() + "\">促销</i>" : ""%></a></div>
                                       <div class="number"> <%# Eval("BarCode")%></div>
                                   </div>     
                                    </td>
                                    
                                    <td style="text-align:left; padding-left:10px;">
                                        <%# Common.MySubstring(Eval("GoodsInfos").ToString(), 50, "...")%>
                                    </td>
                                    <td>
                                         <%# OrderInfoType.proTypePrce(Eval("vdef1").ToString(), Eval("vdef2").ToString(), Eval("Price").ToString())%>
                                         <i <%# Eval("vdef1").ToString()=="0"?"":"style=\"color:Red;\""  %>>
                                        <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></i>
                                    </td>
                                    <td>
                                        <%# Eval("GoodsNum").ToString() %>
                                        <%# OrderInfoType.proType(Eval("vdef1").ToString() ,Eval("vdef2").ToString()) %>
                                    </td>
                                    <td>
                                        <%# Convert.ToDecimal(Eval("Total")).ToString("N")%>
                                    </td>
                                </tr>
                            
                        </ItemTemplate>
                        <FooterTemplate>
                                    <tr id="trTotal" runat="server" visible='<%#bool.Parse((rptgoods.Items.Count!=0).ToString())%>'>
                                       
                                        <%--<td style="text-align: center;">
                                        <span id="SumNum">
                                            <%# SelectGoods.SumNum(this.DisId,this.CompID).ToString("0.00") %></span>
                                    </td>--%>
                                        <td  colspan="6">
                                       <div class="money">
                                            <div class="mo-t"><%# OrderInfoType.proOrderType(ProIDD, ProPrice,ProType) %> </div>
                                           合 计： <b id="SumTatol" class="size"><%# SelectGoods.SumTotal(this.DisID, this.CompID).ToString("N")%></b>
                                        </div>    
                                       </td>
                                    </tr>
                                    <tr id="tr" runat="server" visible='<%# bool.Parse((rptgoods.Items.Count==0).ToString())%>' class="noordl">
                                        <td colspan="6" align="center">
                                            无匹配数据
                                        </td>
                                    </tr>
                                    
                         </FooterTemplate>
                    </asp:Repeater>
                    </tbody>
                </table>
            </div>
        
        
        
        
        
        
    </div>
    </div>
    </form>
</body>
</html>
