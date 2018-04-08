<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepDetailsList.aspx.cs" Inherits="Distributor_Rep_RepDetailsList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>订单支付明细</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../js/CommonJs.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    location.href = $("#A1").attr("href");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });
        function info(Id, PreType, orderid) {
            window.location.href = 'PayOrderInfo.aspx?RepDetailsList&orderid='+orderid+'&PreType='+PreType+'&KeyID=' + Id;
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="RepDetailsList" />
    <div class="rightCon">
    <div class="info">
        <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
        <a id="navigation2" href="/Distributor/Rep/RepDetailsList.aspx" class="cur">订单支付明细</a>
    </div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                </div>
            <div class="right">
                <ul class="term">
                         <li><label class="head">选择厂商：</label>
                            <select id="ddrComp" name="" style=" width:120px;" runat="server" class="xl">
                            </select>
                        </li>
                        <li>
                            <label class="head">
                            订单编号：</label><input id="orderid" runat="server" type="text" class="box" style="width:110px;" />
                        </li>
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a><a href="javascript:void(0);" onclick="javascript:location.href='RepDetailsList.aspx'" class="btnBl"><i
                    class="resetIcon"></i>重置</a>
                    <a href="javascript:void(0)"  class="btnBl liSenior"><i class="resetIcon "></i>高级</a>
            </div>
        </div>
        <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">
                            支付方式：</label>
                            <select name="ddrPayType" runat="server" id="ddrPayType" class="downBox">
                                <option value="-1">全部</option>
                                <option value="1">快捷支付</option>
                                <option value="2">银联支付</option>
                                <option value="3">网银支付</option>
                                <option value="4">B2B网银支付</option>
                                <option value="5">线下支付</option>
                                <option value="6">支付宝支付</option>
                                <option value="7">微信支付</option>
                                <option value="8">企业钱包支付</option>
                            </select>
                        </li>
                    <li>
                        <label class="head">
                            支付日期：</label><input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate" style=" width:100px;"
                                readonly="readonly" type="text" class="Wdate box" value="" /><i class="txt">—</i><input name="txtArriveDate1" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" value="" style=" width:100px;" /></li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
                    </ul>
                </div>
            </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="rptOrder" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    订单编号
                                </th>
                                <th>
                                    订单金额
                                </th>
                                <th>
                                    支付日期
                                </th>
                                <%--<th>
                                    金融机构
                                </th>--%>
                                <th>
                                    支付方式
                                </th>
                                <th>支付状态</th>
                                <th>确认状态</th>
                                <th>
                                    支付金额
                                </th>
                                <th>
                                    手续费
                                </th>
                                <%--<th>
                                    收款方
                                </th>--%>
                                <%--<th>
                                    备注
                                </th>--%>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href="javascript:void(0)" onclick='info("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>","<%# Common.DesEncrypt(Eval("PreType").ToString(), Common.EncryptKey) %>","<%# Common.DesEncrypt(Eval("orderID").ToString(), Common.EncryptKey) %>")'>
                                <%# Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "ReceiptNo")%>&nbsp;</a>
                            </td>
                            <td>
                                <%# Convert.ToDecimal(Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "AuditAmount")).ToString("N")%>&nbsp;</a>
                            </td>
                            <td>
                                <%# Convert.ToDateTime(Eval("Date")).ToString("yyyy-MM-dd")%>&nbsp;
                            </td>
                            <%--<td>
                                <%# GetBankName(Eval("BankID").ToString()) %>&nbsp;
                            </td>--%>
                            <td>
                                 <%# Eval("Source").ToString() %>&nbsp;
                            </td>
                            <td><%# Convert.ToString(Eval("status"))=="1"?"成功":"失败"%></td>
                            <td><%#Common.GetVdef9(Convert.ToString(Eval("vedf9")),Convert.ToString( Eval("Source")))%></td>
                            <td>
                                 <%# Convert.ToDecimal(Eval("Price") == DBNull.Value ? 0 : Eval("Price")).ToString("N")%>&nbsp;
                            </td>
                             <td>
                                 <%# Convert.ToDecimal(Eval("sxf")) == 0 ? "0.00" : Eval("sxf").ToString()%>&nbsp;
                            </td>
                            <%--<td>
                                 <%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()),"CompName")%>&nbsp;
                            </td>--%>
                            <%--<td title="<%#Eval("Remark").ToString()%>" style="cursor:pointer;">
                                <%# GetStr(Eval("Remark").ToString())%>&nbsp;
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td><font color="red">总计</font></td>
                                <td colspan="5">&nbsp;</td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                                </td>
                                <td>
                                    <asp:label ID="Label1" runat="server" Text="" style="color:Red;"><%=sxf.ToString("N") %></asp:label>
                                </td>
                            </tr>
                    </FooterTemplate>
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
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
            </div>
            <!--分页 end-->
        <!--订单管理 end-->
    </div>
    </div>
    </form>
</body>
</html>
