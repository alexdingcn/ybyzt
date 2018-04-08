<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderOutList.aspx.cs" Inherits="Company_Order_OrderOutList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>发货记录</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });
        });

        $(document).ready(function () {

            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            //重置
            $("#li_Reset").click(function () {
                window.location.href = 'OrderOutList.aspx';
            });
        });

        function goOut(Id) {
            window.location.href = 'OrderOutInfo.aspx?KeyID=' + Id+"&go=1";
        }

    </script>
    <style type="text/css">
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-1" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />

    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="#">发货记录</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li style=" display:none;">发货编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server"
                        class="textBox" maxlength="50"/>
                    </li>
                    <li>下单时间:
                        <input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 90%;">
                <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />行 
                </li>
                <li>
                    代理商名称:<input id="txtDisName" maxlength="50" runat="server" type="text" class="textBox" />&nbsp;&nbsp;
                </li>
                <li>
                    订单编号:<input name="txtReceiptNo1" maxlength="50" type="text" id="txtReceiptNo1" runat="server"
                        class="textBox" />&nbsp;&nbsp;
                </li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="rptOrder">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <%--<th><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)"/></th>--%>
                            <th style=" width:150px;">
                                发货编号
                            </th>
                            <th>
                                订单编号
                            </th>
                            <th>
                                代理商名称
                            </th>
                            <th style=" width:150px;">
                                下单时间
                            </th>
                            <%--<th>
                                收货地址
                            </th>--%>
                            <th style=" width:100px;">
                                订单总金额
                            </th>
                            <th>
                                发货时间
                            </th>
                            <th>
                                发货人
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("Id") %>'>
                    <%--<td>
                            <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                            <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("Id") %>' />
                        </td>--%>
                    <td>
                        <a href="javascript:void(0)" onclick='goOut("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>")'>
                            <%# Eval("ReceiptNo") %></a>
                    </td>
                    <td>
                         <a href='OrderAuditInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("OrderID").ToString(), Common.EncryptKey) %>&go=1&type=2'>
                            <%# OrderInfoType.getOrder(Eval("OrderID"), "ReceiptNo")%></a>
                    </td>
                    <td>
                        <%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()),"DisName") %>
                    </td>
                    <td>
                        <%# OrderInfoType.getOrder(Eval("OrderID"), "CreateDate").ToDateTime().ToString("yyyy-MM-dd HH:mm")%>
                    </td>
                    <%--<td>
                        <%# Common.GetAddr(Convert.ToInt32(OrderInfoType.getOrder(Eval("OrderID"), "AddrID")))%>
                    </td>--%>
                    <td>
                        <%# OrderInfoType.getOrder(Eval("OrderID"), "AuditAmount")%>
                    </td>
                    <td>
                        <%# Convert.ToDateTime(Eval("SendDate").ToString())==DateTime.MinValue?"": Convert.ToDateTime(Eval("SendDate")).ToString("yyyy-MM-dd HH:mm:ss")%>
                    </td>
                    <td>
                        <%# Common.GetUserName(Convert.ToInt32(Eval("CreateUserID").ToString())) %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    </form>
</body>
</html>
