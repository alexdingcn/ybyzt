<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="Company_Report_Pay" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售订单管理</title>

    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
        <!--当前位置 end--> 

        <div class="rightinfo" style="margin-top:0px; margin-left:0px; width:auto;">
            <!--功能按钮 start-->
            
            <!--功能按钮 end-->
            
            <!--信息列表 start-->
                <table class="tablelist"  id="TbDisList">
                <thead id="Thead1" runat="server">
                    <tr>
                        <th runat="server" id="num">订单编号</th>
                        <th runat="server" id="date">下单日期</th>
                        <%if (Request["type"].ToString() == "2")
                          { %><th>费用名称</th><%} %>
                        <th runat="server" id="amount">订单金额</th>
                        <th >支付金额</th>
                        <th runat="server" id="state">订单状态</th>
                        <th>支付状态</th>
                    </tr>
                </thead>
                <tbody id="Tbody1" runat="server">
                  <asp:Repeater ID="rptOrder" runat="server" >
                   <ItemTemplate>
                    <tr>
                         <td><div class="tcle"><%# Eval("ReceiptNo")%></div></td>
                            <td><div class="tcle"><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd")%></div></td>
                            <%if (Request["type"].ToString() == "2")
                            { %><td title="<%# Eval("vdef2")%>"><div class="tcle"><%# Convert.ToString(Eval("vdef2")).Length > 8 ? Convert.ToString(Eval("vdef2")).Substring(0, 8) : Convert.ToString(Eval("vdef2"))%></div></td><%} %>
                            <td><div class="tcle"><%# (Convert.ToDecimal(Eval("AuditAmount") == DBNull.Value ? 0 : Eval("AuditAmount")) + Convert.ToDecimal(Eval("OtherAmount") == DBNull.Value ? 0 : Eval("OtherAmount"))).ToString("N")%></div></td>
                            <td><div class="tcle"><%# (Convert.ToDecimal(Eval("PayedAmount") == DBNull.Value ? 0 : Eval("PayedAmount"))).ToString("N")%></div></td>
                            <td><div class="tcle"><%# OrderInfoType.OState(int.Parse(Eval("ID").ToString()))%></div></td>
                            <td><div class="tcle"><%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></div></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                    </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end--> 
        
        </div>
    </form>
</body>
</html>
