<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="Admin_Systems_OrderList" EnableEventValidation="false" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>订单查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $('.tablelist tbody tr:odd').addClass('odd');
            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
        });
        function goInfo(id) {
            alert(id)
            location.href = "neworder/orderdetail.aspx?KeyId=" + id;
        }
    </script>

    <style type="text/css">
        .tablelist td a{
            text-decoration:underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="salemanid" runat="server" />
        <input type="hidden" id="hid" runat="server" />
        <input type="hidden" id="aspx" runat="server" value="OrderList.aspx" />
        <uc1:Org runat="server" ID="txtDisArea" />
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
             <a href="#">订单查询</a><i>></i>
            <a href="OrderList.aspx">订单查询</a>
        </div>
        <!--当前位置 end--> 

        <input id="hid_Alert" type="hidden" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        
            <!--功能按钮 start-->
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li onclick="javascript:location.href='OrderList.aspx'"><span>
                        <img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbOrderList" Visible="true" />
                        <li class="liSenior"><span>
                        <img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                        class="textBox txtCompName" maxlength="20" /></li>
                        <li>
                            订单编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" class="textBox"  maxlength="30"/>
                        </li>
                        <li>下单日期：
                            <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                 id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                            <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                 id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                        
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden" style="display:none;">
            <ul style="width: 900px;">
                <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 </li>
                <li>总价区间：
                    <input name="txtTotalAmount1" id="txtTotalAmount1" runat="server" type="text" class="textBox2" />
                    -
                    <input name="txtTotalAmount2" id="txtTotalAmount2" runat="server" type="text" class="textBox2" />&nbsp;&nbsp;
                </li>
                <li>订单来源:
                    <select id="ddrAddType" runat="server" class="downBox" name="AddType">
                        <option value="-1">全部</option>
                        <option value="1">正常下单</option>
                        <option value="2">企业补单</option>
                        <option value="3">APP下单</option>
                        <%--<option value="3">APP下单</option>--%>
                    </select>&nbsp;&nbsp; </li>
               <%-- <li>订单类型:
                    <select id="ddrOtype" runat="server" class="downBox" name="AddType">
                        <option value="-1">全部</option>
                        <option value="0">销售订单</option>
                        <option value="1">赊销订单</option>
                        <option value="2">特价订单</option>
                    </select>&nbsp;&nbsp; </li>--%>
                <li>支付状态：
                        <select name="PayState" runat="server" id="ddrPayState" class="downBox">
                            <option value="-1">全部</option>
                            <option value="0">未支付</option>
                            <option value="1">部分支付</option>
                            <option value="2">已支付</option>
                            <option value="5">已退款</option>
                        </select>&nbsp;&nbsp;
                </li>
                <li>订单状态:
                        <select name="ddrOState" runat="server" id="ddrOState" class="downBox">
                                <option value="-2">全部</option>
                                <option value="1">已提交</option>
                                <option value="2">已审核</option>
                                <option value="3">退货处理</option>
                                <option value="4">已发货</option>
                                <option value="5">已到货</option>
                                <option value="6">已作废</option>
                               
                        </select>&nbsp;&nbsp;
                </li>
                <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
            </ul>
        </div>
            <!--信息列表 start-->

            <asp:Repeater runat="server" ID="rptOrder">
                <HeaderTemplate>
                    <table class="tablelist" id="TbOrderList">
                        <thead>
                            <tr>
                                <th>订单编号</th>
                                <th>厂商名称</th>
                                <th>下单日期</th>
                                <th>订单状态</th>
                                <th>支付状态</th>
                                <th>订单总价(元)</th>
                                <%--<th>订单类型</th>--%>
                                <th>订单来源</th>                                
                                <th>制单人</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id='tr_<%# Eval("Id") %>' >
                        <td><a href="OrderInfo.aspx?KeyId=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>" ><%# Eval("ReceiptNo") %></a></td>
                        <td><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("CompID") %>&type=4&atitle=订单查询&btitle=订单查询' ><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%> </a></td>
                        <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></td>
                        <td><%# Enum.GetName(typeof(Enums.OrderState),Eval("OState"))%></td>
                        <td><%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></td>
                        <td>
                           <%-- <%# Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00")%>--%>
                            <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></div>
                        </td>
                       <%-- <td><%# OrderInfoType.OType(int.Parse(Eval("OType").ToString()))%></td>--%>
                        <td><%# Enum.GetName(typeof(Enums.AddType),Eval("AddType"))%> </td>                   
                        <td><%# getUserTName(Eval("CreateUserID").ToString().ToInt(0))%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
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
