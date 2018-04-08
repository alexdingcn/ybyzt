<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderZdtsList.aspx.cs"
    Inherits="Company_Order_OrderZdtsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>账单查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })

        $(document).ready(function () {
            
            $("#btnAdd").click(function () {
                //$(".tip").fadeIn(200);
                //$(window.parent.leftFrame.document).find(".menuson li.active").removeClass("active");
                //window.parent.leftFrame.document.getElementById("dkxd").className = "active";

                window.location.href = 'OrderZdtsAdd.aspx?type=1';
            });

            // 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'OrderZdtsList.aspx';
            });
        });

    </script>
    <style>
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />

    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../Order/OrderZdtsList.aspx">账单管理</a></li><li>></li>
                <li><a href="#">账单查询</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="btnAdd"><span>
                    <img src="../images/t01.png" /></span><font>新增账单</font></li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>账单编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" class="textBox" maxlength="50"/></li>
                    <li>账单日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 90%;">
                <li>每页<input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 
                </li>
                <li>总价区间:<input name="txtTotalAmount1" id="txtTotalAmount1" onkeyup="KeyInt2(this)" runat="server"
                        type="text" class="textBox2" maxlength="50"/> -
                    <input name="txtTotalAmount2" id="txtTotalAmount2" onkeyup="KeyInt2(this)" runat="server"
                        type="text" class="textBox2" maxlength="50"/>&nbsp;&nbsp; 
                </li>

                <li>支付状态:<select name="PayState" runat="server" id="ddrPayState" class="downBox">
                        <option value="-1">全部</option>
                        <option value="0">未支付</option>
                        <option value="1">部分支付</option>
                        <option value="2">已支付</option>
                    </select>&nbsp;&nbsp; 
                </li>
                <li>
                    代理商名称:<input id="txtDisName" runat="server" type="text" class="textBox" maxlength="50"/>&nbsp;&nbsp;
                </li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="rptOrder" OnItemCommand="rptOrder_ItemCommand">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <th class="t3">
                                账单编号
                            </th>
                            <th class="t3">
                                代理商名称
                            </th>
                            <th class="t1">
                                账单日期
                            </th>
                             <th class="t1">
                                有效截止日期
                            </th>
                            <th class="t3">
                                费用名称
                            </th>
                            <th class="t4">
                                支付状态
                            </th>
                            <th class="t5">
                                账单金额(元)
                            </th>
                           
                            <th class="t1">
                                制单人
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("Id") %>'>
                    
                    <td>
                        <div class="tc"><a href="javascript:void(0)" onclick='goInfo_ZD("<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>")'>
                            <%# Eval("ReceiptNo") %></a></div>
                    </td>
                    <td>
                       <div class="tcle"> <%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()),"DisName") %></div>
                    </td>
                    <td>
                        <div class="tc"><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></div>
                    </td>
                     <td>
                       <div class="tc"> <%# Convert.ToDateTime(Eval("ArriveDate")).ToString("yyyy-MM-dd")%></div>
                    </td>
                    <td>
                        <div class="tcle"><%# Eval("vdef2")%></div>
                    </td>
                    <td>
                       <div class="tc"> <%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></div>
                    </td>
                    <td>
                       <div class="tc"> <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></div>
                    </td>
                    
                    <td>
                       <div class="tc"> <%# Common.GetUserName(Convert.ToInt32(Eval("CreateUserID").ToString()))%></div>
                    </td>
                    
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->

        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" style="display: none" />
        
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
