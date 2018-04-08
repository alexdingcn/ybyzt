<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisUserList.aspx.cs" Inherits="Admin_Systems_DisUserList" EnableEventValidation="false" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代理商管理员查询</title>
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
            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="salemanid" runat="server" />
        <input type="hidden" id="hid" runat="server" />
        <input type="hidden" id="aspx" runat="server" value="DisUserList.aspx" />
        <uc1:Org runat="server" ID="txtDisArea" />
           <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">代理商管理</a><i>></i>
            <a href="DisUserList.aspx">代理商管理员查询</a>
        </div>
              <div class="tools">
             

                    <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbDisUserList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                         <li>登录帐号:<input  runat="server" id="txtuname" type="text" class="textBox"/></li>
                         <li>创建日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                        </li>
                         <li>状  态:
                        <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:DropDownList>
                         </li>
                    </ul>
                </div>
             </div>
             <div class="hidden" style="display:none;">
               <ul style="">
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />&nbsp;条</li>
                    <li>代理商名称:<input  runat="server" id="txtDisName" type="text" class="textBox"/>&nbsp;&nbsp;</li>           
                    <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>         
                    <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                        class="textBox txtCompName" />&nbsp;&nbsp;</li>    
               </ul>
             </div>
             <table class="tablelist" id="TbDisUserList">
                <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>代理商名称</th>
                        <th>厂商名称</th>
                        <th>姓名</th>
                        <th>联系人手机</th>
                        <th>状态</th>
                        <th>类型</th>
                        <th>创建日期</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Disuser" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><a  href="DisUserInfo.aspx?KeyID=<%#Eval("ID") %>" style=" text-decoration:underline;"><%# Eval("username")%></a></td>
                         <td><a  href='DisInfo.aspx?KeyID=<%#Eval("DisID") %>&type=0' style=" text-decoration:underline;"> <%# Common.GetDisValue(Convert.ToInt32(Eval("Disid").ToString()), "DisName")%> </a></td>
                         <td><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("CompID") %>&type=4&atitle=代理商管理&btitle=代理商管理员查询' ><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%> </a></td>
                         <td><%# Eval("TrueName") %></td>
                         <td><%# Eval("Phone")%></td>
                         <td><%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></td>
                         <td><%# Eval("Type").ToString() == "5" ? "管理员" : "普通用户"%></td>
                         <td><%# Convert.ToDateTime(Eval("CreateDate").ToString()).ToString("yyyy-MM-dd")%></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

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
          
          </div>

    </form>
</body>
</html>
