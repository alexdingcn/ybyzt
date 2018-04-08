<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateList.aspx.cs" Inherits="Distributor_RebateList" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>返利</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");

            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
        });
        
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="OrderList" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
    <div class="rightCon">
    <div class="info">
         <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
         <a id="navigation2" href="RebateList.aspx" class="cur">返利列表</a></div>
        <!--功能条件 start-->
        <div class="userFun">
        <div class="left">
                    <i style="font-size: 15px;">返利余额:</i><i style="font-size: 15px; color: Red; font-weight:bold;">￥<%=tb.ToString("0.00")%>&nbsp;</i>
                   <%--<i style="font-size: 15px;">累计收益:</i><i style="font-size: 15px; color: Red; font-weight:bold;">￥5&nbsp;</i>  --%>                 
                 
                </div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">返利单编号：</label>
                        <input id="txtRebateCode" runat="server" type="text" class="box" style="width:110px;" />
                    </li>
                    <li>状态:
                        <asp:DropDownList ID="ddlRebateState" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="0">所有</asp:ListItem>
                            <asp:ListItem Value="1" Selected="True">有效</asp:ListItem>
                            <asp:ListItem Value="2">失效</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>
                         <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server" class="box3" />
                         <label class="head">行</label>
                    </li>
                </ul>
                <a href="javascript:void(0)" class="btnBl" id="Search"><i class="searchIcon"></i>搜索</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10"></div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="rptOrder" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th class="t1">
                                    返利单编号
                                </th>
                                <th class="t2">
                                    返利金额
                                </th>
                                <th class="t2">
                                    可用金额
                                </th>
                                <th class="t2">
                                    返利类型
                                </th>
                                <th class="t3">
                                    有效期
                                </th>
                                 <th class="t2">
                                     状 态
                                 </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a style="text-decoration: underline;" href='RebateInfo.aspx?KeyID=<%#Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>'>
                                        <%# Eval("ReceiptNo").ToString()%>
                                    </a>
                            </td>
                            <td>
                                <%# Eval("RebateAmount", "{0:N2}")%>
                            </td>
                            <td>
                                <%# Eval("EnableAmount", "{0:N2}")%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("RebateType").ToString() == "1" ? "整单返利" : "分摊返利"%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("StartDate", "{0:yyyy-MM-dd}")%> 至 <%# Eval("EndDate", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td style="width:100px;">
                                <%# Eval("RebateState").ToString() == "1" ? "有效" : "失效"%>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                     <FooterTemplate>
                        <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                            <td><font color="red">总计</font></td>
                            <td>
                                <div class="tc"><asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label></div>
                            </td>
                             <td>
                                <div class="tc"><asp:label ID="Label1" runat="server" Text="" style="color:Red;"><%=tb.ToString("N") %></asp:label></div>
                            </td>
                           <td colspan="5">&nbsp;</td>
                        </tr>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--订单管理 end-->
        <!--分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
            NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
            ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
            TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
            CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
            ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
            CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
        </webdiyer:AspNetPager>
        </div>
        <!--分页 end-->
    </div>
    </div>
    </form>
</body>
</html>
