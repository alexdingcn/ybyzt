<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferMoneyRpt.aspx.cs" Inherits="Company_Report_TransferMoneyRpt" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>转账汇款</title>

    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
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
                window.location.href = 'TransferMoneyRpt.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
        <div class="place">
	        <span>位置：</span>
	        <ul class="placeul">
                <li><a href="../../Company/index.aspx">我的桌面 </a></li>
                <li><a href="#">我要管账</a></li>
                <li><a href="#">转账汇款查询</a></li>
	        </ul>
        </div>
        <!--当前位置 end--> 

        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />

        <div class="rightinfo">
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                        <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>
                            代理商名称:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox"/> 
                        </li>
                        <li>
                            流水帐号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" class="textBox" />
                        </li>
                        <li>
                            汇款日期:<input runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtBCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                        -&nbsp;
                            <input runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />
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
            </ul>
        </div>
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <table class="tablelist" id="TbList">
                            <thead>
                                <tr>
                                    <th>代理商名称</th>
                                    <%--<th>代理商代码</th>--%>
                                    <th>流水帐号</th>
                                    <th>金额（元）</th>
                                    <th>汇款日期</th>
                                    <th>汇款人</th>
                                    <%--<th>支付状态</th>
                                    <th>款项类型</th>--%>
                                    <th>备注</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("ID") %>' >
                            <td><%# Common.GetDisValue(Eval("DisID").ToString().ToInt(0), "DisName")%></td>
                            <%--<td><%# Common.GetDisValue(Eval("DisID").ToString().ToInt(0), "DisCode")%></td>--%>
                            <td><%# Eval("ID")%></td>
                            <td><%# Convert.ToDecimal(Eval("Price").ToString() == "" ? "0" : Eval("Price").ToString()).ToString("N")%></td>
                            <td><%# Convert.ToDateTime(Eval("CreatDate").ToString()).ToString("yyyy-MM-dd")%></td>
                            <td><%# new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(Eval("CrateUser"))).TrueName%></td>
                            <%--<td><%# Common.GetNameBYPrePayMentStart(Convert.ToInt32(Eval("Start")))%></td>
                            <td><%# Common.GetPrePayStartName(Convert.ToInt32(Eval("PreType")))%></td>--%>
                            <td title="<%# Eval("vdef1").ToString()%>" style="cursor:pointer;"><%# GetStr(Eval("vdef1").ToString() != "" ? Eval("vdef1").ToString() : "无")%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
<%--                        <tr id="trTotal" runat="server" visible='<%#bool.Parse((rptOrder.Items.Count!=0).ToString())%>'>
                            <td><font color="red">总计</font></td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                            </td>
                            <td colspan="3">&nbsp;</td>
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

