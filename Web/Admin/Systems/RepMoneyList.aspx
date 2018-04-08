<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepMoneyList.aspx.cs" Inherits="Admin_Systems_RepMoneyList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>转账汇款报表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
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
            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'RepMoneyList.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">报表查询</a><i>></i>
            <a href="RepMoneyList.aspx">转账汇款</a>
        </div>
        <!--当前位置 end--> 

        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li id="li_Reset"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                            class="textBox txtCompName" />
                        </li>                       
                        <li>
                            代理商名称:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox"/>
                        </li>
                        <li>
                            起止日期:<input runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtBCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                        -&nbsp;
                            <input runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                        </li>
                        <li>
                            每页显示<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" OnItemDataBound="rptOrder_ItemDataBound" >
                    <HeaderTemplate>
                        <table class="tablelist">
                            <thead>
                                <tr>
                                    <th>厂商名称</th>
                                    <th>代理商名称</th>
                                    <th>流水号</th>
                                    <th>转账总金额</th>
                                    <th>入账方式</th>
                                    <th>入账时间</th>
                                    <th>备注</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("DisID") %>' >
                            <td><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%></td>
                            <td><%# Common.GetDisValue(Eval("DisID").ToString().ToInt(0), "DisName")%></td>
                            <td><%# Eval("ID")%></td>
                            <td><%# Convert.ToDecimal(Eval("Price").ToString()).ToString("N")%></td>
                            <td><%# Common.GetDisValue(Convert.ToInt32(Eval("Disid").ToString()), "DisName")%></td>
                            <td><%# Convert.ToDateTime(Eval("CreatDate").ToString()).ToString("yyyy-MM-dd")%></td>
                            <td><%# GetStr(Eval("vdef1").ToString() != "" ? Eval("vdef1").ToString() : "无")%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td><font color="red">合计</font></td>
                                <td colspan="2">&nbsp;</td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"></asp:label>
                                </td>
                                <td colspan="3">&nbsp;</td>
                            </tr>
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