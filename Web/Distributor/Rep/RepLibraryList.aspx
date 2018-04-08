<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepLibraryList.aspx.cs" Inherits="Distributor_Rep_RepLibraryList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>销售报表</title>
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
    <Left:Left ID="Left1" runat="server" ShowID="RepGoodsList" />
    <input type="hidden" id="hid_Alert">
    <div class="rightCon">
    <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="/Distributor/RepLibraryList.aspx" class="cur">销售报表</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                </div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">商品：</label>
                        <input id="txtGoodsName" runat="server" type="text" class="box" style="width:110px;" />
                    </li>
                    <li><label class="head">医院：</label>
                           <input id="txthospital" runat="server" type="text" class="box" style="width:110px;" />
                        </li>
                    <li>
                        <label class="head">出库日期：</label>
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate" style=" width:100px;"
                                readonly="readonly" type="text" class="Wdate box" value="" />
                        <i class="txt">—</i>
                        <input name="txtArriveDate1" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" value="" style=" width:100px;" />
                    </li>
                    <li> 
                        <label class="head">每页</label>
                        <input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                            class="box3" />
                        <label class="head">行</label>
                    </li>
                </ul>
                <a id="A1" href="#" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a>
                <a href="javascript:void(0);" onclick="javascript:location.href='RepLibraryList.aspx'" class="btnBl"><i class="resetIcon"></i>重置</a>
                <%--<a href="javascript:void(0)"  class="btnBl liSenior"><i class="resetIcon"></i>高级</a>--%>
            </div>
        </div>

        <%--<div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
             <div class="right">
                <ul class="term">
                     
                </ul>
            </div>
        </div>--%>
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
                                    商品名称
                                </th>
                                <th>
                                    规格属性
                                </th>
                                <th>
                                    医院
                                </th>
                                <th>
                                    账期
                                </th>
                                <th>
                                    销售数量
                                </th>
                                <th>
                                    销售金额
                                </th>
                                
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="left" style="padding-left:10px;" title='<%#Eval("GoodsName").ToString() %>'>
                                <%# Eval("GoodsName").ToString().Length < 30 ? Eval("goodsName").ToString() : Eval("GoodsName").ToString().Substring(0, 30)%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("ValueInfo")%>
                            </td>
                            <td>
                                 <%# Eval("hospital")%>&nbsp;
                            </td>
                             <td><%# Eval("PaymentDays").ToString() == "0" ? "" : Eval("PaymentDays") %></td>
                            <td>
                                  <%# Convert.ToDecimal(Eval("OutNUm") == DBNull.Value ? 0 : Eval("OutNUm")).ToString("0")%>&nbsp;
                            </td>
                            <td>
                                <%# Convert.ToDecimal(Eval("sumAmount") == DBNull.Value ? 0 : Eval("sumAmount")).ToString("N")%>&nbsp;
                            </td>
                           
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td align="left" style="padding-left:10px;"><font color="red">总计</font></td>
                                <td colspan="4">&nbsp;</td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                                </td>
                            </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div style="padding: 10px 0px 0px 8px;color:red;"	><span ></span></div>
        <!--分页 start-->
            <div class="pagin" style="margin-top:0;">
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
