<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsOrder.aspx.cs" Inherits="Distributor_Rep_CMerchantsOrder" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>采购报表</title>
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
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="RepMonthList" />
    <input type="hidden" id="hid_Alert">
    <div class="rightCon">
    <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
        <a id="navigation2" href="/Distributor/RepMoneyList.aspx" class="cur">采购报表</a></div>
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
                        <label class="head">商品名称：</label>
                        <input id="txtGoodsName" runat="server" type="text" class="box" style="width:110px;" /></li>
                    <li>
                        <label class="head">
                           日 期：</label><input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate" style=" width:100px;"
                                readonly="readonly" type="text" class="Wdate box" value="" /><i class="txt">—</i><input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" value="" style=" width:100px;" /></li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
                 
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a><a href="javascript:void(0);" onclick="javascript:location.href='CMerchantsOrder.aspx'" class="btnBl"><i
                    class="resetIcon"></i>重置</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th>
                            商品
                        </th>
                        <%--<th>
                            规格属性
                        </th>--%>
                        <th>
                            厂商
                        </th>
                        <th>
                            数量
                        </th>
                        <th>
                            采购金额
                        </th>
                        <th>
                            销售金额
                        </th>
                        <th>
                            授权期限
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptOrder" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("GoodsName").ToString()%></td>
                            <%--<td><%# Eval("ValueInfo")%></td>--%>
                            <td><%# Eval("CompName").ToString()%></td>
                            <td><%# Convert.ToDecimal(Eval("num") == DBNull.Value ? 0 : Eval("num")).ToString("0")%></td>
                            <td><%# Convert.ToDecimal(Eval("GoodsPrice") == DBNull.Value ? 0 : Eval("GoodsPrice")).ToString("N")%></td>
                           <%-- <td><%# LibraryDetail(Eval("ID").ToString()) %></td>--%>
                            <td><%# Convert.ToDecimal(Eval("LdAmount") == DBNull.Value ? 0 : Eval("LdAmount")).ToString("N")%></td>
                            <td><%# Eval("ForceDate","{0:yyyy-MM-dd}") +"~"+ Eval("InvalidDate", "{0:yyyy-MM-dd}")%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%--<tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                            <td><font color="red">总计</font></td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                            </td>
                            <td>
                                <asp:label ID="Label1" runat="server" Text="" style="color:Red;"><%=tb.ToString("N") %></asp:label>
                            </td>
                        </tr>--%>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
           
        </div>
        <!--订单管理 end-->
        <%--<div style="padding: 10px 0px 0px 8px;color:red;"><span >备注：进货统计订单为已审核、已发货、已到货状态的订单。</span></div>--%>
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
