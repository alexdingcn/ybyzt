<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsTypeList.aspx.cs" Inherits="Admin_Systems_GoodsTypeList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品分类管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            })
        })
        $(document).ready(function () {

            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            $(".liSenior").on("click", function () {
                $("div.hidden").slideToggle(100);
            })

            $("li#libtnAdd").on("click", function () {
                location.href = "GoodsTypeModify.aspx";
            })
        })

        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                alert("- 每页显示数量不能为空");
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //重载
        function Reset() {
            location.href = "GoodsTypeList.aspx";
        }
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
            <a href="IndustryList.aspx">商品分类管理</a>
    </div>
    <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnAdd"><span>
                    <img src="../../Company/images/t01.png" /></span>新增商品分类</li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li onclick="return ChkPage()"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <li onclick="Reset()"><span>
                        <img src="../../Company/images/t06.png" /></span>重置</li>
                </ul>
                <ul class="toolbar3">
                    <li>商品分类名称:<input runat="server" id="txtname" type="text" class="textBox" /></li>
                    <li>排 序:<input runat="server" id="txtsort" type="text" class="textBox" style="width:40px;" /></li>
                    <li>状 态:
                        <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 45px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th>
                        商品分类名称
                    </th>
                    <th>
                        排 序
                    </th>
                    <th>
                        状 态
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Distribute" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a style="text-decoration: underline;" href='GoodsTypeInfo.aspx?KeyID=<%#Eval("Id") %>'>
                                    <%# Eval("GoodsTypeName")%></a>
                            </td>
                            <td>
                                <%# Eval("SortIndex")%>
                            </td>
                            <td>
                                <%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsEnabled")) == "0" ? "<font color=red>禁用</font>" : "启用"%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btn_SearchClick" />
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
