<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnOrderList.aspx.cs"
    Inherits="Distributor_ReturnOrderList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>退单查询</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    location.href = $("#A1").attr("href");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });

    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="ReturnOrderList" />
        <div class="rightCon">
            <div class="info">
                 <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="ReturnOrderList.aspx" class="cur">退单查询</a></div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">退单编号：</label>
                            <input type="text" id="orderid" runat="server" class="box" style="width: 110px;" /></li>
                        <li>退货日期：
                            <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 110px;
                                height: 22px" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                                value="" />&nbsp;-
                            <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 110px;
                                height: 22px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                                value="" />&nbsp;&nbsp; </li>
                        <li>
                            <label class="head">退货状态：</label>
                                <select id="styte" runat="server" name="" class="xl">
                                    <option value="-2">全部</option>
                                    <option value="-1">已拒绝</option>
                                    <option value="0">未提交</option>
                                    <option value="1">待审核</option>
                                    <option value="2">已退货</option>
                                    <option value="4">已退货款</option>
                            </select></li>
                        
                    </ul>
                    <a href="#" id="A1" onserverclick="A_Seek" runat="server" class="btnBl"><i class="searchIcon">
                    </i>搜索</a> <a href="#" class="btnBl liSenior"><i class="resetIcon "></i>高级</a> <a
                        href="javascript:void(0);" onclick="javascript:location.href='ReturnOrderList.aspx'" class="btnBl"><i class="resetIcon"></i>重置</a>
                </div>
            </div>
            <div class="hidden userFun" style="text-align: right; padding-right: 160px; padding-top: 10px;
                display: none;">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">订单类型:</label>
                            <select id="ddrOtype" runat="server" style="width: 100px;" class="xl" name="AddType">
                                <option value="-1">全部</option>
                                <option value="0">销售订单</option>
                                <option value="1">赊销订单</option>
                                <option value="2">特价订单</option>
                            </select>
                        </li>
                        <li>
                            <label class="head">每页</label>
                            <input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this)" id="txtPager"
                                    name="txtPager" runat="server" class="box3" />
                            <label class="head">行</label>
                    </li>
                    </ul>
                </div>
            </div>
            <!--功能条件 end-->
            <div class="blank10">
            </div>
            <!--订单管理 start-->
            <div class="orderNr">
                <table class="PublicList list" id="returnorder" border="0" cellspacing="0" cellpadding="0">
                    <asp:Repeater ID="rptOrder" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th style=" width:20%">
                                        退单编号
                                    </th>
                                    <th>
                                        订单金额
                                    </th>
                                    <%--<th>
                                        应退金额
                                    </th>--%>
                                    <th>
                                        订单类型
                                    </th>
                                    <%--<th>
                                        创建人
                                    </th>--%>
                                    <th>
                                        退货人
                                    </th>
                                    <th>
                                        退货日期
                                    </th>
                                    <th>
                                        退货状态
                                    </th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td>
                                        <a href='<%#"returnorderinfo.aspx?KeyID="+Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>'>
                                            <%# GetReceiptNo(Eval("id").ToString())%>&nbsp;</a>
                                    </td>
                                    <td>
                                        <%# Math.Round((decimal)Eval("AuditAmount"),2).ToString("N") %>
                                    </td>
                                    <%--<td>
                                        <%# Math.Round((decimal)Eval("PayedAmount"),2)%>
                                    </td>--%>
                                    <td>
                                        <%# OrderInfoType.OType((int)Eval("otype")) %>&nbsp;
                                    </td>
                                    <%--<td>
                                        <%# Common.GetUserValue((int)Eval("CreateUserID"), "truename")%>&nbsp;
                                    </td>--%>
                                    <td>
                                        <%# Eval("ReturnMoneyUser")%>&nbsp;
                                    </td>
                                    <td>
                                        <%# Eval("ReturnMoneyDate","{0:yyyy-MM-dd HH:mm}") %>&nbsp;
                                    </td>
                                    <td>
                                        <%# GetROrder(Eval("id").ToString()) %>&nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                
            </div>
            <!--分页 start-->
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
                <!--分页 end-->
            <!--订单管理 end-->
        </div>
    </div>
    </form>
</body>
</html>
