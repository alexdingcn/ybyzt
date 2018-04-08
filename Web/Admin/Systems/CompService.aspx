<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompService.aspx.cs" Inherits="Admin_Systems_CompService" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>购买服务用户</title>
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
            location.href = "MessageList.aspx";
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
            <a href="CompService.aspx">购买服务用户</a>
    </div>
    <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li onclick="return ChkPage()"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <li onclick="Reset()"><span>
                        <img src="../../Company/images/t06.png" /></span>重置</li>
                   
                </ul>
                <ul class="toolbar3">
                    <li>厂商名称:<input runat="server" id="CompName" type="text" class="textBox" /></li>
                   <%-- <li> &nbsp; &nbsp; 电话:<input runat="server" id="txtUserPhone" type="text" class="textBox" /></li>--%>
                    <li> &nbsp; &nbsp; 服务类型:
                        <asp:DropDownList ID="drpState" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="2">月费</asp:ListItem>
                            <asp:ListItem Value="1">年费</asp:ListItem>
                        </asp:DropDownList></li>
                    <li> &nbsp; &nbsp; 每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
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
                        厂商名称
                    </th>
                    <th>
                        服务类型
                    </th>
                    <th>
                        服务金额
                    </th>
                    <th>
                        到期时间
                    </th>
                    <th>
                        开通日期
                    </th>
                    <th>
                        处理状态
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="ServiceList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("CompName").ToString()%>
                            </td>
                            <td>
                                <%# Eval("ServiceType").ToString()=="1"?"年费":"月费"%>
                            </td>
                            <td>
                                <%# Eval("Price")%>
                            </td>
                            <td>
                                <%#Eval("OutData","{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                <%# Eval("CreateDate", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                已支付
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
