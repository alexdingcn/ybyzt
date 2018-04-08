<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayOrderList.aspx.cs" Inherits="Company_Pay_PayOrderList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单收款补录</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
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
                window.location.href = 'OrderCreateAdd.aspx?type=1';
            });

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            //批量删除
            //$("#VolumeDel").on("click", function () {
            //$("#btnVolumeDel").trigger("click");
            //    fromDel('提示', '确认删除吗？', Del);
            //});

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'PayOrderList.aspx';
            });
        });
        
        //补录
        function pay(ordid, start) {
            window.location.href = 'PaymentEdit.aspx?start=' + start + '&KeyID=' + ordid;
        }
        //补录详情
        function goPreInfo(Id) {
            window.location.href = 'PaymentInfo.aspx?KeyID=' + Id;
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
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../pay/PayOrderList.aspx">订单收款补录</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
    <input id="hid_Alert" type="hidden" />
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <%--<li class="click" id="btnAdd"><span>
                    <img src="../images/t01.png" /></span>代客下单</li>--%>
                <%--<li class="click2"><span><img src="../images/t02.png" /></span>编辑</li>--%>
                <%--<li id="VolumeDel"><span><img src="../images/t03.png" /></span>批量删除</li>--%>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <li class="liSenior" ><span>
                        <img src="../images/t07.png" /></span>高级</li>
                    <%--<li><span><img src="../images/t07.png" /></span>导出</li>--%>
                </ul>
                <ul class="toolbar3">
                   
                    <li>代理商名称:
                        <input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox" />
                        </li>
                         <li>支付状态:
                        <select id="ddrPaytype" runat="server" style="width: 100px;" class="downBox" name="ddrPaytype">
                            <option value="-1">全部</option>
                            <option value="0">未支付</option>
                            <option value="1">部分支付</option>
                            <option value="2">已支付</option>
                        </select>&nbsp;&nbsp; </li>
                   <%-- <li>订单类型:
                        <select id="ddrOtype" runat="server" style="width: 100px;" class="downBox" name="AddType">
                            <option value="-1">全部</option>
                            <option value="0">销售订单</option>
                            <option value="1">赊销订单</option>
                            <option value="2">特价订单</option>
                        </select>&nbsp;&nbsp; </li>
                    <li>订单来源:
                        <select id="ddraddtypess" runat="server" style="width: 100px;" class="downBox" name="AddType">
                            <option value="-1">全部</option>
                            <option value="1">正常下单</option>
                            <option value="2">企业补单</option>
                            <option value="3">APP下单</option>
                        </select>&nbsp;&nbsp; </li>--%>
                         <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 90%;">
                <li>订单编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server"
                        class="textBox" />
                    </li>
                    <li>下单日期：
                        <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                    </li>
                <li>总价区间：
                    <input name="txtTotalAmount1" id="txtTotalAmount1" onkeyup="KeyInt2(this)" runat="server"
                        type="text" class="textBox2" />
                    -
                    <input name="txtTotalAmount2" id="txtTotalAmount2" onkeyup="KeyInt2(this)" runat="server"
                        type="text" class="textBox2" />&nbsp;&nbsp; </li>
                <%--<li>订单来源:
                    <select id="ddrAddType" runat="server" style="width: 100px;" class="downBox" name="AddType">
                        <option value="-1">全部</option>
                        <option value="1">正常下单</option>
                        <option value="2">企业补单</option>
                        <option value="3">APP下单</option>
                    </select>&nbsp;&nbsp; </li>--%>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="rptOrder" OnItemCommand="rptOrder_ItemCommand">
            <HeaderTemplate>
                <table class="tablelist">
                    <thead>
                        <tr>
                            <%--<th><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)"/></th>--%>
                            <th class="t3">
                                订单编号
                            </th>
                           <%-- <th>
                                订单类型
                            </th>--%>
                            <th class="t2">
                                下单日期
                            </th>
                            <th class="t6">
                                代理商名称
                            </th>
                            <th class="t1">
                                订单状态
                            </th>
                             <th class="t1">
                                支付状态
                            </th>
                            <th class="t5">
                                订单总价(元)
                            </th>
                            <%--<th>
                                订单来源
                            </th>--%>
                            <%--<th>
                                下单人
                            </th>--%>
                            <th class="t2">
                                操作
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
                       <div class="tc"> <a href="javascript:void(0)" onclick='goInfo(<%# Eval("Id") %>)'>
                            <%# Eval("ReceiptNo") %></a></div>
                    </td>
                   <%-- <td>
                        <%# OrderInfoType.OType(int.Parse(Eval("OType").ToString()))%>
                    </td>--%>
                    <td>
                       <div class="tc"><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></div>
                    </td>
                    <td>
                        <div class="tcle"><%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()),"DisName") %></div>
                    </td>
                    <td>
                        <div class="tc"><%# OrderInfoType.OState(int.Parse(Eval("ID").ToString()))%></div>
                    </td>
                    <td>
                         <div class="tc"><%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></div>
                    </td>
                    <td>
                        <div class="tc"> <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></div>
                    </td>
                   <%-- <td>
                        <%# OrderInfoType.AddType(int.Parse(Eval("AddType").ToString()))%>
                    </td>--%>
                    <%--<td>
                        <%# Common.GetUserName(Convert.ToInt32(Eval("CreateUserID").ToString()))%>
                    </td>--%>
                    <td  align="center">
                      <div class="tc"> <%# Getmessage(Convert.ToInt32(Eval("OState")), Convert.ToInt32(Eval("Id")),Convert.ToInt32(Eval("PayState")) )%>
                      <%--<a href="javascript:void(0)"   onclick='pay("<%# Eval("Id") %>","<%# Eval("OState") %>","<%# Eval("PayState") %>")' class="tablelinkQx"  id="clikcbl"> 补录</a>
                      <a href="javascript:void(0)"   onclick='goIncz("<%# Eval("Id") %>")'  class="tablelinkQx" id="clikccz">查看详情</a>
                      --%>
                      &nbsp; <a href="javascript:void(0)"   onclick='goInfo("<%# Eval("Id") %>")'  class="tablelinkQx" id="clikccz">订单详情</a></div>
                      
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
        <asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" OnClick="btnVolumeDel_Click"
            Style="display: none" />
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
