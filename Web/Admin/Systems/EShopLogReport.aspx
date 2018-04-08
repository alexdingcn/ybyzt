<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EShopLogReport.aspx.cs" Inherits="EShopLogReport" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>店铺点击量查询</title>
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
                location.href = "EShopLogReport.aspx";
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
            <a href="EShopLogReport.aspx">店铺点击量查询</a>
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
                </ul>
                <ul class="toolbar3">
                    
                    <li>公司名称:<input name="txtCompName" type="text" id="txtCompName" runat="server" class="textBox" />
                    </li>
                    <li>访问时间:
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
        <table class="tablelist">
            <thead>
                <tr>
                    <th>
                        访问店铺
                    </th>
                    <th>
                        点击量
                    </th>
                    <th>
                        
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rtp_loginLog" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Module")%>
                            </td>
                            <td>
                                <%# Eval("Num")%>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <!--列表分页 start-->
        <div class="pagin" style=" display:none; ">
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
