<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginLog.aspx.cs" Inherits="Admin_Systems_LoginLog" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录日志</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
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

            //重载
            $("#Reset").on("click", function () {
                location.href = "LoginLog.aspx";
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
            <a href="#">系统管理</a><i>></i>
            <a href="NewsList.aspx">登录日志</a>
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
                    <li id="Reset"><span>
                        <img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbLoginLogList" Visible="true" />
                </ul>
                <ul class="toolbar3">
                    <li>企业/代理商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server" class="textBox" />
                    </li>
                    <li>用 户:<input name="txtLoginName" type="text" id="txtLoginName" runat="server" class="textBox" />
                    </li>
                    <li>类 型:
                        <select name="ddlloginUserType" runat="server" id="ddlloginUserType" style="width: 135px;"
                            class="downBox">
                            <option value="-1">请选择</option>
                            <option value="0">平台后台管理员登录 </option>
                            <option value="4">企业管理员登录</option>
                            <option value="5">代理商管理员登录</option>
                        </select>
                    </li>
                    <li>登录时间:
                        <input name="txtDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                    </li>
                </ul>
            </div>
        </div>
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
        <!--功能按钮 end-->
        <table class="tablelist"  id="TbLoginLogList">
            <thead>
                <tr>
                    <th>
                        用 户
                    </th>
                    <th>
                        企业/代理商名称
                    </th>
                    <th>
                        类 型
                    </th>
                    <th>
                        登录IP地址
                    </th>
                    <th>
                        登录页面
                    </th>
                    <th>
                        登录时间
                    </th>
                    <th>
                        日志说明
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rtp_loginLog" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("LoginName")%>
                            </td>
                            <td>
                                <%# Eval("Module")%>
                            </td>
                            <td>
                                <%# Login(Eval("LoginUserType"))%>
                            </td>
                            <td>
                                <%# Eval("LoginIp").ToString() == "::1" ? "127.0.0.1" : Eval("LoginIp") %>
                            </td>
                            <td style=" max-width:200px; line-height:20px; word-break:break-all;">
                                <%# Eval("LoginPage")%>
                            </td>
                            <td>
                                <%# Eval("LoginStartDate")%>
                            </td>
                            <td>
                                <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
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
