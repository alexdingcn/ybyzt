<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinancingDetailList.aspx.cs" Inherits="Company_Financing_FinancingDetailList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>跨行出金明细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $("li#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            })

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="" />
    <div class="rightinfo">

    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="#">在线融资</a></li><li>></li>
            <li><a href="#">跨行出金明细</a></li>
        </ul>
    </div>
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>交易流水号:<input name="txtReceiptNo" type="text" id="txtFinancingNo" runat="server" class="textBox" maxlength="50"/></li>
                    <%--<li>下单日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                    </li>--%>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 90%;">
                <li>每页<input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 
                </li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="repfinan">
            <HeaderTemplate>
               <table class="tablelist" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th>
                                交易流水号
                            </th>
                            <th>
                                订单编号
                            </th>
                            <th>
                                发生额
                            </th>
                            <th>
                                制单日期
                            </th>
                            <th>
                                类型
                            </th>
                            <th>
                                状态
                            </th>
                            <th>
                                备注
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Convert.ToDateTime(Eval("ts")).ToString("yyyyMMdd") + getIDLen(Eval("ID").ToString())%>
                    </td>
                    <td>
                        <%#GetOrderNo(Convert.ToInt32(Eval("OrderID").ToString()))%>
                    </td>
                    <td>
                        <%#Convert.ToDecimal(Eval("AclAmt")).ToString("0.00") %>
                    </td>
                    <td>
                        <%#Convert.ToDateTime(Eval("ts")).ToString("yyyy-MM-dd")%>
                    </td>
                    <td>
                        <%#Common.GetFinacingType(Convert.ToInt32(Eval("Type").ToString())) %>
                    </td>
                    <td>
                        <%#Eval("State").ToString() == "1" ? "成功" : Eval("State").ToString()=="3"?"处理中":"其他"%>
                    </td>
                    <td title="<%# Eval("vdef1")%>" style="cursor:pointer;">
                        <%# GetStr(Eval("vdef1").ToString())%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->

        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="A_Seek" style="display: none" />

        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>

    </form>
</body>
</html>
