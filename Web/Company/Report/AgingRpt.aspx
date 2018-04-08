<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgingRpt.aspx.cs" Inherits="Company_Report_AgingRpt" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>账龄统计</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <style>
        .timeClass a
        {
            height: 23px;
            line-height: 23px;
            display: inline-block;
            border: 1px solid #ddd;
            padding: 0px 10px;
            margin: 0 0 0 5px;
        }
        a.hover
        {
            color: #fff;
            border: 1px solid #5e89c9;
            background: #779bd1;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //回车键事件
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'AgingRpt.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-5" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="AgingRpt.aspx">账龄统计</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                </ul>
                <ul class="toolbar3">
                    <li>代理商:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox" />
                    </li>
                    <%--  <li class="timeClass"><a href="javascript:;" class="hover">本周</a> <a href="javascript:;">
                        本月</a> <a href="javascript:;">本年</a> </li>--%>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <asp:Repeater ID="rptOrder" runat="server">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <th class="t3">
                                代理商
                            </th>
                            <th class="t3">
                                未支付金额
                            </th>
                            <th class="t3">
                                1-30
                            </th>
                            <th class="t3">
                                31-60
                            </th>
                            <th class="t3">
                                61-90
                            </th>
                            <th class="t3">
                                91-180
                            </th>
                            <th class="t3">
                                180-
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <div class="tc">
                            <%# Common.GetDis( Convert.ToInt32( Eval("disid")),"disname")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# getSum(Eval("AuditAmount").ToString(), Eval("AuditAmount2").ToString(), Eval("AuditAmount3").ToString(), Eval("AuditAmount4").ToString(), Eval("AuditAmount5").ToString()).ToString("N")%>
                        </div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount") == DBNull.Value ? 0 : Eval("AuditAmount")).ToString("N")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount2") == DBNull.Value ? 0 : Eval("AuditAmount2")).ToString("N")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount3") == DBNull.Value ? 0 : Eval("AuditAmount3")).ToString("N")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount4") == DBNull.Value ? 0 : Eval("AuditAmount4")).ToString("N")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount5") == DBNull.Value ? 0 : Eval("AuditAmount5")).ToString("N")%></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex && rptOrder.Items.Count!=0).ToString())%>'>
                    <td>
                        <div class="tcle">
                            <font color="red">总计</font></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="total1" runat="server" Text="" Style="color: Red;"><%=zta.ToString("N")%></asp:Label></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="total2" runat="server" Text="" Style="color: Red;"><%=ztb.ToString("N") %></asp:Label></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="Label1" runat="server" Text="" Style="color: Red;"><%=ztc.ToString("N") %></asp:Label></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="Label2" runat="server" Text="" Style="color: Red;"><%=ztd.ToString("N")%></asp:Label></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="Label3" runat="server" Text="" Style="color: Red;"><%=zte.ToString("N") %></asp:Label></div>
                    </td>
                    <td>
                        <div class="tc">
                            <asp:Label ID="Label4" runat="server" Text="" Style="color: Red;"><%=ztf.ToString("N")%></asp:Label></div>
                    </td>
                </tr>
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
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    <div style="margin-left: 120px;">
        <span style="padding: 20px 0px 0px 20px; color: red;">统计说明：1、订单通过厂商审核后才计入统计 &nbsp;&nbsp;&nbsp;&nbsp;2、退货单通过审核后才计入统计&nbsp;&nbsp;&nbsp;&nbsp;3、作废的订/退货单不计入统计</span></div>
    </form>
</body>
</html>
