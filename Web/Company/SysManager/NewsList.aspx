<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsList.aspx.cs" Inherits="NewsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>信息发布</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

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
                location.href = "NewsModify.aspx";
            })
        })

        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //重载
        function Reset() {
            location.href = "NewsList.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">
    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
            <li><a href="../SysManager/NewsList.aspx">信息发布</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
    
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnAdd"><span>
                    <img src="../../Company/images/t01.png" /></span>新增信息</li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li onclick="return ChkPage()"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>信息标题:<input runat="server" id="txtnewtitle" type="text" class="textBox" /></li>
                    <li>信息类型:
                        <asp:DropDownList ID="ddlNewType" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">新闻</asp:ListItem>
                            <asp:ListItem Value="2">通知</asp:ListItem>
                            <asp:ListItem Value="3">公告</asp:ListItem>
                            <asp:ListItem Value="4">促销</asp:ListItem>
                            <asp:ListItem Value="5">企业动态</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>是否置顶:
                        <asp:DropDownList ID="ddlNewTop" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">置顶</asp:ListItem>
                            <asp:ListItem Value="0">非置顶</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>店铺发布:
                        <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">所有</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>每页显示<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 45px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th class="t6">
                        信息标题
                    </th>
                    <th class="t5">
                        信息类型
                    </th>
                     <th class="t2">
                     发布日期
                    </th>
                    <th class="t4">
                        是否置顶
                    </th>
                    <th class="t4">
                        店铺发布
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Distribute" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tcle"><a style="text-decoration: underline;" href='NewsInfo.aspx?KeyID=<%#Eval("Id") %>'>
                                    <%#Util.GetSubString(Eval("NewsTitle").ToString(), 50)%>
                                </a></div>
                            </td>
                            <td>
                                <div class="tc"><%# Common.GetCPNewStateName(Eval("NewsType").ToString())%></div>
                            </td>
                            <td>
                            <div class="tc"><%# Eval("CreateDate", "{0:yyyy-MM-dd HH:mm}")%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsTop")) == "0" ? "非置顶" : "置顶"%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsEnabled")) == "0" ? "<font color=red>否</font>" : "是"%></div>
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
