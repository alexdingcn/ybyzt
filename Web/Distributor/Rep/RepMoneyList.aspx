<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepMoneyList.aspx.cs" Inherits="Distributor_Rep_RepMoneyList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>我的报表</title>
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
        function reset() {
            window.location.href = 'RepMoneyList.aspx';
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="RepMoneyList" />
    <div class="rightCon">
    <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="#" class="cur">我的报表</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                </div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            起止日期：</label><input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate" style=" width:100px;"
                                readonly="readonly" type="text" class="Wdate box" value="" /><i class="txt">—</i><input name="txtArriveDate1" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" value="" style=" width:100px;" /></li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
                 
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a><a href="" onclick="reset()" class="btnBl"><i
                    class="resetIcon"></i>重置</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    流水号
                                </th>
                                <th>
                                    转账总金额
                                </th>
                                <th>
                                    入账方式
                                </th>
                                <th>
                                    入账时间
                                </th>
                                <th>
                                    备注
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("ID")%>&nbsp;
                            </td>
                            <td>
                                <%# Convert.ToDecimal(Eval("Price").ToString()).ToString("N") %>&nbsp;
                            </td>
                            <td>
                                 <%# Common.GetDisValue(Convert.ToInt32(Eval("Disid").ToString()), "DisName")%>&nbsp;
                            </td>
                            <td>
                                 <%# Convert.ToDateTime(Eval("CreatDate").ToString()).ToString("yyyy-MM-dd")%>&nbsp;
                            </td>
                            <td>
                                <%# GetStr(Eval("vdef1").ToString() != "" ? Eval("vdef1").ToString() : "无")%>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td><font color="red">总计</font></td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                                </td>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
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
        <!--订单管理 end-->
    </div>
    </div>
    </form>
</body>
</html>