<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Distributor_UserList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>我的桌面</title><%--
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        //转账汇款页面
        function pay(Id) {
            window.location.href = 'Pay/Recharge.aspx?KeyID=' + Id;
        }
    </script>
  <style type="text/css">
         .userAcco .btn
        {
            background: #ff8106;
            height: 24px;
            padding: 0px 20px;
            line-height: 24px;
            border-radius: 10px;
            color: #fff;
            display: inline-block;
            width: auto;
        }
             
        .userAcco .btn:hover
        {
             background:#ff4e02;
        }
        .userLcol li a
        {
            color: red;
        }
 </style>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head runat="server" />
    <div class="w1200">
    <Left:Left runat="server" ShowID="nav-6" />
    <div class="rightCon">
    	<div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">我的桌面</a></div>

        <!--用户信息 start-->
        <div class="userInfo">
            <div class="userAcco">
                <i>企业钱包余额</i><b class="red" style=" font-weight:bold;">￥<%=price %></b><a href="Pay/remittanceAdd.aspx" class="btn">充
                    值</a></div>
            <ul class="userLcol">
                    <li><i>待支付订单：</i><a href='pay/orderPayList.aspx'><%=payCount %></a>&nbsp;单</li>
                    <li><i>待收货订单：</i><a href='ReceivingList.aspx'><%=ReceiveCount %></a>&nbsp;单</li>
                    <li><i>本月订单数：</i><a href='Rep/RepOrderList.aspx'><%=OrderCount %></a>&nbsp;单</li>
                    <li><i>本月订购额：</i><a href='Rep/RepMonthList.aspx?type=1'><%=MonthSum %></a>&nbsp;元</li>
                    <li><i>本月付款额：</i><a href='Rep/RepDetailsList.aspx'><%=PaymentSum %></a>&nbsp;元</li>
                    <li><i>本月应付额：</i><a href='pay/orderPayList.aspx?S=1'><%=PayableSum  %></a>&nbsp;元</li>
                </ul>
            <ul class="userCounts">
                <li><span class="icon u_i1"></span><a href="UserEdit.aspx">修改基本资料</a></li><li><span
                    class="icon u_i2"></span><a href="DeliveryList.aspx">维护收货地址</a></li>
                <li><span class="icon u_i3"></span><a href="PhoneEdit.aspx">修改绑定手机</a></li><li><span
                    class="icon u_i4"></span><a href="pay/PayQuickly.aspx">维护快捷支付</a></li>
            </ul>
        </div>
        <!--用户信息 end-->
        <div class="blank10">
        </div>
        <!--交易明细 start-->
        <div class="userTrend">
            <div class="uTitle">
                <b>企业钱包清单</b></div>
            <asp:Repeater runat="server" ID="rptDis">
                <HeaderTemplate>
                    <table class="PublicList list">
                        <thead>
                            <tr>
                                <th>
                                    流水帐号
                                </th>
                                <th>
                                    订单编号
                                </th>
                                <th>
                                    款项类型
                                </th>
                                <th>
                                    日期
                                </th>
                                <th>
                                    支付状态
                                </th>
                                <th>
                                    审核状态
                                </th>
                                <th>
                                    金额(元)
                                </th>
                                <%--<th>
                                    备注
                                </th>
                                <th style="text-align: center; di">
                                    操作
                                </th>--%>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <%--<td><a href="javascript:void(0)" tip="<%# Eval("ID") %>" class="tablelinkQx2" id="A1" style="text-decoration:underline;"><%# Eval("ID")%></a></td>--%>
                        <td>
                            <a href='pay/PrePayInfo.aspx?KeyID=<%# Eval("ID") %>'>
                                <%# Eval("ID")%>
                                &nbsp;</a>
                        </td>
                        <td>
                            <a href='neworder/orderdetail.aspx?KeyID=<%# Eval("OrderID") %>&type1=OrderList'>
                                <%# Convert.ToInt32(Eval("OrderID"))==0?"":new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(Eval("OrderID"))).ReceiptNo %>
                                &nbsp;</a>
                        </td>
                        <td>
                            <%# Common.GetPrePayStartName(Convert.ToInt32(Eval("PreType")))%>
                        </td>
                        <td>
                            <%# Convert.ToDateTime(Eval("Paytime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td>
                            <%# Common.GetNameBYPrePayMentStart(Convert.ToInt32(Eval("Start"))) %>
                        </td>
                        <td>
                            <%# Common.GetNameBYPreStart(Convert.ToInt32(Eval("AuditState")))%>
                        </td>
                        <td>
                            <%# Math.Round((decimal)Eval("Price"),2)%>
                        </td>
                        <%--  <td>
                            <%# Eval("vdef1")%>
                        </td>
                        <td style="width: 150px; display:none;" align="center" >
                         <%# Getmessage(Convert.ToInt32(Eval("Start")), Convert.ToInt32(Eval("ID")))%>
                        </td>--%>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody> </table>
                </FooterTemplate>
            </asp:Repeater>
            <!--分页 start-->
            <!--列表分页 start-->
            <div class="page pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="30%"
                    CustomInfoStyle="padding:5px 0 0 30px;cursor: default;color: #737373;text-align: left;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="3" OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <div class="blank10" style="clear:both; height:10px; overflow:hidden;"></div>
            <!--列表分页 end-->
            <!--分页 end-->
        </div>
        <div class="blank10"></div>
        <!--交易明细 end-->
    </div>
    </div>

    </form>
    
  
</body>
</html>
