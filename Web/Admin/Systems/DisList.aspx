<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisList.aspx.cs" Inherits="Admin_Systems_DisList" EnableEventValidation="false"  %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代理商查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })
        })
        $(document).ready(function () {

            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })

        })
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
            <a href="#">代理商管理</a><i>></i>
            <a href="#">代理商查询</a>
        </div>
        <uc1:Org runat="server" ID="txtDisArea" />
        <!--当前位置 end--> 
            <!--功能按钮 start-->
 <input type="hidden" id="salemanid" runat="server" />
 <input type="hidden" id="hid" runat="server" />
 <input type="hidden" id="aspx" runat="server" value="DisList.aspx" />
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbDisList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <%--<li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                        class="textBox txtCompName" /></li>--%>
                        <li>代理商名称:<input  runat="server" id="txtDisName" type="text" class="textBox"/></li>
                        <%--<li>代理商简称:<input  runat="server" id="txtShortName" type="text" class="textBox"/></li>--%>
                        <li>入驻日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden" style="display:none;">
               <ul style="">
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />&nbsp;条</li>
                    <li>启用:<asp:DropDownList runat="server" ID="ddlState" CssClass="downBox">
                        <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                        <asp:ListItem Value="1" Text="启用"></asp:ListItem>
                        <asp:ListItem Value="0" Text="禁用"></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;</li>
                    <li>审核状态:<asp:DropDownList runat="server" ID="ddlAUState" CssClass="downBox">
                           <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                           <asp:ListItem Value="2" Text="已审"></asp:ListItem>
                           <asp:ListItem Value="0" Text="未审"></asp:ListItem>
                          </asp:DropDownList>&nbsp;&nbsp;</li>
                    <%--<li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>--%>
               </ul>
             </div>
            <!--信息列表 start-->
            <table class="tablelist" id="TbDisList">
                <thead>
                    <tr>
                        <th>代理商名称</th>
                      <%--  <th>厂商名称</th>--%>
                       
                       <%-- <th>代理商分类</th>
                        <th>代理商区域</th>--%>
                        <th>代理商地区</th>
                        <th>审核状态</th>
                        <th>入驻日期</th>
                        <th>是否启用</th>
                        <th>联系人</th>
                        <th>联系人手机</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Distribute" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><a style=" text-decoration:underline; " href='DisInfo.aspx?KeyID=<%#Eval("Id") %>' ><%# Eval("DisName") %></a> </td>
                        <%-- <td><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("CompID") %>&type=4&atitle=代理商管理&btitle=代理商查询' ><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%> </a></td>--%>
                         
                       <%--  <td><%# Common.GetDisTypeNameById(Convert.ToInt32(Eval("DisTypeID").ToString()))%></td>
                         <td><%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></td>--%>
                         <td><%# Eval("Province").ToString() + Eval("City").ToString() + Eval("Area").ToString() + Eval("Address").ToString() %></td>
                         <td><%# Eval("AuditState").ToString() == "2" ? "已审" : "<span style='color:red'>未审</span>"%></td>
                         <td><%# Convert.ToDateTime(Eval("CreateDate").ToString()).ToString("yyyy-MM-dd")%></td>
                         <td><%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></td>
                         <td> <%# Eval("Principal")%></td>
                         <td><%# Eval("phone")%></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
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
