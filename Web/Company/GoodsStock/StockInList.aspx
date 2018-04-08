<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockInList.aspx.cs" Inherits="Company_GoodsStock_StockInList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品入库列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $("#btnAdd").click(function () {
                
                var text = $(this).text().substring($(this).text().length-4)
                if (text == "新增入库") {
                    location.href = "StockInEdit.aspx?type=1"; //新增入库
                }
                else {
                    location.href = "StockInEdit.aspx?type=2"; //新增出库
                }
            })

            $("#Search").click(function () {
                var bool=true;
                if ($("#txtReceiptNo").val() == "") bool = false;
                if ($("#txtCreateDate").val() == "") bool = false;
                if ($("#txtEndCreateDate").val() == "") bool = false;
                $("#btnSearch")[0].click()
            })
        })
    </script>
        <style>
                #DropDownList1,#DropDownList2 {
    width: 100px;
    height: 25px;
    line-height: 25px;
    border: 1px solid #d1d1d1;
    text-indent: 5px;
    margin-left: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="javascript:;" runat="server" id="tiele">商品入库列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="btnAdd" runat="server" tip="1"><span>
                    <img src="../images/t01.png" /></span><font>新增入库</font></li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search" runat="server" ><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<li id="export"><span>
                        <img src="../images/tp3.png" /></span>导出</li>--%>
                </ul>
                <ul class="toolbar3">
                    <li>单号:<input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" class="textBox"
                        maxlength="50" /></li>
                    <li>类型: <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="全部">全部</asp:ListItem>
                        <asp:ListItem Value="采购入库">采购入库</asp:ListItem>
                        <asp:ListItem Value="盘点入库">盘点入库</asp:ListItem>
                        <asp:ListItem Value="其他入库">其他入库</asp:ListItem>
                    </asp:DropDownList></li>
                      <li>状态: <asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem Value="1">全部</asp:ListItem>
                        <asp:ListItem Value="0">待审核</asp:ListItem>
                        <asp:ListItem Value="2">已审核</asp:ListItem>
                    </asp:DropDownList></li>
                    <li>日期:<input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})"
                        style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                        value="" />&nbsp;-&nbsp;<input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})"
                            style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                            value="" />
                    </li>
                </ul>
            </div>
        </div>
        <!--信息列表 start-->
        <table class="tablelist" id="TbList">
            <thead>
                <tr>
                    <th class="t3" id="thNO" runat="server">
                        入库单号
                    </th>
                    <th class="t3" id="thDate" runat="server">
                        入库日期
                    </th>
                    <th class="t2" id="thType" runat="server">
                        入库类型
                    </th>
                    <th class="t1">
                        制单人
                    </th>
                    <th class="t5">
                        状态
                    </th>
                </tr>
            </thead>
            <tbody>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                      <tr>
                    <td>
                        <div class="tc">
               <a href="StockInInfo.aspx?type=<%#Eval("Type")%>&no=<%#Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey)%>" style="text-decoration:underline;">
                         <%# Eval("OrderNO") %></a></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%#Convert.ToDateTime( Eval("ChkDate")).ToString("yyyy-MM-dd") %></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("StockType") %></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# GetName(Convert.ToInt32( Eval("CreateUserID"))) %></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("State").ToString()=="2"?"已审核":"待审核" %></div>
                          </div>
                    </td>
                </tr>

                </ItemTemplate>
            </asp:Repeater>
              
              
            </tbody>
        </table>
        <!--信息列表 end-->
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
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
