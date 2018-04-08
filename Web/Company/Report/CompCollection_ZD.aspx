<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompCollection_ZD.aspx.cs" Inherits="Company_Report_CompCollection_ZD" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>账单收款明细</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'CompCollection_ZD.aspx';
            });
        });

        function goAuditInfo(Id, page) {
            window.location.href = '../Order/OrderAuditInfo.aspx?KeyID=' + Id + "&page=" + page;
        }
        //转到详细页
        function GotReturnInfo(Id) {
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 550) / 2; //计算宽度
            var index = layerCommon.openWindow('账单详情', '../Report/CompOrderInfo_ZD.aspx?KeyID=' + Id, '900px', '450px');
            $("#hid_Alert").val(index);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-2" />
        <input type="hidden" id="hid_Alert" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />

        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../Report/CompCollection.aspx">账单收款明细</a></li>
	            </ul>
            </div>
            <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../images/t04.png" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                        <li class="liSenior" style="display:none;"><span><img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                         <li>
                            账单编号：<input id="orderid" runat="server" type="text" class="textBox" />
                        </li>
                        <li>
                            代理商名称:<input id="txtDisName" runat="server" type="text" class="textBox" />
                        </li>
                        
                        <li>
                            收款日期:
                            <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                            -&nbsp;
                            <input name="txtECreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden">
            <ul style="width: 90%;">
                <li>
                    每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                </li>
                <li>
                    收款来源:
                    <select name="ddrPayType" runat="server" id="ddrPayType" class="downBox">
                        <option value="-1">全部</option>
                        <option value="1">快捷支付</option>
                        <option value="2">网银支付</option>
                        <option value="3">企业钱包支付</option>
                        <option value="4">转账汇款</option>
                        <option value="5">现金</option>
                        <option value="6">票据</option>
                        <option value="7">其它</option>
                    </select>&nbsp;&nbsp;
                </li>
            </ul>
        </div>
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <table class="tablelist" id="TbList">
                            <thead>
                                <tr>
                                    <th class="t3">账单编号</th>
                                    <th class="t1">收款日期</th>
                                    <th class="t2">费用名称</th>
                                    <th class="t3">代理商名称</th>
                                    <%--<th>金融机构</th>--%>
                                    <th class="t2">收款来源</th>
                                    <%--<th>渠道</th>--%>
                                    <th class="t5">收款金额</th>
                                    <%--<th>对账状态</th>--%>
                                    <%--<th>备注</th>--%>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("ID") %>' >                            
                            <td>
                               <div class="tc"><a style=" text-decoration:underline;" href="javascript:void(0)" onclick='GotReturnInfo(<%# Eval("ID") %>)'>
                                <%# Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "ReceiptNo")%>&nbsp;</a></div>
                            </td>
                            <td> <div class="tc"><%# Convert.ToDateTime(Eval("Date")).ToString("yyyy-MM-dd")%></td></div>
                            <td> <div class="tc"><%# Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "vdef2")%></td></div>
                            <td> <div class="tcle"><%# Eval("DisName").ToString()%></td></div>
                            <%--<td><%# GetBankName(Eval("BankID").ToString())%></td>--%>
                            <td> <div class="tc"><%# Eval("Source").ToString()%></td></div>
                            <td> <div class="tc"><%# Convert.ToDecimal(Eval("Price") == DBNull.Value ? 0 : Convert.ToDecimal(Eval("Price")) - Convert.ToDecimal(Eval("sxf")+"" == "" ? "0" : Eval("sxf"))).ToString("N")%></div></td>
                            <%--<td title="<%#Eval("Remark").ToString()%>" style="cursor:pointer;"><%# GetStr(Eval("Remark").ToString())%></td>--%>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                     <%--  <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                            <td><font color="red">总计</font></td>
                            <td colspan="4">&nbsp;</td>
                            <td>
                                <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>--%>
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
