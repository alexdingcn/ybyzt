<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateList.aspx.cs" Inherits="Company_SysManager_RebateList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>返利查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>
        $(function() {
            $(document).on("keydown", function(e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            });
        });

        $(document).ready(function() {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            $(".liSenior").on("click", function() {
                $("div.hidden").slideToggle(100);
            });

            $("li#libtnAdd").on("click", function() {
                location.href = "RebateModify.aspx?Type=2";
            });
        });

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
            location.href = "RebateList.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/RebateList.aspx">返利查询</a></li>
            </ul>   
        </div>
        <!--当前位置 end-->
    
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnAdd" runat="server">
                    <span><img src="../../Company/images/t01.png" /></span>新增返利
                </li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li onclick="return ChkPage()">
                        <span><img src="../../Company/images/t04.png" /></span>搜索
                    </li>
                </ul>
                <ul class="toolbar3">
                    <li>返利单号:<input runat="server" id="txtRebateCode" type="text" class="textBox" /></li>
                    <li>代理商名称:<input runat="server" id="txtDisName" type="text" class="textBox" /></li>
                    <li>状态:
                        <asp:DropDownList ID="ddlRebateState" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="0">全部</asp:ListItem>
                            <asp:ListItem Value="1" Selected="True">有效</asp:ListItem>
                            <asp:ListItem Value="2">失效</asp:ListItem>
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
                    <th class="t2">返利编号</th>
                    <th class="t6">代理商名称</th>
                    <th class="t4">返利金额</th>
                    <th class="t4">可用余额</th>
                    <%--<th class="t4">返利类型</th>--%>
                    <th class="t6">有效期</th>
                    <th class="t4">状态</th>
                    <th class="t5">操作</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Distribute" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tcle">
                                    <a style="text-decoration: underline;" href='RebateInfo.aspx?KeyID=<%#Eval("Id") %>'>
                                        <%# Eval("ReceiptNo").ToString()%>
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="tc"><%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()), "DisName")%></div>
                            </td>
                            <td>
                                <div class="tc"><%# Eval("RebateAmount", "{0:N2}")%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# Eval("EnableAmount", "{0:N2}")%></div>
                            </td>
                            <%--<td>
                               <div class="tc"> <%# Eval("RebateType").ToString() == "1" ? "整单返利" : "分摊返利"%></div>
                            </td>--%>                            
                            <td>
                                <div class="tc"><%# Eval("StartDate", "{0:yyyy-MM-dd}")%> 至 <%# Eval("EndDate", "{0:yyyy-MM-dd}")%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# Eval("RebateState").ToString() == "1" ? "有效" : "失效"%></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <% if(Common.HasRight(this.CompID, this.UserID, "1119")) {%>
                                    <a href='RebateModify.aspx?KeyID=<%#Eval("Id") %>&&Type=2'><%# Eval("RebateState").ToString() == "1" ? "修改" : ""%></a>
                                     &nbsp;&nbsp;&nbsp;
                                    <% } %>
                                   
                                    <a href='RebateInfo.aspx?KeyID=<%#Eval("Id") %>'>查看</a>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                      <FooterTemplate>
                        <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&Rpt_Distribute.Items.Count!=0).ToString())%>'>
                            <td><div class="tcle"><font color="red">总计</font></div></td>
                            <td >&nbsp;</td>
                            <td>
                                <div class="tc"><asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label></div>
                            </td>
                             <td>
                                <div class="tc"><asp:label ID="Label1" runat="server" Text="" style="color:Red;"><%=tb.ToString("N") %></asp:label></div>
                            </td>
                           <td colspan="4">&nbsp;</td>
                        </tr>
                            </tbody>
                        </table>
                    </FooterTemplate>
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
